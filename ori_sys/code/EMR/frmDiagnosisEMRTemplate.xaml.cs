using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using MessageBoxControl;
using PalashDynamic.Localization;
using PalashDynamics.Controls;
using PalashDynamics.Administration;
using System.Windows.Printing;
using System.IO;

namespace EMR
{
    public partial class frmDiagnosisEMRTemplate : ChildWindow
    {
        public frmDiagnosisEMRTemplate()
        {
            InitializeComponent();
        }

        WaitIndicator Indicatior = new WaitIndicator();
        public Patient SelectedPatient { get; set; }
        public FormDetail SelectedHistoryStructure { get; set; }
        public FormDetail SelectedFormStructure { get; set; }
        public string SelectedUser { get; set; }
        public List<clsPatientPrescriptionDetailVO> PrescriptionList { get; set; }
        public List<clsPatientPrescriptionDetailVO> MedicationList { get; set; }
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList { get; set; }

        public List<clsPatientEMRDetailsVO> listPatientEMRDetails { get; set; }
        public List<clsPatientEMRUploadedFilesVO> listPatientEMRUploadedFiles { get; set; }
        public bool IsSaved = false;
        public bool IsView = false;
        bool IsFirstTime = false;
        public int VisitTypeID { get; set; }
        public int TemplateID { get; set; }
        public string DiagnosisCode { get; set; }
        public string DiagnosisName { get; set; }
        private System.Collections.ObjectModel.ObservableCollection<string> listOfReports = new System.Collections.ObjectModel.ObservableCollection<string>();
        public clsVisitVO CurrentVisit { get; set; }
        System.Windows.Controls.Primitives.Popup p = null;
        System.Windows.Controls.Primitives.Popup pf = null;
        public clsCaseReferralVO objCaseReferral { get; set; }
        public clsPCRVO objPCR { get; set; }
        public clsVarianceVO variance { get; set; }
        public bool IsConsultationVisit = false;
        //public bool IsTemplateByDoctor = false;
        public string TemplateByNurse;
        public bool IsFreeze = false;

        public event RoutedEventHandler OnAddButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        bool IsEditRecord = false;
        string Tab = string.Empty;
        bool IsSaveRecord = false;
        public bool IsEnabledControl = false;
        public bool TemplateLoad = false;
        public clsBPControlVO objBPDetails { get; set; }
        public List<clsEyeControlVO> EyeList { get; set; }
        public List<clsEyeSubjectiveCorrectnControlVO> EyeSubCorrctnList { get; set; }
        public List<clsEyeSubjectiveCorrectnControlVO> EyePupilaryReactionList { get; set; }
        bool IsBPControl = false;
        bool IsVisionControl = false;
        bool IsGPControl = false;
        public clsVisionVO objVisionDetails { get; set; }
        public clsGlassPowerVO objGlassPower { get; set; }
        bool GetBPFromDB = false;
        bool GetVisionFromDB = false;
        bool GetGPFromDB = false;

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (listPatientEMRDetails == null)
                listPatientEMRDetails = new List<clsPatientEMRDetailsVO>();
            if (IsEnabledControl == false)
            {
                cmdSave.IsEnabled = false;
            }

            EyeList = new List<clsEyeControlVO>();
            EyeSubCorrctnList = new List<clsEyeSubjectiveCorrectnControlVO>();
            EyePupilaryReactionList = new List<clsEyeSubjectiveCorrectnControlVO>();
            //FillPhysicalExam();
            if (TemplateLoad)
                GetEMRDetails(TemplateID);
            else
                FillPhysicalExam();
        }

        private void cmbComplaint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Form.RowDefinitions.Clear();
            Form.Children.Clear();
            SelectedFormStructure = null;
            scroolForm.ScrollToTop();
        }
        public List<clsPatientEMRDetailsVO> EmrDetails = new List<clsPatientEMRDetailsVO>();

        private void GetEMRDetails(long TemplateID)
        {
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            clsGetPatientDiagnosisEMRDetailsBizActionVO BizAction = new clsGetPatientDiagnosisEMRDetailsBizActionVO();
            BizAction.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
            BizAction.PatientID = this.SelectedPatient.PatientId;
            BizAction.PatientUnitID = this.SelectedPatient.patientUnitID;
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.Tab = "Consultation";
            BizAction.TemplateID = TemplateID;
            if (IsEnabledControl == true)
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.UnitID = 0;
            GetGPFromDB = false;
            GetBPFromDB = false;
            GetVisionFromDB = false;

            PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
            {
                if (argsBizActionObjPatientHistoryData.Error == null)
                {
                    if (argsBizActionObjPatientHistoryData.Result != null)
                    {

                        EmrDetails = ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).EMRDetailsList;
                        if (EmrDetails.Count > 0)
                            IsEditRecord = true;

                        //Added By Saily P on 05.12.13 Purpose - New control.
                        if (((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).isBPControl == true)
                        {
                            objBPDetails = new clsBPControlVO();
                            objBPDetails = ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).objBPControl;
                            GetBPFromDB = true;
                        }
                        if (((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).isVisionControl == true)
                        {
                            objVisionDetails = new clsVisionVO();
                            objVisionDetails = ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).objVisionControl;
                            GetVisionFromDB = true;
                        }
                        if (((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).isGPControl == true)
                        {
                            objGlassPower = new clsGlassPowerVO();
                            objGlassPower = ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).objGPControl;
                            GetGPFromDB = true;
                        }

                        if (((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).EyeList != null && ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).EyeList.Count > 0)
                        {
                            EyeList = ((clsGetPatientDiagnosisEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).EyeList;
                        }

                        FillPhysicalExam();
                    }
                    else
                    {
                        FillPhysicalExam();
                    }
                }
                else
                {
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            };
            clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientBizActionObjPatientHistoryData.CloseAsync();

        }

        private void FillPhysicalExam()
        {
            //this.Cursor = Cursors.Wait;
            Indicatior.Show();
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                #region Physical Exam Details
                clsGetPatientPhysicalExamDetailsBizActionVO BizActionObjPatientPHEData = new clsGetPatientPhysicalExamDetailsBizActionVO();
                BizActionObjPatientPHEData.PatientID = 0;
                BizActionObjPatientPHEData.PatientUnitID = 0;
                BizActionObjPatientPHEData.TemplateID = TemplateID;
                BizActionObjPatientPHEData.VisitID = 0;
                BizActionObjPatientPHEData.UnitID = 0;

                PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
                {
                    if (argsBizActionObjPatientHistoryData.Error == null)
                    {
                        if (argsBizActionObjPatientHistoryData.Result != null && ((clsGetPatientPhysicalExamDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse != null)
                        {
                            IsFirstTime = true;
                            this.SelectedFormStructure = ((clsGetPatientPhysicalExamDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse.XmlDeserialize<FormDetail>();

                            IsView = IsFreeze;
                            GenratePreview();
                            MapRelations();
                            IsFirstTime = false;
                            IsSaved = true;
                            //IsEditRecord = isEditRecord;
                            Indicatior.Close();
                            GC.Collect();
                            this.Cursor = Cursors.Arrow;
                        }
                        else
                        {
                            //this.Cursor = Cursors.Arrow;
                            Indicatior.Close();
                            MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Template Not Found.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgbx.Show();
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                clientBizActionObjPatientHistoryData.ProcessAsync(BizActionObjPatientPHEData, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientBizActionObjPatientHistoryData.CloseAsync();

                #endregion
            }
            catch (Exception)
            {
                //this.Cursor = Cursors.Arrow;
                Indicatior.Close();
            }
        }

        private void MapRelations()
        {

            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.Relations != null && SelectedFormStructure.Relations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.Relations)
                {
                    FieldDetail source = null;
                    FieldDetail target = null;
                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.SourceSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.SourceFieldId)
                                {
                                    source = field;
                                    source.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (source != null && target != null)
                    {
                        item.SourceField = source;
                        item.TargetField = target;
                        item.SourceSection = source.Parent.Title;
                        item.TargetSection = target.Parent.Title;
                        if (((FrameworkElement)target.LabelControl) != null)
                        {
                            ((FrameworkElement)target.LabelControl).Visibility = ((FrameworkElement)target.Control).Visibility = Visibility.Collapsed;

                        }

                        target.RelationCondition = item.ExpCondition;
                        if (source.RelationalFieldList == null)
                            source.RelationalFieldList = new List<FieldDetail>();
                        source.RelationalFieldList.Add(target);
                        if (source.Settings is DecimalFieldSetting)
                        {
                            if (source.Control is StackPanel)
                            {
                                decUnit_LostFocus(((StackPanel)source.Control).Children[0], new RoutedEventArgs());
                            }
                            else
                            {
                                decUnit_LostFocus(source.Control, new RoutedEventArgs());
                            }
                        }
                        if (source.Settings is BooleanFieldSetting)
                        {
                            chk_Click(source.Control, new RoutedEventArgs());
                        }
                        if (source.Settings is ListFieldSetting)
                        {
                            if (source.Control is ComboBox)
                            {
                                cmbList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<DynamicListItem>(), new List<DynamicListItem>()));
                            }
                        }
                        if (source.Settings is AutomatedListFieldSetting)
                        {
                            if (((AutomatedListFieldSetting)source.Settings).ControlType == AutoListControlType.ComboBox)
                            {
                                AutoComboList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                            }
                            if (((AutomatedListFieldSetting)source.Settings).ControlType == AutoListControlType.ListBox)
                            {
                                if (((AutomatedListFieldSetting)source.Settings).ChoiceMode == SelectionMode.Single)
                                {
                                    AutoComboList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                                }
                                if (((AutomatedListFieldSetting)source.Settings).ChoiceMode == SelectionMode.Multiples)
                                {
                                    lbAutoList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                                }
                            }
                            if (((AutomatedListFieldSetting)source.Settings).ControlType == AutoListControlType.CheckListBox)
                            {
                                CLBCI_chkItemClicked(source.Control, new RoutedEventArgs());
                            }
                        }
                        if (source.Settings is InvestigationFieldSetting)
                        {
                            if (((InvestigationFieldSetting)source.Settings).ControlType == AutoListControlType.ComboBox)
                            {
                                InvestAutoComboList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                            }
                            if (((InvestigationFieldSetting)source.Settings).ControlType == AutoListControlType.ListBox)
                            {
                                if (((InvestigationFieldSetting)source.Settings).ChoiceMode == SelectionMode.Single)
                                {
                                    InvestAutoComboList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                                }
                                if (((InvestigationFieldSetting)source.Settings).ChoiceMode == SelectionMode.Multiples)
                                {
                                    lbInvestAutoList_SelectionChanged(source.Control, new SelectionChangedEventArgs(new List<MasterListItem>(), new List<MasterListItem>()));
                                }
                            }
                            if (((InvestigationFieldSetting)source.Settings).ControlType == AutoListControlType.CheckListBox)
                            {
                                InvestCLBCI_chkItemClicked(source.Control, new RoutedEventArgs());
                            }
                        }
                    }
                }
            }
        }

        private void MapPCR()
        {
            objPCR = null;
            objPCR = new clsPCRVO();
            PrescriptionList = null;
            PrescriptionList = new List<clsPatientPrescriptionDetailVO>();

            ServiceList = null;
            ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();

            EyeList = null;
            EyeList = new List<clsEyeControlVO>();

            objBPDetails = new clsBPControlVO();
            objVisionDetails = new clsVisionVO();
            objGlassPower = new clsGlassPowerVO();

            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.PCRRelations != null && SelectedFormStructure.PCRRelations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.PCRRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;

                        string tempStr = "";
                        DateTime? tempDt = null;
                        //Consultation
                        //Follow Up Consultation
                        //    Medication
                        if ((CurrentVisit.VisitTypeID == 1 && ((SectionDetail)target.Parent).Tab == "Consultation") || (CurrentVisit.VisitTypeID != 1 && ((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
                        {
                            switch (target.DataType.Id)
                            {
                                case 1:
                                    tempStr = ((TextFieldSetting)target.Settings).Value;
                                    if (source.ID == 6 && tempStr != null && tempStr != "")
                                    {
                                        ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                        {
                                            ServiceID = 0,
                                            ServiceName = tempStr,
                                            IsOther = true,
                                            Reason = "",
                                        });
                                    }
                                    break;
                                case 2:
                                    if (((BooleanFieldSetting)target.Settings).Value && ((FrameworkElement)target.Control).Visibility == Visibility.Visible)
                                    {
                                        tempStr = target.Title;
                                        if (source.ID == 6 && tempStr != null && tempStr != "")
                                        {
                                            ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                            {
                                                ServiceID = 0,
                                                ServiceName = tempStr,
                                                IsOther = true,
                                                Reason = "",
                                            });
                                        }
                                    }
                                    break;
                                case 3:
                                    tempDt = ((DateFieldSetting)target.Settings).Date;
                                    break;
                                case 4:
                                    ListFieldSetting listSetting = (ListFieldSetting)target.Settings;
                                    switch (listSetting.ChoiceMode)
                                    {
                                        case SelectionMode.Single:
                                            switch (listSetting.ControlType)
                                            {
                                                case ListControlType.ComboBox:
                                                    if (listSetting.SelectedItem != null)
                                                        tempStr = ((DynamicListItem)listSetting.SelectedItem).Title;
                                                    break;
                                            }
                                            break;
                                        case SelectionMode.Multiples:
                                            if (listSetting.SelectedItems != null && listSetting.SelectedItems.Count > 0)
                                            {
                                                foreach (var ob in listSetting.SelectedItems)
                                                {
                                                    if (ob != null)
                                                        tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + ((DynamicListItem)listSetting.SelectedItem).Title;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case 5:
                                    if (((DecimalFieldSetting)target.Settings).Value != null)
                                    {
                                        tempStr = target.Title + ": " + ((DecimalFieldSetting)target.Settings).Value.ToString();
                                        tempStr = ((DecimalFieldSetting)target.Settings).Unit != null ? tempStr + " " + ((DecimalFieldSetting)target.Settings).Unit : tempStr;
                                    }
                                    else
                                    {
                                        tempStr = target.Title + ":N/A";
                                    }

                                    break;
                                case 6:
                                    tempStr = ((HyperlinkFieldSetting)target.Settings).Url;
                                    break;
                                case 7:
                                    //Header
                                    break;
                                case 8:
                                    LookUpFieldSetting LookUpSetting = ((LookUpFieldSetting)target.Settings);

                                    switch (LookUpSetting.ChoiceMode)
                                    {
                                        case SelectionMode.Single:
                                            if (LookUpSetting.SelectedItem != null)
                                            {
                                                tempStr = ((DynamicListItem)LookUpSetting.SelectedItem).Title;
                                            }
                                            if (LookUpSetting.IsAlternateText == true && LookUpSetting.AlternateText != null && LookUpSetting.AlternateText != "")
                                                tempStr = (tempStr == "" ? "" : (tempStr + " OR ")) + LookUpSetting.AlternateText;
                                            break;
                                    }
                                    break;
                                case 9:
                                    MedicationFieldSetting medSetting = ((MedicationFieldSetting)target.Settings);
                                    for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                                    {
                                        if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                        {
                                            PrescriptionList.Add(new clsPatientPrescriptionDetailVO()
                                            {
                                                DrugID = (((Medication)medSetting.ItemsSource[i]).Drug.ID),
                                                Dose = (((Medication)medSetting.ItemsSource[i]).Dose),
                                                Route = ((Medication)medSetting.ItemsSource[i]).Route == null ? "" : ((Medication)medSetting.ItemsSource[i]).Route.Description,
                                                Frequency = ((Medication)medSetting.ItemsSource[i]).Frequency.ToString(),
                                                Days = (((Medication)medSetting.ItemsSource[i]).Day),
                                                Quantity = (((Medication)medSetting.ItemsSource[i]).Quantity),
                                                IsOther = false,
                                            });
                                        }
                                    }
                                    break;
                                case 10:
                                    // Follow Up Medaication
                                    break;
                                case 11:
                                    OtherInvestigationFieldSetting InvestSetting = (OtherInvestigationFieldSetting)target.Settings;
                                    for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
                                    {
                                        if (((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation != null && ((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation != "--Select--")
                                        {
                                            tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + ((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation.ToString();
                                            if (source.ID == 6 && tempStr != null && tempStr != "")
                                            {
                                                ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                                {
                                                    ServiceID = 0,
                                                    ServiceName = tempStr,
                                                    IsOther = true,
                                                    Reason = "",
                                                });
                                            }
                                        }
                                    }
                                    break;
                                case 12:
                                    ListOfCheckBoxesFieldSetting listSetting1 = (ListOfCheckBoxesFieldSetting)target.Settings;
                                    for (int i = 0; i < listSetting1.ItemsSource.Count; i++)
                                    {
                                        if (listSetting1.SelectedItems[i])
                                        {
                                            tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + (listSetting1.ItemsSource[i]).ToString();

                                            if (listSetting1.ListType == "Nutrition List" && (listSetting1.ItemsSource[i]) == "Other" && listSetting1.OtherText != null)
                                                tempStr = tempStr + ":" + listSetting1.OtherText;
                                        }
                                    }
                                    break;
                                case 13:
                                    AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)target.Settings);
                                    switch (AutolistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (AutolistSetting.SelectedItem != null)
                                            {
                                                tempStr = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (AutolistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (AutolistSetting.SelectedItem != null)
                                                    {
                                                        tempStr = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                    {
                                                        foreach (var obList in AutolistSetting.SelectedItems)
                                                            tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + obList.Description.ToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)AutolistSetting.ItemSource[k]).Status)
                                                {
                                                    tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + ((MasterListItem)AutolistSetting.ItemSource[k]).Description.ToString();
                                                }
                                            }
                                            break;
                                    }
                                    break;

                                //added by Saily P on 29.11.13 Purpose - Added new control
                                case 18:
                                    IsBPControl = true;
                                    BPControlSetting BPSetting = ((BPControlSetting)target.Settings);
                                    for (int i = 0; i < BPSetting.ItemsSource.Count; i++)
                                    {
                                        if (((BPControl)BPSetting.ItemsSource[i]).BP1 != null)
                                        {
                                            objBPDetails = new clsBPControlVO()
                                            {
                                                BP1 = ((BPControl)BPSetting.ItemsSource[i]).BP1,
                                                BP2 = ((BPControl)BPSetting.ItemsSource[i]).BP2,
                                            };
                                        }
                                    }
                                    break;
                                case 19:
                                    IsVisionControl = true;
                                    VisionControlSetting VisionSetting = ((VisionControlSetting)target.Settings);
                                    for (int i = 0; i < VisionSetting.ItemsSource.Count; i++)
                                    {
                                        if (((VisionControl)VisionSetting.ItemsSource[i]).SPH != null)
                                        {
                                            objVisionDetails = new clsVisionVO()
                                            {
                                                SPH = ((VisionControl)VisionSetting.ItemsSource[i]).SPH,
                                                CYL = ((VisionControl)VisionSetting.ItemsSource[i]).CYL,
                                                Axis = ((VisionControl)VisionSetting.ItemsSource[i]).Axis,
                                            };
                                        }
                                    }
                                    break;
                                case 20:
                                    IsGPControl = true;
                                    GlassPowerControlSetting GPSetting = ((GlassPowerControlSetting)target.Settings);
                                    for (int i = 0; i < GPSetting.ItemsSource.Count; i++)
                                    {
                                        if (((GlassPower)GPSetting.ItemsSource[i]).SPH1 != null)
                                        {
                                            objGlassPower = new clsGlassPowerVO()
                                            {
                                                SPH1 = ((GlassPower)GPSetting.ItemsSource[i]).SPH1,
                                                CYL1 = ((GlassPower)GPSetting.ItemsSource[i]).CYL1,
                                                Axis1 = ((GlassPower)GPSetting.ItemsSource[i]).Axis1,
                                                VA1 = ((GlassPower)GPSetting.ItemsSource[i]).VA1,
                                                SPH2 = ((GlassPower)GPSetting.ItemsSource[i]).SPH2,
                                                CYL2 = ((GlassPower)GPSetting.ItemsSource[i]).CYL2,
                                                Axis2 = ((GlassPower)GPSetting.ItemsSource[i]).Axis2,
                                                VA2 = ((GlassPower)GPSetting.ItemsSource[i]).VA2,
                                            };
                                        }
                                    }
                                    break;
                                //
                                case 14:
                                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)target.Settings);
                                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                                    {
                                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null)
                                        {
                                            PrescriptionList.Add(new clsPatientPrescriptionDetailVO()
                                            {
                                                DrugID = 0,
                                                DrugName = ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug.ToString(),
                                                Dose = (((OtherMedication)OthermedSetting.ItemsSource[i]).Dose),
                                                Route = ((OtherMedication)OthermedSetting.ItemsSource[i]).Route == null ? "" : ((OtherMedication)OthermedSetting.ItemsSource[i]).Route.Description,
                                                Frequency = ((OtherMedication)OthermedSetting.ItemsSource[i]).Frequency.ToString(),
                                                Days = (((OtherMedication)OthermedSetting.ItemsSource[i]).Day),
                                                Quantity = (((OtherMedication)OthermedSetting.ItemsSource[i]).Quantity),
                                                IsOther = true,
                                                Reason = (((OtherMedication)OthermedSetting.ItemsSource[i]).Reason),
                                            });
                                        }
                                    }
                                    break;
                                case 15:
                                    InvestigationFieldSetting InvestlistSetting = ((InvestigationFieldSetting)target.Settings);
                                    switch (InvestlistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (InvestlistSetting.SelectedItem != null)
                                            {
                                                tempStr = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString();
                                                ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                                {
                                                    ServiceID = ((MasterListItem)InvestlistSetting.SelectedItem).ID,
                                                    ServiceName = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString(),
                                                    IsOther = false,
                                                    Reason = "",
                                                });
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (InvestlistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (InvestlistSetting.SelectedItem != null)
                                                    {
                                                        tempStr = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString();
                                                        ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                                        {
                                                            ServiceID = ((MasterListItem)InvestlistSetting.SelectedItem).ID,
                                                            ServiceName = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString(),
                                                            IsOther = false,
                                                            Reason = "",
                                                        });
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (InvestlistSetting.SelectedItems != null && InvestlistSetting.SelectedItems.Count > 0)
                                                    {
                                                        foreach (var obList in InvestlistSetting.SelectedItems)
                                                        {
                                                            tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + obList.Description.ToString();
                                                            ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                                            {
                                                                ServiceID = obList.ID,
                                                                ServiceName = obList.Description.ToString(),
                                                                IsOther = false,
                                                                Reason = "",
                                                            });
                                                        }
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < InvestlistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)InvestlistSetting.ItemSource[k]).Status)
                                                {
                                                    tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + ((MasterListItem)InvestlistSetting.ItemSource[k]).Description.ToString();
                                                    ServiceList.Add(new clsDoctorSuggestedServiceDetailVO()
                                                    {
                                                        ServiceID = ((MasterListItem)InvestlistSetting.ItemSource[k]).ID,
                                                        ServiceName = ((MasterListItem)InvestlistSetting.ItemSource[k]).Description.ToString(),
                                                        IsOther = false,
                                                        Reason = "",
                                                    });
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        switch (source.ID)
                        {
                            case 1:
                                objPCR.ComplaintReported = ((objPCR.ComplaintReported == null || objPCR.ComplaintReported == "") ? "" : (objPCR.ComplaintReported + ", ")) + tempStr;
                                break;
                            case 2:
                                objPCR.ChiefComplaints = ((objPCR.ChiefComplaints == null || objPCR.ChiefComplaints == "") ? "" : (objPCR.ChiefComplaints + ", ")) + tempStr;
                                break;
                            case 3:
                                objPCR.PastHistory = ((objPCR.PastHistory == null || objPCR.PastHistory == "") ? "" : (objPCR.PastHistory + ", ")) + tempStr;
                                break;
                            case 4:
                                objPCR.DrugHistory = ((objPCR.DrugHistory == null || objPCR.DrugHistory == "") ? "" : (objPCR.DrugHistory + ", ")) + tempStr;
                                break;
                            case 5:
                                objPCR.Allergies = ((objPCR.Allergies == null || objPCR.Allergies == "") ? "" : (objPCR.Allergies + ", ")) + tempStr;
                                break;
                            case 6:
                                objPCR.Investigations = ((objPCR.Investigations == null || objPCR.Investigations == "") ? "" : (objPCR.Investigations + ", ")) + tempStr;
                                break;
                            case 7:
                                objPCR.PovisionalDiagnosis = ((objPCR.PovisionalDiagnosis == null || objPCR.PovisionalDiagnosis == "") ? "" : (objPCR.PovisionalDiagnosis + ", ")) + tempStr;
                                break;
                            case 8:
                                objPCR.FinalDiagnosis = ((objPCR.FinalDiagnosis == null || objPCR.FinalDiagnosis == "") ? "" : (objPCR.FinalDiagnosis + ", ")) + tempStr;
                                break;
                            case 9:
                                objPCR.Hydration = ((objPCR.Hydration == null || objPCR.Hydration == "") ? "" : (objPCR.Hydration + ", ")) + tempStr;
                                break;
                            case 10:
                                objPCR.IVHydration4 = ((objPCR.IVHydration4 == null || objPCR.IVHydration4 == "") ? "" : (objPCR.IVHydration4 + ", ")) + tempStr;
                                break;
                            case 11:
                                objPCR.ZincSupplement = ((objPCR.ZincSupplement == null || objPCR.ZincSupplement == "") ? "" : (objPCR.ZincSupplement + ", ")) + tempStr;
                                break;
                            case 12:
                                objPCR.Nutritions = ((objPCR.Nutritions == null || objPCR.Nutritions == "") ? "" : (objPCR.Nutritions + ", ")) + tempStr;
                                break;
                            case 13:
                                //Medication
                                break;
                            case 14:
                                objPCR.AdvisoryAttached = ((objPCR.AdvisoryAttached == null || objPCR.AdvisoryAttached == "") ? "" : (objPCR.AdvisoryAttached + ", ")) + tempStr;
                                break;
                            case 15:
                                objPCR.WhenToVisitHospital = ((objPCR.WhenToVisitHospital == null || objPCR.WhenToVisitHospital == "") ? "" : (objPCR.WhenToVisitHospital + ", ")) + tempStr;
                                break;
                            case 16:
                                objPCR.SpecificInstructions = ((objPCR.SpecificInstructions == null || objPCR.SpecificInstructions == "") ? "" : (objPCR.SpecificInstructions + ", ")) + tempStr;
                                break;
                            case 17:
                                if (tempDt != null)
                                    objPCR.FollowUpDate = tempDt;
                                break;
                            case 18:
                                objPCR.FollowUpAt = ((objPCR.FollowUpAt == null || objPCR.FollowUpAt == "") ? "" : (objPCR.FollowUpAt + ", ")) + tempStr;
                                break;
                            case 19:
                                objPCR.ReferralTo = ((objPCR.ReferralTo == null || objPCR.ReferralTo == "") ? "" : (objPCR.ReferralTo + ", ")) + tempStr;
                                break;
                        }
                    }
                }
            }


        }

        private void GetHistoryMedication()
        {
            PrescriptionList = null;
            PrescriptionList = new List<clsPatientPrescriptionDetailVO>();

            if (SelectedHistoryStructure != null && SelectedHistoryStructure.SectionList != null)
            {
                FieldDetail target = null;
                foreach (var section in SelectedHistoryStructure.SectionList)
                {
                    foreach (var field in section.FieldList)
                    {
                        target = field;
                        target.Parent = section;
                        if (target != null)
                        {

                            if ((((SectionDetail)target.Parent).Tab == "History"))
                            {
                                switch (target.DataType.Id)
                                {

                                    case 9:
                                        MedicationFieldSetting medSetting = ((MedicationFieldSetting)target.Settings);
                                        for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                                        {
                                            if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                            {
                                                PrescriptionList.Add(new clsPatientPrescriptionDetailVO()
                                                {
                                                    DrugID = (((Medication)medSetting.ItemsSource[i]).Drug.ID),
                                                    DrugName = (((Medication)medSetting.ItemsSource[i]).Drug.Description),
                                                    Dose = (((Medication)medSetting.ItemsSource[i]).Dose),
                                                    Route = ((Medication)medSetting.ItemsSource[i]).Route == null ? "" : ((Medication)medSetting.ItemsSource[i]).Route.Description,
                                                    Frequency = ((Medication)medSetting.ItemsSource[i]).Frequency.ToString(),
                                                    Days = (((Medication)medSetting.ItemsSource[i]).Day),
                                                    Quantity = (((Medication)medSetting.ItemsSource[i]).Quantity),
                                                    IsOther = false,
                                                    FromHistory = true,
                                                    SelectedRoute = ((Medication)medSetting.ItemsSource[i]).Route,

                                                });
                                            }
                                        }
                                        break;

                                    case 14:
                                        OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)target.Settings);
                                        for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                                        {
                                            if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null)
                                            {
                                                PrescriptionList.Add(new clsPatientPrescriptionDetailVO()
                                                {
                                                    DrugID = 0,
                                                    DrugName = ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug.ToString(),
                                                    Dose = (((OtherMedication)OthermedSetting.ItemsSource[i]).Dose),
                                                    Route = ((OtherMedication)OthermedSetting.ItemsSource[i]).Route == null ? "" : ((OtherMedication)OthermedSetting.ItemsSource[i]).Route.Description,
                                                    Frequency = ((OtherMedication)OthermedSetting.ItemsSource[i]).Frequency.ToString(),
                                                    Days = (((OtherMedication)OthermedSetting.ItemsSource[i]).Day),
                                                    Quantity = (((OtherMedication)OthermedSetting.ItemsSource[i]).Quantity),
                                                    IsOther = true,
                                                    FromHistory = true,
                                                    Reason = (((OtherMedication)OthermedSetting.ItemsSource[i]).Reason),
                                                });
                                            }
                                        }
                                        break;




                                }
                            }
                        }

                    }
                }
            }
        }

        private void MapCaseReferral()
        {
            objCaseReferral = null;
            objCaseReferral = new clsCaseReferralVO();
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.CaseReferralRelations != null && SelectedFormStructure.CaseReferralRelations.Count > 0)
            {
                foreach (var item in SelectedFormStructure.CaseReferralRelations)
                {
                    MasterListItem source = null;
                    FieldDetail target = null;
                    source = item.SourceField;

                    foreach (var section in SelectedFormStructure.SectionList)
                    {
                        if (section.UniqueId.ToString() == item.TargetSectionId)
                        {
                            foreach (var field in section.FieldList)
                            {
                                if (field.UniqueId.ToString() == item.TargetFieldId)
                                {
                                    target = field;
                                    target.Parent = section;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (source != null && target != null)
                    {
                        item.TargetField = target;
                        item.TargetSection = target.Parent.Title;

                        string tempStr = "";
                        DateTime? tempDt = null;
                        long tempId = 0;

                        if ((CurrentVisit.VisitTypeID == 1 && ((SectionDetail)target.Parent).Tab == "Consultation") || (CurrentVisit.VisitTypeID != 1 && ((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
                        {
                            switch (target.DataType.Id)
                            {
                                case 1:
                                    tempStr = ((TextFieldSetting)target.Settings).Value;
                                    break;
                                case 2:
                                    if (((BooleanFieldSetting)target.Settings).Value && ((FrameworkElement)target.Control).Visibility == Visibility.Visible)
                                        tempStr = target.Title;
                                    break;
                                case 3:
                                    tempDt = ((DateFieldSetting)target.Settings).Date;
                                    break;
                                case 4:
                                    ListFieldSetting listSetting = (ListFieldSetting)target.Settings;
                                    switch (listSetting.ChoiceMode)
                                    {
                                        case SelectionMode.Single:
                                            switch (listSetting.ControlType)
                                            {
                                                case ListControlType.ComboBox:
                                                    if (listSetting.SelectedItem != null)
                                                        tempStr = ((DynamicListItem)listSetting.SelectedItem).Title;
                                                    break;
                                            }
                                            break;
                                        case SelectionMode.Multiples:
                                            if (listSetting.SelectedItems != null && listSetting.SelectedItems.Count > 0)
                                            {
                                                foreach (var ob in listSetting.SelectedItems)
                                                {
                                                    if (ob != null)
                                                        tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + ((DynamicListItem)listSetting.SelectedItem).Title;
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case 5:
                                    if (((DecimalFieldSetting)target.Settings).Value != null)
                                    {
                                        tempStr = target.Title + ": " + ((DecimalFieldSetting)target.Settings).Value.ToString();
                                        tempStr = ((DecimalFieldSetting)target.Settings).Unit != null ? tempStr + " " + ((DecimalFieldSetting)target.Settings).Unit : tempStr;
                                    }
                                    else
                                    {
                                        tempStr = target.Title + ":N/A";
                                    }
                                    break;
                                case 6:
                                    tempStr = ((HyperlinkFieldSetting)target.Settings).Url;
                                    break;
                                case 7:
                                    //Header
                                    break;
                                case 8:
                                    LookUpFieldSetting LookUpSetting = ((LookUpFieldSetting)target.Settings);

                                    switch (LookUpSetting.ChoiceMode)
                                    {
                                        case SelectionMode.Single:
                                            if (LookUpSetting.SelectedItem != null)
                                            {
                                                tempStr = ((DynamicListItem)LookUpSetting.SelectedItem).Title;
                                            }
                                            if (LookUpSetting.IsAlternateText == true && LookUpSetting.AlternateText != null && LookUpSetting.AlternateText != "")
                                                tempStr = (tempStr == "" ? "" : (tempStr + " OR ")) + LookUpSetting.AlternateText;
                                            break;
                                    }
                                    break;
                                case 9:
                                    //Medication
                                    break;
                                case 10:
                                    // Follow Up Medaication
                                    break;
                                case 11:
                                    OtherInvestigationFieldSetting InvestSetting = (OtherInvestigationFieldSetting)target.Settings;
                                    for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
                                    {
                                        if (((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation != null && ((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation != "--Select--")
                                        {
                                            tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + ((OtherInvestigation)InvestSetting.ItemsSource[i]).Investigation.ToString();
                                        }
                                    }
                                    break;
                                case 12:
                                    ListOfCheckBoxesFieldSetting listSetting1 = (ListOfCheckBoxesFieldSetting)target.Settings;
                                    for (int i = 0; i < listSetting1.ItemsSource.Count; i++)
                                    {
                                        if (listSetting1.SelectedItems[i])
                                        {
                                            tempStr = (tempStr == "" ? "" : (tempStr + ",\t")) + (listSetting1.ItemsSource[i]).ToString();
                                        }
                                    }
                                    break;
                                case 13:
                                    AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)target.Settings);
                                    switch (AutolistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (AutolistSetting.SelectedItem != null)
                                            {
                                                tempStr = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                tempId = ((MasterListItem)AutolistSetting.SelectedItem).ID;
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (AutolistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (AutolistSetting.SelectedItem != null)
                                                    {
                                                        tempStr = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                        tempId = ((MasterListItem)AutolistSetting.SelectedItem).ID;
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                    {
                                                        foreach (var obList in AutolistSetting.SelectedItems)
                                                            tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + obList.Description.ToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)AutolistSetting.ItemSource[k]).Status)
                                                {
                                                    tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + ((MasterListItem)AutolistSetting.ItemSource[k]).Description.ToString();
                                                }
                                            }
                                            break;
                                    }
                                    break;
                                case 14:
                                    // Other Medication
                                    break;
                                case 15:
                                    InvestigationFieldSetting InvestlistSetting = ((InvestigationFieldSetting)target.Settings);
                                    switch (InvestlistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (InvestlistSetting.SelectedItem != null)
                                            {
                                                tempStr = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString();
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (InvestlistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (InvestlistSetting.SelectedItem != null)
                                                    {
                                                        tempStr = ((MasterListItem)InvestlistSetting.SelectedItem).Description.ToString();
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (InvestlistSetting.SelectedItems != null && InvestlistSetting.SelectedItems.Count > 0)
                                                    {
                                                        foreach (var obList in InvestlistSetting.SelectedItems)
                                                            tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + obList.Description.ToString();
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < InvestlistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)InvestlistSetting.ItemSource[k]).Status)
                                                {
                                                    tempStr = (tempStr == "" ? "" : (tempStr + ", ")) + ((MasterListItem)InvestlistSetting.ItemSource[k]).Description.ToString();
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                        switch (source.ID)
                        {
                            case 1:
                                objCaseReferral.ReferredToDoctorID = tempId;//((objPCR.ComplaintReported == null || objPCR.ComplaintReported == "") ? "" : (objPCR.ComplaintReported + ", ")) + tempStr;
                                break;
                            case 2:
                                objCaseReferral.ReferredToClinicID = tempId;//((objPCR.ChiefComplaints == null || objPCR.ChiefComplaints == "") ? "" : (objPCR.ChiefComplaints + ", ")) + tempStr;
                                break;
                            case 3:
                                objCaseReferral.ReferredToMobile = ((objCaseReferral.ReferredToMobile == null || objCaseReferral.ReferredToMobile == "") ? "" : (objCaseReferral.ReferredToMobile + ", ")) + tempStr;
                                break;
                            case 4:
                                objCaseReferral.ProvisionalDiagnosis = ((objCaseReferral.ProvisionalDiagnosis == null || objCaseReferral.ProvisionalDiagnosis == "") ? "" : (objCaseReferral.ProvisionalDiagnosis + ", ")) + tempStr;
                                break;
                            case 5:
                                objCaseReferral.ChiefComplaints = ((objCaseReferral.ChiefComplaints == null || objCaseReferral.ChiefComplaints == "") ? "" : (objCaseReferral.ChiefComplaints + ", ")) + tempStr;
                                break;
                            case 6:
                                objCaseReferral.Summary = ((objCaseReferral.Summary == null || objCaseReferral.Summary == "") ? "" : (objCaseReferral.Summary + ", ")) + tempStr;
                                break;
                            case 7:
                                objCaseReferral.Observations = ((objCaseReferral.Observations == null || objCaseReferral.Observations == "") ? "" : (objCaseReferral.Observations + ", ")) + tempStr;
                                break;
                        }
                    }
                }
            }
        }

        private void GenratePreview()
        {
            Form.RowDefinitions.Clear();
            Form.Children.Clear();

            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null)
            {
                foreach (var item in SelectedFormStructure.SectionList)
                {
                    AddNodeItems(item);
                }
            }

        }

        bool WritePerm = true;
        public void AddNodeItems(SectionDetail PItem)
        {
            RowDefinition Row = new RowDefinition();

            Form.RowDefinitions.Add(Row);

            Grid section = GetSectionLayout(PItem.Title);

            if (PItem.IsToolTip == true)
            {
                ((Border)section.Children[1]).DataContext = PItem.ToolTipText;
                ((Border)section.Children[1]).MouseEnter += new MouseEventHandler(FormDesigner_MouseEnter);
            }

            if (SelectedUser != null || SelectedUser != "")
            {
                int i = 0;
                bool user = false;
                while (i < PItem.ReadPermission.Count)
                {
                    if (SelectedUser == PItem.ReadPermission[i])
                    {
                        user = true;
                    }
                    i++;
                }


                if (PItem.Tab == "Consultation")
                {
                    if (user == true)
                    {
                        section.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        section.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    section.Visibility = Visibility.Collapsed;
                }

                section.Visibility = Visibility.Visible;
                WritePerm = false;
                i = 0;
                while (i < PItem.ReadWritePermission.Count)
                {
                    if (SelectedUser == PItem.ReadWritePermission[i])
                    {
                        WritePerm = true;
                    }
                    i++;
                }

                // Disable Consultation controls when VisitType is follow up

                if (CurrentVisit.VisitTypeID != 1 && PItem.Tab == "Consultation")
                {
                    WritePerm = true;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == false || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID != CurrentVisit.UnitId)
                        WritePerm = true;
                }
            }

            Grid container = (Grid)((Border)section.Children[0]).Child;

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(200, GridUnitType.Auto);
            ColumnDefinition column2 = new ColumnDefinition();
            container.ColumnDefinitions.Add(column1);
            container.ColumnDefinitions.Add(column2);
            Grid.SetRow(section, Form.RowDefinitions.Count - 1);
            Form.Children.Add(section);

            if (PItem.FieldList != null)
                foreach (var item in PItem.FieldList)
                {
                    item.Parent = PItem;
                    AddNodeItems(container, item, false);
                }
        }

        void FormDesigner_MouseEnter(object sender, MouseEventArgs e)
        {
            if (p != null)
            {
                if (!p.IsOpen)
                {
                    //((Border)((ScrollViewer)p.Child).Content).Child = null;
                    TextBox tbl = new TextBox();
                    tbl.IsEnabled = false;
                    //tbl.Background = new SolidColorBrush(Colors.Yellow);
                    tbl.Text = ((FrameworkElement)sender).DataContext.ToString();
                    tbl.TextWrapping = TextWrapping.Wrap;
                    ((Border)((ScrollViewer)p.Child).Content).Child = tbl;
                    //((Border)((ScrollViewer)p.Child).Content).Child = (TextBox)((Border)sender).DataContext;
                    GeneralTransform gt = ((Border)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = gt.Transform(new Point(0, 0));
                    double controlTop = offset.Y + ((Border)sender).ActualHeight;
                    double controlLeft = offset.X;
                    ((ScrollViewer)p.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                    //((ScrollViewer)p.Child).MaxHeight = this.ActualHeight - controlTop + 10;
                    ((ScrollViewer)p.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop - 10;

                    p.VerticalOffset = controlTop;
                    p.HorizontalOffset = controlLeft;
                    p.IsOpen = true;
                }
            }
        }

        bool ForFlag = false;
        bool FrequencyFlag = false;
        public void AddNodeItems(Grid Container, FieldDetail PItem, bool IdDependentField)
        {
            RowDefinition Row = new RowDefinition();
            Row.Height = new GridLength(23, GridUnitType.Auto);
            Container.RowDefinitions.Add(Row);
            TextBlock FTitle = new TextBlock();
            FTitle.Tag = PItem.DataType.Id;
            FTitle.HorizontalAlignment = HorizontalAlignment.Right;
            FTitle.VerticalAlignment = VerticalAlignment.Center;
            FTitle.Margin = new Thickness(2);
            FTitle.Text = PItem.Title + (string.IsNullOrEmpty(PItem.Title) ? "" : " : ");
            if (PItem.DataType.Id != 7)
            {
                Grid.SetRow(FTitle, Container.RowDefinitions.Count - 1);
                Container.Children.Add(FTitle);
            }
            PItem.LabelControl = FTitle;
            switch (PItem.DataType.Id)
            {
                case 1:
                    #region Case 1
                    TextBox Field = new TextBox();
                    if (PItem.IsToolTip == true)
                    {
                        Field.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }
                    Field.IsEnabled = WritePerm;

                    if (TemplateID == 2)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Color/Consistency")
                        {
                            Field.Name = "ColorConsistency";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "Provisional Diagnosis")
                        {
                            Field.Name = "RMProvisionalDiagnosis";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "Chief Complaints")
                        {
                            Field.Name = "RMChiefComplaints";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "Specific Request")
                        {
                            Field.Name = "RMSpecificRequest";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Patient Complaint as reported")
                        {
                            Field.Name = "ComplaintReported";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Past Medical History" && PItem.Title == "Past Medical History")
                        {
                            Field.Name = "PastMedicalHistory";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Past Medical History" && PItem.Title == "Drug History")
                        {
                            Field.Name = "DrugHistory";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Examination" && PItem.Title == "Final Diagnosis")
                        {
                            Field.Name = "FinalDiagnosis";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional diagnosis summary" && PItem.Title == "Provisional Diagnosis")
                        {
                            Field.Name = "FollowUpProvisionalDiagnosis";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Status/Current Complaints" && PItem.Title == "Current Complaints")
                        {
                            Field.Name = "RCurrentComplaints";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Examination" && PItem.Title == "Examination")
                        {
                            Field.Name = "RExamination";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Follow up Management" && PItem.Title == "for")
                        {
                            if (!ForFlag)
                            {
                                Field.Name = "FUMFor";
                            }
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Follow up Management" && PItem.Title == "Instructions")
                        {
                            if (!ForFlag)
                            {
                                Field.Name = "FUMInstructions";
                                ForFlag = true;
                            }
                        }
                    }
                    else if (TemplateID == 4)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Other tests ")
                        {
                            Field.Name = "InvestOtherTests";
                        }
                    }
                    else if (TemplateID == 5)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Other tests")
                        {
                            Field.Name = "InvestOtherTests";
                        }
                    }
                    else if (TemplateID == 6)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Other investigations")
                        {
                            Field.Name = "InvestOtherinvestigations";
                        }
                    }
                    else if (TemplateID == 7)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Other tests")
                        {
                            Field.Name = "InvestOtherTests";
                        }
                    }


                    Field.LostFocus += new RoutedEventHandler(TextField_LostFocus);
                    Field.GotFocus += new RoutedEventHandler(TextField_GotFocus);
                    Field.DataContext = PItem;
                    Binding binding = new Binding("Settings.Value");
                    binding.Mode = BindingMode.TwoWay;
                    Field.SetBinding(TextBox.TextProperty, binding);
                    Field.Margin = new Thickness(2);
                    if (IsView == true)
                    {
                        Field.IsReadOnly = true;
                    }
                    else
                    {
                        Field.IsReadOnly = false;
                    }
                    Grid.SetRow(Field, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(Field, 1);
                    if (!(((TextFieldSetting)PItem.Settings).Mode))
                    {

                        Field.AcceptsReturn = true;
                        Field.Height = 60;
                        Field.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    }
                    if (IdDependentField)
                        FTitle.Visibility = Field.Visibility = Visibility.Collapsed;
                    var item1 = from EDT in EmrDetails
                                where (EDT.ControlName == PItem.Name
                                )
                                select EDT;
                    if (item1.ToList().Count > 0)
                    {
                        foreach (var item in item1)
                        {
                            if (item.ControlName == PItem.Name)
                            {
                                Field.Text = item.Value;
                            }
                        }
                    }
                    PItem.Control = Field;
                    Container.Children.Add(Field);
                    break;
                    #endregion
                case 2:
                    #region Case 2 Main
                    if ((((BooleanFieldSetting)PItem.Settings).Mode))
                    {
                        CheckBox chk = new CheckBox();
                        #region Added By Harish
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(chk, tt);
                            #region new added by harish
                            chk.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #endregion


                        if (TemplateID == 2)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "Stool M/E")
                            {
                                chk.Name = "StoolME";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "Stool Culture")
                            {
                                chk.Name = "StoolCulture";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "CBC")
                            {
                                chk.Name = "CBC";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "BUN Creatinine")
                            {
                                chk.Name = "BUNCreatinine";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "Serum electrolytes")
                            {
                                chk.Name = "SerumElectro";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "X-Ray chest")
                            {
                                chk.Name = "XRayChest";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "Blood gases")
                            {
                                chk.Name = "BloodGases";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigation" && PItem.Title == "Ova / parasite test")
                            {
                                chk.Name = "OvaParasiteTest";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Observation Protocol")
                            {
                                chk.Name = "EMObservationProtocol";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "ORS advice")
                            {
                                chk.Name = "EMORSAdvice";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Nutrition advice")
                            {
                                chk.Name = "EMNutritionAdvice";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit clinic")
                            {
                                chk.Name = "EMVisitClinic";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit hospital")
                            {
                                chk.Name = "EMVisitHospital";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Tests conducted")
                            {
                                chk.Name = "EMTestsConducted";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Medication explained")
                            {
                                chk.Name = "EMMedicationExplained";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up visit discussed")
                            {
                                chk.Name = "EMFollowUpVisitDiscussed";
                            }
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "follow up/telephonic follow up scheduled")
                            {
                                chk.Name = "EMFollowUpScheduled";
                            }
                        }
                        else if (TemplateID == 4)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentation" && PItem.Title == "Heart burn")
                            {
                                chk.Name = "PrsntHeartBurn";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Cough with GERD")
                            {
                                chk.Name = "PDCoughWithGERD";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Acute cough")
                            {
                                chk.Name = "PDAcuteCough";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Persistent cough")
                            {
                                chk.Name = "PDPersistentCough";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Chronic cough ")
                            {
                                chk.Name = "PDChronicCough";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Cough with rhinitis")
                            {
                                chk.Name = "PDCoughWithRhinitis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Cough with post nasal drips")
                            {
                                chk.Name = "PDCoughWithPostNasalDrips";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Cough with chest infection")
                            {
                                chk.Name = "PDCoughWithChestInfection";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Cough with bronchitis/ asthma/COPD/pneumoconiosis ")
                            {
                                chk.Name = "PDCoughWithBronchitisAsthmaCOPDpneumoconiosis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Tropical Eosinophilia ")
                            {
                                chk.Name = "PDTropicalEosinophilia";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis " && PItem.Title == "Tuberculosis ")
                            {
                                chk.Name = "PDTuberculosis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Runny nose / Rhinitis")
                            {
                                chk.Name = "HistRunnyNoseRhinitis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == " Post nasal drip")
                            {
                                chk.Name = "HistPostNasalDrip";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Risk Factors" && PItem.Title == "Cardiac problems (CHF)")
                            {
                                chk.Name = "RiskCardiacProblemsCHF";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Persistent cough in patients with co-existing cardiovascular disease")
                            {
                                chk.Name = "AlarmPersistentCoughCardiovascularDisease";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Respiratory ")
                            {
                                chk.Name = "PERespiratory";
                            }

                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Sinus imaging")
                            {
                                chk.Name = "InvestSinusImaging";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Pertussis infection nasopharyngeal culture ")
                            {
                                chk.Name = "InvestPertussisInfectionNasopharyngealCulture";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Sputum culture & sensitivity")
                            {
                                chk.Name = "InvestSputumCultureSensitivity";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Mantoux test")
                            {
                                chk.Name = "InvestMantouxTest";
                            }

                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with dyspnea at rest ")
                            {
                                chk.Name = "AlarmCoughWithDyspneaAtRest";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough accompanied with breathlessness and coughing up pink frothy mucus")
                            {
                                chk.Name = "AlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Intractable cough")
                            {
                                chk.Name = "AlarmIntractableCough";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Persistent cough in patients with co-existing respiratory disease")
                            {
                                chk.Name = "AlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with blood (hemoptysis)")
                            {
                                chk.Name = "AlarmCoughWithBloodHemoptysis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with tachycardia")
                            {
                                chk.Name = "AlarmCoughWithTachycardia";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with unexplained weight loss with fevers and night chills")
                            {
                                chk.Name = "AlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with associated cyanosis")
                            {
                                chk.Name = "AlarmCoughWithAssociatedCyanosis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Persistent cough not responsive to empirical therapy/ drug treatment")
                            {
                                chk.Name = "AlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with dyspnea with mild activity")
                            {
                                chk.Name = "AlarmCoughWithDyspneaWithMildActivity";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with tachypnea")
                            {
                                chk.Name = "AlarmCoughWithTachypnea";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with high fever")
                            {
                                chk.Name = "AlarmCoughWithHighFever";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features " && PItem.Title == "Cough with stridor ( retropharyngeal abcess)")
                            {
                                chk.Name = "AlarmCoughWithStridorRetropharyngealAbcess";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Worsen during day time")
                            {
                                chk.Name = "HistWorsenDuringDayTime";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Worsen during night time")
                            {
                                chk.Name = "HistWorsenDuringNightTime";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Worsen with any postural change")
                            {
                                chk.Name = "HistWorsenWithAnyPosturalChange";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Persistent")
                            {
                                chk.Name = "HistPersistent";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Happens in bouts, followed by intervals of freedom")
                            {
                                chk.Name = "HistHappensInBoutsFollowedByIntervalsOfFreedom";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "No specific pattern")
                            {
                                chk.Name = "HistNoSpecificPattern";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Breathlessness / wheezing / stridor ")
                            {
                                chk.Name = "HistBreathlessnessWheezingStridor";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Characteristic ‘whoop’")
                            {
                                chk.Name = "HistCharacteristicWhoop";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Unexplained weight loss with fevers and night chills")
                            {
                                chk.Name = "HistUnexplainedWeightLossWithFeversAndNightChills";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Bodyache/ myalgia ")
                            {
                                chk.Name = "HistBodyacheMyalgia";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Rash or any other signs")
                            {
                                chk.Name = "HistRashOrAnyOtherSigns";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "History of foreign body inhalation")
                            {
                                chk.Name = "HistHistoryOfForeignBodyInhalation";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Intractable cough")
                            {
                                chk.Name = "HistIntractableCough";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Sputum production")
                            {
                                chk.Name = "HistSputumProduction";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Purulent sputum ")
                            {
                                chk.Name = "HistPurulentSputum";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Presence of blood")
                            {
                                chk.Name = "HistPresenceOfBlood";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Coughing up pink frothy mucus")
                            {
                                chk.Name = "HistCoughingUpPinkFrothyMucus";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Risk Factors" && PItem.Title == "H/o Asthma/ COPD")
                            {
                                chk.Name = "RiskHoAsthmaCOPD";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Risk Factors" && PItem.Title == "Smoking ")
                            {
                                chk.Name = "RiskSmoking";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Risk Factors" && PItem.Title == "ACE drug use")
                            {
                                chk.Name = "RiskACEDrugUse";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Lifestyle changes ")
                            {
                                chk.Name = "EMLifeStyleChanges";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Dietary advice")
                            {
                                chk.Name = "EMDietaryAdvice";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit clinic")
                            {
                                chk.Name = "EMWhenToVisitClinic";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit hospital")
                            {
                                chk.Name = "EMWhenToVisitHospital";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Tests conducted")
                            {
                                chk.Name = "EMTestsConducted";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Medication explained")
                            {
                                chk.Name = "EMMedicationExplained";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up visit discussed")
                            {
                                chk.Name = "EMFollowUpVisitDiscussed";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up/telephonic follow up scheduled")
                            {
                                chk.Name = "EMFollowUpTelephonicFollowUpScheduled";
                            }
                        }
                        else if (TemplateID == 7)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Frequent watery or loose stool")
                            {
                                chk.Name = "PresFreqWateryLooseStool";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Diarrhea accompanied with fever above 102.2 F")
                            {
                                chk.Name = "AlarmDiarrheaFever";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Presence of mucus in the stool")
                            {
                                chk.Name = "PresMucusStool";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Presence of blood in the stool")
                            {
                                chk.Name = "PresBloodStool";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Dysentry")
                            {
                                chk.Name = "PDDysentry";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Decreased urination")
                            {
                                chk.Name = "PresDecreasedUrination";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Sunken eyes ")
                            {
                                chk.Name = "PresSunkenEyes";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Dry mouth or skin ")
                            {
                                chk.Name = "PresDryMouthOrSkin";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Severe dehydration")
                            {
                                chk.Name = "AlarmSevereDehydration";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Persistent diarrhea")
                            {
                                chk.Name = "PDPersistentDiarrhea";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "C. difficile assay")
                            {
                                chk.Name = "InvestCDifficileAssay";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "General Examination" && PItem.Title == "Pallor")
                            {
                                chk.Name = "GEPallor";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Acute watery diarrhea")
                            {
                                chk.Name = "PDAcuteWateryDiarrhea";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Abdominal distension")
                            {
                                chk.Name = "AlarmAbdominalDistension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Prolonged symptoms of diarrhea (>14 days)")
                            {
                                chk.Name = "AlarmProlongedSymptomsOfDiarrhea14Days";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Persistent vomiting")
                            {
                                chk.Name = "AlarmPersistentVomiting";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Passing of black tarry stool")
                            {
                                chk.Name = "AlarmPassingOfBlackTarryStool";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Severe pain in the abdomen or rectum")
                            {
                                chk.Name = "AlarmSeverePainInTheAbdomenOrRectum";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Diarrhea with severe abdominal pain in a patient older than 50 years or in the elderly (above 60 years)")
                            {
                                chk.Name = "AlarmDiarrheaWithSevereAbdominalPain";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Drowsiness or listlessness")
                            {
                                chk.Name = "AlarmDrowsinessOrListlessness";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Convulsions or loss of consciousness")
                            {
                                chk.Name = "AlarmConvulsionsOrLossOfConsciousness";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Inability to administer oral rehydration therapy")
                            {
                                chk.Name = "AlarmInabilityToAdministerOralRehydrationTherapy";
                            }
                        }
                        else if (TemplateID == 5)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentation" && PItem.Title == "Nausea and vomiting")
                            {
                                chk.Name = "PresNauseaVomiting";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Hypertension with symptoms of nausea and vomiting")
                            {
                                chk.Name = "AlarmHypertensionNauseaVomiting";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Pre hypertension")
                            {
                                chk.Name = "PDPrehypertension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Stage I hypertension/Mild hypertension")
                            {
                                chk.Name = "PDStageIhypertensionMildhypertension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Stage II hypertension/Moderate hypertension")
                            {
                                chk.Name = "PDStageIIhypertensionModeratehypertension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Stage III hypertension/Severe hypertension")
                            {
                                chk.Name = "PDStageIIIhypertensionSeverehypertension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Persistent systolic blood pressure recording of more than 200 mmHg even after medication")
                            {
                                chk.Name = "AlarmPersistentsystolic200aftermedication";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Ultrasound abdomen")
                            {
                                chk.Name = "InvestUltrasoundAbdomen";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Doppler flow study")
                            {
                                chk.Name = "InvestDopplerFlowStudy";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentation" && PItem.Title == "Blurred vision")
                            {
                                chk.Name = "PresBlurredVision";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentation" && PItem.Title == "Confusion")
                            {
                                chk.Name = "PresConfusion";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentation" && PItem.Title == "Loss of consciousness")
                            {
                                chk.Name = "PresLossOfConsciousness";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "History of sudden blackouts")
                            {
                                chk.Name = "HistSuddenBlackouts";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Complaints of weakness of the limbs")
                            {
                                chk.Name = "HistWeaknessOfLimbs";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Slurred speech ")
                            {
                                chk.Name = "HistSlurredSpeech";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Tongue  protruding to one side")
                            {
                                chk.Name = "HistTongueProtrudingToOneSide";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Reduce weight")
                            {
                                chk.Name = "MgmtReduceWeight";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Adopt DASH eating plan")
                            {
                                chk.Name = "MgmtAdoptDASHEatingPlan";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Restrict salt in the diet")
                            {
                                chk.Name = "MgmtRestrictSaltInTheDiet";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Exercise regularly")
                            {
                                chk.Name = "MgmtExerciseRegularly";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Reduce alcohol consumption")
                            {
                                chk.Name = "MgmtReduceAlcoholConsumption";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Stop smoking")
                            {
                                chk.Name = "MgmtStopSmoking";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Lifestyle changes ")
                            {
                                chk.Name = "EMLifeStyleChanges";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Dietary advice")
                            {
                                chk.Name = "EMDietaryAdvice";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit clinic")
                            {
                                chk.Name = "EMWhenToVisitClinic";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit hospital")
                            {
                                chk.Name = "EMWhenToVisitHospital";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Tests conducted")
                            {
                                chk.Name = "EMTestsConducted";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Medication explained")
                            {
                                chk.Name = "EMMedicationExplained";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up visit discussed")
                            {
                                chk.Name = "EMFollowUpVisitDiscussed";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up / telephonic follow up scheduled")
                            {
                                chk.Name = "EMFollowUpTelephonicFollowUpScheduled";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Hypertensive emergencies where severe elevation of blood pressure (SBP> 220 mm of Hg or DBP>130 mm of Hg) is seen")
                            {
                                chk.Name = "AlarmHypertensiveEmergenciesSevereElevationOfBloodPressureSBP220OrDBP130";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Patients with severe retinopathy (hemorrhage and papilloedema) and malignant hypertension")
                            {
                                chk.Name = "AlarmPatientsWithSevereRetinopathyHemorrhageAndPapilloedemaAndMalignantHypertension";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Raised serum creatinine or low plasma potassium in the absence of a diuretic")
                            {
                                chk.Name = "AlarmRaisedSerumCreatinineOrLowPlasmaPotassiumInTheAbsenceOfDiuretic";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Hematuria, proteinuria or cells in urine")
                            {
                                chk.Name = "AlarmHematuriaProteinuriaOrCellsInUrine";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Sudden and severe headaches ")
                            {
                                chk.Name = "AlarmSuddenAndSevereHeadaches";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Symptoms of blurred vision ")
                            {
                                chk.Name = "AlarmSymptomsOfBlurredVision";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Symptoms of dizziness ")
                            {
                                chk.Name = "AlarmSymptomsOfDizziness";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Sudden blackouts")
                            {
                                chk.Name = "AlarmSuddenBlackouts";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Complaints of weakness of the limbs")
                            {
                                chk.Name = "AlarmComplaintsOfWeaknessOfTheLimbs";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Confusion")
                            {
                                chk.Name = "AlarmConfusion";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Loss of consciousness")
                            {
                                chk.Name = "AlarmLossOfConsciousness";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Complaints of slurred speech ")
                            {
                                chk.Name = "AlarmComplaintsOfSlurredSpeech";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Complaints of tongue  protruding to one side")
                            {
                                chk.Name = "AlarmComplaintsOfTongueProtrudingToOneSide";
                            }
                        }
                        else if (TemplateID == 6)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Presentations" && PItem.Title == "Reflux-like symptoms : heartburn or acid regurgitation")
                            {
                                chk.Name = "PresRefluxSymptomsHeartburnAcidRegurgitation";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Gastro esophageal reflux disease (GERD)")
                            {
                                chk.Name = "PDGastroEsophagealRefluxDiseaseGERD";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Gastrointestinal bleeding( blood in vomiting and stools)")
                            {
                                chk.Name = "AlarmGastrointestinalBleedingVomitingStools";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "General Examination" && PItem.Title == "Pallor")
                            {
                                chk.Name = "GEPallor";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "General Examination" && PItem.Title == "Lymphadenopathy")
                            {
                                chk.Name = "GELymphadenopathy";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Palpable mass or lymphadenopathy")
                            {
                                chk.Name = "AlarmPalpableMassLymphadenopathy";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Duodenal ulcer")
                            {
                                chk.Name = "PDDuodenalUlcer";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Peptic ulcer")
                            {
                                chk.Name = "PDPepticUlcer";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Gastric ulcers")
                            {
                                chk.Name = "PDGastricUlcers";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Drug induced gastritis ")
                            {
                                chk.Name = "PDDrugInducedGastritis";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Serology test")
                            {
                                chk.Name = "InvestSerologytest";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Age >55 years, with new onset symptoms")
                            {
                                chk.Name = "AlarmAge55yearsWithNewOnsetSymptoms";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Unexplained weight loss (>3kg)")
                            {
                                chk.Name = "AlarmUnexplainedWeightLoss3kg";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Progressive dysphagia or persistent painful swallowing")
                            {
                                chk.Name = "AlarmProgressiveDysphagiaOrPersistentPainfulSwallowing";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Persistent vomiting")
                            {
                                chk.Name = "AlarmPersistentVomiting";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Signs of anemia (Pallor)")
                            {
                                chk.Name = "AlarmSignsOfAnemiaPallor";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Alarm Features" && PItem.Title == "Jaundice")
                            {
                                chk.Name = "AlarmJaundice";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Helicobacter pylori infection")
                            {
                                chk.Name = "PDHelicobacterPyloriInfection";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Lifestyle changes ")
                            {
                                chk.Name = "EMLifeStyleChanges";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Dietary advice")
                            {
                                chk.Name = "EMDietaryAdvice";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit clinic")
                            {
                                chk.Name = "EMWhenToVisitClinic";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "When to visit hospital")
                            {
                                chk.Name = "EMWhenToVisitHospital";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Tests conducted")
                            {
                                chk.Name = "EMTestsConducted";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Medication explained")
                            {
                                chk.Name = "EMMedicationExplained";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up visit discussed")
                            {
                                chk.Name = "EMFollowUpVisitDiscussed";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Follow up/telephonic follow up scheduled")
                            {
                                chk.Name = "EMFollowUpTelephonicFollowUpScheduled";
                            }
                        }


                        chk.IsEnabled = WritePerm;

                        chk.IsThreeState = false;
                        chk.Margin = new Thickness(2);
                        chk.Click += new RoutedEventHandler(chk_Click);
                        chk.DataContext = PItem;
                        binding = new Binding("Settings.Value");
                        binding.Mode = BindingMode.TwoWay;
                        chk.SetBinding(CheckBox.IsCheckedProperty, binding);
                        if (IsView == true)
                        {
                            chk.IsEnabled = false;
                        }
                        else
                        {
                            chk.IsEnabled = true;
                        }
                        Grid.SetRow(chk, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(chk, 1);
                        var itemCheck = from EDT in EmrDetails
                                        where (EDT.ControlName == PItem.Name
                                        )
                                        select EDT;

                        if (itemCheck.ToList().Count > 0)
                        {
                            foreach (var item in itemCheck)
                            {
                                if (item.ControlName == PItem.Name)
                                {
                                    chk.IsChecked = Convert.ToBoolean(item.Value);
                                }
                            }
                        }
                        PItem.Control = chk;
                        Container.Children.Add(chk);
                        if (IdDependentField)
                            FTitle.Visibility = chk.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        StackPanel panel = new StackPanel();
                        #region Added By Harish
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(panel, tt);
                            #region new added by harish
                            panel.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #endregion
                        panel.DataContext = PItem;
                        panel.Orientation = Orientation.Horizontal;
                        RadioButton yes = new RadioButton();
                        RadioButton No = new RadioButton();
                        if (TemplateID == 4)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Spirometry ")
                            {
                                yes.Name = "InvestSpirometry";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Past History" && PItem.Title == "On any cough medications")
                            {
                                yes.Name = "PHistOnAnyCoughMedicationsYes";
                                No.Name = "PHistOnAnyCoughMedicationsNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Advisory")
                            {
                                yes.Name = "EMAdvisoryYes";
                                No.Name = "EMAdvisoryNo";
                            }
                        }
                        else if (TemplateID == 5)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Drug History" && PItem.Title == "Anti hypertensives")
                            {
                                yes.Name = "DHistAntihypertensivesYes";
                                No.Name = "DHistAntihypertensivesNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Risk Factors" && PItem.Title == "Diabetes")
                            {
                                yes.Name = "RFDiabetesYes";
                                No.Name = "RFDiabetesNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Past History" && PItem.Title == "Renal/Kidney disease")
                            {
                                yes.Name = "PHRenalKidneyDiseaseYes";
                                No.Name = "PHRenalKidneyDiseaseNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Family History" && PItem.Title == "Diabetes")
                            {
                                yes.Name = "FHDiabetesYes";
                                No.Name = "FHDiabetesNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Family History" && PItem.Title == "Hyperlipidemia")
                            {
                                yes.Name = "FHHyperlipidemiaYes";
                                No.Name = "FHHyperlipidemiaNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Pedal edema")
                            {
                                yes.Name = "PEPedalEdemaYes";
                                No.Name = "PEPedalEdemaNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Signs of anemia")
                            {
                                yes.Name = "PESignsOfAnemiaYes";
                                No.Name = "PESignsOfAnemiaNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Advisory")
                            {
                                yes.Name = "EMAdvisoryYes";
                                No.Name = "EMAdvisoryNo";
                            }
                        }
                        else if (TemplateID == 6)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Blood in stool")
                            {
                                yes.Name = "HistBloodStoolYes";
                                No.Name = "HistBloodStoolNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Blood in vomitus ")
                            {
                                yes.Name = "HistBloodVomitusYes";
                                No.Name = "HistBloodVomitusNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "General Examination" && PItem.Title == "Masses")
                            {
                                yes.Name = "GEMassesYes";
                                No.Name = "GEMassesNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Education Management" && PItem.Title == "Advisory")
                            {
                                yes.Name = "EMAdvisoryYes";
                                No.Name = "EMAdvisoryNo";
                            }
                        }
                        else if (TemplateID == 7)
                        {
                            if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Recent antibiotic use")
                            {
                                yes.Name = "HistRecentAntibioticUseYes";
                                No.Name = "HistRecentAntibioticUseNo";
                            }
                            else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Blood in Stools")
                            {
                                yes.Name = "HistBloodInStoolsYes";
                                No.Name = "HistBloodInStoolsNo";
                            }
                        }

                        yes.IsEnabled = WritePerm;

                        Binding byes = new Binding("Settings.Value");
                        byes.Converter = new PalashDynamics.Service.DataTemplateServiceRef1.BoolToYesNoConverter();
                        byes.ConverterParameter = "true";
                        byes.Mode = BindingMode.TwoWay;
                        yes.SetBinding(RadioButton.IsCheckedProperty, byes);
                        yes.Margin = new Thickness(2);
                        yes.Click += new RoutedEventHandler(chk_Click);
                        yes.Content = "Yes";

                        No.IsEnabled = WritePerm;

                        Binding bno = new Binding("Settings.Value");
                        bno.Converter = new PalashDynamics.Service.DataTemplateServiceRef1.BoolToYesNoConverter();
                        bno.ConverterParameter = "false";
                        bno.Mode = BindingMode.TwoWay;
                        No.SetBinding(RadioButton.IsCheckedProperty, bno);
                        No.Margin = new Thickness(2);
                        No.Click += new RoutedEventHandler(chk_Click);
                        No.Content = "No";
                        panel.Children.Add(yes);
                        panel.Children.Add(No);
                        Grid.SetRow(panel, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(panel, 1);
                        PItem.Control = panel;
                        Container.Children.Add(panel);
                        if (PItem.IsRequired)
                        {
                            yes.SetValidation(PItem.Title + " is required.");
                            yes.RaiseValidationError();
                            No.SetValidation(PItem.Title + " is required.");
                            No.RaiseValidationError();
                        }
                        if (IdDependentField)
                        {
                            FTitle.Visibility = panel.Visibility = Visibility.Collapsed;
                        }
                    }
                    break;
                    #endregion
                case 3:
                    #region Case 3 Main
                    DatePicker dtp = new DatePicker();
                    if (TemplateID == 2)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Follow up Management" && PItem.Title == "Date(Hospital)")
                        {
                            dtp.Name = "HospitalDate";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Follow up Management" && PItem.Title == "Date(Telephonic)")
                        {
                            dtp.Name = "TelephonicDate";
                        }
                    }
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(dtp, tt);
                        #region new added by harish
                        dtp.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    dtp.IsEnabled = WritePerm;

                    if (!IsSaved && ((DateFieldSetting)PItem.Settings).IsDefaultDate)
                    {
                        if (VisitTypeID == 1)
                        {
                            if (((SectionDetail)PItem.Parent).Tab == "Consultation")
                            {
                                if ((bool)((DateFieldSetting)PItem.Settings).Mode)
                                    ((DateFieldSetting)PItem.Settings).Date = (DateTime.Now).AddDays(Convert.ToDouble(((DateFieldSetting)PItem.Settings).Days));
                                else
                                    ((DateFieldSetting)PItem.Settings).Date = (DateTime.Now).AddDays(-Convert.ToDouble(((DateFieldSetting)PItem.Settings).Days));
                            }
                        }
                        else if (VisitTypeID == 2 || VisitTypeID == 3)
                        {
                            if (((SectionDetail)PItem.Parent).Tab != "Consultation")
                            {
                                if ((bool)((DateFieldSetting)PItem.Settings).Mode)
                                    ((DateFieldSetting)PItem.Settings).Date = (DateTime.Now).AddDays(Convert.ToDouble(((DateFieldSetting)PItem.Settings).Days));
                                else
                                    ((DateFieldSetting)PItem.Settings).Date = (DateTime.Now).AddDays(-Convert.ToDouble(((DateFieldSetting)PItem.Settings).Days));
                            }
                        }

                    }
                    #region new added by harish
                    dtp.DataContext = PItem;
                    #endregion
                    dtp.Margin = new Thickness(2);
                    binding = new Binding("Settings.Date");
                    binding.Mode = BindingMode.TwoWay;
                    dtp.SetBinding(DatePicker.SelectedDateProperty, binding);
                    if (IsView == true)
                    {
                        dtp.IsEnabled = false;
                    }
                    else
                    {
                        dtp.IsEnabled = true;
                    }
                    //////////////////////////////Added By Nilesh

                    var itemDate = from EDT in EmrDetails
                                   where (EDT.ControlName == PItem.Name
                                   )
                                   select EDT;

                    if (itemDate.ToList().Count > 0)
                    {
                        foreach (var item in itemDate)
                        {
                            if (item.ControlName == PItem.Name)
                            {
                                if (item.Value != null)
                                    try
                                    {
                                        dtp.SelectedDate = Convert.ToDateTime(item.Value);
                                    }
                                    catch (Exception) { }
                            }
                        }
                    }
                    /////////////////////////////////////////////

                    //dtp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(dtp_SelectedDateChanged);                    
                    Grid.SetRow(dtp, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(dtp, 1);
                    PItem.Control = dtp;
                    if (IdDependentField)
                        FTitle.Visibility = dtp.Visibility = Visibility.Collapsed;

                    Container.Children.Add(dtp);
                    break;
                    #endregion
                case 4:
                    #region Case 4
                    ListFieldSetting listSetting = ((ListFieldSetting)PItem.Settings);

                    switch (listSetting.ChoiceMode)
                    {
                        case SelectionMode.Single:
                            #region Case 4 Sub Single ComboBox
                            switch (listSetting.ControlType)
                            {
                                case ListControlType.ComboBox:

                                    ComboBox cmbList = new ComboBox();
                                    #region Added By Harish
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(cmbList, tt);
                                        #region new added by harish
                                        cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    #endregion


                                    if (TemplateID == 2)
                                    {
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Urine output")
                                        {
                                            cmbList.Name = "UrineOutput";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "General Conditions")
                                        {
                                            cmbList.Name = "GeneralCondition";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Eye")
                                        {
                                            cmbList.Name = "Eye";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Skin Colour")
                                        {
                                            cmbList.Name = "SkinColour";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Thirst")
                                        {
                                            cmbList.Name = "Thirst";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Skin/Turgor")
                                        {
                                            cmbList.Name = "SkinTurgor";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Hydration Status")
                                        {
                                            cmbList.Name = "PhysicalExaminationHydrationStatus";
                                        }

                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Status")
                                        {
                                            cmbList.Name = "NutritionStatus";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Hydration Status" && PItem.Title == "Hydration status")
                                        {
                                            cmbList.Name = "HydrationStatus";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Provisional Diagnosis" && PItem.Title == "Provisional Diagnosis")
                                        {
                                            cmbList.Name = "ProvisionalDiagnosis";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Follow up Management" && PItem.Title == "for")
                                        {
                                            cmbList.Name = "ManagementFollowUpFor";
                                        }

                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "To(Hospital/Specialist)")
                                        {
                                            cmbList.Name = "RMTo";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "At(Location)")
                                        {
                                            cmbList.Name = "RMAt";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "For(Reason)")
                                        {
                                            cmbList.Name = "RMFor";
                                        }
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Refferal Management" && PItem.Title == "By(Urgency)")
                                        {
                                            cmbList.Name = "RMBy";
                                        }
                                    }
                                    else if (TemplateID == 4)
                                    {
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Duration")
                                        {
                                            cmbList.Name = "CoughDuration";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Respiratory ")
                                        {
                                            cmbList.Name = "PERespiratory";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Referral Management" && PItem.Title == "To(Hospital/Specialist)")
                                        {
                                            cmbList.Name = "RMToHospital";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Onset ")
                                        {
                                            cmbList.Name = "CoughOnset";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Past History" && PItem.Title == "Course of drug treatment")
                                        {
                                            cmbList.Name = "PHistCourseOfDrugTreatment";
                                        }
                                    }
                                    else if (TemplateID == 5)
                                    {
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Referral Management" && PItem.Title == "To(Hospital/Specialist)")
                                        {
                                            cmbList.Name = "RMToHospital";
                                        }
                                    }
                                    else if (TemplateID == 6)
                                    {
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Referral Management" && PItem.Title == "To(Hospital/Specialist)")
                                        {
                                            cmbList.Name = "RMToHospital";
                                        }
                                    }
                                    else if (TemplateID == 7)
                                    {
                                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Urine output")
                                        {
                                            cmbList.Name = "HistUrineOutput";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Hydration Status" && PItem.Title == "Hydration status")
                                        {
                                            cmbList.Name = "HSHydrationStatus";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "General condition")
                                        {
                                            cmbList.Name = "PEGeneralCondition";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Thirst")
                                        {
                                            cmbList.Name = "PEThirst";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Mucus membrane")
                                        {
                                            cmbList.Name = "PEMucusMembrane";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Skin pinch (abdomen)")
                                        {
                                            cmbList.Name = "PESkinPinchAbdomen";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Urine output")
                                        {
                                            cmbList.Name = "PEUrineOutput";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Skin colour")
                                        {
                                            cmbList.Name = "PESkinColour";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Referral Management" && PItem.Title == "To(Hospital/Specialist)")
                                        {
                                            cmbList.Name = "RMToHospital";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Abdominal examination")
                                        {
                                            cmbList.Name = "PEAbdominalExamination";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Eye")
                                        {
                                            cmbList.Name = "PEEye";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Skin temperature/ color")
                                        {
                                            cmbList.Name = "PESkinTemperatureColor";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Pulse rate")
                                        {
                                            cmbList.Name = "PEPulseRate";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Blood pressure")
                                        {
                                            cmbList.Name = "PEBloodPressure";
                                        }
                                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Abdominal examination")
                                        {
                                            cmbList.Name = "PEAbdominalExamination";
                                        }
                                    }

                                    cmbList.IsEnabled = WritePerm;
                                    cmbList.DataContext = PItem;

                                    PItem.Control = cmbList;
                                    cmbList.SelectionChanged += new SelectionChangedEventHandler(cmbList_SelectionChanged);
                                    if (listSetting.SelectedItem != null)
                                        listSetting.SelectedItem = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                                    else
                                    {
                                        if (listSetting.ItemSource.Count != 0)
                                            listSetting.SelectedItem = listSetting.ItemSource[0];
                                        listSetting.SelectedItem = null;
                                    }

                                    ////// Added By Nilesh Raut
                                    var itemCombo = from EDT in EmrDetails
                                                    where (EDT.ControlName == PItem.Name
                                                    )
                                                    select EDT;
                                    if (itemCombo.ToList().Count > 0)
                                    {
                                        foreach (var item in itemCombo)
                                        {
                                            if (item.ControlName == PItem.Name)
                                            {

                                                if (item.Value != null && item.Value != "")
                                                {
                                                    listSetting.SelectedItem = listSetting.ItemSource.Where(i => i.Title == item.Value).Single();
                                                }
                                            }
                                        }
                                    }
                                    ////// /////////////////////////////
                                    cmbList.DisplayMemberPath = "Title";

                                    Binding Sourcebinding = new Binding("Settings.ItemSource");
                                    Sourcebinding.Mode = BindingMode.OneWay;
                                    cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                    Binding SIbinding = new Binding("Settings.SelectedItem");
                                    SIbinding.Mode = BindingMode.TwoWay;
                                    cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                                    cmbList.Margin = new Thickness(2);

                                    if (IsView == true)
                                    {
                                        cmbList.IsEnabled = false;
                                    }
                                    else
                                    {
                                        cmbList.IsEnabled = true;
                                    }
                                    Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(cmbList, 1);

                                    if (PItem.IsRequired)
                                    {
                                        cmbList.SetValidation(PItem.Title + " is required.");
                                        cmbList.RaiseValidationError();
                                    }
                                    if (IdDependentField)
                                        FTitle.Visibility = cmbList.Visibility = Visibility.Visible;



                                    Container.Children.Add(cmbList);
                                    break;
                                case ListControlType.RadioButton:
                                    break;
                            }
                            break;
                            #endregion
                        case SelectionMode.Multiples:
                            #region Case 4 Sub multiple ListBox
                            ListBox lbList = new ListBox();
                            #region Added By Harish
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lbList, tt);
                                #region new added by harish
                                lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            #endregion
                            lbList.IsEnabled = WritePerm;
                            lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                            lbList.DataContext = PItem;
                            Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                            Sourcebinding1.Mode = BindingMode.OneWay;
                            lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                            if (listSetting.SelectedItems != null && listSetting.SelectedItems.Count > 0)
                                foreach (var item in listSetting.SelectedItems)
                                {
                                    lbList.SelectedItems.Add(listSetting.ItemSource.Where(i => i.Title == item.Title).Single());
                                }
                            lbList.MaxHeight = 100;
                            ////// Added By Nilesh Raut
                            var itemComboList = from EDT in EmrDetails
                                                where (EDT.ControlName == PItem.Name
                                                )
                                                select EDT;
                            if (itemComboList.ToList().Count > 0)
                            {
                                foreach (var item in itemComboList)
                                {
                                    if (item.ControlName == PItem.Name)
                                    {
                                        if (item.Value != null && item.Value != "")
                                        {
                                            try
                                            {
                                                //lbList.SelectedItem = lbList.ItemSource.Where(i => i.Title == item.Value).Single();
                                                lbList.SelectedItems.Add(listSetting.ItemSource.Where(i => i.Title == item.Value).Single());
                                            }
                                            catch (Exception) { }
                                        }
                                    }
                                }
                            }
                            ////// /////////////////////////////
                            lbList.DisplayMemberPath = "Title";
                            lbList.Margin = new Thickness(2);
                            lbList.SelectionChanged += new SelectionChangedEventHandler(lbList_SelectionChanged);
                            if (IsView == true)
                            {
                                lbList.IsEnabled = false;
                            }
                            else
                            {
                                lbList.IsEnabled = true;
                            }
                            Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(lbList, 1);
                            lbList.ItemsSource = listSetting.ItemSource;
                            PItem.Control = lbList;
                            if (IdDependentField)
                                FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                            Container.Children.Add(lbList);
                            break;
                            #endregion
                    }
                    break;
                    #endregion
                case 5:
                    #region Case 5 - USE
                    StackPanel DecP = new StackPanel();
                    DecP.DataContext = PItem;
                    //PItem.Control = DecP;
                    DecP.Orientation = Orientation.Horizontal;
                    TextBox DecField = new TextBox();
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(DecField, tt);
                        #region new added by harish
                        DecP.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    #region Hard Coded

                    if (TemplateID == 2)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Temprature")
                        {
                            DecField.Name = "Temprature";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Pulse")
                        {
                            DecField.Name = "Pulse";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Respiratory Rate")
                        {
                            DecField.Name = "Respiratory";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Systolic")
                        {
                            DecField.Name = "Systolic";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Diastolic")
                        {
                            DecField.Name = "Diastolic";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Weight")
                        {
                            DecField.Name = "Weight";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Height")
                        {
                            DecField.Name = "Height";
                        }


                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Temprature")
                        {
                            DecField.Name = "ROTemprature";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Pulse")
                        {
                            DecField.Name = "ROPulse";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Respiratory Rate")
                        {
                            DecField.Name = "RORespiratory";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Systolic")
                        {
                            DecField.Name = "ROSystolic";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Diastolic")
                        {
                            DecField.Name = "RODiastolic";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Weight")
                        {
                            DecField.Name = "ROWeight";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Reassesment Observation" && PItem.Title == "Height")
                        {
                            DecField.Name = "ROHeight";
                        }

                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Height/Age")
                        {
                            DecField.Name = "StuntingPercent";
                            DecField.TextChanged += new TextChangedEventHandler(DecField_TextChanged);
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Physical Examination" && PItem.Title == "Height/Weight")
                        {
                            DecField.Name = "WastingPercent";
                            DecField.TextChanged += new TextChangedEventHandler(DecField_TextChanged);
                        }

                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Days")
                        {
                            DecField.Name = "StoolDays";
                        }
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Hours")
                        {
                            DecField.Name = "StoolHours";
                        }

                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Frequency")
                        {
                            if (FrequencyFlag)
                            {
                                DecField.Name = "VomitingFrequency";
                                FrequencyFlag = false;
                            }
                            else
                            {
                                DecField.Name = "StoolFrequency";
                                FrequencyFlag = true;
                            }
                        }

                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Duration")
                        {
                            if (!FrequencyFlag)
                            {
                                DecField.Name = "VomitingDuration";
                                FrequencyFlag = true;
                            }
                        }
                    }
                    else if (TemplateID == 4)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Temperature")
                        {
                            DecField.Name = "Temprature";
                        }
                    }
                    else if (TemplateID == 7)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Temperature")
                        {
                            DecField.Name = "ObserveTemperature";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Since")
                        {
                            if (((DecimalFieldSetting)PItem.Settings).Unit != null && ((DecimalFieldSetting)PItem.Settings).Unit == "days")
                                DecField.Name = "HistSinceDays";
                        }
                    }
                    else if (TemplateID == 5)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Systolic")
                        {
                            DecField.Name = "ObserveSystolic";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Observations" && PItem.Title == "Diastolic")
                        {
                            DecField.Name = "ObserveDiastolic";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "History" && PItem.Title == "Systolic")
                        {
                            DecField.Name = "HistSystolic";
                        }
                    }


                    #endregion
                    DecField.IsEnabled = WritePerm;
                    DecField.VerticalAlignment = VerticalAlignment.Center;
                    DecField.Margin = new Thickness(2);
                    DecP.Children.Add(DecField);
                    TextBlock decUnit = new TextBlock();
                    decUnit.Margin = new Thickness(2);
                    decUnit.VerticalAlignment = VerticalAlignment.Center;
                    DecP.Children.Add(decUnit);
                    Grid.SetRow(DecP, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(DecP, 1);
                    if (PItem.Settings != null)
                    {
                        binding = new Binding("Settings.Value");
                        binding.Mode = BindingMode.TwoWay;
                        DecField.SetBinding(TextBox.TextProperty, binding);
                        DecField.Width = 50;
                        if (string.IsNullOrEmpty(DecField.Text))
                            DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.HasValue ? ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString() : "";
                        DecField.LostFocus += new RoutedEventHandler(decUnit_LostFocus);
                        DecField.TextChanged += new TextChangedEventHandler(DecField_TextChanged);
                        DecField.KeyDown += new KeyEventHandler(DecField_KeyDown);
                        DecField.TextAlignment = TextAlignment.Right;
                        decUnit.Text = string.IsNullOrEmpty(((DecimalFieldSetting)PItem.Settings).Unit) ? "" : ((DecimalFieldSetting)PItem.Settings).Unit;
                        //DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString();
                        //DecField.Text = ((DecimalFieldSetting)PItem.Settings).DefaultValue.ToString();
                    }
                    if (PItem.IsRequired)
                    {
                        DecField.SetValidation(PItem.Title + " is required.");
                        DecField.RaiseValidationError();
                    }
                    if (IdDependentField)
                        FTitle.Visibility = DecP.Visibility = Visibility.Collapsed;
                    ////////////////////// Added By Nilesh
                    var itemNumeric = from EDT in EmrDetails
                                      where (EDT.ControlName == PItem.Name
                                      )
                                      select EDT;
                    if (itemNumeric.ToList().Count > 0)
                    {
                        foreach (var item in itemNumeric)
                        {
                            if (item.ControlName == PItem.Name)
                            {
                                DecField.Text = item.Value;
                            }
                        }
                    }
                    /////////////////////End///////////////////
                    PItem.Control = DecP;
                    Container.Children.Add(DecP);
                    break;
                    #endregion
                case 6:
                    #region Case 6 Main -- Use
                    HyperlinkButton HyperBtn = new HyperlinkButton();
                    HyperBtn.VerticalAlignment = VerticalAlignment.Center;
                    HyperBtn.IsTabStop = false;
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(HyperBtn, tt);
                        #region new added by harish
                        HyperBtn.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion

                    if (((HyperlinkFieldSetting)PItem.Settings).Url != null && ((HyperlinkFieldSetting)PItem.Settings).Url != "")
                    {
                        HyperBtn.Content = ((HyperlinkFieldSetting)PItem.Settings).Url;
                        HyperBtn.TargetName = ((HyperlinkFieldSetting)PItem.Settings).Url;
                        HyperBtn.Click += new RoutedEventHandler(HyperBtn_Click);
                    }
                    HyperBtn.DataContext = PItem;
                    PItem.Control = HyperBtn;
                    if (IsView == true)
                    {
                        HyperBtn.IsEnabled = false;
                    }
                    else
                    {
                        HyperBtn.IsEnabled = true;
                    }
                    Grid.SetRow(HyperBtn, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(HyperBtn, 1);
                    Container.Children.Add(HyperBtn);
                    break;
                    #endregion
                case 7:
                    #region Case 7 Main -- Use
                    FTitle.FontFamily = new FontFamily("Portable User Interface");
                    FTitle.Foreground = this.Resources["Heading"] as Brush;
                    FTitle.FontWeight = FontWeights.Bold;
                    FTitle.FontStyle = FontStyles.Italic;
                    FTitle.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                    Grid.SetRow(FTitle, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(FTitle, 0);
                    Grid.SetColumnSpan(FTitle, 2);
                    Container.Children.Add(FTitle);
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(FTitle, tt);
                        #region new added by harish
                        FTitle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    FTitle.DataContext = PItem;
                    break;
                    #endregion
                case 8:
                    #region Case 8 Main
                    LookUpFieldSetting LookUpSetting = ((LookUpFieldSetting)PItem.Settings);

                    switch (LookUpSetting.ChoiceMode)
                    {
                        case SelectionMode.Single:
                            ComboBox cmbList = new ComboBox();

                            if (TemplateID == 2)
                            {
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Hydration")
                                {
                                    cmbList.Name = "HydrationStatusManagement";
                                }
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "IV Hydration")
                                {
                                    cmbList.Name = "Hydration4StatusManagement";
                                }
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Zinc Supplement")
                                {
                                    cmbList.Name = "ZincSupplementManagement";

                                    if (LookUpSetting.ItemSource.Count == 0)
                                    {
                                        LookUpSetting.SelectedSource = new DynamicListItem() { Id = 1, Title = "Source4", Value = "Source4" };
                                        IGetList src = null;
                                        src = new Source4();
                                        LookUpSetting.ItemSource = src.GetList();
                                    }
                                }
                            }

                            if (LookUpSetting.ItemSource != null && LookUpSetting.ItemSource.Count != 0)
                            {
                                cmbList.ItemsSource = LookUpSetting.ItemSource;
                                if (LookUpSetting.SelectedItem != null)
                                {
                                    var item = LookUpSetting.ItemSource.Where(i => i.Title == LookUpSetting.SelectedItem.Title).Single();
                                    cmbList.SelectedItem = item;
                                }
                                else
                                {
                                    cmbList.SelectedItem = null;
                                }
                            }


                            cmbList.IsEnabled = WritePerm;
                            cmbList.DisplayMemberPath = "Title";
                            cmbList.Margin = new Thickness(2);
                            cmbList.DataContext = PItem;
                            Binding Sourcebinding = new Binding("Settings.ItemSource");
                            Sourcebinding.Mode = BindingMode.TwoWay;
                            cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                            Binding SIbinding = new Binding("Settings.SelectedItem");
                            SIbinding.Mode = BindingMode.TwoWay;
                            cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                            //cmbList.ItemsSource = ((IGetList)(Assembly.GetExecutingAssembly().CreateInstance(LookUpSetting.SelectedSource.Value))).GetList();
                            if (!LookUpSetting.IsAlternateText)
                            {
                                #region Added By Harish
                                if (PItem.IsToolTip == true)
                                {
                                    //ToolTipService.SetToolTip(cmbList, tt);
                                    #region new added by harish
                                    cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                    #endregion
                                }
                                #endregion
                                if (IsView == true)
                                {
                                    cmbList.IsEnabled = false;
                                }
                                else
                                {
                                    cmbList.IsEnabled = true;
                                }
                                Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                                Grid.SetColumn(cmbList, 1);
                                //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                                PItem.Control = cmbList;
                                if (IdDependentField)
                                    cmbList.Visibility = Visibility.Collapsed;
                                Container.Children.Add(cmbList);
                            }
                            else
                            {
                                Grid lookupgrid = new Grid();
                                #region Added By Harish
                                if (PItem.IsToolTip == true)
                                {
                                    //ToolTipService.SetToolTip(lookupgrid, tt);
                                    lookupgrid.DataContext = PItem;
                                    #region new added by harish
                                    lookupgrid.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                    #endregion
                                }
                                #endregion
                                ColumnDefinition col1 = new ColumnDefinition();
                                ColumnDefinition col2 = new ColumnDefinition();
                                ColumnDefinition col3 = new ColumnDefinition();
                                col2.Width = new GridLength(30, GridUnitType.Auto);
                                lookupgrid.ColumnDefinitions.Add(col1);
                                lookupgrid.ColumnDefinitions.Add(col2);
                                lookupgrid.ColumnDefinitions.Add(col3);

                                Grid.SetColumn(cmbList, 0);

                                TextBlock or = new TextBlock();
                                or.VerticalAlignment = VerticalAlignment.Center;
                                or.Margin = new Thickness(2);
                                or.Text = "Or";
                                Grid.SetColumn(or, 1);
                                TextBox Alt = new TextBox();
                                Alt.IsEnabled = WritePerm;
                                or.Margin = new Thickness(2);
                                Grid.SetColumn(Alt, 2);
                                //cmbList.SelectedIndex = listSetting.DefaultSelectedItemIndex;
                                Grid.SetRow(lookupgrid, Container.RowDefinitions.Count - 1);
                                Grid.SetColumn(lookupgrid, 1);

                                lookupgrid.Children.Add(cmbList);
                                lookupgrid.Children.Add(or);
                                lookupgrid.Children.Add(Alt);
                                PItem.Control = lookupgrid;
                                if (IdDependentField)
                                    FTitle.Visibility = cmbList.Visibility = Visibility.Collapsed;
                                Container.Children.Add(lookupgrid);
                            }

                            break;
                        case SelectionMode.Multiples:
                            ListBox lbList = new ListBox();
                            #region Added By Harish
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lbList, tt);
                                #region new added by harish
                                lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            #endregion
                            lbList.IsEnabled = WritePerm;
                            lbList.MaxHeight = 100;
                            lbList.DisplayMemberPath = "Title";
                            lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                            lbList.Margin = new Thickness(2);
                            lbList.DataContext = PItem;
                            if (IsView == true)
                            {
                                lbList.IsEnabled = false;
                            }
                            else
                            {
                                lbList.IsEnabled = true;
                            }
                            Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(lbList, 1);
                            //lbList.ItemsSource = ((IGetList)(Assembly.GetExecutingAssembly().CreateInstance(LookUpSetting.SelectedSource.Value))).GetList();
                            PItem.Control = lbList;
                            if (IdDependentField)
                                FTitle.Visibility = lbList.Visibility = Visibility.Collapsed;
                            Container.Children.Add(lbList);
                            break;
                    }
                    break;
                    #endregion
                case 9:

                case 10:
                    #region Case 10 Main
                    RowDefinition Row1 = new RowDefinition();
                    Row1.Height = new GridLength();
                    Container.RowDefinitions.Add(Row1);

                    Grid s = new Grid();
                    s.Name = "FollowUpMedication";
                    s = GetGridSchema(s);

                    Grid.SetRow(s, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(s, 0);
                    Grid.SetColumnSpan(s, 2);
                    Container.Children.Add(s);
                    break;
                    #endregion
                case 11:
                    #region Case 11 Main
                    OtherInvestigationFieldSetting InvestSetting = ((OtherInvestigationFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox lstBoxInvest = new ListBox();
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBoxInvest, tt);
                        #region new added by harish
                        lstBoxInvest.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    lstBoxInvest.DataContext = PItem;
                    lstBoxInvest.IsEnabled = WritePerm;
                    //lstBoxInvest.SelectionChanged += new SelectionChangedEventHandler(lstBoxInvest_SelectionChanged);
                    lstBoxInvest.Margin = new Thickness(2);


                    if (TemplateID == 4)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Other tests ")
                        {
                            lstBoxInvest.Name = "InvestOtherTests";
                        }
                    }


                    for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
                    {
                        InvestigationRepetorControlItem irci = new InvestigationRepetorControlItem();
                        irci.OnAddRemoveClick += new RoutedEventHandler(irci_OnAddRemoveClick);
                        irci.cmbSelectionChanged += new RoutedEventHandler(irci_cmbSelectionChanged);
                        //irci.txtRemarksLostFocus += new RoutedEventHandler(irci_txtRemarksLostFocus);
                        irci.txtRemarksTextChanged += new RoutedEventHandler(irci_txtRemarksTextChanged);
                        //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                        InvestSetting.ItemsSource[i].Index = i;
                        InvestSetting.ItemsSource[i].Command = ((i == InvestSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                        InvestSetting.ItemsSource[i].Parent = lstBoxInvest;
                        InvestSetting.ItemsSource[i].InvestigationSetting = InvestSetting;
                        irci.DataContext = InvestSetting.ItemsSource[i];
                        lstBoxInvest.Items.Add(irci);

                    }

                    if (IsView == true)
                    {
                        lstBoxInvest.IsEnabled = false;
                    }
                    else
                    {
                        lstBoxInvest.IsEnabled = true;
                    }
                    Grid.SetRow(lstBoxInvest, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(lstBoxInvest, 1);
                    if (IdDependentField)
                        FTitle.Visibility = lstBoxInvest.Visibility = Visibility.Collapsed;
                    PItem.Control = lstBoxInvest;
                    Container.Children.Add(lstBoxInvest);
                    break;
                    #endregion
                case 12:
                    #region Case 12 Main
                    ListOfCheckBoxesFieldSetting listSetting1 = ((ListOfCheckBoxesFieldSetting)PItem.Settings);
                    //ListBox lbList1 = new ListBox();
                    ComboBox lbList1 = new ComboBox();
                    if (TemplateID == 2)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Nutritional advise")
                        {
                            lbList1.Name = "NutritionAdvise";
                        }
                    }
                    lbList1.SelectionChanged += new SelectionChangedEventHandler(lbList1_SelectionChanged);
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lbList1, tt);
                        #region new added by harish
                        lbList1.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    //lbList1.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                    lbList1.DataContext = PItem;
                    lbList1.IsEnabled = WritePerm;


                    int j = 0;
                    while (j < listSetting1.ItemsSource.Count)
                    {
                        CheckBox chk = new CheckBox();

                        if (listSetting1.ListType == "Nutrition List" && listSetting1.ItemsSource[j] == "Other")
                            chk.Name = "chkOtherN";
                        if (listSetting1.ListType == "Other Alarms" && listSetting1.ItemsSource[j] == "Other")
                            chk.Name = "chkOtherA";

                        chk.Tag = j;
                        chk.Checked += new RoutedEventHandler(chk_Checked);
                        chk.Unchecked += new RoutedEventHandler(chk_Unchecked);

                        chk.Content = listSetting1.ItemsSource[j];
                        chk.Margin = new Thickness(2);
                        //chk.Click += new RoutedEventHandler(chk_Click);

                        chk.DataContext = PItem;
                        chk.IsChecked = listSetting1.SelectedItems[j];

                        lbList1.Items.Add(chk);
                        j++;
                    }
                    //lbList1.MaxHeight = 100;
                    lbList1.MaxHeight = 100;
                    lbList1.Margin = new Thickness(2);
                    if (IsView == true)
                    {
                        lbList1.IsEnabled = false;
                    }
                    else
                    {
                        lbList1.IsEnabled = true;
                    }
                    Grid.SetRow(lbList1, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(lbList1, 1);
                    PItem.Control = lbList1;
                    if (IdDependentField)
                        FTitle.Visibility = lbList1.Visibility = Visibility.Visible;
                    Container.Children.Add(lbList1);

                    #region Code for Other Nutrition TextBox
                    RowDefinition Row2 = new RowDefinition();
                    Row2.Height = new GridLength(23, GridUnitType.Auto);
                    Container.RowDefinitions.Add(Row2);

                    TextBlock FTitle1 = new TextBlock();
                    if (!listSetting1.IsOtherText)
                        FTitle1.Visibility = Visibility.Collapsed;

                    if (listSetting1.ListType == "Nutrition List")
                    {
                        FTitle1.Name = "ONTitle";
                        FTitle1.Text = "Other Nutrition : ";
                    }
                    if (listSetting1.ListType == "Other Alarms")
                    {
                        FTitle1.Name = "OATitle";
                        FTitle1.Text = "Other Alarms : ";
                    }
                    FTitle1.HorizontalAlignment = HorizontalAlignment.Right;
                    FTitle1.VerticalAlignment = VerticalAlignment.Center;
                    FTitle1.Margin = new Thickness(2);

                    Grid.SetRow(FTitle1, Container.RowDefinitions.Count - 1);
                    Container.Children.Add(FTitle1);

                    TextBox Field2 = new TextBox();
                    if (!listSetting1.IsOtherText)
                        Field2.Visibility = Visibility.Collapsed;
                    if (listSetting1.ListType == "Nutrition List")
                    {
                        Field2.Name = "ONField";
                    }
                    if (listSetting1.ListType == "Other Alarms")
                    {
                        Field2.Name = "OAField";
                    }

                    Field2.DataContext = PItem;
                    Field2.IsEnabled = WritePerm;
                    Binding binding2 = new Binding("Settings.OtherText");
                    binding2.Mode = BindingMode.TwoWay;
                    Field2.SetBinding(TextBox.TextProperty, binding2);
                    Field2.Margin = new Thickness(2);
                    Grid.SetRow(Field2, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(Field2, 1);

                    Field2.AcceptsReturn = true;
                    Field2.Height = 60;
                    Field2.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

                    Container.Children.Add(Field2);
                    #endregion
                    break;
                    #endregion
                case 13:
                    #region Case 13 Main
                    AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)PItem.Settings);
                    switch (AutolistSetting.ControlType)
                    {
                        case AutoListControlType.ComboBox:
                            #region Region for Auto Combo
                            ComboBox cmbList = new ComboBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(cmbList, tt);
                                #region new added by harish
                                cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            cmbList.IsEnabled = WritePerm;
                            cmbList.DataContext = PItem;
                            PItem.Control = cmbList;


                            cmbList.DisplayMemberPath = "Description";
                            cmbList.SelectedValuePath = "ID";
                            /////////////Added By Nilesh R
                            var itemComboSingleSel = from EDT in EmrDetails
                                                     where (EDT.ControlName == PItem.Name
                                                     )
                                                     select EDT;
                            ////////////////////////////////////////
                            if (AutolistSetting.IsDynamic == true)
                            {
                                clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                BizAction.MasterList = new List<MasterListItem>();
                                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                client1.ProcessCompleted += (s1, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                        ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;

                                        Binding Sourcebinding = new Binding("Settings.ItemSource");
                                        Sourcebinding.Mode = BindingMode.OneWay;
                                        cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                        Binding SIbinding = new Binding("Settings.SelectedItem");
                                        SIbinding.Mode = BindingMode.TwoWay;
                                        cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);

                                        if (itemComboSingleSel.ToList().Count > 0)
                                        {
                                            foreach (var item in itemComboSingleSel)
                                            {
                                                if (item.ControlName == PItem.Name)
                                                {
                                                    if (item.Value != null && item.Value != "")
                                                    {
                                                        try
                                                        {

                                                            cmbList.SelectedItem = ((List<MasterListItem>)cmbList.ItemsSource).First(dd => dd.Description == item.Value);

                                                        }
                                                        catch (Exception) { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                };
                                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client1.CloseAsync();
                            }
                            else
                            {
                                Binding Sourcebinding = new Binding("Settings.ItemSource");
                                Sourcebinding.Mode = BindingMode.OneWay;
                                cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                Binding SIbinding = new Binding("Settings.SelectedItem");
                                SIbinding.Mode = BindingMode.TwoWay;
                                cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                            }
                            cmbList.Margin = new Thickness(2);
                            Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(cmbList, 1);
                            //cmbList.ItemsSource = AutolistSetting.ItemSource;
                            if (AutolistSetting.SelectedItem != null)
                                cmbList.SelectedValue = AutolistSetting.SelectedItem.ID;

                            if (AutolistSetting.IsDynamic == false)
                            {
                                if (itemComboSingleSel.ToList().Count > 0)
                                {
                                    foreach (var item in itemComboSingleSel)
                                    {
                                        if (item.ControlName == PItem.Name)
                                        {
                                            if (item.Value != null && item.Value != "")
                                            {
                                                try
                                                {
                                                    cmbList.SelectedItem = ((List<MasterListItem>)cmbList.ItemsSource).First(dd => dd.Description == item.Value);
                                                }
                                                catch (Exception) { }
                                            }
                                        }
                                    }
                                }
                            }
                            ///////////////////////////
                            cmbList.SelectionChanged += new SelectionChangedEventHandler(AutoComboList_SelectionChanged);
                            if (PItem.IsRequired)
                            {
                                cmbList.SetValidation(PItem.Title + " is required.");
                                cmbList.RaiseValidationError();
                            }
                            if (IdDependentField)
                                FTitle.Visibility = cmbList.Visibility = Visibility.Visible;
                            Container.Children.Add(cmbList);
                            #endregion
                            break;
                        case AutoListControlType.ListBox:
                            switch (AutolistSetting.ChoiceMode)
                            {
                                case SelectionMode.Single:
                                    //Region for Auto List (SelectionMode-Single)
                                    #region Region for Auto List (SelectionMode-Single)
                                    ListBox lbListSingle = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbListSingle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbListSingle.IsEnabled = WritePerm;
                                    lbListSingle.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                                    lbListSingle.DataContext = PItem;
                                    lbListSingle.DisplayMemberPath = "Description";
                                    lbListSingle.SelectedValuePath = "ID";
                                    /////////////Added By Nilesh R
                                    var itemSelectSingle = from EDT in EmrDetails
                                                           where (EDT.ControlName == PItem.Name
                                                           )
                                                           select EDT;
                                    if (AutolistSetting.IsDynamic == true)
                                    {
                                        clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                        BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                        BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                        BizAction.MasterList = new List<MasterListItem>();
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s1, args) =>
                                        {
                                            if (args.Error == null && args.Result != null)
                                            {
                                                List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                                ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;

                                                Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                                Sourcebinding1.Mode = BindingMode.OneWay;
                                                lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                                Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                                SIbinding1.Mode = BindingMode.TwoWay;
                                                lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);


                                                if (itemSelectSingle.ToList().Count > 0)
                                                {
                                                    foreach (var item in itemSelectSingle)
                                                    {
                                                        if (item.ControlName == PItem.Name)
                                                        {
                                                            if (item.Value != null && item.Value != "")
                                                            {
                                                                try
                                                                {
                                                                    lbListSingle.SelectedItem = ((List<MasterListItem>)lbListSingle.ItemsSource).First(dd => dd.Description == item.Value);
                                                                }
                                                                catch (Exception) { }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        };
                                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client1.CloseAsync();
                                    }
                                    else
                                    {
                                        Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                        Sourcebinding1.Mode = BindingMode.OneWay;
                                        lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                        Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                        SIbinding1.Mode = BindingMode.TwoWay;
                                        lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);
                                    }
                                    lbListSingle.MaxHeight = 100;

                                    lbListSingle.Margin = new Thickness(2);

                                    Grid.SetRow(lbListSingle, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbListSingle, 1);

                                    //lbListSingle.ItemsSource = AutolistSetting.ItemSource;
                                    if (AutolistSetting.SelectedItem != null)
                                        lbListSingle.SelectedValue = AutolistSetting.SelectedItem.ID;

                                    if (AutolistSetting.IsDynamic == false)
                                    {

                                        if (itemSelectSingle.ToList().Count > 0)
                                        {
                                            foreach (var item in itemSelectSingle)
                                            {
                                                if (item.ControlName == PItem.Name)
                                                {
                                                    if (item.Value != null && item.Value != "")
                                                    {
                                                        try
                                                        {
                                                            lbListSingle.SelectedItem = ((List<MasterListItem>)lbListSingle.ItemsSource).First(dd => dd.Description == item.Value);
                                                        }
                                                        catch (Exception) { }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    ///////////////////////////

                                    lbListSingle.SelectionChanged += new SelectionChangedEventHandler(AutoComboList_SelectionChanged);
                                    PItem.Control = lbListSingle;

                                    if (IdDependentField)
                                        FTitle.Visibility = lbListSingle.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbListSingle);
                                    #endregion
                                    break;
                                case SelectionMode.Multiples:
                                    //Region for Auto List (SelectionMode-Multiple)
                                    #region Region for Auto List (SelectionMode-Multiple)
                                    ListBox lbList = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbList.IsEnabled = WritePerm;
                                    lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                                    lbList.DataContext = PItem;
                                    //// Added By Nilesh R
                                    var itemMultiSelList = from EDT in EmrDetails
                                                           where (EDT.ControlName == PItem.Name
                                                           )
                                                           select EDT;
                                    ///////////END///////////
                                    if (AutolistSetting.IsDynamic == true)
                                    {
                                        clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                        BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                        BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                        BizAction.MasterList = new List<MasterListItem>();
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s1, args) =>
                                        {
                                            if (args.Error == null && args.Result != null)
                                            {
                                                List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                                ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;

                                                Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                                Sourcebinding2.Mode = BindingMode.OneWay;
                                                lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);

                                                if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                    foreach (var item in AutolistSetting.SelectedItems)
                                                    {
                                                        lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                                    }
                                                lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);


                                                if (itemMultiSelList.ToList().Count > 0)
                                                {
                                                    foreach (var item in itemMultiSelList)
                                                    {
                                                        if (item.ControlName == PItem.Name)
                                                        {
                                                            if (item.Value != null && item.Value != "")
                                                            {
                                                                try
                                                                {
                                                                    // cmbList.SelectedItem = List < MasterListItem > cmbList.ItemsSource.Where(i => i.Title == item.Value).Single();
                                                                    //cmbList.SelectedItem = ((List<MasterListItem>)cmbList.ItemsSource).First(dd => dd.Description == item.Value);
                                                                    lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.Description == item.Value).Single());

                                                                }
                                                                catch (Exception) { }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        };
                                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client1.CloseAsync();
                                    }
                                    else
                                    {
                                        Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                        Sourcebinding2.Mode = BindingMode.OneWay;
                                        lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);

                                        if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                            foreach (var item in AutolistSetting.SelectedItems)
                                            {
                                                lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                            }
                                        lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);
                                    }
                                    lbList.MaxHeight = 100;
                                    lbList.DisplayMemberPath = "Description";
                                    //lbList.SelectedValuePath = "ID";
                                    lbList.Margin = new Thickness(2);
                                    //lbList.SelectionChanged += new SelectionChangedEventHandler(lbAutoList_SelectionChanged);
                                    Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbList, 1);
                                    //lbList.ItemsSource = AutolistSetting.ItemSource;
                                    ///////////////////// Added By Nilesh R
                                    if (AutolistSetting.IsDynamic == false)
                                    {

                                        if (itemMultiSelList.ToList().Count > 0)
                                        {
                                            foreach (var item in itemMultiSelList)
                                            {
                                                if (item.ControlName == PItem.Name)
                                                {
                                                    if (item.Value != null && item.Value != "")
                                                    {
                                                        try
                                                        {
                                                            lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.Description == item.Value).Single());
                                                        }
                                                        catch (Exception) { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    /////////////////////
                                    PItem.Control = lbList;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbList);
                                    #endregion
                                    break;
                            }
                            break;
                        case AutoListControlType.CheckListBox:
                            FTitle.VerticalAlignment = VerticalAlignment.Top;
                            FTitle.Margin = new Thickness(2, 8, 2, 0);
                            ListBox CheckListBox = new ListBox();
                            CheckListBox.MaxHeight = 100;
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lstBox, tt);
                                #region new added by harish
                                CheckListBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            CheckListBox.IsEnabled = WritePerm;
                            CheckListBox.DataContext = PItem;
                            CheckListBox.Tag = PItem;
                            CheckListBox.Margin = new Thickness(2);
                            //// Added By Nilesh R
                            var itemCheckList = from EDT in EmrDetails
                                                where (EDT.ControlName == PItem.Name
                                                )
                                                select EDT;
                            ///////////END///////////
                            if (AutolistSetting.IsDynamic == true)
                            {
                                clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                                BizAction.MasterTable = ((MasterListItem)AutolistSetting.SelectedTable).Description;
                                BizAction.ColumnName = ((MasterListItem)AutolistSetting.SelectedColumn).Description;

                                BizAction.MasterList = new List<MasterListItem>();
                                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                client1.ProcessCompleted += (s1, args) =>
                                {
                                    if (args.Error == null && args.Result != null)
                                    {
                                        List<MasterListItem> SourceList = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                                        if (AutolistSetting.ControlType == AutoListControlType.CheckListBox)
                                        {
                                            foreach (var SLI in SourceList)
                                            {
                                                foreach (var ItemSourceItem in ((AutomatedListFieldSetting)PItem.Settings).ItemSource)
                                                {
                                                    if (SLI.ID == ItemSourceItem.ID)
                                                    {
                                                        SLI.Status = ItemSourceItem.Status;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        ((AutomatedListFieldSetting)PItem.Settings).ItemSource = SourceList;
                                        for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                        {
                                            CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                            CLBCI.chkItemClicked += new RoutedEventHandler(CLBCI_chkItemClicked);
                                            CLBCI.DataContext = AutolistSetting.ItemSource[k];
                                            /////////////Added By Nilesh R
                                            if (itemCheckList.ToList().Count > 0)
                                            {
                                                foreach (var item in itemCheckList)
                                                {
                                                    if (item.Value == AutolistSetting.ItemSource[k].Description)
                                                    {
                                                        AutolistSetting.ItemSource[k].Status = true;
                                                        AutolistSetting.ItemSource[k].Selected = true;
                                                        CLBCI.IsSelected = true;
                                                    }
                                                }
                                            }
                                            ///////////////////END/////////////////
                                            CLBCI.Tag = PItem;
                                            CheckListBox.Items.Add(CLBCI);
                                        }
                                    }
                                };
                                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client1.CloseAsync();
                            }
                            else
                            {
                                for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                {
                                    CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                    CLBCI.chkItemClicked += new RoutedEventHandler(CLBCI_chkItemClicked);
                                    CLBCI.DataContext = AutolistSetting.ItemSource[k];
                                    /////////////Added By Nilesh R
                                    if (itemCheckList.ToList().Count > 0)
                                    {
                                        foreach (var item in itemCheckList)
                                        {
                                            if (item.Value == AutolistSetting.ItemSource[k].Description)
                                            {
                                                AutolistSetting.ItemSource[k].Status = true;
                                                AutolistSetting.ItemSource[k].Selected = true;
                                                CLBCI.IsSelected = true;
                                            }
                                        }
                                    }
                                    ///////////////////////////
                                    CLBCI.Tag = PItem;
                                    CheckListBox.Items.Add(CLBCI);
                                }
                            }
                            if (IsView == true)
                            {
                                CheckListBox.IsEnabled = false;
                            }
                            else
                            {
                                CheckListBox.IsEnabled = true;
                            }

                            Grid.SetRow(CheckListBox, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(CheckListBox, 1);
                            if (IdDependentField)
                                FTitle.Visibility = CheckListBox.Visibility = Visibility.Collapsed;
                            PItem.Control = CheckListBox;
                            Container.Children.Add(CheckListBox);
                            break;
                    }
                    break;
                    #endregion
                //Added by Saily P on 29.11.13 Purpose - Added new control
                case 18:
                    IsBPControl = true;
                    if (GetBPFromDB == true)
                    {
                        BPControlSetting BPSetting = ((BPControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);

                        ListBox OtherlstBox = new ListBox();

                        OtherlstBox.DataContext = new clsBPControlVO();

                        for (int i = 0; i < BPSetting.ItemsSource.Count; i++)
                        {
                            ((BPControl)BPSetting.ItemsSource[i]).BP1 = objBPDetails.BP1;
                            ((BPControl)BPSetting.ItemsSource[i]).BP2 = objBPDetails.BP2;
                            BPControlItem OtBPmrci = new BPControlItem();
                            OtBPmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            BPSetting.ItemsSource[i].Index = i;
                            BPSetting.ItemsSource[i].Parent = OtherlstBox;
                            BPSetting.ItemsSource[i].BPSetting = BPSetting;
                            OtBPmrci.DataContext = BPSetting.ItemsSource[i];
                            OtherlstBox.Items.Add(OtBPmrci);
                        }

                        Grid.SetRow(OtherlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(OtherlstBox, 1);

                        PItem.Control = OtherlstBox;

                        Container.Children.Add(OtherlstBox);
                    }
                    else
                    {
                        BPControlSetting BPSetting = ((BPControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);
                        ListBox OtherlstBox = new ListBox();
                        #region Added By Harish
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(lstBox, tt);
                            #region new added by harish
                            OtherlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #endregion

                        OtherlstBox.DataContext = PItem;
                        OtherlstBox.IsEnabled = WritePerm;
                        OtherlstBox.Margin = new Thickness(2);
                        for (int i = 0; i < BPSetting.ItemsSource.Count; i++)
                        {
                            BPControlItem OtBPmrci = new BPControlItem();
                            OtBPmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            BPSetting.ItemsSource[i].Index = i;
                            BPSetting.ItemsSource[i].Parent = OtherlstBox;
                            BPSetting.ItemsSource[i].BPSetting = BPSetting;
                            OtBPmrci.DataContext = BPSetting.ItemsSource[i];
                            OtherlstBox.Items.Add(OtBPmrci);
                        }

                        if (IsView == true)
                        {
                            OtherlstBox.IsEnabled = false;
                        }
                        else
                        {
                            OtherlstBox.IsEnabled = true;
                        }


                        Grid.SetRow(OtherlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(OtherlstBox, 1);
                        if (IdDependentField)
                            FTitle.Visibility = OtherlstBox.Visibility = Visibility.Collapsed;
                        PItem.Control = OtherlstBox;
                        Container.Children.Add(OtherlstBox);
                    }

                    break;
                case 19:
                    IsVisionControl = true;
                    if (GetVisionFromDB == true)
                    {
                        VisionControlSetting VisionSetting = ((VisionControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);

                        ListBox VisionlstBox = new ListBox();
                        VisionlstBox.DataContext = new clsVisionVO();

                        for (int i = 0; i < VisionSetting.ItemsSource.Count; i++)
                        {
                            ((VisionControl)VisionSetting.ItemsSource[i]).SPH = objVisionDetails.SPH;
                            ((VisionControl)VisionSetting.ItemsSource[i]).CYL = objVisionDetails.CYL;
                            ((VisionControl)VisionSetting.ItemsSource[i]).Axis = objVisionDetails.Axis;

                            VisionControlItem Visionmrci = new VisionControlItem();
                            Visionmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            VisionSetting.ItemsSource[i].Index = i;
                            VisionSetting.ItemsSource[i].Parent = VisionlstBox;
                            VisionSetting.ItemsSource[i].VisionSetting = VisionSetting;
                            Visionmrci.DataContext = VisionSetting.ItemsSource[i];
                            VisionlstBox.Items.Add(Visionmrci);
                        }

                        Grid.SetRow(VisionlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(VisionlstBox, 1);

                        PItem.Control = VisionlstBox;
                        Container.Children.Add(VisionlstBox);
                    }
                    else
                    {
                        VisionControlSetting VisionSetting = ((VisionControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);
                        ListBox VisionlstBox = new ListBox();
                        #region Added By Harish
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(lstBox, tt);
                            #region new added by harish
                            VisionlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #endregion
                        VisionlstBox.DataContext = PItem;
                        VisionlstBox.IsEnabled = WritePerm;
                        VisionlstBox.Margin = new Thickness(2);
                        for (int i = 0; i < VisionSetting.ItemsSource.Count; i++)
                        {
                            VisionControlItem Visionmrci = new VisionControlItem();
                            Visionmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            VisionSetting.ItemsSource[i].Index = i;
                            VisionSetting.ItemsSource[i].Parent = VisionlstBox;
                            VisionSetting.ItemsSource[i].VisionSetting = VisionSetting;
                            Visionmrci.DataContext = VisionSetting.ItemsSource[i];
                            VisionlstBox.Items.Add(Visionmrci);
                        }

                        if (IsView == true)
                        {
                            VisionlstBox.IsEnabled = false;
                        }
                        else
                        {
                            VisionlstBox.IsEnabled = true;
                        }
                        Grid.SetRow(VisionlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(VisionlstBox, 1);
                        if (IdDependentField)
                            FTitle.Visibility = VisionlstBox.Visibility = Visibility.Collapsed;
                        PItem.Control = VisionlstBox;
                        Container.Children.Add(VisionlstBox);
                    }
                    break;
                case 20:
                    IsGPControl = true;
                    if (GetGPFromDB == true)
                    {
                        GlassPowerControlSetting GPSetting = ((GlassPowerControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);

                        ListBox GPlstBox = new ListBox();
                        GPlstBox.DataContext = new clsGlassPowerVO();
                        for (int i = 0; i < GPSetting.ItemsSource.Count; i++)
                        {
                            ((GlassPower)GPSetting.ItemsSource[i]).SPH1 = objGlassPower.SPH1;
                            ((GlassPower)GPSetting.ItemsSource[i]).CYL1 = objGlassPower.CYL1;
                            ((GlassPower)GPSetting.ItemsSource[i]).Axis1 = objGlassPower.Axis1;
                            ((GlassPower)GPSetting.ItemsSource[i]).VA1 = objGlassPower.VA1;
                            ((GlassPower)GPSetting.ItemsSource[i]).CYL2 = objGlassPower.CYL2;
                            ((GlassPower)GPSetting.ItemsSource[i]).Axis2 = objGlassPower.Axis2;
                            ((GlassPower)GPSetting.ItemsSource[i]).SPH2 = objGlassPower.SPH2;
                            ((GlassPower)GPSetting.ItemsSource[i]).VA2 = objGlassPower.VA2;


                            GlassPowerControlItem GPmrci = new GlassPowerControlItem();
                            GPmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            GPSetting.ItemsSource[i].Index = i;
                            GPSetting.ItemsSource[i].Parent = GPlstBox;
                            GPSetting.ItemsSource[i].GlassPowerSetting = GPSetting;
                            GPmrci.DataContext = GPSetting.ItemsSource[i];
                            GPlstBox.Items.Add(GPmrci);
                        }
                        Grid.SetRow(GPlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(GPlstBox, 1);

                        PItem.Control = GPlstBox;
                        Container.Children.Add(GPlstBox);
                    }
                    else
                    {
                        GlassPowerControlSetting GPSetting = ((GlassPowerControlSetting)PItem.Settings);
                        FTitle.VerticalAlignment = VerticalAlignment.Top;
                        FTitle.Margin = new Thickness(2, 8, 2, 0);
                        ListBox GPlstBox = new ListBox();
                        #region Added By Harish
                        if (PItem.IsToolTip == true)
                        {
                            //ToolTipService.SetToolTip(lstBox, tt);
                            #region new added by harish
                            GPlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            #endregion
                        }
                        #endregion
                        GPlstBox.DataContext = PItem;
                        GPlstBox.IsEnabled = WritePerm;
                        GPlstBox.Margin = new Thickness(2);
                        for (int i = 0; i < GPSetting.ItemsSource.Count; i++)
                        {
                            GlassPowerControlItem GPmrci = new GlassPowerControlItem();
                            GPmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                            GPSetting.ItemsSource[i].Index = i;
                            GPSetting.ItemsSource[i].Parent = GPlstBox;
                            GPSetting.ItemsSource[i].GlassPowerSetting = GPSetting;
                            GPmrci.DataContext = GPSetting.ItemsSource[i];
                            GPlstBox.Items.Add(GPmrci);
                        }

                        if (IsView == true)
                        {
                            GPlstBox.IsEnabled = false;
                        }
                        else
                        {
                            GPlstBox.IsEnabled = true;
                        }
                        Grid.SetRow(GPlstBox, Container.RowDefinitions.Count - 1);
                        Grid.SetColumn(GPlstBox, 1);
                        if (IdDependentField)
                            FTitle.Visibility = GPlstBox.Visibility = Visibility.Collapsed;
                        PItem.Control = GPlstBox;
                        Container.Children.Add(GPlstBox);
                    }
                    break;
                case 14:
                    #region Case 14 Main  --Not Use
                    OtherMedicationFieldSetting OtherMedSetting = ((OtherMedicationFieldSetting)PItem.Settings);
                    FTitle.VerticalAlignment = VerticalAlignment.Top;
                    FTitle.Margin = new Thickness(2, 8, 2, 0);
                    ListBox OtherMedlstBox = new ListBox();
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(lstBox, tt);
                        #region new added by harish
                        OtherMedlstBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    OtherMedlstBox.DataContext = PItem;
                    OtherMedlstBox.IsEnabled = WritePerm;
                    OtherMedlstBox.Margin = new Thickness(2);


                    if (TemplateID == 4)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other antiallergics")
                        {
                            OtherMedlstBox.Name = "ManagementOtherAntiallergics";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Others")
                        {
                            OtherMedlstBox.Name = "ManagementOthers";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other drugs")
                        {
                            OtherMedlstBox.Name = "ManagementOtherDrugs";
                        }
                    }
                    else if (TemplateID == 5)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Others")
                        {
                            OtherMedlstBox.Name = "ManagementOthers";
                        }
                    }
                    else if (TemplateID == 6)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other antibiotics")
                        {
                            OtherMedlstBox.Name = "ManagementOtherAntibiotics";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other drugs")
                        {
                            OtherMedlstBox.Name = "ManagementOtherDrugs";
                        }
                    }
                    else if (TemplateID == 7)
                    {
                        if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other antibiotics")
                        {
                            OtherMedlstBox.Name = "ManagementOtherAntibiotics";
                        }
                        else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Management" && PItem.Title == "Other drugs")
                        {
                            OtherMedlstBox.Name = "ManagementOtherDrugs";
                        }
                    }


                    for (int i = 0; i < OtherMedSetting.ItemsSource.Count; i++)
                    {
                        OtherMedicatioRepeterControlItem Otmrci = new OtherMedicatioRepeterControlItem();
                        Otmrci.OnAddRemoveClick += new RoutedEventHandler(Otmrci_OnAddRemoveClick);
                        Otmrci.cmbSelectionChanged += new RoutedEventHandler(Otmrci_cmbSelectionChanged);
                        Otmrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                        Otmrci.txtDayChanged += new TextChangedEventHandler(mrci_txtDayChanged);
                        Otmrci.txtFreqChanged += new TextChangedEventHandler(mrci_txtFreqChanged);
                        Otmrci.txtQtyChanged += new TextChangedEventHandler(mrci_txtQtyChanged);

                        //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                        OtherMedSetting.ItemsSource[i].Index = i;
                        OtherMedSetting.ItemsSource[i].Command = ((i == OtherMedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                        OtherMedSetting.ItemsSource[i].Parent = OtherMedlstBox;
                        OtherMedSetting.ItemsSource[i].MedicationSetting = OtherMedSetting;
                        Otmrci.DataContext = OtherMedSetting.ItemsSource[i];
                        OtherMedlstBox.Items.Add(Otmrci);
                    }
                    if (IsView == true)
                    {
                        OtherMedlstBox.IsEnabled = false;
                    }
                    else
                    {
                        OtherMedlstBox.IsEnabled = true;
                    }
                    Grid.SetRow(OtherMedlstBox, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(OtherMedlstBox, 1);
                    if (IdDependentField)
                        FTitle.Visibility = OtherMedlstBox.Visibility = Visibility.Collapsed;
                    PItem.Control = OtherMedlstBox;
                    Container.Children.Add(OtherMedlstBox);

                    break;
                    #endregion
                case 15:
                    #region Case 15 Main -- Not Use
                    InvestigationFieldSetting InvestAutolistSetting = ((InvestigationFieldSetting)PItem.Settings);
                    switch (InvestAutolistSetting.ControlType)
                    {
                        case AutoListControlType.ComboBox:
                            #region Region for Auto Combo
                            ComboBox cmbList = new ComboBox();
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(cmbList, tt);
                                #region new added by harish
                                cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }
                            cmbList.IsEnabled = WritePerm;
                            cmbList.DataContext = PItem;
                            PItem.Control = cmbList;

                            cmbList.DisplayMemberPath = "Description";
                            cmbList.SelectedValuePath = "ID";

                            Binding Sourcebinding = new Binding("Settings.ItemSource");
                            Sourcebinding.Mode = BindingMode.OneWay;
                            cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                            Binding SIbinding = new Binding("Settings.SelectedItem");
                            SIbinding.Mode = BindingMode.TwoWay;
                            cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);
                            cmbList.Margin = new Thickness(2);
                            Grid.SetRow(cmbList, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(cmbList, 1);
                            //cmbList.ItemsSource = AutolistSetting.ItemSource;
                            if (InvestAutolistSetting.SelectedItem != null)
                                cmbList.SelectedValue = InvestAutolistSetting.SelectedItem.ID;

                            cmbList.SelectionChanged += new SelectionChangedEventHandler(InvestAutoComboList_SelectionChanged);
                            if (PItem.IsRequired)
                            {
                                cmbList.SetValidation(PItem.Title + " is required.");
                                cmbList.RaiseValidationError();
                            }
                            if (IdDependentField)
                                FTitle.Visibility = cmbList.Visibility = Visibility.Visible;
                            Container.Children.Add(cmbList);
                            #endregion
                            break;
                        case AutoListControlType.ListBox:
                            switch (InvestAutolistSetting.ChoiceMode)
                            {
                                case SelectionMode.Single:
                                    //Region for Auto List (SelectionMode-Single)
                                    #region Region for Auto List (SelectionMode-Single)
                                    ListBox lbListSingle = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbListSingle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbListSingle.IsEnabled = WritePerm;
                                    lbListSingle.SelectionMode = System.Windows.Controls.SelectionMode.Single;
                                    lbListSingle.DataContext = PItem;
                                    lbListSingle.DisplayMemberPath = "Description";
                                    lbListSingle.SelectedValuePath = "ID";
                                    Binding Sourcebinding1 = new Binding("Settings.ItemSource");
                                    Sourcebinding1.Mode = BindingMode.OneWay;
                                    lbListSingle.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding1);
                                    Binding SIbinding1 = new Binding("Settings.SelectedItem");
                                    SIbinding1.Mode = BindingMode.TwoWay;
                                    lbListSingle.SetBinding(ListBox.SelectedItemProperty, SIbinding1);

                                    lbListSingle.MaxHeight = 100;

                                    lbListSingle.Margin = new Thickness(2);

                                    Grid.SetRow(lbListSingle, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbListSingle, 1);

                                    //lbListSingle.ItemsSource = AutolistSetting.ItemSource;
                                    if (InvestAutolistSetting.SelectedItem != null)
                                        lbListSingle.SelectedValue = InvestAutolistSetting.SelectedItem.ID;

                                    lbListSingle.SelectionChanged += new SelectionChangedEventHandler(InvestAutoComboList_SelectionChanged);
                                    PItem.Control = lbListSingle;

                                    if (IdDependentField)
                                        FTitle.Visibility = lbListSingle.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbListSingle);
                                    #endregion
                                    break;
                                case SelectionMode.Multiples:
                                    //Region for Auto List (SelectionMode-Multiple)
                                    #region Region for Auto List (SelectionMode-Multiple)
                                    ListBox lbList = new ListBox();
                                    if (PItem.IsToolTip == true)
                                    {
                                        //ToolTipService.SetToolTip(lbList, tt);
                                        #region new added by harish
                                        lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                        #endregion
                                    }
                                    lbList.IsEnabled = WritePerm;
                                    lbList.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                                    lbList.DataContext = PItem;

                                    Binding Sourcebinding2 = new Binding("Settings.ItemSource");
                                    Sourcebinding2.Mode = BindingMode.OneWay;
                                    lbList.SetBinding(ListBox.ItemsSourceProperty, Sourcebinding2);

                                    if (InvestAutolistSetting.SelectedItems != null && InvestAutolistSetting.SelectedItems.Count > 0)
                                        foreach (var item in InvestAutolistSetting.SelectedItems)
                                        {
                                            lbList.SelectedItems.Add(InvestAutolistSetting.ItemSource.Where(i => i.ID == item.ID).Single());
                                        }

                                    lbList.MaxHeight = 100;
                                    lbList.DisplayMemberPath = "Description";
                                    //lbList.SelectedValuePath = "ID";
                                    lbList.Margin = new Thickness(2);
                                    lbList.SelectionChanged += new SelectionChangedEventHandler(lbInvestAutoList_SelectionChanged);
                                    Grid.SetRow(lbList, Container.RowDefinitions.Count - 1);
                                    Grid.SetColumn(lbList, 1);
                                    //lbList.ItemsSource = AutolistSetting.ItemSource;

                                    PItem.Control = lbList;
                                    if (IdDependentField)
                                        FTitle.Visibility = lbList.Visibility = Visibility.Visible;
                                    Container.Children.Add(lbList);
                                    #endregion
                                    break;
                            }
                            break;
                        case AutoListControlType.CheckListBox:
                            FTitle.VerticalAlignment = VerticalAlignment.Top;
                            FTitle.Margin = new Thickness(2, 8, 2, 0);
                            ListBox CheckListBox = new ListBox();
                            CheckListBox.MaxHeight = 100;
                            if (PItem.IsToolTip == true)
                            {
                                //ToolTipService.SetToolTip(lstBox, tt);
                                #region new added by harish
                                CheckListBox.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                                #endregion
                            }


                            if (TemplateID == 4)
                            {
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Labs")
                                {
                                    CheckListBox.Name = "InvestLabs";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Radiology")
                                {
                                    CheckListBox.Name = "InvestRadiology";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Labs & services")
                                {
                                    CheckListBox.Name = "InvestAdditionalInvestigations";
                                }

                            }
                            else if (TemplateID == 5)
                            {
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Lab Services")
                                {
                                    CheckListBox.Name = "InvestLabServices";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "ECG")
                                {
                                    CheckListBox.Name = "InvestECG";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Labs")
                                {
                                    CheckListBox.Name = "InvestLabs";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Ambulatory blood pressure monitoring ")
                                {
                                    CheckListBox.Name = "InvestAmbulatoryBloodPressureMonitoring";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Radiology")
                                {
                                    CheckListBox.Name = "InvestRadiology";
                                }
                            }
                            else if (TemplateID == 6)
                            {
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Labs")
                                {
                                    CheckListBox.Name = "InvestLabs";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Radiology")
                                {
                                    CheckListBox.Name = "InvestRadiology";
                                }
                            }
                            else if (TemplateID == 7)
                            {
                                if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Labs")
                                {
                                    CheckListBox.Name = "InvestLabs";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Ova / parasite test")
                                {
                                    CheckListBox.Name = "InvestOvaParasiteTest";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Clinitest or Benedict’s test ")
                                {
                                    CheckListBox.Name = "InvestClinitestOrBenedictsTest";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Stool exam for phenolphthalein and magnesium sulphate")
                                {
                                    CheckListBox.Name = "InvestStoolExamForPhenolphthaleinAndMagnesiumSulphate";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Radiology")
                                {
                                    CheckListBox.Name = "InvestRadiology";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Small bowel biopsy ")
                                {
                                    CheckListBox.Name = "InvestSmallBowelBiopsy ";
                                }
                                else if ((string)((Grid)((Border)Container.Parent).Parent).Tag == "Investigations" && PItem.Title == "Sigmoidoscopy or colonoscopy with biopsies ")
                                {
                                    CheckListBox.Name = "InvestSigmoidoscopyOrColonoscopyWithBiopsies";
                                }
                            }

                            CheckListBox.IsEnabled = WritePerm;
                            CheckListBox.DataContext = PItem;
                            CheckListBox.Tag = PItem;
                            CheckListBox.Margin = new Thickness(2);
                            for (int k = 0; k < InvestAutolistSetting.ItemSource.Count; k++)
                            {
                                CheckListBoxControlItem CLBCI = new CheckListBoxControlItem();
                                CLBCI.chkItemClicked += new RoutedEventHandler(InvestCLBCI_chkItemClicked);
                                CLBCI.DataContext = InvestAutolistSetting.ItemSource[k];
                                CLBCI.Tag = PItem;
                                CheckListBox.Items.Add(CLBCI);
                            }
                            if (IsView == true)
                            {
                                CheckListBox.IsEnabled = false;
                            }
                            else
                            {
                                CheckListBox.IsEnabled = true;
                            }
                            Grid.SetRow(CheckListBox, Container.RowDefinitions.Count - 1);
                            Grid.SetColumn(CheckListBox, 1);
                            if (IdDependentField)
                                FTitle.Visibility = CheckListBox.Visibility = Visibility.Collapsed;
                            PItem.Control = CheckListBox;
                            Container.Children.Add(CheckListBox);
                            break;
                    }
                    break;
                    #endregion
                case 16:
                    #region Case 16 Main -- Use
                    TimePicker TP = new TimePicker();
                    TP.Width = (double)100;
                    TP.HorizontalAlignment = HorizontalAlignment.Left;
                    #region Added By Harish
                    if (PItem.IsToolTip == true)
                    {
                        //ToolTipService.SetToolTip(dtp, tt);
                        #region new added by harish
                        TP.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        #endregion
                    }
                    #endregion
                    TP.IsEnabled = WritePerm;
                    #region new added by harish
                    TP.DataContext = PItem;
                    #endregion
                    TP.Margin = new Thickness(2);
                    binding = new Binding("Settings.Time");
                    binding.Mode = BindingMode.TwoWay;
                    TP.SetBinding(TimePicker.ValueProperty, binding);
                    if (IsView == true)
                    {
                        TP.IsEnabled = false;
                    }
                    else
                    {
                        TP.IsEnabled = true;
                    }
                    //////////////////////////////Added By Nilesh

                    var itemTime = from EDT in EmrDetails
                                   where (EDT.ControlName == PItem.Name
                                   )
                                   select EDT;

                    if (itemTime.ToList().Count > 0)
                    {
                        foreach (var item in itemTime)
                        {
                            if (item.ControlName == PItem.Name)
                            {
                                if (item.Value != null)
                                    try
                                    {
                                        TP.Value = Convert.ToDateTime(item.Value);
                                        //dtp.SelectedDate = Convert.ToDateTime(item.Value);
                                    }
                                    catch (Exception) { }
                            }
                        }
                    }
                    /////////////////////////////////////////////
                    Grid.SetRow(TP, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(TP, 1);
                    PItem.Control = TP;
                    if (IdDependentField)
                        FTitle.Visibility = TP.Visibility = Visibility.Collapsed;

                    Container.Children.Add(TP);
                    break;
                    #endregion
            }
            if (PItem.DependentFieldDetail != null && PItem.DependentFieldDetail.Count > 0)
                foreach (var item in PItem.DependentFieldDetail)
                {
                    AddNodeItems(Container, item, true);
                }
        }

        void FUrci_OnViewClick(object sender, RoutedEventArgs e)
        {
            var lstBox = ((FileUpload)((HyperlinkButton)sender).DataContext).Parent;
            var FUSetting = ((FileUpload)((HyperlinkButton)sender).DataContext).FileUploadSetting;

            if (((FileUpload)((HyperlinkButton)sender).DataContext).FileName != null && ((FileUpload)((HyperlinkButton)sender).DataContext).FileName != "")
            {
                if (((FileUpload)((HyperlinkButton)sender).DataContext).Data != null)
                {
                    string FullFile = ((FieldDetail)lstBox.DataContext).Name + ((FileUpload)((HyperlinkButton)sender).DataContext).Index + ((FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                            listOfReports.Add(FullFile);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((FileUpload)((HyperlinkButton)sender).DataContext).Data);
                }
                else
                {
                    clsGetPatientEMRUploadedFilesBizActionVO BizActionFU = new clsGetPatientEMRUploadedFilesBizActionVO();
                    BizActionFU.ControlName = ((FieldDetail)lstBox.DataContext).Name;
                    BizActionFU.PatientID = CurrentVisit.PatientId;
                    BizActionFU.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionFU.VisitID = CurrentVisit.ID;


                    BizActionFU.TemplateID = TemplateID;
                    BizActionFU.UnitID = CurrentVisit.UnitId;
                    BizActionFU.ControlIndex = ((FileUpload)((HyperlinkButton)sender).DataContext).Index;

                    Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient clientFU = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);
                    clientFU.ProcessCompleted += (s1, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            List<clsPatientEMRUploadedFilesVO> lstFU = ((clsGetPatientEMRUploadedFilesBizActionVO)args.Result).objPatientEMRUploadedFiles;
                            if (lstFU != null && lstFU.Count > 0)
                            {
                                for (int i = 0; i < lstFU.Count; i++)
                                {
                                    //FUSetting.ItemsSource[lstFU[i].ControlIndex].Data = lstFU[i].Value;
                                    ((FileUpload)((HyperlinkButton)sender).DataContext).Data = lstFU[i].Value;
                                }

                                string FullFile = ((FieldDetail)lstBox.DataContext).Name + ((FileUpload)((HyperlinkButton)sender).DataContext).Index + ((FileUpload)((HyperlinkButton)sender).DataContext).FileName;

                                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                client.GlobalUploadFileCompleted += (s, args1) =>
                                {
                                    if (args1.Error == null)
                                    {
                                        HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + FullFile });
                                        listOfReports.Add(FullFile);
                                    }
                                };
                                client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", FullFile, ((FileUpload)((HyperlinkButton)sender).DataContext).Data);
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "File cannot be loaded.\nError occured during file retrieving.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                            mgbx.Show();
                        }
                    };
                    clientFU.ProcessAsync(BizActionFU, ((IApplicationConfiguration)App.Current).CurrentUser);
                    clientFU.CloseAsync();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This File is not uploaded. Please upload the File then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }

        void FUrci_OnBrowseClick(object sender, RoutedEventArgs e)
        {
            var lstBox = ((FileUpload)((Button)sender).DataContext).Parent;
            var FUSetting = ((FileUpload)((Button)sender).DataContext).FileUploadSetting;

            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                ((FileUpload)((Button)sender).DataContext).FileName = openDialog.File.Name;
                ((TextBox)((Grid)((Button)sender).Parent).FindName("FileName")).Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        ((FileUpload)((Button)sender).DataContext).Data = new byte[stream.Length];
                        stream.Read(((FileUpload)((Button)sender).DataContext).Data, 0, (int)stream.Length);
                        ((FileUpload)((Button)sender).DataContext).FileInfo = openDialog.File;
                    }
                }
                catch (Exception)
                {
                    string msgText = "Error while reading file.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        void Otmrci_cmbSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).SelectedItem != null && IsFirstTime == false)
            {
                if ((string)((AutoCompleteComboBox)sender).Tag == "Route")
                {
                    ((OtherMedication)((AutoCompleteComboBox)sender).DataContext).Route = (MasterListItem)((AutoCompleteComboBox)sender).SelectedItem;
                }
            }

        }

        void Otmrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            var lstBox = ((OtherMedication)((HyperlinkButton)sender).DataContext).Parent;
            var MedSetting = ((OtherMedication)((HyperlinkButton)sender).DataContext).MedicationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                MedSetting.ItemsSource.RemoveAt(((OtherMedication)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                MedSetting.ItemsSource.Add(new OtherMedication() { RouteSource = Helpers.GetRouteList() });
            }
            lstBox.Items.Clear();
            for (int i = 0; i < MedSetting.ItemsSource.Count; i++)
            {
                OtherMedicatioRepeterControlItem mrci = new OtherMedicatioRepeterControlItem();
                mrci.OnAddRemoveClick += new RoutedEventHandler(Otmrci_OnAddRemoveClick);
                mrci.cmbSelectionChanged += new RoutedEventHandler(Otmrci_cmbSelectionChanged);
                mrci.txtKeyDown += new KeyEventHandler(DecField_KeyDown);
                mrci.txtDayChanged += new TextChangedEventHandler(mrci_txtDayChanged);
                mrci.txtFreqChanged += new TextChangedEventHandler(mrci_txtFreqChanged);
                mrci.txtQtyChanged += new TextChangedEventHandler(mrci_txtQtyChanged);

                //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                MedSetting.ItemsSource[i].Index = i;
                MedSetting.ItemsSource[i].Command = ((i == MedSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                MedSetting.ItemsSource[i].Parent = lstBox;
                MedSetting.ItemsSource[i].MedicationSetting = MedSetting;
                mrci.DataContext = MedSetting.ItemsSource[i];
                lstBox.Items.Add(mrci);
            }
        }

        void mrci_txtQtyChanged(object sender, TextChangedEventArgs e)
        {
            if (IsFirstTime == false)
            {
                if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsPositiveNumberValid())
                {

                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                if (((TextBox)sender).Text == "")
                    ((TextBox)sender).Text = "0";

                if (((TextBox)sender).DataContext is Medication)
                {
                    ((Medication)((TextBox)sender).DataContext).Quantity = Convert.ToInt32(((TextBox)sender).Text);
                }
                else if (((TextBox)sender).DataContext is OtherMedication)
                {
                    ((OtherMedication)((TextBox)sender).DataContext).Quantity = Convert.ToInt32(((TextBox)sender).Text);
                }
            }
        }

        void mrci_txtFreqChanged(object sender, TextChangedEventArgs e)
        {
            if (IsFirstTime == false)
            {
                if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsPositiveNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                if (((TextBox)sender).Text == "")
                    ((TextBox)sender).Text = "0";

                if (((TextBox)sender).DataContext is Medication)
                {
                    ((Medication)((TextBox)sender).DataContext).Frequency = Convert.ToInt32(((TextBox)sender).Text);
                    ((Medication)((TextBox)sender).DataContext).Quantity = ((Medication)((TextBox)sender).DataContext).Day * Convert.ToInt32(((Medication)((TextBox)sender).DataContext).Frequency);
                }
                else if (((TextBox)sender).DataContext is OtherMedication)
                {
                    ((OtherMedication)((TextBox)sender).DataContext).Frequency = Convert.ToInt32(((TextBox)sender).Text);
                    ((OtherMedication)((TextBox)sender).DataContext).Quantity = ((OtherMedication)((TextBox)sender).DataContext).Day * Convert.ToInt32(((OtherMedication)((TextBox)sender).DataContext).Frequency);
                }
            }
        }

        void mrci_txtDayChanged(object sender, TextChangedEventArgs e)
        {
            if (IsFirstTime == false)
            {
                if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsPositiveNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                if (((TextBox)sender).Text == "")
                    ((TextBox)sender).Text = "0";

                if (((TextBox)sender).DataContext is Medication)
                {
                    ((Medication)((TextBox)sender).DataContext).Day = Convert.ToInt32(((TextBox)sender).Text);
                    ((Medication)((TextBox)sender).DataContext).Quantity = ((Medication)((TextBox)sender).DataContext).Day * Convert.ToInt32(((Medication)((TextBox)sender).DataContext).Frequency);
                }
                else if (((TextBox)sender).DataContext is OtherMedication)
                {
                    ((OtherMedication)((TextBox)sender).DataContext).Day = Convert.ToInt32(((TextBox)sender).Text);
                    ((OtherMedication)((TextBox)sender).DataContext).Quantity = ((OtherMedication)((TextBox)sender).DataContext).Day * Convert.ToInt32(((OtherMedication)((TextBox)sender).DataContext).Frequency);
                }

            }
        }

        //CheckListBox Selection and Relation Management
        void CLBCI_chkItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).Tag))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).Tag)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).Tag)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        try
                                        {
                                            if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }

                                        break;
                                    case ComboOperations.NotEqualTo:
                                        try
                                        {
                                            if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }

                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception)
            {

                //throw;
            }
        }

        void InvestCLBCI_chkItemClicked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    if (((FrameworkElement)(((FieldDetail)(((Control)sender).Tag))).Control).Visibility == Visibility.Visible)
                    {
                        if (((FieldDetail)(((Control)sender).Tag)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((Control)sender).Tag)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                                {
                                    switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case ComboOperations.EqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }

                                            break;
                                        case ComboOperations.NotEqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }

                                            break;
                                    }

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }

                }
                catch (Exception)
                {

                    //throw;
                }
            }
        }

        //AutoCombo + AutoList(Single Selection Mode) Selection and Relation management
        void AutoComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception)
            {

                //throw;
            }
        }

        void InvestAutoComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {
                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                            {
                                switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID == ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.ID)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }

            }
            catch (Exception)
            {

                //throw;
            }
        }

        // Selection and Relation management for AutoList MultiSelect mode
        void lbAutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((AutomatedListFieldSetting)field.Settings).SelectedItems == null)
                    ((AutomatedListFieldSetting)field.Settings).SelectedItems = new List<MasterListItem>();
                ((AutomatedListFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((AutomatedListFieldSetting)field.Settings).SelectedItems.Add((MasterListItem)item);
                }

                try
                {
                    if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                                {
                                    switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case ComboOperations.EqualTo:
                                            try
                                            {
                                                if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }


                                            break;
                                        case ComboOperations.NotEqualTo:
                                            try
                                            {
                                                if (((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((AutomatedListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }

                                            break;
                                    }

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }

                }
                catch (Exception)
                {

                    //throw;
                }
            }
        }

        void lbInvestAutoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((InvestigationFieldSetting)field.Settings).SelectedItems == null)
                    ((InvestigationFieldSetting)field.Settings).SelectedItems = new List<MasterListItem>();
                ((InvestigationFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((InvestigationFieldSetting)field.Settings).SelectedItems.Add((MasterListItem)item);
                }

                try
                {
                    if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement && item.RelationCondition is CheckListExpression<bool>)
                                {
                                    switch (((CheckListExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case ComboOperations.EqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            }


                                            break;
                                        case ComboOperations.NotEqualTo:
                                            try
                                            {
                                                if (((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItems.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID)) != null)
                                                {
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            }

                                            break;
                                    }

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }

                }
                catch (Exception)
                {

                    //throw;
                }
            }
        }


        void Field_MouseEnter(object sender, MouseEventArgs e)
        {
            if (pf != null)
            {
                if (!pf.IsOpen)
                {
                    //TextBlock tbl = new TextBlock();
                    TextBox tbl = new TextBox();
                    tbl.IsEnabled = false;
                    //tbl.Background = new SolidColorBrush(Colors.Yellow);
                    tbl.Text = ((FieldDetail)((FrameworkElement)sender).DataContext).ToolTipText;
                    tbl.TextWrapping = TextWrapping.Wrap;

                    //((Border)((ScrollViewer)pf.Child).Content).Child = null;
                    ((Border)((ScrollViewer)pf.Child).Content).Child = tbl;
                    GeneralTransform gt = ((FrameworkElement)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = gt.Transform(new Point(0, 0));
                    double controlTop = offset.Y + ((FrameworkElement)sender).ActualHeight;
                    double controlLeft = offset.X;
                    ((ScrollViewer)pf.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                    //((ScrollViewer)pf.Child).MaxHeight = this.ActualHeight - controlTop + 10;
                    ((ScrollViewer)pf.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop;
                    pf.VerticalOffset = controlTop;
                    pf.HorizontalOffset = controlLeft;
                    pf.IsOpen = true;
                }
            }
        }

        void lbList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            ((ComboBox)sender).SelectedItem = null;
        }

        void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            unchecked
            {
                if (((CheckBox)sender).DataContext != null)
                {
                    ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).SelectedItems[(Int32)((CheckBox)sender).Tag] = false;

                    if (((CheckBox)sender).Name == "chkOtherN")
                    {
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).IsOtherText = false;
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).OtherText = "";
                        if ((TextBlock)Form.FindName("ONTitle") != null && (TextBox)Form.FindName("ONField") != null)
                        {
                            ((TextBlock)Form.FindName("ONTitle")).Visibility = Visibility.Collapsed;
                            ((TextBox)Form.FindName("ONField")).Visibility = Visibility.Collapsed;
                        }
                    }
                    if (((CheckBox)sender).Name == "chkOtherA")
                    {
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).IsOtherText = false;
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).OtherText = "";
                        if ((TextBlock)Form.FindName("OATitle") != null && (TextBox)Form.FindName("OAField") != null)
                        {
                            ((TextBlock)Form.FindName("OATitle")).Visibility = Visibility.Collapsed;
                            ((TextBox)Form.FindName("OAField")).Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        void chk_Checked(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            checked
            {
                if (((CheckBox)sender).DataContext != null)
                {
                    ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).SelectedItems[(Int32)((CheckBox)sender).Tag] = true;

                    if (((CheckBox)sender).Name == "chkOtherN")
                    {
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).IsOtherText = true;

                        if ((TextBlock)Form.FindName("ONTitle") != null && (TextBox)Form.FindName("ONField") != null)
                        {
                            ((TextBlock)Form.FindName("ONTitle")).Visibility = Visibility.Visible;
                            ((TextBox)Form.FindName("ONField")).Visibility = Visibility.Visible;
                        }
                    }
                    if (((CheckBox)sender).Name == "chkOtherA")
                    {
                        ((ListOfCheckBoxesFieldSetting)(((FieldDetail)((CheckBox)sender).DataContext).Settings)).IsOtherText = true;
                        if ((TextBlock)Form.FindName("OATitle") != null && (TextBox)Form.FindName("OAField") != null)
                        {
                            ((TextBlock)Form.FindName("OATitle")).Visibility = Visibility.Visible;
                            ((TextBox)Form.FindName("OAField")).Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        void irci_txtRemarksTextChanged(object sender, RoutedEventArgs e)
        {
            ((OtherInvestigation)((TextBox)sender).DataContext).Remarks = (string)((TextBox)sender).Text;
        }

        void irci_cmbSelectionChanged(object sender, RoutedEventArgs e)
        {
            ((OtherInvestigation)((ComboBox)sender).DataContext).Investigation = (string)((ComboBox)sender).SelectedValue;
        }

        void irci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {
            var lstBoxInvest = ((OtherInvestigation)((HyperlinkButton)sender).DataContext).Parent;
            var InvestSetting = ((OtherInvestigation)((HyperlinkButton)sender).DataContext).InvestigationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                InvestSetting.ItemsSource.RemoveAt(((OtherInvestigation)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                InvestSetting.ItemsSource.Add(new OtherInvestigation() { InvestigationSource = Helpers.GetInvestigationList() });
            }

            lstBoxInvest.Items.Clear();
            for (int i = 0; i < InvestSetting.ItemsSource.Count; i++)
            {
                InvestigationRepetorControlItem irci = new InvestigationRepetorControlItem();
                irci.OnAddRemoveClick += new RoutedEventHandler(irci_OnAddRemoveClick);
                irci.cmbSelectionChanged += new RoutedEventHandler(irci_cmbSelectionChanged);
                //irci.txtRemarksLostFocus += new RoutedEventHandler(irci_txtRemarksLostFocus);
                irci.txtRemarksTextChanged += new RoutedEventHandler(irci_txtRemarksTextChanged);
                //Medication m = new Medication() { Command = "Add", Index = lstBox.Items.Count, Parent = lstBox, DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() };
                InvestSetting.ItemsSource[i].Index = i;
                InvestSetting.ItemsSource[i].Command = ((i == InvestSetting.ItemsSource.Count - 1) ? "Add" : "Remove");
                InvestSetting.ItemsSource[i].Parent = lstBoxInvest;
                InvestSetting.ItemsSource[i].InvestigationSetting = InvestSetting;
                irci.DataContext = InvestSetting.ItemsSource[i];
                lstBoxInvest.Items.Add(irci);
            }
        }

        void lstBoxInvest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void dtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
            string s = ((DatePicker)sender).SelectedDate.ToString();
        }

        private Grid GetGridHeading()
        {
            Grid grdAnti = new Grid();
            grdAnti.Margin = new Thickness(2, 2, 2, 2);
            RowDefinition r = new RowDefinition();
            r.Height = new GridLength(25, GridUnitType.Auto);
            grdAnti.RowDefinitions.Add(r);

            ColumnDefinition c = new ColumnDefinition();
            //c.Width = new GridLength();
            grdAnti.ColumnDefinitions.Add(c);

            //Used with Dose
            //ColumnDefinition c1 = new ColumnDefinition();          
            //grdAnti.ColumnDefinitions.Add(c1);

            int i = 0;
            while (i < 4)
            {
                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = new GridLength();
                grdAnti.ColumnDefinitions.Add(c2);
                i++;
            }


            LinearGradientBrush a = new LinearGradientBrush();
            GradientStop gs1 = new GradientStop();
            gs1.Offset = 0;
            gs1.Color = Color.FromArgb(20, 0, 0, 155);
            a.GradientStops.Add(gs1);

            TextBox txtName = new TextBox();
            txtName.Background = a;
            txtName.IsReadOnly = true;
            txtName.Text = "Drug Name";
            //txtName.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);
            grdAnti.Children.Add(txtName);

            TextBox txtRoute = new TextBox();
            txtRoute.Background = a;
            txtRoute.IsReadOnly = true;
            txtRoute.Text = "Route";
            //txtRoute.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtRoute, 0);
            Grid.SetColumn(txtRoute, 1);
            grdAnti.Children.Add(txtRoute);

            TextBox txtFrequency = new TextBox();
            txtFrequency.Background = a;
            txtFrequency.IsReadOnly = true;
            txtFrequency.Text = "Frequency";
            //txtFrequency.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtFrequency, 0);
            Grid.SetColumn(txtFrequency, 2);
            grdAnti.Children.Add(txtFrequency);

            TextBox txtDays = new TextBox();
            txtDays.Background = a;
            txtDays.IsReadOnly = true;
            txtDays.Text = "Days";
            //txtRoute.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtDays, 0);
            Grid.SetColumn(txtDays, 3);
            grdAnti.Children.Add(txtDays);

            TextBox txtQty = new TextBox();
            txtQty.Background = a;
            txtQty.IsReadOnly = true;
            txtQty.Text = "Quantity";
            //txtRoute.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtQty, 0);
            Grid.SetColumn(txtQty, 4);
            grdAnti.Children.Add(txtQty);

            return grdAnti;
        }

        private Grid AddGridItem(Grid g, string s1, string s2, string s3, string s4, int s5, int s6)
        {
            RowDefinition r = new RowDefinition();
            r.Height = new GridLength(25, GridUnitType.Auto);
            g.RowDefinitions.Add(r);

            TextBox txtName = new TextBox();
            txtName.IsReadOnly = true;
            txtName.Text = s1 == null ? "" : s1;
            Grid.SetRow(txtName, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtName, 0);
            g.Children.Add(txtName);


            TextBox txtRoute = new TextBox();
            txtRoute.IsReadOnly = true;
            txtRoute.Text = s3 == null ? "" : s3;
            Grid.SetRow(txtRoute, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtRoute, 1);
            g.Children.Add(txtRoute);

            TextBox txtFrequency = new TextBox();
            txtFrequency.IsReadOnly = true;
            txtFrequency.Text = s4 == null ? "" : s4;
            Grid.SetRow(txtFrequency, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtFrequency, 2);
            g.Children.Add(txtFrequency);

            TextBox txtDays = new TextBox();
            txtDays.IsReadOnly = true;
            txtDays.Text = s5.ToString();
            Grid.SetRow(txtDays, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtDays, 3);
            g.Children.Add(txtDays);

            TextBox txtQty = new TextBox();
            txtQty.IsReadOnly = true;
            txtQty.Text = s6.ToString();
            Grid.SetRow(txtQty, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtQty, 4);
            g.Children.Add(txtQty);

            return g;
        }

        private Grid GetGridSchema(Grid MainGrid)
        {
            //Grid MainGrid=new Grid();
            MainGrid.Margin = new Thickness(2, 2, 2, 2);
            MainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            int i = 0;

            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(200, GridUnitType.Auto);
            MainGrid.ColumnDefinitions.Add(col);

            ColumnDefinition col1 = new ColumnDefinition();
            MainGrid.ColumnDefinitions.Add(col1);

            while (i < 4)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength();
                MainGrid.RowDefinitions.Add(row);
                i++;
            }

            TextBlock tb1 = new TextBlock();
            //tb1.Text = "Antiemetics : ";
            tb1.Text = "Medication : ";
            Grid.SetRow(tb1, 0);
            MainGrid.Children.Add(tb1);

            TextBlock tb2 = new TextBlock();
            //Added at razi
            tb2.Visibility = Visibility.Collapsed;
            tb2.Text = "Antibiotics : ";
            Grid.SetRow(tb2, 1);
            MainGrid.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            //Added at razi
            tb3.Visibility = Visibility.Collapsed;
            tb3.Text = "Antipyretic : ";
            Grid.SetRow(tb3, 2);
            MainGrid.Children.Add(tb3);

            TextBlock tb4 = new TextBlock();
            //Added at razi
            tb4.Visibility = Visibility.Collapsed;
            tb4.Text = "Antispasmodic : ";
            Grid.SetRow(tb4, 3);
            MainGrid.Children.Add(tb4);

            int k = 0;
            Grid g1 = GetGridHeading();
            //ListBox lst = (ListBox)Form.FindName("ManagementAntiemetics");
            ListBox lst = (ListBox)Form.FindName("GeneralMedication");
            if (lst != null)
            {
                PCRS.ItemsSource1 = new List<Medication>();
                while (k < lst.Items.Count)
                {
                    string s1, s2, s3, s4;
                    int s5, s6;
                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug != null)
                        s1 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug.Description);
                    else
                        s1 = "";
                    s2 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Dose);

                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route != null)
                        s3 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route.Description;
                    else
                        s3 = "";

                    s4 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Frequency.ToString();
                    s5 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Day);
                    s6 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Quantity);
                    g1 = AddGridItem(g1, s1, s2, s3, s4, s5, s6);
                    PCRS.ItemsSource1.Add(((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext));
                    k++;
                }
            }

            lst = null;
            Grid g2 = GetGridHeading();
            //Added at razi
            g2.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntibiotics");
            if (lst != null)
            {
                PCRS.ItemsSource2 = new List<Medication>();
                k = 0;
                while (k < lst.Items.Count)
                {
                    string s1, s2, s3, s4;
                    int s5, s6;
                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug != null)
                        s1 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug.Description);
                    else
                        s1 = "";
                    s2 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Dose);

                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route != null)
                        s3 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route.Description;
                    else
                        s3 = "";

                    s4 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Frequency.ToString();
                    s5 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Day);
                    s6 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Quantity);
                    g2 = AddGridItem(g2, s1, s2, s3, s4, s5, s6);
                    PCRS.ItemsSource2.Add(((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext));
                    k++;
                }
            }

            lst = null;
            Grid g3 = GetGridHeading();
            //Added at razi
            g3.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntipyretic");
            if (lst != null)
            {
                PCRS.ItemsSource3 = new List<Medication>();
                k = 0;
                while (k < lst.Items.Count)
                {
                    string s1, s2, s3, s4;
                    int s5, s6;
                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug != null)
                        s1 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug.Description);
                    else
                        s1 = "";
                    s2 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Dose);

                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route != null)
                        s3 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route.Description;
                    else
                        s3 = "";

                    s4 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Frequency.ToString();
                    s5 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Day);
                    s6 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Quantity);
                    g3 = AddGridItem(g3, s1, s2, s3, s4, s5, s6);
                    PCRS.ItemsSource3.Add(((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext));
                    k++;
                }
            }

            lst = null;
            Grid g4 = GetGridHeading();
            //Added at razi
            g4.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntispasmodic");

            if (lst != null)
            {
                PCRS.ItemsSource4 = new List<Medication>();
                k = 0;
                while (k < lst.Items.Count)
                {
                    string s1, s2, s3, s4;
                    int s5, s6;
                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug != null)
                        s1 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Drug.Description);
                    else
                        s1 = "";
                    s2 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Dose);

                    if (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route != null)
                        s3 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Route.Description;
                    else
                        s3 = "";

                    s4 = ((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Frequency.ToString();
                    s5 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Day);
                    s6 = (((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext).Quantity);
                    g4 = AddGridItem(g4, s1, s2, s3, s4, s5, s6);
                    PCRS.ItemsSource4.Add(((Medication)((MedicatioRepeterControlItem)lst.Items[k]).DataContext));
                    k++;
                }
            }

            Grid.SetRow(g1, 0);
            Grid.SetColumn(g1, 1);
            MainGrid.Children.Add(g1);

            Grid.SetRow(g2, 1);
            Grid.SetColumn(g2, 1);
            MainGrid.Children.Add(g2);

            Grid.SetRow(g3, 2);
            Grid.SetColumn(g3, 1);
            MainGrid.Children.Add(g3);

            Grid.SetRow(g4, 3);
            Grid.SetColumn(g4, 1);
            MainGrid.Children.Add(g4);

            return MainGrid;
        }

        void HyperBtn_Click(object sender, RoutedEventArgs e)
        {

            if (((HyperlinkButton)sender).TargetName == "Razi_Case Referral Sheet_31_08_2010.doc")
            {

                PrintDocument Pd = new PrintDocument();
                Pd.PrintPage += new EventHandler<PrintPageEventArgs>(Pd_PrintPage);
                Pd.Print("CaseReferral");

            }
            else
            {
                //HtmlPage.PopupWindow(new Uri(file), "_blank", op);
                HtmlPage.Window.Invoke("alertText", new string[] { ((HyperlinkButton)sender).TargetName });
            }
            //HtmlPage.Window.Invoke("open", new object[] { "../Images/" + ((HyperlinkButton)sender).TargetName});
        }

        void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //throw new NotImplementedException();
            CaseReferralSheet CRS = new CaseReferralSheet();
            string CC = "";
            try
            {

                CRS.SetParticulars(this.SelectedPatient.Name, this.SelectedPatient.Age.ToString(), this.SelectedPatient.Gender, this.SelectedPatient.Address, this.SelectedPatient.Occupation, this.SelectedPatient.Phone, DateTime.Now.ToString(), this.SelectedPatient.ClinicRegNo);
                CRS.SetReferralDetails("", (DynamicListItem)((ComboBox)Form.FindName("RMTo")).SelectedItem != null ? ((DynamicListItem)((ComboBox)Form.FindName("RMTo")).SelectedItem).Title : "", "", (DynamicListItem)((ComboBox)Form.FindName("RMAt")).SelectedItem != null ? ((DynamicListItem)((ComboBox)Form.FindName("RMAt")).SelectedItem).Title : "", "", "");

                CC = Title.ToString();
                CRS.SetProDiagChiefComplaint(((TextBox)Form.FindName("RMProvisionalDiagnosis")).Text, CC);
            }
            catch (Exception)
            {

            }
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            e.PageVisual = CRS;
            Indicatior.Close();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        void DecField_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        void DecField_TextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();

            // Checking for Valid Decimal Number
            if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsItDecimal())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }


            if (((TextBox)sender).Name == "StuntingPercent" && IsFirstTime == false)
            {
                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "")
                {
                    double stun = double.Parse(((TextBox)sender).Text);
                    ComboBox cmbList = (ComboBox)Form.FindName("NutritionStatus");
                    cmbList.SelectedItem = null;
                    ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = null;
                    if (stun > 95)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Normal").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Normal").Single();
                    }
                    if (stun > 87.5 && stun <= 95)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Mildy impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Mildy impaired").Single();
                    }
                    if (stun > 80 && stun <= 87.5)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderately impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderately impaired").Single();
                    }
                    if (stun <= 80)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Severly impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Severly impaired").Single();
                    }
                }
            }
            if (((TextBox)sender).Name == "WastingPercent")
            {
                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && IsFirstTime == false)
                {
                    double wast = double.Parse(((TextBox)sender).Text);
                    ComboBox cmbList = (ComboBox)Form.FindName("NutritionStatus");
                    cmbList.SelectedItem = null;
                    ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = null;
                    if (wast > 90)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Normal").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Normal").Single();
                    }
                    if (wast > 80 && wast <= 90)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Mildy impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Mildy impaired").Single();
                    }
                    if (wast > 70 && wast <= 80)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderately impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderately impaired").Single();
                    }
                    if (wast <= 70)
                    {
                        ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Severly impaired").Single();
                        cmbList.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbList.DataContext).Settings).ItemSource.Where(i => i.Title == "Severly impaired").Single();
                    }
                }
            }
        }

        void cmbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();

            try
            {
                if (((FrameworkElement)(((FieldDetail)(((Control)sender).DataContext))).Control).Visibility == Visibility.Visible)
                {


                    if (((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList != null)
                        foreach (var item in ((FieldDetail)(((Control)sender).DataContext)).RelationalFieldList)
                        {
                            if (item.Control is FrameworkElement && item.RelationCondition is ComboExpression<bool>)
                            {
                                switch (((ComboExpression<bool>)item.RelationCondition).Operation)
                                {
                                    case ComboOperations.EqualTo:
                                        if (((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((ComboExpression<bool>)item.RelationCondition).SelectedItem == ((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.Title)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                    case ComboOperations.NotEqualTo:
                                        if (((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem != null && ((ComboExpression<bool>)item.RelationCondition).SelectedItem != ((ListFieldSetting)((FieldDetail)((Control)sender).DataContext).Settings).SelectedItem.Title)
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        }
                                        else
                                        {
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        }
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }
                }
            }
            catch (Exception)
            {

                //throw;
            }

            if (IsFirstTime == false)
            {

                if (TemplateID == 4)
                {
                    if (sender is ComboBox && ((ComboBox)sender).Name == "CoughDuration")
                    {
                        if (((ComboBox)sender).SelectedItem != null)
                        {
                            if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Less than 3 weeks")
                            {
                                if (Form.FindName("PDAcuteCough") is CheckBox)
                                {
                                    CheckBox chkAcuteCough = (CheckBox)Form.FindName("PDAcuteCough");
                                    chkAcuteCough.IsChecked = true;
                                    chk_Click(chkAcuteCough, new RoutedEventArgs());
                                }
                            }
                            else
                            {
                                if (Form.FindName("PDAcuteCough") is CheckBox)
                                {
                                    CheckBox chkAcuteCough = (CheckBox)Form.FindName("PDAcuteCough");
                                    chkAcuteCough.IsChecked = false;
                                    chk_Click(chkAcuteCough, new RoutedEventArgs());
                                }
                            }

                            if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "3 weeks to 8 weeks")
                            {
                                if (Form.FindName("PDPersistentCough") is CheckBox)
                                {
                                    CheckBox chkPersistentCough = (CheckBox)Form.FindName("PDPersistentCough");
                                    chkPersistentCough.IsChecked = true;
                                    chk_Click(chkPersistentCough, new RoutedEventArgs());
                                }
                            }
                            else
                            {
                                if (Form.FindName("PDPersistentCough") is CheckBox)
                                {
                                    CheckBox chkPersistentCough = (CheckBox)Form.FindName("PDPersistentCough");
                                    chkPersistentCough.IsChecked = false;
                                    chk_Click(chkPersistentCough, new RoutedEventArgs());
                                }
                            }

                            if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "More than 8 weeks")
                            {
                                if (Form.FindName("PDChronicCough") is CheckBox)
                                {
                                    CheckBox chkPDChronicCough = (CheckBox)Form.FindName("PDChronicCough");
                                    chkPDChronicCough.IsChecked = true;
                                    chk_Click(chkPDChronicCough, new RoutedEventArgs());
                                }
                            }
                            else
                            {
                                if (Form.FindName("PDChronicCough") is CheckBox)
                                {
                                    CheckBox chkPDChronicCough = (CheckBox)Form.FindName("PDChronicCough");
                                    chkPDChronicCough.IsChecked = false;
                                    chk_Click(chkPDChronicCough, new RoutedEventArgs());
                                }
                            }
                        }
                    }

                    if (sender is ComboBox && ((ComboBox)sender).Name == "CoughDuration")
                    {
                        if (Form.FindName("RiskCardiacProblemsCHF") is CheckBox && Form.FindName("AlarmPersistentCoughCardiovascularDisease") is CheckBox)
                        {
                            CheckBox chkCardiac = (CheckBox)Form.FindName("RiskCardiacProblemsCHF");
                            CheckBox chkAlarm = (CheckBox)Form.FindName("AlarmPersistentCoughCardiovascularDisease");
                            if (chkCardiac.IsChecked == true && ((ComboBox)sender).SelectedItem != null && (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "3 weeks to 8 weeks" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "More than 8 weeks"))
                            {
                                chkAlarm.IsChecked = true;
                                chkAlarm.Visibility = Visibility.Visible;
                                ((FrameworkElement)((FieldDetail)chkAlarm.DataContext).LabelControl).Visibility = Visibility.Visible;
                            }
                            else
                            {
                                chkAlarm.IsChecked = false;
                                chkAlarm.Visibility = Visibility.Collapsed;
                                ((FrameworkElement)((FieldDetail)chkAlarm.DataContext).LabelControl).Visibility = Visibility.Collapsed;
                            }
                        }
                    }

                    if (sender is ComboBox && ((ComboBox)sender).Name == "PERespiratory")
                    {
                        if (Form.FindName("Temprature") is TextBox && Form.FindName("PDCoughWithChestInfection") is CheckBox)
                        {
                            TextBox txtTemp = (TextBox)Form.FindName("Temprature");
                            CheckBox chkCoughChest = (CheckBox)Form.FindName("PDCoughWithChestInfection");
                            if (txtTemp.Text != null && txtTemp.Text != "" && Convert.ToDecimal(txtTemp.Text) > 101 && ((ComboBox)sender).SelectedItem != null && ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Ronchi" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Crepts")
                            {
                                chkCoughChest.IsChecked = true;
                                chk_Click(chkCoughChest, new RoutedEventArgs());
                            }
                            else
                            {
                                chkCoughChest.IsChecked = false;
                                chk_Click(chkCoughChest, new RoutedEventArgs());
                            }
                        }
                    }
                }
                else if (TemplateID == 7)
                {
                    if (sender is FrameworkElement && (((FrameworkElement)sender).Name == "HistUrineOutput" || ((FrameworkElement)sender).Name == "PEGeneralCondition" || ((FrameworkElement)sender).Name == "PEThirst" || ((FrameworkElement)sender).Name == "PEMucusMembrane" || ((FrameworkElement)sender).Name == "PESkinPinchAbdomen" || ((FrameworkElement)sender).Name == "PEUrineOutput"))
                    {
                        if (Form.FindName("HSHydrationStatus") is ComboBox && Form.FindName("HistUrineOutput") is ComboBox && Form.FindName("PEGeneralCondition") is ComboBox && Form.FindName("PEThirst") is ComboBox && Form.FindName("PEMucusMembrane") is ComboBox && Form.FindName("PESkinPinchAbdomen") is ComboBox && Form.FindName("PEUrineOutput") is ComboBox)
                        {
                            ComboBox cmbHSHydrationStatus = (ComboBox)Form.FindName("HSHydrationStatus");
                            ComboBox cmbHistUrineOutput = (ComboBox)Form.FindName("HistUrineOutput");
                            ComboBox cmbPEGeneralCondition = (ComboBox)Form.FindName("PEGeneralCondition");
                            ComboBox cmbPEThirst = (ComboBox)Form.FindName("PEThirst");
                            ComboBox cmbPEMucusMembrane = (ComboBox)Form.FindName("PEMucusMembrane");
                            ComboBox cmbPESkinPinchAbdomen = (ComboBox)Form.FindName("PESkinPinchAbdomen");
                            ComboBox cmbPEUrineOutput = (ComboBox)Form.FindName("PEUrineOutput");

                            if ((cmbHistUrineOutput.SelectedItem != null && ((DynamicListItem)cmbHistUrineOutput.SelectedItem).Title == "Absent") || (cmbPEGeneralCondition.SelectedItem != null && ((DynamicListItem)cmbPEGeneralCondition.SelectedItem).Title == "Very lethargic or drowsy") || (cmbPEThirst.SelectedItem != null && ((DynamicListItem)cmbPEThirst.SelectedItem).Title == "Intense") || (cmbPEMucusMembrane.SelectedItem != null && ((DynamicListItem)cmbPEMucusMembrane.SelectedItem).Title == "Parched") || (cmbPESkinPinchAbdomen.SelectedItem != null && ((DynamicListItem)cmbPESkinPinchAbdomen.SelectedItem).Title == "Retracts very slowly (>2 Sec)") || (cmbPEUrineOutput.SelectedItem != null && ((DynamicListItem)cmbPEUrineOutput.SelectedItem).Title == "Minimal"))
                            {
                                if (((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource != null && ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Count > 0)
                                {
                                    try
                                    {
                                        ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Severe dehydration").Single();
                                        cmbHSHydrationStatus.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Severe dehydration").Single();
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            else if ((cmbHistUrineOutput.SelectedItem != null && ((DynamicListItem)cmbHistUrineOutput.SelectedItem).Title == "Decreased") || (cmbPEGeneralCondition.SelectedItem != null && ((DynamicListItem)cmbPEGeneralCondition.SelectedItem).Title == "Slightly lethargic") || (cmbPEThirst.SelectedItem != null && ((DynamicListItem)cmbPEThirst.SelectedItem).Title == "Moderate") || (cmbPEMucusMembrane.SelectedItem != null && ((DynamicListItem)cmbPEMucusMembrane.SelectedItem).Title == "Dry") || (cmbPESkinPinchAbdomen.SelectedItem != null && ((DynamicListItem)cmbPESkinPinchAbdomen.SelectedItem).Title == "Retracts slowly (1-2 Sec) ") || (cmbPEUrineOutput.SelectedItem != null && ((DynamicListItem)cmbPEUrineOutput.SelectedItem).Title == "Decreased"))
                            {
                                if (((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource != null && ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Count > 0)
                                {
                                    try
                                    {
                                        ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderate dehydration").Single();
                                        cmbHSHydrationStatus.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Moderate dehydration").Single();
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            else if ((cmbPEThirst.SelectedItem != null && ((DynamicListItem)cmbPEThirst.SelectedItem).Title == "Mild") || (cmbPEMucusMembrane.SelectedItem != null && ((DynamicListItem)cmbPEMucusMembrane.SelectedItem).Title == "Slightly dry"))
                            {
                                if (((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource != null && ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Count > 0)
                                {
                                    try
                                    {
                                        ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Mild dehydration").Single();
                                        cmbHSHydrationStatus.SelectedItem = ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).ItemSource.Where(i => i.Title == "Mild dehydration").Single();
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                ((ListFieldSetting)((FieldDetail)cmbHSHydrationStatus.DataContext).Settings).SelectedItem = null;
                                cmbHSHydrationStatus.SelectedItem = null;
                            }
                        }
                    }
                    else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PESkinColour")
                    {
                        if (Form.FindName("GEPallor") is CheckBox && Form.FindName("InvestLabs") is ListBox)
                        {
                            CheckBox chkGEPallor = (CheckBox)Form.FindName("GEPallor");
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");

                            if ((((ComboBox)sender).SelectedItem != null && ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Pale") || (chkGEPallor.IsChecked == true))
                            {
                                if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                {
                                    foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                    {
                                        if (a.Description == "Complete Blood Count (CBC)")
                                        {
                                            a.Status = true;
                                            InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                {
                                    foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                    {
                                        if (a.Description == "Complete Blood Count (CBC)")
                                        {
                                            a.Status = false;
                                            InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (IsFirstTime == false)
            {

                if (TemplateID == 2)
                {
                    switch (((ComboBox)sender).Name)
                    {
                        case "ProvisionalDiagnosis":
                            TextBox txtRMPD = (TextBox)Form.FindName("RMProvisionalDiagnosis");
                            if (txtRMPD != null)
                                txtRMPD.Text = ((DynamicListItem)((ComboBox)sender).SelectedItem).Title;
                            break;
                    }
                }

            }

            #region Code For Validate Investigations According to Provisional Diagnosis & Hydration Status Version3
            if (IsFirstTime == false)
            {

                if (TemplateID == 2)
                {
                    switch (((ComboBox)sender).Name)
                    {
                        case "HydrationStatus":
                            ComboBox cmbHyd = (ComboBox)Form.FindName("HydrationStatusManagement");
                            ComboBox cmbHyd4 = (ComboBox)Form.FindName("Hydration4StatusManagement");
                            ComboBox cmbZinc = (ComboBox)Form.FindName("ZincSupplementManagement");

                            IGetList src = null;

                            if (((ComboBox)sender).SelectedItem != null)
                            {
                                ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");

                                #region Code for Validating Investigations
                                if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Moderate dehydration" && (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Acute watery diarrhea" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea")))
                                {
                                    #region Set Investigations for Condition1 and Condition2 ((acute Watery Diarrhea or Dysentry or Persistent Diarrhea) and Moderate Hydration)
                                    if (Form.FindName("SerumElectro") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("SerumElectro")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("SerumElectro"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("BUNCreatinine") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("BUNCreatinine")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("BUNCreatinine"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when condition 1 & 2 are not satisfied
                                    if (Form.FindName("SerumElectro") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("SerumElectro")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("SerumElectro"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("BUNCreatinine") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("BUNCreatinine")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("BUNCreatinine"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }

                                if ((((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Mild dehydration" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Moderate dehydration") && (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea")))
                                {
                                    #region Set Investigations for Condition 4,5 and 6 ((Dysentry or Persistent Diarrhea) and (Mild or Moderate Hydration))
                                    if (Form.FindName("StoolME") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolME")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("StoolME"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("StoolCulture") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolCulture")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("StoolCulture"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("OvaParasiteTest") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("OvaParasiteTest")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("OvaParasiteTest"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when condition 4,5 and 6 are not satisfied
                                    if (Form.FindName("StoolME") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolME")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("StoolME"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("StoolCulture") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolCulture")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("StoolCulture"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("OvaParasiteTest") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("OvaParasiteTest")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("OvaParasiteTest"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                #endregion

                                #region Set ItemsSource of cmbHyd

                                if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Mild dehydration" && cmbHyd != null)
                                {
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = new DynamicListItem() { Id = 1, Title = "Source1", Value = "Source1" };
                                    //cmbHyd.ItemsSource = null;
                                    //cmbHyd.SelectedItem = null;
                                    //src = new Source1();
                                    //cmbHyd.ItemsSource = src.GetList();

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                    listSetting.SelectedSource = new DynamicListItem() { Id = 1, Title = "Source1", Value = "Source1" };
                                    src = new Source1();
                                    listSetting.ItemSource = src.GetList();
                                    cmbHyd.ItemsSource = listSetting.ItemSource;
                                    if (listSetting.SelectedItem != null)
                                    {
                                        var item = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                                        cmbHyd.SelectedItem = item;
                                    }
                                }
                                else if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Moderate dehydration" && cmbHyd != null)
                                {
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = new DynamicListItem() { Id = 2, Title = "Source2", Value = "Source2" };
                                    //src = new Source2();
                                    //cmbHyd.ItemsSource = src.GetList();

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                    listSetting.SelectedSource = new DynamicListItem() { Id = 1, Title = "Source2", Value = "Source2" };
                                    src = new Source2();
                                    listSetting.ItemSource = src.GetList();
                                    cmbHyd.ItemsSource = listSetting.ItemSource;
                                    if (listSetting.SelectedItem != null)
                                    {
                                        var item = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                                        cmbHyd.SelectedItem = item;
                                    }
                                }
                                else
                                {
                                    #region Set ItemsSource of cmbHyd to NULL
                                    if (cmbHyd != null)
                                    {
                                        //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = null;
                                        //cmbHyd.ItemsSource = null;             

                                        LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                        listSetting.SelectedSource = null;
                                        cmbHyd.ItemsSource = null;
                                        cmbHyd.SelectedItem = null;
                                    }
                                    #endregion
                                }

                                #endregion

                                #region Set ItemSource of cmbHyd4 to NULL
                                if (cmbHyd4 != null && ((DynamicListItem)((ComboBox)sender).SelectedItem).Title != "Severe dehydration")
                                {
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings).SelectedSource = null;
                                    //cmbHyd4.ItemsSource = null;             

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings;
                                    listSetting.SelectedSource = null;
                                    cmbHyd4.ItemsSource = null;
                                    cmbHyd4.SelectedItem = null;
                                }
                                #endregion


                                #region Set ItemsSource of cmbHyd4
                                if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Severe dehydration" && cmbHyd4 != null)
                                {
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings).SelectedSource = new DynamicListItem() { Id = 3, Title = "Source3", Value = "Source3" };
                                    //src = new Source3();
                                    //cmbHyd4.ItemsSource = src.GetList();

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings;
                                    listSetting.SelectedSource = new DynamicListItem() { Id = 1, Title = "Source3", Value = "Source3" };
                                    src = new Source3();
                                    listSetting.ItemSource = src.GetList();
                                    cmbHyd4.ItemsSource = listSetting.ItemSource;
                                    if (listSetting.SelectedItem != null)
                                    {
                                        var item = listSetting.ItemSource.Where(i => i.Title == listSetting.SelectedItem.Title).Single();
                                        cmbHyd4.SelectedItem = item;
                                    }
                                }
                                else
                                {
                                    if (cmbHyd4 != null)
                                    {
                                        //((LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings).SelectedSource = null;
                                        //cmbHyd4.ItemsSource = null;             

                                        LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd4.DataContext).Settings;
                                        listSetting.SelectedSource = null;
                                        cmbHyd4.ItemsSource = null;
                                        cmbHyd4.SelectedItem = null;
                                    }
                                }
                                #endregion

                                #region Set ItemsSource of cmbHyd to NULL
                                if (cmbHyd != null && ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Severe dehydration")
                                {
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = null;
                                    //cmbHyd.ItemsSource = null;             

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                    listSetting.SelectedSource = null;
                                    cmbHyd.ItemsSource = null;
                                    cmbHyd.SelectedItem = null;
                                }
                                #endregion

                            }
                            else
                            {
                                #region Set Hyd and Hyd4 to NULL
                                if (Form.FindName("HydrationStatusManagement") is ComboBox)
                                {
                                    cmbHyd = (ComboBox)Form.FindName("HydrationStatusManagement");
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = null;
                                    //cmbHyd.ItemsSource = null;

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                    listSetting.SelectedSource = null;
                                    cmbHyd.ItemsSource = null;
                                    cmbHyd.SelectedItem = null;
                                }
                                if (Form.FindName("Hydration4StatusManagement") is ComboBox)
                                {
                                    cmbHyd = (ComboBox)Form.FindName("Hydration4StatusManagement");
                                    //((LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings).SelectedSource = null;
                                    //cmbHyd.ItemsSource = null;

                                    LookUpFieldSetting listSetting = (LookUpFieldSetting)((FieldDetail)cmbHyd.DataContext).Settings;
                                    listSetting.SelectedSource = null;
                                    cmbHyd.ItemsSource = null;
                                    cmbHyd.SelectedItem = null;
                                }
                                #endregion
                            }


                            break;


                        case "ProvisionalDiagnosis":
                            if (((ComboBox)sender).SelectedItem != null)
                            {
                                cmbHyd = (ComboBox)Form.FindName("HydrationStatus");
                                ComboBox cmbSkin = (ComboBox)Form.FindName("SkinColour");

                                if ((((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Acute watery diarrhea" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Persistent diarrhea" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Dysentry") && (cmbHyd != null && (DynamicListItem)cmbHyd.SelectedItem != null && ((DynamicListItem)cmbHyd.SelectedItem).Title == "Moderate dehydration"))
                                {
                                    #region Set Investigations for Condition1 and Condition2 ((acute Watery Diarrhea or Dysentry or Persistent Diarrhea) and Moderate Hydration)
                                    if (Form.FindName("SerumElectro") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("SerumElectro")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("SerumElectro"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("BUNCreatinine") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("BUNCreatinine")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("BUNCreatinine"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when condition 1 & 2 are not satisfied
                                    if (Form.FindName("SerumElectro") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("SerumElectro")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("SerumElectro"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("BUNCreatinine") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("BUNCreatinine")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("BUNCreatinine"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }


                                if ((((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Acute watery diarrhea" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Persistent diarrhea" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Dysentry") && (cmbSkin != null && (DynamicListItem)cmbSkin.SelectedItem != null && ((DynamicListItem)cmbSkin.SelectedItem).Title == "Pale"))
                                {
                                    #region Set Investigations for Condition3 ((acute Watery Diarrhea or Dysentry or Persistent Diarrhea) and Skin Colour Pale)
                                    if (Form.FindName("CBC") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("CBC")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("CBC"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when Condition 3 is not satisfied
                                    if (Form.FindName("CBC") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("CBC")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("CBC"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }


                                if ((((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Persistent diarrhea" || ((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Dysentry") && (cmbHyd != null && (DynamicListItem)cmbHyd.SelectedItem != null && (((DynamicListItem)cmbHyd.SelectedItem).Title == "Mild dehydration" || ((DynamicListItem)cmbHyd.SelectedItem).Title == "Moderate dehydration")))
                                {
                                    #region Set Investigations for Condition 4,5 and 6 ((Dysentry or Persistent Diarrhea) and (Mild or Moderate Hydration))
                                    if (Form.FindName("StoolME") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolME")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("StoolME"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("StoolCulture") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolCulture")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("StoolCulture"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("OvaParasiteTest") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("OvaParasiteTest")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("OvaParasiteTest"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when condition 4,5 and 6 are not satisfied
                                    if (Form.FindName("StoolME") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolME")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("StoolME"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("StoolCulture") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("StoolCulture")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("StoolCulture"), new RoutedEventArgs());
                                    }
                                    if (Form.FindName("OvaParasiteTest") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("OvaParasiteTest")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("OvaParasiteTest"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                            }
                            break;

                        case "SkinColour":
                            if (((ComboBox)sender).SelectedItem != null)
                            {
                                ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");

                                if (((DynamicListItem)((ComboBox)sender).SelectedItem).Title == "Pale" && (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Acute watery diarrhea" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea")))
                                {
                                    #region Set Investigations for Condition3 ((acute Watery Diarrhea or Dysentry or Persistent Diarrhea) and Skin Colour Pale)
                                    if (Form.FindName("CBC") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("CBC")).IsChecked = true;
                                        chk_Click((CheckBox)Form.FindName("CBC"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Disable Investigations when Condition 3 is not satisfied
                                    if (Form.FindName("CBC") is CheckBox)
                                    {
                                        ((CheckBox)Form.FindName("CBC")).IsChecked = false;
                                        chk_Click((CheckBox)Form.FindName("CBC"), new RoutedEventArgs());
                                    }
                                    #endregion
                                }
                            }
                            break;
                    }
                }
            }


            #endregion


        }

        void lbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                FieldDetail field = (FieldDetail)((ListBox)sender).DataContext;
                if (((ListFieldSetting)field.Settings).SelectedItems == null)
                    ((ListFieldSetting)field.Settings).SelectedItems = new List<DynamicListItem>();
                ((ListFieldSetting)field.Settings).SelectedItems.Clear();
                foreach (var item in ((ListBox)sender).SelectedItems)
                {
                    ((ListFieldSetting)field.Settings).SelectedItems.Add((DynamicListItem)item);
                }
            }
        }

        void TextField_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((TextBox)sender) != null && ((TextBox)sender).DataContext != null)
                {
                    if (((FieldDetail)((TextBox)sender).DataContext).IsRequired)
                    {
                        ((TextBox)sender).SetValidation(((FieldDetail)((TextBox)sender).DataContext).Title + " required.");
                        ((TextBox)sender).RaiseValidationError();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        void TextField_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((TextBox)sender) != null && ((TextBox)sender).DataContext != null)
                {
                    if (((FieldDetail)((TextBox)sender).DataContext).IsRequired && string.IsNullOrEmpty(((TextBox)sender).Text))
                    {
                        ((TextBox)sender).SetValidation(((FieldDetail)((TextBox)sender).DataContext).Title + " required.");
                        ((TextBox)sender).RaiseValidationError();
                    }
                    else
                    {
                        ((TextBox)sender).ClearValidationError();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        string log = "";

        void decUnit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {

                log = "Starting....";
                try
                {
                    if (sender is TextBox && ((FrameworkElement)(((FieldDetail)(((TextBox)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        if (((TextBox)sender).Text == "")
                        {
                            ((DecimalFieldSetting)((FieldDetail)((TextBox)sender).DataContext).Settings).Value = null;
                        }
                        log += ("\r" + ((FieldDetail)(((TextBox)sender).DataContext)).Title);
                        foreach (var item in ((FieldDetail)(((TextBox)sender).DataContext)).DependentFieldDetail)
                        {
                            log += ("\r" + item.Title);
                            if (item.Control is FrameworkElement)
                            {
                                if (((TextBox)sender).Text == "")
                                {
                                    ((DecimalFieldSetting)((FieldDetail)((TextBox)sender).DataContext).Settings).Value = null;
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    if (item.Condition is DecimalExpression<decimal>)
                                        switch (((DecimalExpression<decimal>)item.Condition).Operation)
                                        {
                                            case DoubleOperations.EqualTo:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue == decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;
                                            case DoubleOperations.NotEqualTo:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue != decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;
                                            case DoubleOperations.GreterThan:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue < decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;

                                            case DoubleOperations.GreterThanEqualTo:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue <= decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;
                                            case DoubleOperations.LessThan:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue > decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;
                                            case DoubleOperations.LessThanEqualTo:
                                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue >= decimal.Parse(((TextBox)sender).Text))
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                else
                                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                break;
                                        }
                                }


                                log += ((FrameworkElement)item.LabelControl).Visibility == Visibility.Collapsed ? Visibility.Collapsed.ToString() : Visibility.Visible.ToString();

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }

                        if (((FieldDetail)(((TextBox)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((TextBox)sender).DataContext)).RelationalFieldList)
                            {
                                log += ("\r" + item.Title);
                                if (item.Control is FrameworkElement)
                                {
                                    if (((TextBox)sender).Text == "")
                                    {
                                        ((DecimalFieldSetting)((FieldDetail)((TextBox)sender).DataContext).Settings).Value = null;
                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        if (item.RelationCondition is DecimalExpression<decimal>)
                                            switch (((DecimalExpression<decimal>)item.RelationCondition).Operation)
                                            {
                                                case DoubleOperations.EqualTo:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue == decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;
                                                case DoubleOperations.NotEqualTo:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue != decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;
                                                case DoubleOperations.GreterThan:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue < decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;

                                                case DoubleOperations.GreterThanEqualTo:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue <= decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;
                                                case DoubleOperations.LessThan:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue > decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;
                                                case DoubleOperations.LessThanEqualTo:
                                                    if (((DecimalExpression<decimal>)item.RelationCondition).ReferenceValue >= decimal.Parse(((TextBox)sender).Text))
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                                    else
                                                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                                    break;
                                            }
                                    }

                                    log += ((FrameworkElement)item.LabelControl).Visibility == Visibility.Collapsed ? Visibility.Collapsed.ToString() : Visibility.Visible.ToString();

                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }
                }
                catch (Exception)
                {

                    //throw;
                }

                if (IsFirstTime == false)
                {

                    if (TemplateID == 4)
                    {
                        if (sender is TextBox && ((TextBox)sender).Name == "Temprature")
                        {

                            if (Form.FindName("PERespiratory") is ComboBox && Form.FindName("PDCoughWithChestInfection") is CheckBox)
                            {
                                ComboBox chkRespiratory = (ComboBox)Form.FindName("PERespiratory");
                                CheckBox chkCoughChest = (CheckBox)Form.FindName("PDCoughWithChestInfection");
                                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Double.Parse(((TextBox)sender).Text) > 101 && chkRespiratory.SelectedItem != null && (((DynamicListItem)chkRespiratory.SelectedItem).Title == "Ronchi" || ((DynamicListItem)chkRespiratory.SelectedItem).Title == "Crepts"))
                                {
                                    chkCoughChest.IsChecked = true;
                                    chk_Click(chkCoughChest, new RoutedEventArgs());
                                }
                            }
                        }
                    }
                    else if (TemplateID == 7)
                    {
                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "ObserveTemperature")
                        {
                            if (Form.FindName("AlarmDiarrheaFever") is CheckBox && Form.FindName("PresFreqWateryLooseStool") is CheckBox)
                            {
                                CheckBox chkAlarmDiarrheaFever = (CheckBox)Form.FindName("AlarmDiarrheaFever");
                                CheckBox chkPresFreqWateryLooseStool = (CheckBox)Form.FindName("PresFreqWateryLooseStool");

                                if (chkPresFreqWateryLooseStool.IsChecked == true && ((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) > 102.2)
                                {
                                    chkAlarmDiarrheaFever.IsChecked = true;
                                    chk_Click(chkAlarmDiarrheaFever, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmDiarrheaFever.IsChecked = false;
                                    chk_Click(chkAlarmDiarrheaFever, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "HistSinceDays")
                        {
                            if (Form.FindName("PDPersistentDiarrhea") is CheckBox)
                            {
                                CheckBox chkPDPersistentDiarrhea = (CheckBox)Form.FindName("PDPersistentDiarrhea");

                                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 14.0)
                                {
                                    chkPDPersistentDiarrhea.IsChecked = true;
                                    chk_Click(chkPDPersistentDiarrhea, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDPersistentDiarrhea.IsChecked = false;
                                    chk_Click(chkPDPersistentDiarrhea, new RoutedEventArgs());
                                }
                            }
                        }
                    }
                    else if (TemplateID == 5)
                    {
                        if (sender is TextBox && ((TextBox)sender).Name == "ObserveSystolic")
                        {
                            if (Form.FindName("AlarmHypertensionNauseaVomiting") is CheckBox && Form.FindName("PresNauseaVomiting") is CheckBox)
                            {
                                CheckBox chkAlarmHypertensionNauseaVomiting = (CheckBox)Form.FindName("AlarmHypertensionNauseaVomiting");
                                CheckBox chkPresNauseaVomiting = (CheckBox)Form.FindName("PresNauseaVomiting");

                                if (chkPresNauseaVomiting.IsChecked == true && ((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) > 140.0)
                                {
                                    chkAlarmHypertensionNauseaVomiting.IsChecked = true;
                                    //chkAlarmHypertensionNauseaVomiting.Visibility = Visibility.Visible;
                                    chk_Click(chkAlarmHypertensionNauseaVomiting, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmHypertensionNauseaVomiting.IsChecked = false;
                                    //chkAlarmHypertensionNauseaVomiting.Visibility = Visibility.Collapsed;
                                    chk_Click(chkAlarmHypertensionNauseaVomiting, new RoutedEventArgs());
                                }
                            }
                        }
                        if (sender is TextBox && ((TextBox)sender).Name == "ObserveSystolic")
                        {
                            if (Form.FindName("PDPrehypertension") is CheckBox && Form.FindName("ObserveDiastolic") is TextBox)
                            {
                                CheckBox chkPDPrehypertension = (CheckBox)Form.FindName("PDPrehypertension");
                                TextBox txtObserveDiastolic = (TextBox)Form.FindName("ObserveDiastolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 120.0 && Convert.ToDouble(((TextBox)sender).Text) <= 139.0) || (txtObserveDiastolic.Text != null && txtObserveDiastolic.Text != "" && Convert.ToDouble(txtObserveDiastolic.Text) >= 80.0 && Convert.ToDouble(txtObserveDiastolic.Text) <= 89.0))
                                {
                                    chkPDPrehypertension.IsChecked = true;
                                    chk_Click(chkPDPrehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDPrehypertension.IsChecked = false;
                                    chk_Click(chkPDPrehypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIhypertensionMildhypertension") is CheckBox && Form.FindName("ObserveDiastolic") is TextBox)
                            {
                                CheckBox chkPDStageIhypertensionMildhypertension = (CheckBox)Form.FindName("PDStageIhypertensionMildhypertension");
                                TextBox txtObserveDiastolic = (TextBox)Form.FindName("ObserveDiastolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 140.0 && Convert.ToDouble(((TextBox)sender).Text) <= 159.0) || (txtObserveDiastolic.Text != null && txtObserveDiastolic.Text != "" && Convert.ToDouble(txtObserveDiastolic.Text) >= 90.0 && Convert.ToDouble(txtObserveDiastolic.Text) <= 99.0))
                                {
                                    chkPDStageIhypertensionMildhypertension.IsChecked = true;
                                    chk_Click(chkPDStageIhypertensionMildhypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIhypertensionMildhypertension.IsChecked = false;
                                    chk_Click(chkPDStageIhypertensionMildhypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIIhypertensionModeratehypertension") is CheckBox && Form.FindName("ObserveDiastolic") is TextBox)
                            {
                                CheckBox chkPDStageIIhypertensionModeratehypertension = (CheckBox)Form.FindName("PDStageIIhypertensionModeratehypertension");
                                TextBox txtObserveDiastolic = (TextBox)Form.FindName("ObserveDiastolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 160.0 && Convert.ToDouble(((TextBox)sender).Text) <= 179.0) || (txtObserveDiastolic.Text != null && txtObserveDiastolic.Text != "" && Convert.ToDouble(txtObserveDiastolic.Text) >= 100.0 && Convert.ToDouble(txtObserveDiastolic.Text) <= 109.0))
                                {
                                    chkPDStageIIhypertensionModeratehypertension.IsChecked = true;
                                    chk_Click(chkPDStageIIhypertensionModeratehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIIhypertensionModeratehypertension.IsChecked = false;
                                    chk_Click(chkPDStageIIhypertensionModeratehypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIIIhypertensionSeverehypertension") is CheckBox && Form.FindName("ObserveDiastolic") is TextBox)
                            {
                                CheckBox chkPDStageIIIhypertensionSeverehypertension = (CheckBox)Form.FindName("PDStageIIIhypertensionSeverehypertension");
                                TextBox txtObserveDiastolic = (TextBox)Form.FindName("ObserveDiastolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 180.0) || (txtObserveDiastolic.Text != null && txtObserveDiastolic.Text != "" && Convert.ToDouble(txtObserveDiastolic.Text) >= 110.0))
                                {
                                    chkPDStageIIIhypertensionSeverehypertension.IsChecked = true;
                                    chk_Click(chkPDStageIIIhypertensionSeverehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIIIhypertensionSeverehypertension.IsChecked = false;
                                    chk_Click(chkPDStageIIIhypertensionSeverehypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("AlarmPersistentsystolic200aftermedication") is CheckBox && Form.FindName("HistSystolic") is TextBox && Form.FindName("DHistAntihypertensivesYes") is RadioButton && Form.FindName("DHistAntihypertensivesNo") is RadioButton)
                            {
                                CheckBox chkAlarmPersistentsystolic200aftermedication = (CheckBox)Form.FindName("AlarmPersistentsystolic200aftermedication");
                                TextBox txtHistSystolic = (TextBox)Form.FindName("HistSystolic");
                                RadioButton rdbDHistAntihypertensivesYes = (RadioButton)Form.FindName("DHistAntihypertensivesYes");
                                RadioButton rdbDHistAntihypertensivesNo = (RadioButton)Form.FindName("DHistAntihypertensivesNo");

                                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 200.0 && txtHistSystolic.Text != null && txtHistSystolic.Text != "" && Convert.ToDouble(txtHistSystolic.Text) >= 200.0 && rdbDHistAntihypertensivesYes.IsChecked == true)
                                {
                                    chkAlarmPersistentsystolic200aftermedication.IsChecked = true;
                                    chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmPersistentsystolic200aftermedication.IsChecked = false;
                                    chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is TextBox && ((TextBox)sender).Name == "ObserveDiastolic")
                        {
                            if (Form.FindName("PDPrehypertension") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkPDPrehypertension = (CheckBox)Form.FindName("PDPrehypertension");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 80.0 && Convert.ToDouble(((TextBox)sender).Text) <= 89.0) || (txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 120.0 && Convert.ToDouble(txtObserveSystolic.Text) <= 139.0))
                                {
                                    chkPDPrehypertension.IsChecked = true;
                                    chk_Click(chkPDPrehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDPrehypertension.IsChecked = false;
                                    chk_Click(chkPDPrehypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIhypertensionMildhypertension") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkPDStageIhypertensionMildhypertension = (CheckBox)Form.FindName("PDStageIhypertensionMildhypertension");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 90.0 && Convert.ToDouble(((TextBox)sender).Text) <= 99.0) || (txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 140.0 && Convert.ToDouble(txtObserveSystolic.Text) <= 159.0))
                                {
                                    chkPDStageIhypertensionMildhypertension.IsChecked = true;
                                    chk_Click(chkPDStageIhypertensionMildhypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIhypertensionMildhypertension.IsChecked = false;
                                    chk_Click(chkPDStageIhypertensionMildhypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIIhypertensionModeratehypertension") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkPDStageIIhypertensionModeratehypertension = (CheckBox)Form.FindName("PDStageIIhypertensionModeratehypertension");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 100.0 && Convert.ToDouble(((TextBox)sender).Text) <= 109.0) || (txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 160.0 && Convert.ToDouble(txtObserveSystolic.Text) <= 179.0))
                                {
                                    chkPDStageIIhypertensionModeratehypertension.IsChecked = true;
                                    chk_Click(chkPDStageIIhypertensionModeratehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIIhypertensionModeratehypertension.IsChecked = false;
                                    chk_Click(chkPDStageIIhypertensionModeratehypertension, new RoutedEventArgs());
                                }
                            }

                            if (Form.FindName("PDStageIIIhypertensionSeverehypertension") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkPDStageIIIhypertensionSeverehypertension = (CheckBox)Form.FindName("PDStageIIIhypertensionSeverehypertension");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                                if ((((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 110.0) || (txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 180.0))
                                {
                                    chkPDStageIIIhypertensionSeverehypertension.IsChecked = true;
                                    chk_Click(chkPDStageIIIhypertensionSeverehypertension, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDStageIIIhypertensionSeverehypertension.IsChecked = false;
                                    chk_Click(chkPDStageIIIhypertensionSeverehypertension, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is TextBox && ((TextBox)sender).Name == "HistSystolic")
                        {
                            if (Form.FindName("AlarmPersistentsystolic200aftermedication") is CheckBox && Form.FindName("ObserveSystolic") is TextBox && Form.FindName("DHistAntihypertensivesYes") is RadioButton && Form.FindName("DHistAntihypertensivesNo") is RadioButton)
                            {
                                CheckBox chkAlarmPersistentsystolic200aftermedication = (CheckBox)Form.FindName("AlarmPersistentsystolic200aftermedication");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");
                                RadioButton rdbDHistAntihypertensivesYes = (RadioButton)Form.FindName("DHistAntihypertensivesYes");
                                RadioButton rdbDHistAntihypertensivesNo = (RadioButton)Form.FindName("DHistAntihypertensivesNo");

                                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && Convert.ToDouble(((TextBox)sender).Text) >= 200.0 && txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 200.0 && rdbDHistAntihypertensivesYes.IsChecked == true)
                                {
                                    chkAlarmPersistentsystolic200aftermedication.IsChecked = true;
                                    chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmPersistentsystolic200aftermedication.IsChecked = false;
                                    chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                }
                            }
                        }
                    }
                }

                if ((((TextBox)sender).Name == "Weight" || ((TextBox)sender).Name == "Height") && IsFirstTime == false)
                {
                    if (Form.FindName("Weight") is TextBox && Form.FindName("Height") is TextBox)
                    {
                        TextBox txtWeight = (TextBox)Form.FindName("Weight");
                        TextBox txtHeight = (TextBox)Form.FindName("Height");
                        if (Form.FindName("StuntingPercent") is TextBox && Form.FindName("WastingPercent") is TextBox)
                        {
                            TextBox txtStuning = (TextBox)Form.FindName("StuntingPercent");
                            TextBox txtWasting = (TextBox)Form.FindName("WastingPercent");

                            if (txtHeight.Text != null && txtHeight.Text != "")
                            {
                                double stun = (double.Parse(txtHeight.Text) / 80) * 100;
                                stun = Math.Round(stun * 100) / 100;
                                txtStuning.Text = stun.ToString();
                            }
                            if (txtWeight.Text != null && txtWeight.Text != "")
                            {
                                double wast = (double.Parse(txtWeight.Text) / 15) * 100;
                                wast = Math.Round(wast * 100) / 100;
                                txtWasting.Text = wast.ToString();
                            }
                        }
                    }
                }


            }
        }

        private void CheckChildElements(FieldDetail pitem, bool Override)
        {
            if (!Override)
            {
                foreach (var item in pitem.DependentFieldDetail)
                {
                    if (item.Control is FrameworkElement)
                    {
                        ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                    }
                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                        CheckChildElements(item, Override);
                }
            }
            else
            {
                CheckChildWithOverRide(pitem, Override);

            }
        }

        private void CheckChildWithOverRide(FieldDetail pitem, bool Override)
        {
            foreach (var item in pitem.DependentFieldDetail)
            {
                if (item.Control is FrameworkElement)
                {
                    if (pitem.Settings is DecimalFieldSetting)
                    {
                        switch (((DecimalExpression<decimal>)item.Condition).Operation)
                        {
                            case DoubleOperations.EqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue == ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.NotEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue != ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.GreterThan:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue < ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;

                            case DoubleOperations.GreterThanEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue <= ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.LessThan:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue > ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case DoubleOperations.LessThanEqualTo:
                                if (((DecimalExpression<decimal>)item.Condition).ReferenceValue >= ((DecimalFieldSetting)pitem.Settings).Value)
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                break;
                        }
                    }

                    if (pitem.Settings is BooleanFieldSetting)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((BooleanFieldSetting)pitem.Settings).Mode)
                                    ((TextBox)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((TextBox)item.Control).Visibility = Visibility.Collapsed;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((BooleanFieldSetting)pitem.Settings).Mode)
                                    ((TextBox)item.Control).Visibility = Visibility.Visible;
                                else
                                    ((TextBox)item.Control).Visibility = Visibility.Collapsed;
                                break;
                        }
                    }

                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                        CheckChildElements(item, Override);

                }
            }
        }

        void mrci_OnAddRemoveClick(object sender, RoutedEventArgs e)
        {

            var lstBox = ((Medication)((HyperlinkButton)sender).DataContext).Parent;
            var MedSetting = ((Medication)((HyperlinkButton)sender).DataContext).MedicationSetting;
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                MedSetting.ItemsSource.RemoveAt(((Medication)((HyperlinkButton)sender).DataContext).Index);
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                MedSetting.ItemsSource.Add(new Medication() { DrugSource = MedSetting.ItemsSource[0].DrugSource, DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
            }
            lstBox.Items.Clear();
        }

        void chk_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                try
                {

                    if (((FrameworkElement)(((FieldDetail)(((FrameworkElement)sender).DataContext))).Control).Visibility == Visibility.Visible)
                    {
                        foreach (var item in ((FieldDetail)(((FrameworkElement)sender).DataContext)).DependentFieldDetail)
                        {
                            if (item.Control is FrameworkElement && item.Condition is BooleanExpression<bool>)
                            {
                                switch (((BooleanExpression<bool>)item.Condition).Operation)
                                {
                                    case BooleanOperations.EqualTo:
                                        if (((BooleanExpression<bool>)item.Condition).ReferenceValue == ((BooleanFieldSetting)((FieldDetail)((FrameworkElement)sender).DataContext).Settings).Value)
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                    case BooleanOperations.NotEqualTo:
                                        if (((BooleanExpression<bool>)item.Condition).ReferenceValue != ((BooleanFieldSetting)((FieldDetail)((FrameworkElement)sender).DataContext).Settings).Value)
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                        else
                                            ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                        break;
                                }

                                if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                    CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                            }
                        }

                        if (((FieldDetail)(((FrameworkElement)sender).DataContext)).RelationalFieldList != null)
                            foreach (var item in ((FieldDetail)(((FrameworkElement)sender).DataContext)).RelationalFieldList)
                            {
                                if (item.Control is FrameworkElement && item.RelationCondition is BooleanExpression<bool>)
                                {
                                    switch (((BooleanExpression<bool>)item.RelationCondition).Operation)
                                    {
                                        case BooleanOperations.EqualTo:
                                            if (((BooleanExpression<bool>)item.RelationCondition).ReferenceValue == ((BooleanFieldSetting)((FieldDetail)((FrameworkElement)sender).DataContext).Settings).Value)
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                        case BooleanOperations.NotEqualTo:
                                            if (((BooleanExpression<bool>)item.RelationCondition).ReferenceValue != ((BooleanFieldSetting)((FieldDetail)((FrameworkElement)sender).DataContext).Settings).Value)
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Visible;
                                            else
                                                ((FrameworkElement)item.LabelControl).Visibility = ((FrameworkElement)item.Control).Visibility = Visibility.Collapsed;
                                            break;
                                    }


                                    if (item.DependentFieldDetail != null && item.DependentFieldDetail.Count > 0)
                                        CheckChildElements(item, ((FrameworkElement)item.Control).Visibility == Visibility.Visible ? true : false);
                                }
                            }
                    }


                }

                catch (Exception)
                {

                    //throw;
                }


                if (IsFirstTime == false)
                {

                    if (TemplateID == 4)
                    {
                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PrsntHeartBurn")
                        {
                            if (Form.FindName("PDCoughWithGERD") is CheckBox)
                            {
                                CheckBox chkCoughWithGERD = (CheckBox)Form.FindName("PDCoughWithGERD");
                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    chkCoughWithGERD.IsChecked = true;
                                    chk_Click(chkCoughWithGERD, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkCoughWithGERD.IsChecked = false;
                                    chk_Click(chkCoughWithGERD, new RoutedEventArgs());
                                }
                            }
                        }

                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "HistRunnyNoseRhinitis")
                        {
                            if (Form.FindName("PDCoughWithRhinitis") is CheckBox)
                            {
                                CheckBox chkPDCoughWithRhinitis = (CheckBox)Form.FindName("PDCoughWithRhinitis");
                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    chkPDCoughWithRhinitis.IsChecked = true;
                                    chk_Click(chkPDCoughWithRhinitis, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDCoughWithRhinitis.IsChecked = false;
                                    chk_Click(chkPDCoughWithRhinitis, new RoutedEventArgs());
                                }
                            }
                        }

                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "HistPostNasalDrip")
                        {
                            if (Form.FindName("PDCoughWithPostNasalDrips") is CheckBox)
                            {
                                CheckBox chkPDCoughWithPostNasalDrips = (CheckBox)Form.FindName("PDCoughWithPostNasalDrips");
                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    chkPDCoughWithPostNasalDrips.IsChecked = true;
                                    chk_Click(chkPDCoughWithPostNasalDrips, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDCoughWithPostNasalDrips.IsChecked = false;
                                    chk_Click(chkPDCoughWithPostNasalDrips, new RoutedEventArgs());
                                }
                            }
                        }

                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "RiskCardiacProblemsCHF")
                        {
                            if (Form.FindName("AlarmPersistentCoughCardiovascularDisease") is CheckBox && Form.FindName("CoughDuration") is ComboBox)
                            {
                                CheckBox chkAlarm = (CheckBox)Form.FindName("AlarmPersistentCoughCardiovascularDisease");
                                ComboBox cmbCough = (ComboBox)Form.FindName("CoughDuration");
                                if (((CheckBox)sender).IsChecked == true && cmbCough.SelectedItem != null && (((DynamicListItem)cmbCough.SelectedItem).Title == "3 weeks to 8 weeks" || ((DynamicListItem)cmbCough.SelectedItem).Title == "More than 8 weeks"))
                                {
                                    //chkAlarm.Visibility = Visibility.Visible;
                                    //((FrameworkElement)((FieldDetail)chkAlarm.DataContext).LabelControl).Visibility = Visibility.Visible;
                                    chkAlarm.IsChecked = true;
                                    chk_Click(chkAlarm, new RoutedEventArgs());
                                }
                                else
                                {
                                    //chkAlarm.Visibility = Visibility.Collapsed;
                                    //((FrameworkElement)((FieldDetail)chkAlarm.DataContext).LabelControl).Visibility = Visibility.Collapsed;
                                    chkAlarm.IsChecked = false;
                                    chk_Click(chkAlarm, new RoutedEventArgs());
                                }
                            }
                        }

                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PDPersistentCough")
                        {
                            if (Form.FindName("InvestSinusImaging") is CheckBox && Form.FindName("InvestSpirometry") is RadioButton && Form.FindName("InvestLabs") is ListBox && Form.FindName("InvestRadiology") is ListBox)
                            {
                                CheckBox chkInvestSinusImaging = (CheckBox)Form.FindName("InvestSinusImaging");
                                RadioButton chkInvestSpirometry = (RadioButton)Form.FindName("InvestSpirometry");
                                ListBox chkInvestLabs = (ListBox)Form.FindName("InvestLabs");
                                ListBox chkInvestRadiology = (ListBox)Form.FindName("InvestRadiology");

                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    chkInvestSinusImaging.IsChecked = true;
                                    chk_Click(chkInvestSinusImaging, new RoutedEventArgs());

                                    chkInvestSpirometry.IsChecked = true;
                                    chk_Click(chkInvestSpirometry, new RoutedEventArgs());

                                    //(((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(chkInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }

                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestRadiology.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestRadiology.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Chest X - Ray")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(chkInvestRadiology, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chkInvestSinusImaging.IsChecked = false;
                                    chk_Click(chkInvestSinusImaging, new RoutedEventArgs());

                                    chkInvestSpirometry.IsChecked = false;
                                    chk_Click(chkInvestSpirometry, new RoutedEventArgs());

                                    //(((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource != null && ((InvestigationFieldSetting)((FieldDetail)((Control)sender).Tag).Settings).ItemSource.First(f => (f.ID == ((CheckListExpression<bool>)item.RelationCondition).SelectedItem.ID && f.Status == true)) != null)
                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(chkInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }

                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestRadiology.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestRadiology.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Chest X - Ray")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(chkInvestRadiology, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (TemplateID == 7)
                    {
                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PresFreqWateryLooseStool")
                        {
                            if (Form.FindName("AlarmDiarrheaFever") is CheckBox && Form.FindName("ObserveTemperature") is TextBox)
                            {
                                CheckBox chkAlarmDiarrheaFever = (CheckBox)Form.FindName("AlarmDiarrheaFever");
                                TextBox txtTemp = (TextBox)Form.FindName("ObserveTemperature");

                                if (((CheckBox)sender).IsChecked == true && txtTemp.Text != null && txtTemp.Text != "" && Convert.ToDouble(txtTemp.Text) > 102.2)
                                {
                                    chkAlarmDiarrheaFever.IsChecked = true;
                                    chk_Click(chkAlarmDiarrheaFever, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmDiarrheaFever.IsChecked = false;
                                    chk_Click(chkAlarmDiarrheaFever, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && (((FrameworkElement)sender).Name == "PresMucusStool" || ((FrameworkElement)sender).Name == "PresBloodStool"))
                        {
                            if (Form.FindName("PDDysentry") is CheckBox && Form.FindName("PDDysentry") is CheckBox && Form.FindName("PresBloodStool") is CheckBox)
                            {
                                CheckBox chkPDDysentry = (CheckBox)Form.FindName("PDDysentry");
                                CheckBox chkPresMucusStool = (CheckBox)Form.FindName("PresMucusStool");
                                CheckBox chkPresBloodStool = (CheckBox)Form.FindName("PresBloodStool");

                                if (chkPresMucusStool.IsChecked == true || chkPresBloodStool.IsChecked == true)
                                {
                                    chkPDDysentry.IsChecked = true;
                                    chk_Click(chkPDDysentry, new RoutedEventArgs());
                                }
                                else if (chkPresMucusStool.IsChecked == false && chkPresBloodStool.IsChecked == false)
                                {
                                    chkPDDysentry.IsChecked = false;
                                    chk_Click(chkPDDysentry, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && (((FrameworkElement)sender).Name == "PresDecreasedUrination" || ((FrameworkElement)sender).Name == "PresSunkenEyes" || ((FrameworkElement)sender).Name == "PresDryMouthOrSkin"))
                        {
                            if (Form.FindName("AlarmSevereDehydration") is CheckBox && Form.FindName("PresDecreasedUrination") is CheckBox && Form.FindName("PresSunkenEyes") is CheckBox && Form.FindName("PresDryMouthOrSkin") is CheckBox)
                            {
                                CheckBox chkAlarmSevereDehydration = (CheckBox)Form.FindName("AlarmSevereDehydration");
                                CheckBox chkPresDecreasedUrination = (CheckBox)Form.FindName("PresDecreasedUrination");
                                CheckBox chkPresSunkenEyes = (CheckBox)Form.FindName("PresSunkenEyes");
                                CheckBox chkPresDryMouthOrSkin = (CheckBox)Form.FindName("PresDryMouthOrSkin");

                                if (chkPresDecreasedUrination.IsChecked == true || chkPresSunkenEyes.IsChecked == true || chkPresDryMouthOrSkin.IsChecked == true)
                                {
                                    chkAlarmSevereDehydration.IsChecked = true;
                                    chk_Click(chkAlarmSevereDehydration, new RoutedEventArgs());
                                }
                                else if (chkPresDecreasedUrination.IsChecked == false && chkPresSunkenEyes.IsChecked == false && chkPresDryMouthOrSkin.IsChecked == false)
                                {
                                    chkAlarmSevereDehydration.IsChecked = false;
                                    chk_Click(chkAlarmSevereDehydration, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && (((FrameworkElement)sender).Name == "HistRecentAntibioticUseYes" || ((FrameworkElement)sender).Name == "HistRecentAntibioticUseNo"))
                        {
                            if (Form.FindName("InvestCDifficileAssay") is CheckBox && Form.FindName("HistRecentAntibioticUseYes") is RadioButton && Form.FindName("HistRecentAntibioticUseNo") is RadioButton)
                            {
                                CheckBox chkInvestCDifficileAssay = (CheckBox)Form.FindName("InvestCDifficileAssay");
                                RadioButton rdbHistRecentAntibioticUseYes = (RadioButton)Form.FindName("HistRecentAntibioticUseYes");
                                RadioButton rdbHistRecentAntibioticUseNo = (RadioButton)Form.FindName("HistRecentAntibioticUseNo");

                                if (rdbHistRecentAntibioticUseYes.IsChecked == true)
                                {
                                    chkInvestCDifficileAssay.IsChecked = true;
                                    chk_Click(chkInvestCDifficileAssay, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkInvestCDifficileAssay.IsChecked = false;
                                    chk_Click(chkInvestCDifficileAssay, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "GEPallor")
                        {
                            if (Form.FindName("PESkinColour") is ComboBox && Form.FindName("InvestLabs") is ListBox)
                            {
                                ComboBox cmbPESkinColour = (ComboBox)Form.FindName("PESkinColour");
                                ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");

                                if ((cmbPESkinColour.SelectedItem != null && ((DynamicListItem)cmbPESkinColour.SelectedItem).Title == "Pale") || (((CheckBox)sender).IsChecked == true))
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PDDysentry")
                        {
                            if (Form.FindName("InvestLabs") is ListBox)
                            {
                                ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");

                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Stool Examination" || a.Description == "Stool for Occult Blood")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Stool Examination" || a.Description == "Stool for Occult Blood")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PDPersistentDiarrhea")
                        {
                            if (Form.FindName("InvestLabs") is ListBox)
                            {
                                ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");

                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Stool Culture and sensitivity")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Stool Culture and sensitivity")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (TemplateID == 5)
                    {
                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PresNauseaVomiting")
                        {
                            if (Form.FindName("AlarmHypertensionNauseaVomiting") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkAlarmAlarmHypertensionNauseaVomiting = (CheckBox)Form.FindName("AlarmHypertensionNauseaVomiting");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                                if (((CheckBox)sender).IsChecked == true && txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) > 140.0)
                                {
                                    chkAlarmAlarmHypertensionNauseaVomiting.IsChecked = true;
                                    //chkAlarmAlarmHypertensionNauseaVomiting.Visibility = Visibility.Visible;
                                    chk_Click(chkAlarmAlarmHypertensionNauseaVomiting, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkAlarmAlarmHypertensionNauseaVomiting.IsChecked = false;
                                    //chkAlarmAlarmHypertensionNauseaVomiting.Visibility = Visibility.Collapsed;
                                    chk_Click(chkAlarmAlarmHypertensionNauseaVomiting, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "DHistAntihypertensivesYes") || (((FrameworkElement)sender).Name == "DHistAntihypertensivesNo")))
                        {
                            if (Form.FindName("HistSystolic") is TextBox && Form.FindName("AlarmPersistentsystolic200aftermedication") is CheckBox && Form.FindName("ObserveSystolic") is TextBox)
                            {
                                CheckBox chkAlarmPersistentsystolic200aftermedication = (CheckBox)Form.FindName("AlarmPersistentsystolic200aftermedication");
                                TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");
                                TextBox txtHistSystolic = (TextBox)Form.FindName("HistSystolic");

                                if (((RadioButton)sender).Name == "DHistAntihypertensivesYes")
                                {
                                    if (txtHistSystolic.Text != null && txtHistSystolic.Text != "" && Convert.ToDouble(txtHistSystolic.Text) >= 200.0 && txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 200.0 && ((RadioButton)sender).IsChecked == true)
                                    {
                                        chkAlarmPersistentsystolic200aftermedication.IsChecked = true;
                                        chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                    }
                                    else
                                    {
                                        chkAlarmPersistentsystolic200aftermedication.IsChecked = false;
                                        chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                    }
                                }
                                else if (((RadioButton)sender).Name == "DHistAntihypertensivesNo")
                                {
                                    if (txtHistSystolic.Text != null && txtHistSystolic.Text != "" && Convert.ToDouble(txtHistSystolic.Text) >= 200.0 && txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 200.0 && ((RadioButton)sender).IsChecked == true)
                                    {
                                        chkAlarmPersistentsystolic200aftermedication.IsChecked = false;
                                        chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                    }
                                    else
                                    {
                                        chkAlarmPersistentsystolic200aftermedication.IsChecked = true;
                                        chk_Click(chkAlarmPersistentsystolic200aftermedication, new RoutedEventArgs());
                                    }
                                }

                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "RFDiabetesYes") || (((FrameworkElement)sender).Name == "RFDiabetesNo") || (((FrameworkElement)sender).Name == "FHDiabetesYes") || (((FrameworkElement)sender).Name == "FHDiabetesNo")))
                        {
                            if (Form.FindName("InvestLabServices") is ListBox)
                            {
                                ListBox lstInvestLabServices = (ListBox)Form.FindName("InvestLabServices");

                                RadioButton rdRFDiabetesYes = (RadioButton)Form.FindName("RFDiabetesYes");
                                RadioButton rdRFDiabetesNo = (RadioButton)Form.FindName("RFDiabetesNo");
                                RadioButton rdFHDiabetesYes = (RadioButton)Form.FindName("FHDiabetesYes");
                                RadioButton rdFHDiabetesNo = (RadioButton)Form.FindName("FHDiabetesNo");

                                if (((RadioButton)sender).Name == "RFDiabetesYes" || ((RadioButton)sender).Name == "FHDiabetesYes")
                                {
                                    if (rdRFDiabetesYes.IsChecked == true || rdFHDiabetesYes.IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Random Blood Sugar (RBS)")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Random Blood Sugar (RBS)")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (((RadioButton)sender).Name == "RFDiabetesNo" || ((RadioButton)sender).Name == "FHDiabetesNo")
                                {
                                    if (rdRFDiabetesNo.IsChecked == true && rdFHDiabetesNo.IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Random Blood Sugar (RBS)")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Random Blood Sugar (RBS)")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && (((FrameworkElement)sender).Name == "PHRenalKidneyDiseaseYes") || (((FrameworkElement)sender).Name == "PHRenalKidneyDiseaseNo"))
                        {
                            if (Form.FindName("InvestUltrasoundAbdomen") is CheckBox && Form.FindName("InvestDopplerFlowStudy") is CheckBox && Form.FindName("InvestLabs") is ListBox)
                            {
                                CheckBox chkInvestUltrasoundAbdomen = (CheckBox)Form.FindName("InvestUltrasoundAbdomen");
                                CheckBox chkInvestInvestDopplerFlowStudy = (CheckBox)Form.FindName("InvestDopplerFlowStudy");
                                ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");

                                if (((RadioButton)sender).Name == "PHRenalKidneyDiseaseYes")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        chkInvestUltrasoundAbdomen.IsChecked = true;
                                        chk_Click(chkInvestUltrasoundAbdomen, new RoutedEventArgs());

                                        chkInvestInvestDopplerFlowStudy.IsChecked = true;
                                        chk_Click(chkInvestInvestDopplerFlowStudy, new RoutedEventArgs());

                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine Clearence Test ")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chkInvestUltrasoundAbdomen.IsChecked = false;
                                        chk_Click(chkInvestUltrasoundAbdomen, new RoutedEventArgs());

                                        chkInvestInvestDopplerFlowStudy.IsChecked = false;
                                        chk_Click(chkInvestInvestDopplerFlowStudy, new RoutedEventArgs());

                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine Clearence Test ")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (((RadioButton)sender).Name == "PHRenalKidneyDiseaseNo")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        chkInvestUltrasoundAbdomen.IsChecked = false;
                                        chk_Click(chkInvestUltrasoundAbdomen, new RoutedEventArgs());

                                        chkInvestInvestDopplerFlowStudy.IsChecked = false;
                                        chk_Click(chkInvestInvestDopplerFlowStudy, new RoutedEventArgs());

                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine Clearence Test ")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chkInvestUltrasoundAbdomen.IsChecked = true;
                                        chk_Click(chkInvestUltrasoundAbdomen, new RoutedEventArgs());

                                        chkInvestInvestDopplerFlowStudy.IsChecked = true;
                                        chk_Click(chkInvestInvestDopplerFlowStudy, new RoutedEventArgs());

                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine Clearence Test ")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabs, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "FHHyperlipidemiaYes") || (((FrameworkElement)sender).Name == "FHHyperlipidemiaNo")))
                        {
                            if (Form.FindName("InvestLabServices") is ListBox)
                            {
                                ListBox lstInvestLabServices = (ListBox)Form.FindName("InvestLabServices");

                                if (((RadioButton)sender).Name == "FHHyperlipidemiaYes")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "LIPID PROFILE : I")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "LIPID PROFILE : I")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (((RadioButton)sender).Name == "FHHyperlipidemiaNo")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "LIPID PROFILE : I")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "LIPID PROFILE : I")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "PEPedalEdemaYes") || (((FrameworkElement)sender).Name == "PEPedalEdemaNo")))
                        {
                            if (Form.FindName("InvestLabServices") is ListBox)
                            {
                                ListBox lstInvestLabServices = (ListBox)Form.FindName("InvestLabServices");

                                if (((RadioButton)sender).Name == "PEPedalEdemaYes")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine - Serum ")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                                if (a.Description == "ELECTROLYTE PROFILE")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine - Serum ")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                                if (a.Description == "ELECTROLYTE PROFILE")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (((RadioButton)sender).Name == "PEPedalEdemaNo")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine - Serum ")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                                if (a.Description == "ELECTROLYTE PROFILE")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Creatinine - Serum ")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                                if (a.Description == "ELECTROLYTE PROFILE")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "PESignsOfAnemiaYes") || (((FrameworkElement)sender).Name == "PESignsOfAnemiaNo")))
                        {
                            if (Form.FindName("InvestLabServices") is ListBox)
                            {
                                ListBox lstInvestLabServices = (ListBox)Form.FindName("InvestLabServices");

                                if (((RadioButton)sender).Name == "PESignsOfAnemiaYes")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Complete Blood Count (CBC)")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Complete Blood Count (CBC)")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (((RadioButton)sender).Name == "PESignsOfAnemiaNo")
                                {
                                    if (((RadioButton)sender).IsChecked == true)
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Complete Blood Count (CBC)")
                                                {
                                                    a.Status = false;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                                        {
                                            foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                            {
                                                if (a.Description == "Complete Blood Count (CBC)")
                                                {
                                                    a.Status = true;
                                                    InvestCLBCI_chkItemClicked(lstInvestLabServices, new RoutedEventArgs());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (TemplateID == 6)
                    {
                        if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "PresRefluxSymptomsHeartburnAcidRegurgitation")
                        {
                            if (Form.FindName("PDGastroEsophagealRefluxDiseaseGERD") is CheckBox)
                            {
                                CheckBox chkPDGastroEsophagealRefluxDiseaseGERD = (CheckBox)Form.FindName("PDGastroEsophagealRefluxDiseaseGERD");

                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    chkPDGastroEsophagealRefluxDiseaseGERD.IsChecked = true;
                                    chk_Click(chkPDGastroEsophagealRefluxDiseaseGERD, new RoutedEventArgs());
                                }
                                else
                                {
                                    chkPDGastroEsophagealRefluxDiseaseGERD.IsChecked = false;
                                    chk_Click(chkPDGastroEsophagealRefluxDiseaseGERD, new RoutedEventArgs());
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "HistBloodStoolYes") || (((FrameworkElement)sender).Name == "HistBloodStoolNo") || (((FrameworkElement)sender).Name == "HistBloodVomitusYes") || (((FrameworkElement)sender).Name == "HistBloodVomitusNo")))
                        {
                            if (Form.FindName("AlarmGastrointestinalBleedingVomitingStools") is CheckBox)
                            {
                                CheckBox chkAlarmGastrointestinalBleedingVomitingStools = (CheckBox)Form.FindName("AlarmGastrointestinalBleedingVomitingStools");

                                RadioButton rdHistBloodStoolYes = (RadioButton)Form.FindName("HistBloodStoolYes");
                                RadioButton rdHistBloodStoolNo = (RadioButton)Form.FindName("HistBloodStoolNo");
                                RadioButton rdHistBloodVomitusYes = (RadioButton)Form.FindName("HistBloodVomitusYes");
                                RadioButton rdHistBloodVomitusNo = (RadioButton)Form.FindName("HistBloodVomitusNo");

                                if (((RadioButton)sender).Name == "HistBloodStoolYes" || ((RadioButton)sender).Name == "HistBloodVomitusYes")
                                {
                                    if (rdHistBloodStoolYes.IsChecked == true || rdHistBloodVomitusYes.IsChecked == true)
                                    {
                                        chkAlarmGastrointestinalBleedingVomitingStools.IsChecked = true;
                                        chk_Click(chkAlarmGastrointestinalBleedingVomitingStools, new RoutedEventArgs());
                                    }
                                    else
                                    {
                                        chkAlarmGastrointestinalBleedingVomitingStools.IsChecked = false;
                                        chk_Click(chkAlarmGastrointestinalBleedingVomitingStools, new RoutedEventArgs());
                                    }
                                }
                                else if (((RadioButton)sender).Name == "HistBloodStoolNo" || ((RadioButton)sender).Name == "HistBloodVomitusNo")
                                {
                                    if (rdHistBloodStoolNo.IsChecked == true && rdHistBloodVomitusNo.IsChecked == true)
                                    {
                                        chkAlarmGastrointestinalBleedingVomitingStools.IsChecked = false;
                                        chk_Click(chkAlarmGastrointestinalBleedingVomitingStools, new RoutedEventArgs());
                                    }
                                    else
                                    {
                                        chkAlarmGastrointestinalBleedingVomitingStools.IsChecked = true;
                                        chk_Click(chkAlarmGastrointestinalBleedingVomitingStools, new RoutedEventArgs());
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((FrameworkElement)sender).Name == "GEPallor")
                        {
                            if (Form.FindName("InvestLabs") is ListBox)
                            {
                                ListBox chkInvestLabs = (ListBox)Form.FindName("InvestLabs");

                                if (((CheckBox)sender).IsChecked == true)
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = true;
                                                InvestCLBCI_chkItemClicked(chkInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource != null)
                                    {
                                        foreach (var a in ((InvestigationFieldSetting)((FieldDetail)chkInvestLabs.Tag).Settings).ItemSource)
                                        {
                                            if (a.Description == "Complete Blood Count (CBC)")
                                            {
                                                a.Status = false;
                                                InvestCLBCI_chkItemClicked(chkInvestLabs, new RoutedEventArgs());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sender is FrameworkElement && ((((FrameworkElement)sender).Name == "GEMassesYes") || (((FrameworkElement)sender).Name == "GEMassesNo") || (((FrameworkElement)sender).Name == "GELymphadenopathy")))
                        {
                            if (Form.FindName("AlarmPalpableMassLymphadenopathy") is CheckBox)
                            {
                                CheckBox chkAlarmPalpableMassLymphadenopathy = (CheckBox)Form.FindName("AlarmPalpableMassLymphadenopathy");

                                RadioButton rdGEMassesYes = (RadioButton)Form.FindName("GEMassesYes");
                                RadioButton rdGEMassesNo = (RadioButton)Form.FindName("GEMassesNo");
                                CheckBox chkGELymphadenopathy = (CheckBox)Form.FindName("GELymphadenopathy");

                                if (sender is CheckBox)
                                {
                                    if (((CheckBox)sender).IsChecked == true || rdGEMassesYes.IsChecked == true)
                                    {
                                        chkAlarmPalpableMassLymphadenopathy.IsChecked = true;
                                        chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                    }
                                    else
                                    {
                                        chkAlarmPalpableMassLymphadenopathy.IsChecked = false;
                                        chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                    }
                                }
                                else if (sender is RadioButton)
                                {
                                    if (((RadioButton)sender).Name == "GEMassesYes")
                                    {
                                        if (((RadioButton)sender).IsChecked == true || chkGELymphadenopathy.IsChecked == true)
                                        {
                                            chkAlarmPalpableMassLymphadenopathy.IsChecked = true;
                                            chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                        }
                                        else
                                        {
                                            chkAlarmPalpableMassLymphadenopathy.IsChecked = false;
                                            chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                        }
                                    }
                                    else if (((RadioButton)sender).Name == "GEMassesNo")
                                    {
                                        if (((RadioButton)sender).IsChecked == true && chkGELymphadenopathy.IsChecked == false)
                                        {
                                            chkAlarmPalpableMassLymphadenopathy.IsChecked = false;
                                            chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                        }
                                        else
                                        {
                                            chkAlarmPalpableMassLymphadenopathy.IsChecked = true;
                                            chk_Click(chkAlarmPalpableMassLymphadenopathy, new RoutedEventArgs());
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

        }

        private Grid GetSectionLayout(string Title)
        {
            Grid OverLayGrid = new Grid();
            OverLayGrid.Tag = Title;
            OverLayGrid.Margin = new Thickness(0, 0, 0, 5);
            Border ContentBorder = new Border();
            ContentBorder.BorderBrush = (Brush)this.Resources["BorderDefault"];
            ContentBorder.BorderThickness = new Thickness(1);
            ContentBorder.CornerRadius = new CornerRadius(5);
            ContentBorder.Padding = new Thickness(5);
            ContentBorder.Margin = new Thickness(0, 8, 0, 0);
            Grid ContentGrid = new Grid();
            ContentGrid.Margin = new Thickness(5, 15, 5, 5);
            ContentBorder.Child = ContentGrid;
            OverLayGrid.Children.Add(ContentBorder);
            Border groupBorder = new Border();
            groupBorder.HorizontalAlignment = HorizontalAlignment.Left;
            groupBorder.Margin = new Thickness(8, 0, 0, 0);
            groupBorder.VerticalAlignment = VerticalAlignment.Top;
            groupBorder.Background = (Brush)this.Resources["BackgroundDefault"];
            groupBorder.BorderBrush = (Brush)this.Resources["BorderDefault"];
            groupBorder.BorderThickness = new Thickness(1);
            groupBorder.CornerRadius = new CornerRadius(5);
            groupBorder.RenderTransformOrigin = new Point(0.5, 0.5);
            TransformGroup groupBorderTransform = new TransformGroup();
            var st = new ScaleTransform();
            st.ScaleY = 0.994;
            var skt = new SkewTransform();
            skt.AngleX = -20;
            var rt = new RotateTransform();
            var tt = new TranslateTransform();
            tt.X = 3.627;
            tt.Y = 0.063;
            groupBorderTransform.Children.Add(st);
            groupBorderTransform.Children.Add(skt);
            groupBorderTransform.Children.Add(rt);
            groupBorderTransform.Children.Add(tt);
            groupBorder.RenderTransform = groupBorderTransform;
            TextBlock tbl = new TextBlock();
            tbl.Text = Title;
            tbl.FontFamily = new FontFamily("Portable User Interface");
            tbl.Margin = new Thickness(10, 1, 10, 1);
            tbl.Foreground = this.Resources["Heading"] as Brush;
            tbl.FontWeight = FontWeights.Bold;
            groupBorder.Child = tbl;
            OverLayGrid.Children.Add(groupBorder);
            return OverLayGrid;

        }

        private void PrintPCR()
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/EMRPatientPrescriptionSummary.aspx?UnitID=" + CurrentVisit.UnitId + "&VisitID=" + CurrentVisit.ID + "&PatientID=" + this.SelectedPatient.PatientId + "&PatientUnitID=" + this.SelectedPatient.patientUnitID + "&TemplateID=" + TemplateID), "_blank");
        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //PatientCaseRecord PCR = GetPCR("PrintPCR");
            //e.PageVisual = PCR;
        }

        PatientCaseRecordSetting PCRS = new PatientCaseRecordSetting();
        //private PatientCaseRecord GetPCR(string str)
        //{
        //    PatientCaseRecord PCR = new PatientCaseRecord();

        //    string ComplaintReported = "";
        //    string ChiefComplaint = "";
        //    string PastMedicalHistory = "";
        //    string DrugHistory = "";
        //    string Allergies = "";
        //    string CC = "";
        //    string Alarm = "";

        //    CC = Title.ToString();

        //    IEnumerator<UIElement> list = (IEnumerator<UIElement>)Form.Children.GetEnumerator();
        //    while (list.MoveNext())
        //    {
        //        Grid sec = (Grid)list.Current;
        //        if ((string)sec.Tag == "Alarm Features")
        //        {
        //            //Grid cont = (Grid)((Border)sec.Children[0]).Child;
        //            IEnumerator<UIElement> lst1 = (IEnumerator<UIElement>)((Grid)((Border)sec.Children[0]).Child).Children.GetEnumerator();

        //            while (lst1.MoveNext())
        //            {
        //                if (lst1.Current is CheckBox)
        //                {
        //                    CheckBox chk = (CheckBox)lst1.Current;
        //                    if (chk.Visibility == Visibility.Visible)
        //                    {
        //                        Alarm += ((FieldDetail)chk.DataContext).Title + ",\t";
        //                    }
        //                }
        //            }
        //            break;
        //        }
        //    }
        //    if (((TextBox)Form.FindName("ComplaintReported")) != null)
        //        ComplaintReported = ((TextBox)Form.FindName("ComplaintReported")).Text == null ? "" : ((TextBox)Form.FindName("ComplaintReported")).Text;
        //    if (((TextBox)Form.FindName("RMChiefComplaints")) != null)
        //        ChiefComplaint = ((TextBox)Form.FindName("RMChiefComplaints")).Text == null ? "" : ((TextBox)Form.FindName("RMChiefComplaints")).Text;
        //    if (((TextBox)Form.FindName("PastMedicalHistory")) != null)
        //        PastMedicalHistory = ((TextBox)Form.FindName("PastMedicalHistory")).Text == null ? "" : ((TextBox)Form.FindName("PastMedicalHistory")).Text;
        //    if (((TextBox)Form.FindName("DrugHistory")) != null)
        //        DrugHistory = ((TextBox)Form.FindName("DrugHistory")).Text == null ? "" : ((TextBox)Form.FindName("DrugHistory")).Text;


        //    string Investigations = "";
        //    int i = 0;
        //    if ((CheckBox)Form.FindName("StoolME") != null && (bool)((CheckBox)Form.FindName("StoolME")).IsChecked == true)
        //    {
        //        Investigations += "Stool ME \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("StoolCulture") != null && (bool)((CheckBox)Form.FindName("StoolCulture")).IsChecked == true)
        //    {
        //        Investigations += "Stool Culture \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("CBC") != null && (bool)((CheckBox)Form.FindName("CBC")).IsChecked == true)
        //    {
        //        Investigations += "CBC \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("BUNCreatinine") != null && (bool)((CheckBox)Form.FindName("BUNCreatinine")).IsChecked == true)
        //    {
        //        Investigations += "BUN Creatinine \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("SerumElectro") != null && (bool)((CheckBox)Form.FindName("SerumElectro")).IsChecked == true)
        //    {
        //        Investigations += "Serum Electroytes \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("XRayChest") != null && (bool)((CheckBox)Form.FindName("XRayChest")).IsChecked == true)
        //    {
        //        Investigations += "XRay Chest \t\t\t\t";
        //        i++;
        //    }
        //    if ((CheckBox)Form.FindName("BloodGases") != null && (bool)((CheckBox)Form.FindName("BloodGases")).IsChecked == true)
        //    {
        //        Investigations += "Blood Gases \t\t\t\t";
        //        i++;
        //    }

        //    Grid Medication = new Grid();
        //    Medication.Margin = new Thickness(2, 2, 2, 2);
        //    Medication.HorizontalAlignment = HorizontalAlignment.Stretch;
        //    int j = 0;

        //    ColumnDefinition col = new ColumnDefinition();
        //    col.Width = new GridLength(200, GridUnitType.Auto);
        //    Medication.ColumnDefinitions.Add(col);

        //    ColumnDefinition col1 = new ColumnDefinition();
        //    Medication.ColumnDefinitions.Add(col1);

        //    while (j < 4)
        //    {
        //        RowDefinition row = new RowDefinition();
        //        row.Height = new GridLength();
        //        Medication.RowDefinitions.Add(row);
        //        j++;
        //    }


        //    TextBlock tb1 = new TextBlock();
        //    tb1.Text = "Hydration : ";
        //    Grid.SetRow(tb1, 0);
        //    Medication.Children.Add(tb1);

        //    TextBox txtHydration = new TextBox();
        //    txtHydration.IsReadOnly = true;
        //    txtHydration.Text = "";
        //    ComboBox cmbHyd = (ComboBox)Form.FindName("HydrationStatusManagement");
        //    if (cmbHyd != null && cmbHyd.ItemsSource != null && cmbHyd.SelectedItem != null)
        //    {
        //        txtHydration.Text = ((DynamicListItem)cmbHyd.SelectedItem).Title == null ? "" : ((DynamicListItem)cmbHyd.SelectedItem).Title;
        //    }
        //    Grid.SetRow(txtHydration, 0);
        //    Grid.SetColumn(txtHydration, 1);
        //    Medication.Children.Add(txtHydration);

        //    TextBlock tb2 = new TextBlock();
        //    tb2.Text = "Hydration4 : ";
        //    Grid.SetRow(tb2, 1);
        //    Medication.Children.Add(tb2);

        //    TextBox txtHydration4 = new TextBox();
        //    txtHydration4.IsReadOnly = true;
        //    txtHydration4.Text = "";
        //    ComboBox cmbHyd4 = (ComboBox)Form.FindName("Hydration4StatusManagement");
        //    if (cmbHyd4 != null && cmbHyd4.ItemsSource != null && cmbHyd4.SelectedItem != null)
        //    {
        //        txtHydration4.Text = ((DynamicListItem)cmbHyd4.SelectedItem).Title == null ? "" : ((DynamicListItem)cmbHyd4.SelectedItem).Title;
        //    }

        //    Grid.SetRow(txtHydration4, 1);
        //    Grid.SetColumn(txtHydration4, 1);
        //    Medication.Children.Add(txtHydration4);


        //    TextBlock tb3 = new TextBlock();
        //    tb3.Text = "Zinc Supplement : ";
        //    Grid.SetRow(tb3, 2);
        //    Medication.Children.Add(tb3);

        //    TextBox txtZinc = new TextBox();
        //    txtZinc.IsReadOnly = true;
        //    txtZinc.Text = "";
        //    ComboBox cmbZinc = (ComboBox)Form.FindName("ZincSupplementManagement");
        //    if (cmbZinc != null && cmbZinc.ItemsSource != null && cmbZinc.SelectedItem != null)
        //    {
        //        txtZinc.Text = ((DynamicListItem)cmbZinc.SelectedItem).Title == null ? "" : ((DynamicListItem)cmbZinc.SelectedItem).Title;
        //    }
        //    Grid.SetRow(txtZinc, 2);
        //    Grid.SetColumn(txtZinc, 1);
        //    Medication.Children.Add(txtZinc);

        //    TextBlock tb4 = new TextBlock();
        //    tb4.Text = "Nutritions : ";
        //    Grid.SetRow(tb4, 3);
        //    Medication.Children.Add(tb4);

        //    TextBox txtNutrition = new TextBox();
        //    txtNutrition.AcceptsReturn = true;
        //    txtNutrition.TextWrapping = TextWrapping.Wrap;
        //    txtNutrition.IsReadOnly = true;
        //    txtNutrition.Text = "";
        //    ComboBox cmbNutrition = (ComboBox)Form.FindName("NutritionAdvise");
        //    if (cmbNutrition != null && cmbNutrition.Items.Count != 0)
        //    {
        //        int k = 0;
        //        while (k < cmbNutrition.Items.Count)
        //        {
        //            if (((CheckBox)cmbNutrition.Items[k]).IsChecked == true)
        //            {
        //                txtNutrition.Text += ((CheckBox)cmbNutrition.Items[k]).Content + ",\t";
        //                if ((string)(((CheckBox)cmbNutrition.Items[k]).Content) == "Other")
        //                {
        //                    txtNutrition.Text += "Others : \t\t";
        //                    TextBox txtOt = (TextBox)Form.FindName("ONField");
        //                    if (txtOt.Text != null && txtOt.Text != "")
        //                        txtNutrition.Text += txtOt.Text;
        //                }
        //            }
        //            k++;
        //        }
        //    }

        //    Grid.SetRow(txtNutrition, 3);
        //    Grid.SetColumn(txtNutrition, 1);
        //    Medication.Children.Add(txtNutrition);

        //    Grid Therapy = new Grid();
        //    Therapy.HorizontalAlignment = HorizontalAlignment.Stretch;
        //    RowDefinition row1 = new RowDefinition();
        //    row1.Height = new GridLength();
        //    Therapy.RowDefinitions.Add(row1);
        //    RowDefinition row2 = new RowDefinition();
        //    row2.Height = new GridLength();
        //    Therapy.RowDefinitions.Add(row2);

        //    Grid Drugs = new Grid();
        //    Drugs.Name = "FollowUpMedication";
        //    Drugs = GetGridSchema(Drugs);

        //    Grid.SetRow(Medication, 0);
        //    Therapy.Children.Add(Medication);
        //    Grid.SetRow(Drugs, 1);
        //    Therapy.Children.Add(Drugs);


        //    #region RMProvisionalDiagnosis
        //    string RMProvisionalDiagnosis = "";
        //    if (((TextBox)Form.FindName("RMProvisionalDiagnosis")) != null && ((TextBox)Form.FindName("RMProvisionalDiagnosis")).Text != null)
        //        RMProvisionalDiagnosis = ((TextBox)Form.FindName("RMProvisionalDiagnosis")).Text;
        //    #endregion

        //    #region FinalDiagnosis
        //    string FinalDiagnosis = "";
        //    if (((TextBox)Form.FindName("FinalDiagnosis")) != null && ((TextBox)Form.FindName("FinalDiagnosis")).Text != null)
        //        FinalDiagnosis = ((TextBox)Form.FindName("FinalDiagnosis")).Text;
        //    #endregion

        //    #region HospitalDate
        //    string HospitalDate = "";
        //    if (((DatePicker)Form.FindName("HospitalDate")) != null && ((DatePicker)Form.FindName("HospitalDate")).SelectedDate != null)
        //        HospitalDate = ((DateTime)((DatePicker)Form.FindName("HospitalDate")).SelectedDate).ToString();
        //    #endregion

        //    #region RMAt
        //    string RMAt = "";
        //    if (Form.FindName("RMAt") != null && ((ComboBox)Form.FindName("RMAt")).SelectedItem != null)
        //        RMAt = ((DynamicListItem)((ComboBox)Form.FindName("RMAt")).SelectedItem).Title;
        //    #endregion

        //    #region RMTo
        //    string RMTo = "";
        //    if (Form.FindName("RMTo") != null && ((ComboBox)Form.FindName("RMTo")).SelectedItem != null)
        //        RMTo = ((DynamicListItem)((ComboBox)Form.FindName("RMTo")).SelectedItem).Title;
        //    #endregion

        //    #region FUMFor(Reason for follow up)
        //    string FUMFor = "";
        //    if (((TextBox)Form.FindName("FUMFor")) != null && ((TextBox)Form.FindName("FUMFor")).Text != null)
        //        FUMFor = ((TextBox)Form.FindName("FUMFor")).Text;
        //    #endregion

        //    #region FUMInstructions(Instructions)
        //    string FUMInstructions = "";
        //    if (((TextBox)Form.FindName("FUMInstructions")) != null && ((TextBox)Form.FindName("FUMInstructions")).Text != null)
        //        FUMInstructions = ((TextBox)Form.FindName("FUMInstructions")).Text;
        //    #endregion

        //    try
        //    {
        //        if (str == "SavePCR")
        //        {
        //            PCRS.Name = this.SelectedPatient.Name;
        //            PCRS.Age = this.SelectedPatient.Age.ToString();
        //            PCRS.Gender = this.SelectedPatient.Gender;
        //            PCRS.Add = this.SelectedPatient.Address;
        //            PCRS.Phone = this.SelectedPatient.Phone;
        //            PCRS.Occupation = this.SelectedPatient.Occupation;
        //            PCRS.Date = DateTime.Now.ToString();
        //            PCRS.ClinicRefNo = this.SelectedPatient.ClinicRegNo;


        //            PCRS.ComplaintReported = ComplaintReported;
        //            PCRS.ChiefComplaint = CC + ",\t" + Alarm;
        //            PCRS.PastHistory = PastMedicalHistory;
        //            PCRS.DrugHistory = DrugHistory;
        //            PCRS.Allergies = Allergies;


        //            PCRS.Investigations = Investigations;
        //            PCRS.ProvisionalDiagnosis = RMProvisionalDiagnosis;
        //            PCRS.FinalDiagnosis = FinalDiagnosis;

        //            PCRS.AdvisoryAttached = "";
        //            PCRS.WhenToVisit = "";
        //            PCRS.SpecificInstructions = "Reason :" + FUMFor + "\rInstructions :" + FUMInstructions;


        //            PCRS.FollowUpDate = HospitalDate;
        //            PCRS.FollowUpAt = RMAt;
        //            PCRS.ReferralTo = RMTo;

        //            PCRS.HydrationStatusManagement = txtHydration.Text;
        //            PCRS.Hydration4StatusManagement = txtHydration4.Text;
        //            PCRS.ZincSupplementManagement = txtZinc.Text;
        //            PCRS.NutritionAdvise = txtNutrition.Text;


        //            //PCR.SetTherapy(Therapy);

        //        }
        //        else if (str == "PrintPCR")
        //        {
        //            PCR.SetParticulars(this.SelectedPatient.Name, this.SelectedPatient.Age.ToString(), this.SelectedPatient.Gender, this.SelectedPatient.Address, this.SelectedPatient.Occupation, this.SelectedPatient.Phone, DateTime.Now.ToString(), this.SelectedPatient.ClinicRegNo);
        //            PCR.SetIllnessSummary(ComplaintReported, CC + ",\t" + Alarm + ChiefComplaint, PastMedicalHistory, DrugHistory, Allergies);
        //            PCR.SetObservation(Investigations, RMProvisionalDiagnosis, FinalDiagnosis);
        //            PCR.SetEducation("", "", "Reason :" + FUMFor + "\rInstructions :" + FUMInstructions);
        //            PCR.SetTherapy(Therapy);
        //            PCR.SetOthers(HospitalDate, RMAt, RMTo);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return PCR;
        //    }

        //    return PCR;

        //}

        private void cmbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //IsFirstTime = true;
            if (SelectedFormStructure != null)
            {
                FrequencyFlag = false;
                ForFlag = false;

                IsFirstTime = true;
                GenratePreview();
                MapRelations();
                IsFirstTime = false;
            }
        }

        private Grid GetHeadingRow(Grid container)
        {
            RowDefinition row1 = new RowDefinition();
            container.RowDefinitions.Add(row1);

            TextBox tblField = new TextBox();
            tblField.Height = 60;
            tblField.IsEnabled = false;
            tblField.Text = "Field";
            tblField.FontFamily = new FontFamily("Portable User Interface");
            tblField.Foreground = this.Resources["Heading"] as Brush;
            tblField.FontWeight = FontWeights.Bold;
            tblField.FontStyle = FontStyles.Italic;
            Grid.SetRow(tblField, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblField, 0);
            container.Children.Add(tblField);

            TextBox tblRuleValue = new TextBox();
            tblRuleValue.Height = 60;
            tblRuleValue.IsEnabled = false;
            tblRuleValue.Text = "Value According to Rules";
            tblRuleValue.FontFamily = new FontFamily("Portable User Interface");
            tblRuleValue.Foreground = this.Resources["Heading"] as Brush;
            tblRuleValue.FontWeight = FontWeights.Bold;
            tblRuleValue.FontStyle = FontStyles.Italic;
            Grid.SetRow(tblRuleValue, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblRuleValue, 1);
            container.Children.Add(tblRuleValue);

            TextBox tblDoctorValue = new TextBox();
            tblDoctorValue.Height = 60;
            tblDoctorValue.IsEnabled = false;
            tblDoctorValue.Text = "Value According to Doctor";
            tblDoctorValue.FontFamily = new FontFamily("Portable User Interface");
            tblDoctorValue.Foreground = this.Resources["Heading"] as Brush;
            tblDoctorValue.FontWeight = FontWeights.Bold;
            tblDoctorValue.FontStyle = FontStyles.Italic;
            Grid.SetRow(tblDoctorValue, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblDoctorValue, 2);
            container.Children.Add(tblDoctorValue);

            return container;
        }

        private Grid GetGridBody(Grid container, string Field, string RuleValue, string DoctorValue)
        {
            RowDefinition row2 = new RowDefinition();
            container.RowDefinitions.Add(row2);

            TextBox tblFieldStoolME = new TextBox();
            tblFieldStoolME.Height = 40;
            tblFieldStoolME.IsEnabled = false;
            tblFieldStoolME.Text = Field;
            Grid.SetRow(tblFieldStoolME, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblFieldStoolME, 0);
            container.Children.Add(tblFieldStoolME);

            TextBox tblStoolMERuleValue = new TextBox();
            tblStoolMERuleValue.Height = 40;
            tblStoolMERuleValue.IsEnabled = false;
            tblStoolMERuleValue.Text = RuleValue;
            Grid.SetRow(tblStoolMERuleValue, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblStoolMERuleValue, 1);
            container.Children.Add(tblStoolMERuleValue);

            TextBox tblStoolMEDoctorValue = new TextBox();
            tblStoolMEDoctorValue.Height = 40;
            tblStoolMEDoctorValue.IsEnabled = false;
            tblStoolMEDoctorValue.Text = DoctorValue;
            Grid.SetRow(tblStoolMEDoctorValue, container.RowDefinitions.Count - 1);
            Grid.SetColumn(tblStoolMEDoctorValue, 2);
            container.Children.Add(tblStoolMEDoctorValue);

            return container;
        }

        private void VarianceItemButton_Click(object sender, RoutedEventArgs e)
        {

            Grid section = GetSectionLayout("Investigation Variance");

            Grid container = (Grid)((Border)section.Children[0]).Child;
            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(200, GridUnitType.Auto);
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(200, GridUnitType.Auto);
            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = new GridLength(200, GridUnitType.Auto);
            //column1.Width = new GridLength(200, GridUnitType.Auto);
            container.ColumnDefinitions.Add(column1);
            container.ColumnDefinitions.Add(column2);
            container.ColumnDefinitions.Add(column3);

            container = GetHeadingRow(container);

            if (Form.FindName("HydrationStatus") is ComboBox)
            {
                ComboBox cmbHydSt = (ComboBox)Form.FindName("HydrationStatus");
                if (cmbHydSt.SelectedItem != null)
                {
                    if (((DynamicListItem)cmbHydSt.SelectedItem).Title == "Mild dehydration")
                    {
                        ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");
                        if (cmbPro != null)
                        {
                            if ((DynamicListItem)cmbPro.SelectedItem != null && ((DynamicListItem)cmbPro.SelectedItem).Title == "Acute watery diarrhea")
                            {
                                if (Form.FindName("StoolME") is CheckBox && (bool)((CheckBox)Form.FindName("StoolME")).IsChecked)
                                {
                                    container = GetGridBody(container, "StoolME", "True", "True");
                                }
                                else
                                {
                                    container = GetGridBody(container, "StoolME", "True", "False");
                                }
                                if (Form.FindName("StoolCulture") is CheckBox)
                                {
                                    container = GetGridBody(container, "StoolCulture", "True", "True");
                                }
                                else
                                {
                                    container = GetGridBody(container, "StoolCulture", "True", "False");
                                }
                            }

                        }
                    }
                    if (((DynamicListItem)cmbHydSt.SelectedItem).Title == "Moderate dehydration")
                    {
                        ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");
                        if (cmbPro != null)
                        {
                            if ((DynamicListItem)cmbPro.SelectedItem != null && ((DynamicListItem)cmbPro.SelectedItem).Title == "Acute watery diarrhea")
                            {
                                if (Form.FindName("StoolME") is CheckBox && (bool)((CheckBox)Form.FindName("StoolME")).IsChecked)
                                {
                                    container = GetGridBody(container, "StoolME", "True", "True");
                                }
                                else
                                {
                                    container = GetGridBody(container, "StoolME", "True", "False");
                                }
                                if (Form.FindName("StoolCulture") is CheckBox)
                                {
                                    container = GetGridBody(container, "StoolCulture", "True", "True");
                                }
                                else
                                {
                                    container = GetGridBody(container, "StoolCulture", "True", "False");
                                }
                            }
                        }
                    }

                    if (((DynamicListItem)cmbHydSt.SelectedItem).Title == "Severe dehydration")
                    {
                        ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");
                        if (cmbPro != null)
                        {
                            if ((DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea"))
                            {
                                if (Form.FindName("CBC") is CheckBox && (bool)((CheckBox)Form.FindName("CBC")).IsChecked)
                                    container = GetGridBody(container, "CBC", "True", "True");
                                else
                                    container = GetGridBody(container, "CBC", "True", "False");

                                if (Form.FindName("BUNCreatinine") is CheckBox && (bool)((CheckBox)Form.FindName("BUNCreatinine")).IsChecked)
                                    container = GetGridBody(container, "BUNCreatinine", "True", "True");
                                else
                                    container = GetGridBody(container, "BUNCreatinine", "True", "False");

                                if (Form.FindName("SerumElectro") is CheckBox && (bool)((CheckBox)Form.FindName("SerumElectro")).IsChecked)
                                    container = GetGridBody(container, "SerumElectro", "True", "True");
                                else
                                    container = GetGridBody(container, "SerumElectro", "True", "False");

                                if (Form.FindName("XRayChest") is CheckBox && (bool)((CheckBox)Form.FindName("XRayChest")).IsChecked)
                                    container = GetGridBody(container, "XRayChest", "True", "True");
                                else
                                    container = GetGridBody(container, "XRayChest", "True", "False");

                                if (Form.FindName("BloodGases") is CheckBox && (bool)((CheckBox)Form.FindName("BloodGases")).IsChecked)
                                    container = GetGridBody(container, "BloodGases", "True", "True");
                                else
                                    container = GetGridBody(container, "BloodGases", "True", "False");
                            }

                        }


                    }
                }
            }


        }

        private void GetVariance()
        {
            variance.ListVariance1 = "";
            variance.ListVariance2 = "";
            variance.ListVariance3 = "";
            variance.ListVariance4 = "";
            variance.ListVariance5 = "";
            variance.ListVariance6 = "";

            if (TemplateID == 2)
            {
                ComboBox cmbHyd = (ComboBox)Form.FindName("HydrationStatus");
                ComboBox cmbPro = (ComboBox)Form.FindName("ProvisionalDiagnosis");



                #region Calculation for Variance1 Version2
                if (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea"))
                {
                    if (Form.FindName("StoolME") is CheckBox && (bool)((CheckBox)Form.FindName("StoolME")).IsChecked == false)
                    {
                        variance.Variance1 = "Standard investigations non compliance flag";
                        variance.ListVariance1 += "StoolME,";
                    }
                    if (Form.FindName("StoolCulture") is CheckBox && (bool)((CheckBox)Form.FindName("StoolCulture")).IsChecked == false)
                    {
                        variance.Variance1 = "Standard investigations non compliance flag";
                        variance.ListVariance1 += "StoolCulture,";
                    }
                }
                #endregion



                #region Calculation for Variance2 Version2
                if (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && ((DynamicListItem)cmbPro.SelectedItem).Title != "Acute watery diarrhea")
                {
                    if (Form.FindName("StoolME") is CheckBox && (bool)((CheckBox)Form.FindName("StoolME")).IsChecked)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                        variance.ListVariance2 += "StoolME,";
                    }
                    if (Form.FindName("StoolCulture") is CheckBox && (bool)((CheckBox)Form.FindName("StoolCulture")).IsChecked)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                        variance.ListVariance2 += "StoolCulture,";
                    }
                    if (Form.FindName("OvaParasiteTest") is CheckBox && (bool)((CheckBox)Form.FindName("OvaParasiteTest")).IsChecked)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                        variance.ListVariance2 += "OvaParasiteTest,";
                    }
                }

                #endregion

                #region Calculation for Variance3
                if (cmbPro != null && (DynamicListItem)cmbPro.SelectedItem != null && (((DynamicListItem)cmbPro.SelectedItem).Title == "Acute watery diarrhea" || (!IsSaved && (((DynamicListItem)cmbPro.SelectedItem).Title == "Dysentry" || ((DynamicListItem)cmbPro.SelectedItem).Title == "Persistent diarrhea"))))
                {
                    ListBox lstbox = (ListBox)Form.FindName("ManagementAntibiotics");
                    if (lstbox != null && lstbox.Items.Count > 0)
                    {
                        string cmb = "";
                        if (((Medication)((MedicatioRepeterControlItem)lstbox.Items[0]).DataContext).Drug != null)
                        {
                            cmb = ((Medication)((MedicatioRepeterControlItem)lstbox.Items[0]).DataContext).Drug.Description;
                        }
                        if (cmb != null && cmb != "")
                        {
                            variance.Variance3 = "Unwarranted Antibiotics prescription flag";
                            int i = 0;
                            while (i < lstbox.Items.Count)
                            {
                                if (((Medication)((MedicatioRepeterControlItem)lstbox.Items[i]).DataContext).Drug != null)
                                {
                                    cmb = ((Medication)((MedicatioRepeterControlItem)lstbox.Items[i]).DataContext).Drug.Description;
                                }

                                if (cmb != null && cmb != "")
                                {
                                    variance.ListVariance3 += cmb + ",";
                                }
                                i++;
                            }
                        }
                    }
                }

                #endregion

                #region Calculation for Variance4
                if (cmbHyd != null && cmbHyd.SelectedItem != null && ((DynamicListItem)cmbHyd.SelectedItem).Title == "Severe dehydration")
                {
                    ComboBox cmbRefTo = (ComboBox)Form.FindName("RMTo");
                    if (cmbRefTo != null && cmbRefTo.SelectedItem == null)
                    {
                        variance.Variance4 = "Referral not made flag";
                        variance.ListVariance4 = "Referral not made";
                    }
                }
                #endregion

                #region Calculation for Variance5

                // Will be Applied after Confirmation By Nilesh Sir

                #endregion

                #region Calculation for Variance6

                // Will be Applied after Masters Received

                #endregion

                #region Calculation for Variance7

                //Exists In SaveTemplate Event

                #endregion

            }
            else if (TemplateID == 4)
            {
                #region Calculation for Variance1 Version1

                bool tempflag = false;

                if (Form.FindName("PDPersistentCough") is FrameworkElement)
                {
                    CheckBox chkPDPersistentCough = (CheckBox)Form.FindName("PDPersistentCough");
                    if (chkPDPersistentCough.IsChecked == true)
                    {

                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Complete Blood Count (CBC)" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Complete Blood Count (CBC),";
                                    }
                                    if (a.Description == "Spirometry" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Spirometry,";
                                    }
                                }
                            }
                        }
                        //Chest X-Ray,Sinus Imaging
                        if (Form.FindName("InvestRadiology") is FrameworkElement)
                        {
                            ListBox lstInvestRadiology = (ListBox)Form.FindName("InvestRadiology");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Chest X - Ray" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Chest X - Ray,";
                                    }
                                    if (a.Description == "SINUS IMAGING" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "SINUS IMAGING,";
                                    }
                                }
                            }
                        }

                    }

                    if (tempflag == true)
                    {
                        variance.Variance1 = "Standard investigations non compliance flag";
                    }
                }

                #endregion

                #region Calculation for Variance2 Version1

                //Other Tests
                tempflag = false;

                if (Form.FindName("PDAcuteCough") is FrameworkElement)
                {
                    CheckBox chkPDAcuteCough = (CheckBox)Form.FindName("PDAcuteCough");
                    if (chkPDAcuteCough.IsChecked == true)
                    {
                        //InvestAdditionalInvestigations
                        if (Form.FindName("InvestAdditionalInvestigations") is FrameworkElement)
                        {
                            ListBox lstInvestAdditionalInvestigations = (ListBox)Form.FindName("InvestAdditionalInvestigations");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestAdditionalInvestigations.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestAdditionalInvestigations.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }

                        //InvestPertussisInfectionNasopharyngealCulture
                        if (Form.FindName("InvestPertussisInfectionNasopharyngealCulture") is CheckBox && (bool)((CheckBox)Form.FindName("InvestPertussisInfectionNasopharyngealCulture")).IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance2 += "Pertussis infection nasopharyngeal culture,";
                        }
                        //InvestOtherTests
                        if (Form.FindName("InvestOtherTests") is TextBox && ((TextBox)Form.FindName("InvestOtherTests")).Text != null && ((TextBox)Form.FindName("InvestOtherTests")).Text != "")
                        {
                            tempflag = true;
                            variance.ListVariance2 += ((TextBox)Form.FindName("InvestOtherTests")).Text;
                        }



                    }

                    if (tempflag == true)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                    }
                }

                #endregion

                #region Calculation for Variance3

                tempflag = false;
                if ((FrameworkElement)Form.FindName("RMToHospital") != null && ((ComboBox)Form.FindName("RMToHospital")).SelectedItem == null)
                {
                    //AlarmCoughWithDyspneaAtRest
                    if (Form.FindName("AlarmCoughWithDyspneaAtRest") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithDyspneaAtRest = (CheckBox)Form.FindName("AlarmCoughWithDyspneaAtRest");
                        if (chkAlarmCoughWithDyspneaAtRest.IsChecked == true && chkAlarmCoughWithDyspneaAtRest.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with dyspnea at rest, ";
                        }
                    }
                    //AlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus
                    if (Form.FindName("AlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus = (CheckBox)Form.FindName("AlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus");
                        if (chkAlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus.IsChecked == true && chkAlarmCoughAccompaniedWithBreathlessnessAndCoughingUpPinkFrothyMucus.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough accompanied with breathlessness and coughing up pink frothy mucus, ";
                        }
                    }
                    //AlarmIntractableCough
                    if (Form.FindName("AlarmIntractableCough") is FrameworkElement)
                    {
                        CheckBox chkAlarmIntractableCough = (CheckBox)Form.FindName("AlarmIntractableCough");
                        if (chkAlarmIntractableCough.IsChecked == true && chkAlarmIntractableCough.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Intractable cough, ";
                        }
                    }
                    //AlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease
                    if (Form.FindName("AlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease") is FrameworkElement)
                    {
                        CheckBox chkAlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease = (CheckBox)Form.FindName("AlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease");
                        if (chkAlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease.IsChecked == true && chkAlarmPersistentCoughInPatientsWithCoExistingRespiratoryDisease.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Persistent cough in patients with co-existing respiratory disease, ";
                        }
                    }
                    //AlarmCoughWithBloodHemoptysis
                    if (Form.FindName("AlarmCoughWithBloodHemoptysis") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithBloodHemoptysis = (CheckBox)Form.FindName("AlarmCoughWithBloodHemoptysis");
                        if (chkAlarmCoughWithBloodHemoptysis.IsChecked == true && chkAlarmCoughWithBloodHemoptysis.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with blood (hemoptysis), ";
                        }
                    }
                    //AlarmCoughWithTachycardia
                    if (Form.FindName("AlarmCoughWithTachycardia") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithTachycardia = (CheckBox)Form.FindName("AlarmCoughWithTachycardia");
                        if (chkAlarmCoughWithTachycardia.IsChecked == true && chkAlarmCoughWithTachycardia.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with tachycardia, ";
                        }
                    }
                    //AlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills
                    if (Form.FindName("AlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills = (CheckBox)Form.FindName("AlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills");
                        if (chkAlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills.IsChecked == true && chkAlarmCoughWithUnexplainedWeightLossWithFeversAndNightChills.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with unexplained weight loss with fevers and  night chills, ";
                        }
                    }
                    //AlarmCoughWithAssociatedCyanosis
                    if (Form.FindName("AlarmCoughWithAssociatedCyanosis") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithAssociatedCyanosis = (CheckBox)Form.FindName("AlarmCoughWithAssociatedCyanosis");
                        if (chkAlarmCoughWithAssociatedCyanosis.IsChecked == true && chkAlarmCoughWithAssociatedCyanosis.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with associated cyanosis, ";
                        }
                    }
                    //AlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment
                    if (Form.FindName("AlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment") is FrameworkElement)
                    {
                        CheckBox chkAlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment = (CheckBox)Form.FindName("AlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment");
                        if (chkAlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment.IsChecked == true && chkAlarmPersistentCoughNotResponsiveToEmpiricalTherapyDrugTreatment.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Persistent cough not responsive to empirical therapy/ drug treatment, ";
                        }
                    }
                    //AlarmCoughWithDyspneaWithMildActivity
                    if (Form.FindName("AlarmCoughWithDyspneaWithMildActivity") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithDyspneaWithMildActivity = (CheckBox)Form.FindName("AlarmCoughWithDyspneaWithMildActivity");
                        if (chkAlarmCoughWithDyspneaWithMildActivity.IsChecked == true && chkAlarmCoughWithDyspneaWithMildActivity.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with dyspnea with mild activity, ";
                        }
                    }
                    //AlarmCoughWithTachypnea
                    if (Form.FindName("AlarmCoughWithTachypnea") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithTachypnea = (CheckBox)Form.FindName("AlarmCoughWithTachypnea");
                        if (chkAlarmCoughWithTachypnea.IsChecked == true && chkAlarmCoughWithTachypnea.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with tachypnea, ";
                        }
                    }
                    //AlarmCoughWithHighFever
                    if (Form.FindName("AlarmCoughWithHighFever") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithHighFever = (CheckBox)Form.FindName("AlarmCoughWithHighFever");
                        if (chkAlarmCoughWithHighFever.IsChecked == true && chkAlarmCoughWithHighFever.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with high fever, ";
                        }
                    }
                    //AlarmCoughWithStridorRetropharyngealAbcess
                    if (Form.FindName("AlarmCoughWithStridorRetropharyngealAbcess") is FrameworkElement)
                    {
                        CheckBox chkAlarmCoughWithStridorRetropharyngealAbcess = (CheckBox)Form.FindName("AlarmCoughWithStridorRetropharyngealAbcess");
                        if (chkAlarmCoughWithStridorRetropharyngealAbcess.IsChecked == true && chkAlarmCoughWithStridorRetropharyngealAbcess.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Cough with stridor (Retropharyngeal abcess), ";
                        }
                    }
                    //AlarmPersistentCoughCardiovascularDisease
                    if (Form.FindName("AlarmPersistentCoughCardiovascularDisease") is FrameworkElement)
                    {
                        CheckBox chkAlarmPersistentCoughCardiovascularDisease = (CheckBox)Form.FindName("AlarmPersistentCoughCardiovascularDisease");
                        if (chkAlarmPersistentCoughCardiovascularDisease.IsChecked == true && chkAlarmPersistentCoughCardiovascularDisease.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Persistent cough in patients with co-existing cardiovascular disease, ";
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance3 = "Referral not made flag";
                }
                #endregion

                #region Calculation for Variance4

                // 4.	Medication protocol non adherence flag

                tempflag = false;
                string tempStr1 = "";
                string tempStr2 = "";
                string tempStr3 = "";
                string tempStr4 = "";
                string tempStr5 = "";
                string tempStr6 = "";
                string tempStr7 = "";
                #region 1
                if (Form.FindName("PDCoughWithPostNasalDrips") is FrameworkElement)
                {
                    CheckBox chkPDCoughWithPostNasalDrips = (CheckBox)Form.FindName("PDCoughWithPostNasalDrips");
                    if (chkPDCoughWithPostNasalDrips.IsChecked == true)
                    {

                        bool temp1 = false;
                        bool temp2 = false;
                        bool temp3 = false;
                        bool temp4 = false;
                        bool temp5 = false;
                        //ManagementNormalSaline                                                                                   
                        if (Form.FindName("ManagementNormalSaline") is FrameworkElement)
                        {
                            ListBox lstManagementNormalSaline = (ListBox)Form.FindName("ManagementNormalSaline");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementNormalSaline.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr1 += "Nasal Decongestants, ";
                        }
                        //ManagementNasalDrops                                                                                   
                        if (Form.FindName("ManagementNasalDrops") is FrameworkElement)
                        {
                            ListBox lstManagementNasalDrops = (ListBox)Form.FindName("ManagementNasalDrops");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementNasalDrops.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr1 += "Nasal Decongestants, ";
                        }

                        //ManagementOthers
                        if (Form.FindName("ManagementOthers") is FrameworkElement)
                        {
                            ListBox lstbxManagementOthers = (ListBox)Form.FindName("ManagementOthers");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOthers.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr1 += "Others, ";
                        }

                        //ManagementBeclomethasoneNasalSpray
                        if (Form.FindName("ManagementBeclomethasoneNasalSpray") is FrameworkElement)
                        {
                            ListBox lstManagementBeclomethasoneNasalSpray = (ListBox)Form.FindName("ManagementBeclomethasoneNasalSpray");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementBeclomethasoneNasalSpray.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr1 += "Beclomethasone Nasal Spray, ";
                        }
                        //ManagementBudesonideNasalSpray
                        if (Form.FindName("ManagementBudesonideNasalSpray") is FrameworkElement)
                        {
                            ListBox lstManagementBudesonideNasalSpray = (ListBox)Form.FindName("ManagementBudesonideNasalSpray");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementBudesonideNasalSpray.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr1 += "Budesonide Nasal Spray, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr1 = "";
                        }
                    }
                }
                #endregion

                #region 2
                if (Form.FindName("PDCoughWithChestInfection") is FrameworkElement)
                {
                    CheckBox chkPDCoughWithChestInfection = (CheckBox)Form.FindName("PDCoughWithChestInfection");
                    if (chkPDCoughWithChestInfection.IsChecked == true)
                    {
                        bool temp1 = false;
                        bool temp2 = false;
                        bool temp3 = false;
                        bool temp4 = false;
                        bool temp5 = false;
                        //ManagementMacrolides                                                                                   
                        if (Form.FindName("ManagementMacrolides") is FrameworkElement)
                        {
                            ListBox lstManagementMacrolides = (ListBox)Form.FindName("ManagementMacrolides");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementMacrolides.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr2 += "Macrolides, ";
                        }
                        //ManagementCephalosporins                                                                                   
                        if (Form.FindName("ManagementCephalosporins") is FrameworkElement)
                        {
                            ListBox lstManagementCephalosporins = (ListBox)Form.FindName("ManagementCephalosporins");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementCephalosporins.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr2 += "Cephalosporins, ";
                        }

                        //ManagementQuinolones                                                                                   
                        if (Form.FindName("ManagementQuinolones") is FrameworkElement)
                        {
                            ListBox lstManagementQuinolones = (ListBox)Form.FindName("ManagementQuinolones");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementQuinolones.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr2 += "Quinolones, ";
                        }

                        //ManagementTetracyclines
                        if (Form.FindName("ManagementTetracyclines") is FrameworkElement)
                        {
                            ListBox lstManagementManagementTetracyclines = (ListBox)Form.FindName("ManagementTetracyclines");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementManagementTetracyclines.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr2 += "Tetracyclines, ";
                        }
                        //ManagementPenicillin
                        if (Form.FindName("ManagementPenicillin") is FrameworkElement)
                        {
                            ListBox lstManagementPenicillin = (ListBox)Form.FindName("ManagementPenicillin");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementPenicillin.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr2 += "Budesonide Nasal Spray, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr2 = "";
                        }
                    }
                }
                #endregion

                #region 3
                if (Form.FindName("PDCoughWithBronchitisAsthmaCOPDpneumoconiosis") is FrameworkElement)
                {
                    CheckBox chkPDCoughWithBronchitisAsthmaCOPDpneumoconiosis = (CheckBox)Form.FindName("PDCoughWithBronchitisAsthmaCOPDpneumoconiosis");
                    if (chkPDCoughWithBronchitisAsthmaCOPDpneumoconiosis.IsChecked == true)
                    {
                        bool temp1 = false;
                        bool temp2 = false;
                        bool temp3 = false;
                        bool temp4 = false;
                        bool temp5 = false;
                        bool temp6 = false;
                        bool temp7 = false;
                        //ManagementIpratropiumBromide                                                                                   
                        if (Form.FindName("ManagementIpratropiumBromide") is FrameworkElement)
                        {
                            ListBox lstManagementIpratropiumBromide = (ListBox)Form.FindName("ManagementIpratropiumBromide");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementIpratropiumBromide.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr3 += "Ipratropium Bromide, ";
                        }
                        //ManagementExpectorant                                                                                   
                        if (Form.FindName("ManagementExpectorant") is FrameworkElement)
                        {
                            ListBox lstManagementExpectorant = (ListBox)Form.FindName("ManagementExpectorant");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementExpectorant.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr3 += "Expectorant, ";
                        }

                        //ManagementExpectorantMucolytic                                                                                   
                        if (Form.FindName("ManagementExpectorantMucolytic") is FrameworkElement)
                        {
                            ListBox lstManagementExpectorantMucolytic = (ListBox)Form.FindName("ManagementExpectorantMucolytic");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementExpectorantMucolytic.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr3 += "Expectorant Mucolytic, ";
                        }

                        //ManagementSalbutamol
                        if (Form.FindName("ManagementSalbutamol") is FrameworkElement)
                        {
                            ListBox lstManagementSalbutamol = (ListBox)Form.FindName("ManagementSalbutamol");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementSalbutamol.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr3 += "Salbutamol, ";
                        }
                        //ManagementSalbutamolTheophylline
                        if (Form.FindName("ManagementSalbutamolTheophylline") is FrameworkElement)
                        {
                            ListBox lstManagementSalbutamolTheophylline = (ListBox)Form.FindName("ManagementSalbutamolTheophylline");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementSalbutamolTheophylline.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr3 += "Salbutamol Theophylline, ";
                        }

                        //ManagementOtherBronchodilators
                        if (Form.FindName("ManagementOtherBronchodilators") is FrameworkElement)
                        {
                            ListBox lstManagementOtherBronchodilators = (ListBox)Form.FindName("ManagementOtherBronchodilators");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherBronchodilators.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp6 = true;
                                    break;
                                }
                            }
                            if (temp6 == false)
                                tempStr3 += "Other Bronchodilators, ";
                        }
                        //ManagementBeclomethasone
                        if (Form.FindName("ManagementBeclomethasone") is FrameworkElement)
                        {
                            ListBox lstManagementBeclomethasone = (ListBox)Form.FindName("ManagementBeclomethasone");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementBeclomethasone.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp7 = true;
                                    break;
                                }
                            }
                            if (temp7 == false)
                                tempStr3 += "Beclomethasone, ";
                        }


                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false && temp6 == false && temp7 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr3 = "";
                        }
                    }
                }
                #endregion

                #region 4
                if (Form.FindName("PDCoughWithGERD") is FrameworkElement)
                {
                    CheckBox chkPDCoughWithGERD = (CheckBox)Form.FindName("PDCoughWithGERD");
                    if (chkPDCoughWithGERD.IsChecked == true)
                    {
                        bool temp1 = false;
                        bool temp2 = false;
                        bool temp3 = false;
                        //ManagementH2Blockers                                                                                   
                        if (Form.FindName("ManagementH2Blockers") is FrameworkElement)
                        {
                            ListBox lstManagementH2Blockers = (ListBox)Form.FindName("ManagementH2Blockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementH2Blockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr4 += "H2 Blockers, ";
                        }
                        //ManagementOtherH2Blockers                                                                                   
                        if (Form.FindName("ManagementOtherH2Blockers") is FrameworkElement)
                        {
                            ListBox lstManagementOtherH2Blockers = (ListBox)Form.FindName("ManagementOtherH2Blockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherH2Blockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr4 += "Other H2 Blockers, ";
                        }

                        //ManagementAntacids                                                                                   
                        if (Form.FindName("ManagementAntacids") is FrameworkElement)
                        {
                            ListBox lstManagementAntacids = (ListBox)Form.FindName("ManagementAntacids");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAntacids.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr4 += "Antacids, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr4 = "";
                        }
                    }
                }
                #endregion

                #region 5
                if (Form.FindName("PDCoughWithRhinitis") is FrameworkElement)
                {
                    CheckBox chkPDCoughWithRhinitis = (CheckBox)Form.FindName("PDCoughWithRhinitis");
                    if (chkPDCoughWithRhinitis.IsChecked == true)
                    {
                        bool temp1 = false;
                        bool temp2 = false;
                        bool temp3 = false;
                        bool temp4 = false;
                        bool temp5 = false;
                        bool temp6 = false;
                        //ManagementAntiallergicCough                                                                                   
                        if (Form.FindName("ManagementAntiallergicCough") is FrameworkElement)
                        {
                            ListBox lstManagementAntiallergicCough = (ListBox)Form.FindName("ManagementAntiallergicCough");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAntiallergicCough.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr5 += "Antiallergic Cough, ";
                        }
                        //ManagementCodeine                                                                                   
                        if (Form.FindName("ManagementCodeine") is FrameworkElement)
                        {
                            ListBox lstManagementCodeine = (ListBox)Form.FindName("ManagementCodeine");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementCodeine.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr5 += "Codeine, ";
                        }

                        //ManagementOtherAntiallergics
                        if (Form.FindName("ManagementOtherAntiallergics") is FrameworkElement)
                        {
                            ListBox lstbxManagementOtherAntiallergics = (ListBox)Form.FindName("ManagementOtherAntiallergics");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherAntiallergics.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr5 += "Other Antiallergics, ";
                        }

                        //ManagementNormalSaline                                                                                   
                        if (Form.FindName("ManagementNormalSaline") is FrameworkElement)
                        {
                            ListBox lstManagementNormalSaline = (ListBox)Form.FindName("ManagementNormalSaline");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementNormalSaline.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr5 += "Nasal Decongestants, ";
                        }
                        //ManagementNasalDrops                                                                                   
                        if (Form.FindName("ManagementNasalDrops") is FrameworkElement)
                        {
                            ListBox lstManagementNasalDrops = (ListBox)Form.FindName("ManagementNasalDrops");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementNasalDrops.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr5 += "Nasal Decongestants, ";
                        }

                        //ManagementOthers
                        if (Form.FindName("ManagementOthers") is FrameworkElement)
                        {
                            ListBox lstbxManagementOthers = (ListBox)Form.FindName("ManagementOthers");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOthers.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp6 = true;
                                    break;
                                }
                            }
                            if (temp6 == false)
                                tempStr5 += "Others, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false && temp6 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr5 = "";
                        }
                    }
                }
                #endregion

                #region 6
                if (Form.FindName("PDTropicalEosinophilia") is FrameworkElement)
                {
                    CheckBox chkPDTropicalEosinophilia = (CheckBox)Form.FindName("PDTropicalEosinophilia");
                    if (chkPDTropicalEosinophilia.IsChecked == true)
                    {
                        bool temp1 = false;
                        //ManagementDiethylcarbamazineHetrazan                                                                                   
                        if (Form.FindName("ManagementDiethylcarbamazineHetrazan") is FrameworkElement)
                        {
                            ListBox lstManagementDiethylcarbamazineHetrazan = (ListBox)Form.FindName("ManagementDiethylcarbamazineHetrazan");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementDiethylcarbamazineHetrazan.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr6 += "Diethylcarbamazine (Hetrazan), ";
                        }

                        if (temp1 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr6 = "";
                        }
                    }
                }
                #endregion

                #region 7
                if (Form.FindName("PDTuberculosis") is FrameworkElement)
                {
                    CheckBox chkPDTuberculosis = (CheckBox)Form.FindName("PDTuberculosis");
                    if (chkPDTuberculosis.IsChecked == true)
                    {
                        bool temp1 = false;
                        //ManagementAntiTBDrugs                                                                                   
                        if (Form.FindName("ManagementAntiTBDrugs") is FrameworkElement)
                        {
                            ListBox lstManagementAntiTBDrugs = (ListBox)Form.FindName("ManagementAntiTBDrugs");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAntiTBDrugs.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr7 += "ManagementAntiTBDrugs, ";
                        }

                        if (temp1 == false)
                        {
                            tempflag = true;
                        }
                        else
                        {
                            tempStr7 = "";
                        }
                    }
                }
                #endregion

                if (tempflag == true)
                {
                    variance.Variance4 = "Medication protocol non adherence flag";
                    variance.ListVariance4 = tempStr1 + tempStr2 + tempStr3 + tempStr4 + tempStr5 + tempStr6 + tempStr7;
                }
                #endregion

                #region Calculation for Variance5
                tempflag = false;
                // 5.	Unwarranted drug prescription flag
                //ManagementOtherAntiallergics
                if (Form.FindName("ManagementOtherAntiallergics") is FrameworkElement)
                {
                    ListBox lstbxManagementOtherAntiallergics = (ListBox)Form.FindName("ManagementOtherAntiallergics");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherAntiallergics.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            tempflag = true;
                            variance.ListVariance5 += ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug.ToString() + ",";
                        }
                    }
                }
                //ManagementOthers
                if (Form.FindName("ManagementOthers") is FrameworkElement)
                {
                    ListBox lstbxManagementOthers = (ListBox)Form.FindName("ManagementOthers");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOthers.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            tempflag = true;
                            variance.ListVariance5 += ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug.ToString() + ",";
                        }
                    }
                }
                //ManagementOtherDrugs
                if (Form.FindName("ManagementOtherDrugs") is FrameworkElement)
                {
                    ListBox lstbxManagementOtherDrugs = (ListBox)Form.FindName("ManagementOtherDrugs");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherDrugs.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            tempflag = true;
                            variance.ListVariance5 += ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug.ToString() + ",";
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance5 = "Unwarranted drug prescription flag";
                }
                #endregion

                #region Calculation for Variance6

                // 6.	Mandatory prompt ignored

                #endregion
            }
            else if (TemplateID == 5)
            {
                #region Calculation for Variance1 Version1
                // 1. Standard investigations non compliance flag

                bool tempflag = false;

                if (Form.FindName("PDStageIhypertensionMildhypertension") is FrameworkElement && Form.FindName("PDStageIIhypertensionModeratehypertension") is FrameworkElement)
                {
                    CheckBox chkPDStageIhypertensionMildhypertension = (CheckBox)Form.FindName("PDStageIhypertensionMildhypertension");
                    CheckBox chkPDStageIIhypertensionModeratehypertension = (CheckBox)Form.FindName("PDStageIIhypertensionModeratehypertension");
                    if (chkPDStageIhypertensionMildhypertension.IsChecked == true || chkPDStageIIhypertensionModeratehypertension.IsChecked == true)
                    {
                        //InvestLabServices-Complete Urine Analysis ( CUE ),Creatinine - Serum ,ELECTROLYTE PROFILE
                        if (Form.FindName("InvestLabServices") is FrameworkElement)
                        {
                            ListBox lstInvestLabServices = (ListBox)Form.FindName("InvestLabServices");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabServices.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Complete Urine Analysis ( CUE )" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Complete Urine Analysis ( CUE ),";
                                    }
                                    if (a.Description == "Creatinine - Serum " && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Creatinine - Serum ,";
                                    }
                                    if (a.Description == "ELECTROLYTE PROFILE" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "ELECTROLYTE PROFILE,";
                                    }
                                }
                            }
                        }

                        //InvestECG-ECG (12 Lead Electrocardiography)
                        if (Form.FindName("InvestECG") is FrameworkElement)
                        {
                            ListBox lstInvestECG = (ListBox)Form.FindName("InvestECG");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestECG.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestECG.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "ECG (12 Lead Electrocardiography)" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "ECG (12 Lead Electrocardiography)";
                                    }
                                }
                            }
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance1 = "Standard investigations non compliance flag";
                }
                #endregion

                #region Calculation for Variance2 Version1
                // 2. Additional investigations ordered flag

                tempflag = false;

                if (Form.FindName("PDPrehypertension") is FrameworkElement)
                {
                    CheckBox chkPDPrehypertension = (CheckBox)Form.FindName("PDPrehypertension");
                    if (chkPDPrehypertension.IsChecked == true)
                    {
                        //InvestLabs
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestAmbulatoryBloodPressureMonitoring
                        if (Form.FindName("InvestAmbulatoryBloodPressureMonitoring") is FrameworkElement)
                        {
                            ListBox lstInvestAmbulatoryBloodPressureMonitoring = (ListBox)Form.FindName("InvestAmbulatoryBloodPressureMonitoring");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestAmbulatoryBloodPressureMonitoring.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestAmbulatoryBloodPressureMonitoring.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestRadiology
                        if (Form.FindName("InvestRadiology") is FrameworkElement)
                        {
                            ListBox lstInvestRadiology = (ListBox)Form.FindName("InvestRadiology");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestOtherTests
                        if (Form.FindName("InvestOtherTests") is TextBox && ((TextBox)Form.FindName("InvestOtherTests")).Text != null && ((TextBox)Form.FindName("InvestOtherTests")).Text != "")
                        {
                            tempflag = true;
                            variance.ListVariance2 += ((TextBox)Form.FindName("InvestOtherTests")).Text;
                        }
                    }

                    if (tempflag == true)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                    }
                }
                #endregion

                #region Calculation for Variance3

                tempflag = false;

                if ((FrameworkElement)Form.FindName("RMToHospital") != null && ((ComboBox)Form.FindName("RMToHospital")).SelectedItem == null)
                {
                    //PDStageIIIhypertensionSeverehypertension
                    if (Form.FindName("PDStageIIIhypertensionSeverehypertension") is FrameworkElement)
                    {
                        CheckBox chkPDStageIIIhypertensionSeverehypertension = (CheckBox)Form.FindName("PDStageIIIhypertensionSeverehypertension");
                        if (chkPDStageIIIhypertensionSeverehypertension.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Stage III hypertension, ";
                        }
                    }
                    //PresBlurredVision
                    if (Form.FindName("PresBlurredVision") is FrameworkElement)
                    {
                        CheckBox chkPresBlurredVision = (CheckBox)Form.FindName("PresBlurredVision");
                        if (chkPresBlurredVision.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Blurred vision, ";
                        }
                    }
                    //HistSuddenBlackouts
                    if (Form.FindName("HistSuddenBlackouts") is FrameworkElement)
                    {
                        CheckBox chkHistSuddenBlackouts = (CheckBox)Form.FindName("HistSuddenBlackouts");
                        if (chkHistSuddenBlackouts.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Sudden blackouts, ";
                        }
                    }
                    //HistWeaknessOfLimbs
                    if (Form.FindName("HistWeaknessOfLimbs") is FrameworkElement)
                    {
                        CheckBox chkHistWeaknessOfLimbs = (CheckBox)Form.FindName("HistWeaknessOfLimbs");
                        if (chkHistWeaknessOfLimbs.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "weakness of the limbs, ";
                        }
                    }
                    //HistSlurredSpeech
                    if (Form.FindName("HistSlurredSpeech") is FrameworkElement)
                    {
                        CheckBox chkHistSlurredSpeech = (CheckBox)Form.FindName("HistSlurredSpeech");
                        if (chkHistSlurredSpeech.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "slurred speech, ";
                        }
                    }
                    //HistTongueProtrudingToOneSide
                    if (Form.FindName("HistTongueProtrudingToOneSide") is FrameworkElement)
                    {
                        CheckBox chkHistTongueProtrudingToOneSide = (CheckBox)Form.FindName("HistTongueProtrudingToOneSide");
                        if (chkHistTongueProtrudingToOneSide.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "tongue protruding to one side, ";
                        }
                    }
                    //PresConfusion
                    if (Form.FindName("PresConfusion") is FrameworkElement)
                    {
                        CheckBox chkPresConfusion = (CheckBox)Form.FindName("PresConfusion");
                        if (chkPresConfusion.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Confusion, ";
                        }
                    }
                    //PresLossOfConsciousness
                    if (Form.FindName("PresLossOfConsciousness") is FrameworkElement)
                    {
                        CheckBox chkPresLossOfConsciousness = (CheckBox)Form.FindName("PresLossOfConsciousness");
                        if (chkPresLossOfConsciousness.IsChecked == true)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Loss of consciousness, ";
                        }
                    }
                    //ObserveSystolic
                    if (Form.FindName("ObserveSystolic") is FrameworkElement)
                    {
                        TextBox txtObserveSystolic = (TextBox)Form.FindName("ObserveSystolic");

                        if (txtObserveSystolic.Text != null && txtObserveSystolic.Text != "" && Convert.ToDouble(txtObserveSystolic.Text) >= 220.0)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "SBP > 220mmHg, ";
                        }
                    }
                    //ObserveDiastolic
                    if (Form.FindName("ObserveDiastolic") is FrameworkElement)
                    {
                        TextBox txtObserveDiastolic = (TextBox)Form.FindName("ObserveDiastolic");

                        if (txtObserveDiastolic.Text != null && txtObserveDiastolic.Text != "" && Convert.ToDouble(txtObserveDiastolic.Text) >= 130.0)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "DBP > 130mmHg, ";
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance3 = "Referral not made flag";
                }
                #endregion

                #region Calculation for Variance4

                // 4.	Medication protocol non adherence flag

                tempflag = false;
                string tempStr1 = "";
                bool temp1 = false;
                bool temp2 = false;
                bool temp3 = false;
                bool temp4 = false;
                bool temp5 = false;
                bool temp6 = false;
                bool temp7 = false;
                #region 1
                if (Form.FindName("PDStageIhypertensionMildhypertension") is FrameworkElement && Form.FindName("PDStageIIhypertensionModeratehypertension") is FrameworkElement)
                {
                    CheckBox chkPDStageIhypertensionMildhypertension = (CheckBox)Form.FindName("PDStageIhypertensionMildhypertension");
                    CheckBox chkPDStageIIhypertensionModeratehypertension = (CheckBox)Form.FindName("PDStageIIhypertensionModeratehypertension");
                    if (chkPDStageIhypertensionMildhypertension.IsChecked == true || chkPDStageIIhypertensionModeratehypertension.IsChecked == true)
                    {

                        //ManagementThiazideDiuretics                                                                                   
                        if (Form.FindName("ManagementThiazideDiuretics") is FrameworkElement)
                        {
                            ListBox lstManagementThiazideDiuretics = (ListBox)Form.FindName("ManagementThiazideDiuretics");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementThiazideDiuretics.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr1 += "Thiazide Diuretics, ";
                        }
                        //ManagementOtherDiuretics                                                                                   
                        if (Form.FindName("ManagementOtherDiuretics") is FrameworkElement)
                        {
                            ListBox lstManagementOtherDiuretics = (ListBox)Form.FindName("ManagementOtherDiuretics");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherDiuretics.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr1 += "Other Diuretics, ";
                        }
                        //ManagementAntiHypertensives
                        if (Form.FindName("ManagementAntiHypertensives") is FrameworkElement)
                        {
                            ListBox lstManagementAntiHypertensives = (ListBox)Form.FindName("ManagementAntiHypertensives");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAntiHypertensives.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr1 += "Anti Hypertensives, ";
                        }

                        //ManagementACEInhibitors
                        if (Form.FindName("ManagementACEInhibitors") is FrameworkElement)
                        {
                            ListBox lstManagementACEInhibitors = (ListBox)Form.FindName("ManagementACEInhibitors");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementACEInhibitors.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr1 += "ACE Inhibitors, ";
                        }

                        //ManagementAngiotensinIIReceptorBlockers
                        if (Form.FindName("ManagementAngiotensinIIReceptorBlockers") is FrameworkElement)
                        {
                            ListBox lstManagementAngiotensinIIReceptorBlockers = (ListBox)Form.FindName("ManagementAngiotensinIIReceptorBlockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAngiotensinIIReceptorBlockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr1 += "Angiotensin II Receptor Blockers, ";
                        }

                        //ManagementOtherCalciumChannelBlockers
                        if (Form.FindName("ManagementOtherCalciumChannelBlockers") is FrameworkElement)
                        {
                            ListBox lstManagementOtherCalciumChannelBlockers = (ListBox)Form.FindName("ManagementOtherCalciumChannelBlockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherCalciumChannelBlockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp6 = true;
                                    break;
                                }
                            }
                            if (temp6 == false)
                                tempStr1 += "Other Calcium Channel Blockers, ";
                        }

                        //ManagementOthers
                        if (Form.FindName("ManagementOthers") is FrameworkElement)
                        {
                            ListBox lstbxManagementOthers = (ListBox)Form.FindName("ManagementOthers");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOthers.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp7 = true;
                                    break;
                                }
                            }
                            if (temp7 == false)
                                tempStr1 += "Others, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false && temp6 == false && temp7 == false)
                        {
                            variance.Variance4 = "Medication protocol non adherence flag";
                            variance.ListVariance4 = tempStr1;
                        }
                        else
                        {
                            variance.ListVariance4 = "";
                        }
                    }
                }
                #endregion
                #endregion

                #region Calculation for Variance5

                // 5.	Unwarranted drug prescription flag

                tempflag = false;
                tempStr1 = "";
                temp1 = false;
                temp2 = false;
                temp3 = false;
                temp4 = false;
                temp5 = false;
                temp6 = false;
                temp7 = false;

                if (Form.FindName("PDPrehypertension") is FrameworkElement)
                {
                    CheckBox chkPDPrehypertension = (CheckBox)Form.FindName("PDPrehypertension");
                    if (chkPDPrehypertension.IsChecked == true)
                    {
                        //ManagementThiazideDiuretics                                                                                   
                        if (Form.FindName("ManagementThiazideDiuretics") is FrameworkElement)
                        {
                            ListBox lstManagementThiazideDiuretics = (ListBox)Form.FindName("ManagementThiazideDiuretics");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementThiazideDiuretics.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    tempStr1 += "Thiazide Diuretics, ";
                                    break;
                                }
                            }
                        }
                        //ManagementOtherDiuretics                                                                                   
                        if (Form.FindName("ManagementOtherDiuretics") is FrameworkElement)
                        {
                            ListBox lstManagementOtherDiuretics = (ListBox)Form.FindName("ManagementOtherDiuretics");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherDiuretics.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    tempStr1 += "Other Diuretics, ";
                                    break;
                                }
                            }
                        }
                        //ManagementAntiHypertensives
                        if (Form.FindName("ManagementAntiHypertensives") is FrameworkElement)
                        {
                            ListBox lstManagementAntiHypertensives = (ListBox)Form.FindName("ManagementAntiHypertensives");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAntiHypertensives.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    tempStr1 += "Anti Hypertensives, ";
                                    break;
                                }
                            }
                        }

                        //ManagementACEInhibitors
                        if (Form.FindName("ManagementACEInhibitors") is FrameworkElement)
                        {
                            ListBox lstManagementACEInhibitors = (ListBox)Form.FindName("ManagementACEInhibitors");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementACEInhibitors.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    tempStr1 += "ACE Inhibitors, ";
                                    break;
                                }
                            }
                        }

                        //ManagementAngiotensinIIReceptorBlockers
                        if (Form.FindName("ManagementAngiotensinIIReceptorBlockers") is FrameworkElement)
                        {
                            ListBox lstManagementAngiotensinIIReceptorBlockers = (ListBox)Form.FindName("ManagementAngiotensinIIReceptorBlockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAngiotensinIIReceptorBlockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp5 = true;
                                    tempStr1 += "Angiotensin II Receptor Blockers, ";
                                    break;
                                }
                            }
                        }

                        //ManagementOtherCalciumChannelBlockers
                        if (Form.FindName("ManagementOtherCalciumChannelBlockers") is FrameworkElement)
                        {
                            ListBox lstManagementOtherCalciumChannelBlockers = (ListBox)Form.FindName("ManagementOtherCalciumChannelBlockers");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherCalciumChannelBlockers.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp6 = true;
                                    tempStr1 += "Other Calcium Channel Blockers, ";
                                    break;
                                }
                            }
                        }
                    }
                }

                //ManagementOthers
                if (Form.FindName("ManagementOthers") is FrameworkElement)
                {
                    ListBox lstbxManagementOthers = (ListBox)Form.FindName("ManagementOthers");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOthers.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            temp7 = true;
                            tempStr1 += "Others, ";
                            break;
                        }
                    }
                }

                if (temp1 == true || temp2 == true || temp3 == true || temp4 == true || temp5 == true || temp6 == true || temp7 == true)
                {
                    variance.Variance5 = "Unwarranted drug prescription flag";
                    variance.ListVariance5 = tempStr1;
                }
                #endregion
            }
            else if (TemplateID == 6)
            {
                #region Calculation for Variance1 Version1
                // 1. Standard investigations non compliance flag

                bool tempflag = false;

                if (Form.FindName("HistBloodStoolYes") is FrameworkElement && Form.FindName("GEPallor") is FrameworkElement)
                {
                    RadioButton rdHistBloodStoolYes = (RadioButton)Form.FindName("HistBloodStoolYes");
                    CheckBox rdGEPallor = (CheckBox)Form.FindName("GEPallor");
                    if (rdHistBloodStoolYes.IsChecked == true || rdGEPallor.IsChecked == true)
                    {
                        //InvestLabs-Complete Blood Count (CBC)
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Complete Blood Count (CBC)" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Complete Blood Count (CBC),";
                                    }
                                }
                            }
                        }
                    }
                }

                if (Form.FindName("PDDuodenalUlcer") is FrameworkElement && Form.FindName("PDPepticUlcer") is FrameworkElement && Form.FindName("PDGastricUlcers") is FrameworkElement)
                {
                    CheckBox chkPDDuodenalUlcer = (CheckBox)Form.FindName("PDDuodenalUlcer");
                    CheckBox chkPDPepticUlcer = (CheckBox)Form.FindName("PDPepticUlcer");
                    CheckBox chkPDGastricUlcers = (CheckBox)Form.FindName("PDGastricUlcers");
                    if (chkPDDuodenalUlcer.IsChecked == true || chkPDPepticUlcer.IsChecked == true || chkPDGastricUlcers.IsChecked == true)
                    {
                        //InvestRadiology-Upper GI Ultrasound,USG abdomen 
                        if (Form.FindName("InvestRadiology") is FrameworkElement)
                        {
                            ListBox lstInvestRadiology = (ListBox)Form.FindName("InvestRadiology");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Upper GI Ultrasound" && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "Upper GI Ultrasound,";
                                    }
                                    if (a.Description == "USG abdomen " && a.Status == false)
                                    {
                                        tempflag = true;
                                        variance.ListVariance1 += "USG abdomen,";
                                    }
                                }
                            }
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance1 = "Standard investigations non compliance flag";
                }
                #endregion

                #region Calculation for Variance2 Version1
                // 2. Additional investigations ordered flag

                tempflag = false;

                if (Form.FindName("PDDrugInducedGastritis") is FrameworkElement)
                {
                    CheckBox chkPDDrugInducedGastritis = (CheckBox)Form.FindName("PDDrugInducedGastritis");
                    if (chkPDDrugInducedGastritis.IsChecked == true)
                    {
                        //InvestLabs
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestRadiology
                        if (Form.FindName("InvestRadiology") is FrameworkElement)
                        {
                            ListBox lstInvestRadiology = (ListBox)Form.FindName("InvestRadiology");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestSerologytest
                        if (Form.FindName("InvestSerologytest") is FrameworkElement)
                        {
                            CheckBox chkInvestSerologytest = (CheckBox)Form.FindName("InvestSerologytest");
                            if (chkInvestSerologytest.IsChecked == true)
                            {
                                tempflag = true;
                                variance.ListVariance2 += "Serology test";
                            }
                        }
                        //InvestOtherinvestigations
                        if (Form.FindName("InvestOtherinvestigations") is TextBox && ((TextBox)Form.FindName("InvestOtherinvestigations")).Text != null && ((TextBox)Form.FindName("InvestOtherinvestigations")).Text != "")
                        {
                            tempflag = true;
                            variance.ListVariance2 += ((TextBox)Form.FindName("Other Investigations")).Text;
                        }
                    }

                    if (tempflag == true)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                    }
                }
                #endregion

                #region Calculation for Variance3

                tempflag = false;
                if ((FrameworkElement)Form.FindName("RMToHospital") != null && ((ComboBox)Form.FindName("RMToHospital")).SelectedItem == null)
                {
                    //AlarmAge55yearsWithNewOnsetSymptoms
                    if (Form.FindName("AlarmAge55yearsWithNewOnsetSymptoms") is FrameworkElement)
                    {
                        CheckBox chkAlarmAge55yearsWithNewOnsetSymptoms = (CheckBox)Form.FindName("AlarmAge55yearsWithNewOnsetSymptoms");
                        if (chkAlarmAge55yearsWithNewOnsetSymptoms.IsChecked == true && chkAlarmAge55yearsWithNewOnsetSymptoms.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Age > 55 years With New On set Symptoms, ";
                        }
                    }
                    //AlarmGastrointestinalBleedingVomitingStools
                    if (Form.FindName("AlarmGastrointestinalBleedingVomitingStools") is FrameworkElement)
                    {
                        CheckBox chkAlarmGastrointestinalBleedingVomitingStools = (CheckBox)Form.FindName("AlarmGastrointestinalBleedingVomitingStools");
                        if (chkAlarmGastrointestinalBleedingVomitingStools.IsChecked == true && chkAlarmGastrointestinalBleedingVomitingStools.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Gastrointestinal Bleeding Vomiting Stools, ";
                        }
                    }
                    //AlarmUnexplainedWeightLoss3kg
                    if (Form.FindName("AlarmUnexplainedWeightLoss3kg") is FrameworkElement)
                    {
                        CheckBox chkAlarmUnexplainedWeightLoss3kg = (CheckBox)Form.FindName("AlarmUnexplainedWeightLoss3kg");
                        if (chkAlarmUnexplainedWeightLoss3kg.IsChecked == true && chkAlarmUnexplainedWeightLoss3kg.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Unexplained Weight Loss > 3kg, ";
                        }
                    }
                    //AlarmProgressiveDysphagiaOrPersistentPainfulSwallowing
                    if (Form.FindName("AlarmProgressiveDysphagiaOrPersistentPainfulSwallowing") is FrameworkElement)
                    {
                        CheckBox chkAlarmProgressiveDysphagiaOrPersistentPainfulSwallowing = (CheckBox)Form.FindName("AlarmProgressiveDysphagiaOrPersistentPainfulSwallowing");
                        if (chkAlarmProgressiveDysphagiaOrPersistentPainfulSwallowing.IsChecked == true && chkAlarmProgressiveDysphagiaOrPersistentPainfulSwallowing.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Progressive Dysphagia Or Persistent Painful Swallowing, ";
                        }
                    }
                    //AlarmPersistentVomiting
                    if (Form.FindName("AlarmPersistentVomiting") is FrameworkElement)
                    {
                        CheckBox chkAlarmPersistentVomiting = (CheckBox)Form.FindName("AlarmPersistentVomiting");
                        if (chkAlarmPersistentVomiting.IsChecked == true && chkAlarmPersistentVomiting.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Persistent Vomiting, ";
                        }
                    }
                    //AlarmSignsOfAnemiaPallor
                    if (Form.FindName("AlarmSignsOfAnemiaPallor") is FrameworkElement)
                    {
                        CheckBox chkAlarmSignsOfAnemiaPallor = (CheckBox)Form.FindName("AlarmSignsOfAnemiaPallor");
                        if (chkAlarmSignsOfAnemiaPallor.IsChecked == true && chkAlarmSignsOfAnemiaPallor.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Signs Of Anemia (Pallor), ";
                        }
                    }
                    //AlarmPalpableMassLymphadenopathy
                    if (Form.FindName("AlarmPalpableMassLymphadenopathy") is FrameworkElement)
                    {
                        CheckBox chkAlarmPalpableMassLymphadenopathy = (CheckBox)Form.FindName("AlarmPalpableMassLymphadenopathy");
                        if (chkAlarmPalpableMassLymphadenopathy.IsChecked == true && chkAlarmPalpableMassLymphadenopathy.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Palpable Mass Lymphadenopathy, ";
                        }
                    }
                    //AlarmJaundice
                    if (Form.FindName("AlarmJaundice") is FrameworkElement)
                    {
                        CheckBox chkAlarmJaundice = (CheckBox)Form.FindName("AlarmJaundice");
                        if (chkAlarmJaundice.IsChecked == true && chkAlarmJaundice.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Jaundice, ";
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance3 = "Referral not made flag";
                }
                #endregion

                #region Calculation for Variance4

                // 4.	Medication protocol non adherence flag

                tempflag = false;
                string tempStr1 = "";
                bool temp1 = false;
                bool temp2 = false;
                bool temp3 = false;
                bool temp4 = false;
                bool temp5 = false;
                #region 1
                if (Form.FindName("PDHelicobacterPyloriInfection") is FrameworkElement)
                {
                    CheckBox chkPDHelicobacterPyloriInfection = (CheckBox)Form.FindName("PDHelicobacterPyloriInfection");
                    if (chkPDHelicobacterPyloriInfection.IsChecked == true)
                    {
                        //ManagementMetronidazoleClarithromycin                                                                                   
                        if (Form.FindName("ManagementMetronidazoleClarithromycin") is FrameworkElement)
                        {
                            ListBox lstManagementMetronidazoleClarithromycin = (ListBox)Form.FindName("ManagementMetronidazoleClarithromycin");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementMetronidazoleClarithromycin.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr1 += "Metronidazole Clarithromycin, ";
                        }
                        //ManagementAmoxicillinClarithromycin                                                                                   
                        if (Form.FindName("ManagementAmoxicillinClarithromycin") is FrameworkElement)
                        {
                            ListBox lstManagementAmoxicillinClarithromycin = (ListBox)Form.FindName("ManagementAmoxicillinClarithromycin");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementAmoxicillinClarithromycin.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr1 += "Amoxicillin Clarithromycin, ";
                        }
                        //ManagementClarithromycinAmoxicillinOmeprazolekit
                        if (Form.FindName("ManagementClarithromycinAmoxicillinOmeprazolekit") is FrameworkElement)
                        {
                            ListBox lstManagementClarithromycinAmoxicillinOmeprazolekit = (ListBox)Form.FindName("ManagementClarithromycinAmoxicillinOmeprazolekit");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementClarithromycinAmoxicillinOmeprazolekit.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr1 += "Clarithromycin Amoxicillin Omeprazole kit, ";
                        }

                        //ManagementOmeprazoleClarithromycinTinidazolekit
                        if (Form.FindName("ManagementOmeprazoleClarithromycinTinidazolekit") is FrameworkElement)
                        {
                            ListBox lstManagementOmeprazoleClarithromycinTinidazolekit = (ListBox)Form.FindName("ManagementOmeprazoleClarithromycinTinidazolekit");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOmeprazoleClarithromycinTinidazolekit.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr1 += "Omeprazole Clarithromycin Tinidazole kit, ";
                        }

                        //ManagementOtherAntibiotics
                        if (Form.FindName("ManagementOtherAntibiotics") is FrameworkElement)
                        {
                            ListBox lstbxManagementOtherAntibiotics = (ListBox)Form.FindName("ManagementOtherAntibiotics");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherAntibiotics.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr1 += "Other Antibiotics, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false)
                        {
                            variance.Variance4 = "Medication protocol non adherence flag";
                            variance.ListVariance4 = tempStr1;
                        }
                        else
                        {
                            variance.ListVariance4 = "";
                        }
                    }
                }
                #endregion
                #endregion

                #region Calculation for Variance5

                // 5.	Unwarranted drug prescription flag

                //ManagementOtherDrugs
                if (Form.FindName("ManagementOtherDrugs") is FrameworkElement)
                {
                    ListBox lstbxManagementOtherDrugs = (ListBox)Form.FindName("ManagementOtherDrugs");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherDrugs.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            variance.Variance5 = "Unwarranted drug prescription flag";
                            variance.ListVariance5 = "Other Drugs";
                            break;
                        }
                    }
                }
                #endregion
            }
            else if (TemplateID == 7)
            {
                #region Calculation for Variance1 Version1
                // 1. Standard investigations non compliance flag

                bool tempflag = false;
                bool temp1 = false;
                bool temp2 = false;
                string tempStr1 = "";

                if (Form.FindName("PDPersistentDiarrhea") is FrameworkElement && Form.FindName("PDDysentry") is FrameworkElement)
                {
                    CheckBox chkPDPersistentDiarrhea = (CheckBox)Form.FindName("PDPersistentDiarrhea");
                    CheckBox chkPDDysentry = (CheckBox)Form.FindName("PDDysentry");
                    if (chkPDPersistentDiarrhea.IsChecked == true || chkPDDysentry.IsChecked == true)
                    {
                        //InvestLabs-Stool Culture and sensitivity,Stool Examination
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Stool Culture and sensitivity" && a.Status == false)
                                    {
                                        temp1 = true;
                                        tempStr1 += "Stool Culture and sensitivity,";
                                    }
                                    if (a.Description == "Stool Examination" && a.Status == false)
                                    {
                                        temp2 = true;
                                        tempStr1 += "Stool Examination,";
                                    }
                                }
                                if (temp1 == true && temp2 == true)
                                {
                                    tempflag = true;
                                }
                                else
                                {
                                    tempStr1 = "";
                                }
                            }
                        }
                    }
                }

                if (Form.FindName("HistBloodInStoolsYes") is FrameworkElement && Form.FindName("GEPallor") is FrameworkElement)
                {
                    RadioButton chkHistBloodInStoolsYes = (RadioButton)Form.FindName("HistBloodInStoolsYes");
                    CheckBox chkGEPallor = (CheckBox)Form.FindName("GEPallor");
                    if (chkHistBloodInStoolsYes.IsChecked == true || chkGEPallor.IsChecked == true)
                    {
                        //InvestLabs-Complete Blood Count (CBC)
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Complete Blood Count (CBC)" && a.Status == false)
                                    {
                                        tempflag = true;
                                        tempStr1 += "Complete Blood Count (CBC),";
                                    }
                                }
                            }
                        }
                    }
                }

                temp1 = false;
                temp2 = false;
                string tempStr2 = "";
                if (Form.FindName("PresBloodStool") is FrameworkElement)
                {
                    CheckBox chkPresBloodStool = (CheckBox)Form.FindName("PresBloodStool");
                    if (chkPresBloodStool.IsChecked == true)
                    {
                        //InvestLabs-Stool for Occult Blood,Stool Examination
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Description == "Stool for Occult Blood" && a.Status == false)
                                    {
                                        temp1 = true;
                                        tempStr2 += "Stool for Occult Blood,";
                                    }
                                    if (a.Description == "Stool Examination" && a.Status == false)
                                    {
                                        temp2 = true;
                                        tempStr2 += "Stool Examination,";
                                    }
                                }
                                if (temp1 == true && temp2 == true)
                                {
                                    tempflag = true;
                                }
                                else
                                {
                                    tempStr2 = "";
                                }
                            }
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance1 = "Standard investigations non compliance flag";
                    variance.ListVariance1 = tempStr1 + tempStr2;
                }
                #endregion

                #region Calculation for Variance2 Version1
                // 2. Additional investigations ordered flag

                tempflag = false;

                if (Form.FindName("PDDysentry") is FrameworkElement && Form.FindName("PDAcuteWateryDiarrhea") is FrameworkElement)
                {
                    CheckBox chkPDDysentry = (CheckBox)Form.FindName("PDDysentry");
                    CheckBox chkPDAcuteWateryDiarrhea = (CheckBox)Form.FindName("PDAcuteWateryDiarrhea");
                    if (chkPDDysentry.IsChecked == true || chkPDAcuteWateryDiarrhea.IsChecked == true)
                    {
                        //InvestLabs
                        if (Form.FindName("InvestLabs") is FrameworkElement)
                        {
                            ListBox lstInvestLabs = (ListBox)Form.FindName("InvestLabs");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestLabs.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestOvaParasiteTest
                        if (Form.FindName("InvestOvaParasiteTest") is FrameworkElement)
                        {
                            ListBox lstInvestOvaParasiteTest = (ListBox)Form.FindName("InvestOvaParasiteTest");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestOvaParasiteTest.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestOvaParasiteTest.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestClinitestOrBenedictsTest
                        if (Form.FindName("InvestClinitestOrBenedictsTest") is FrameworkElement)
                        {
                            ListBox lstInvestClinitestOrBenedictsTest = (ListBox)Form.FindName("InvestClinitestOrBenedictsTest");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestClinitestOrBenedictsTest.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestClinitestOrBenedictsTest.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestStoolExamForPhenolphthaleinAndMagnesiumSulphate
                        if (Form.FindName("InvestStoolExamForPhenolphthaleinAndMagnesiumSulphate") is FrameworkElement)
                        {
                            ListBox lstInvestStoolExamForPhenolphthaleinAndMagnesiumSulphate = (ListBox)Form.FindName("InvestStoolExamForPhenolphthaleinAndMagnesiumSulphate");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestStoolExamForPhenolphthaleinAndMagnesiumSulphate.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestStoolExamForPhenolphthaleinAndMagnesiumSulphate.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestRadiology
                        if (Form.FindName("InvestRadiology") is FrameworkElement)
                        {
                            ListBox lstInvestRadiology = (ListBox)Form.FindName("InvestRadiology");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestRadiology.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestSmallBowelBiopsy
                        if (Form.FindName("InvestSmallBowelBiopsy") is FrameworkElement)
                        {
                            ListBox lstInvestSmallBowelBiopsy = (ListBox)Form.FindName("InvestSmallBowelBiopsy");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestSmallBowelBiopsy.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestSmallBowelBiopsy.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestSigmoidoscopyOrColonoscopyWithBiopsies
                        if (Form.FindName("InvestSigmoidoscopyOrColonoscopyWithBiopsies") is FrameworkElement)
                        {
                            ListBox lstInvestSigmoidoscopyOrColonoscopyWithBiopsies = (ListBox)Form.FindName("InvestSigmoidoscopyOrColonoscopyWithBiopsies");
                            if (((InvestigationFieldSetting)((FieldDetail)lstInvestSigmoidoscopyOrColonoscopyWithBiopsies.Tag).Settings).ItemSource != null)
                            {
                                foreach (var a in ((InvestigationFieldSetting)((FieldDetail)lstInvestSigmoidoscopyOrColonoscopyWithBiopsies.Tag).Settings).ItemSource)
                                {
                                    if (a.Status == true)
                                    {
                                        tempflag = true;
                                        variance.ListVariance2 += a.Description.ToString() + ",";
                                    }
                                }
                            }
                        }
                        //InvestOtherTests
                        if (Form.FindName("InvestOtherTests") is TextBox && ((TextBox)Form.FindName("InvestOtherTests")).Text != null && ((TextBox)Form.FindName("InvestOtherTests")).Text != "")
                        {
                            tempflag = true;
                            variance.ListVariance2 += ((TextBox)Form.FindName("Other Tests")).Text;
                        }
                    }

                    if (tempflag == true)
                    {
                        variance.Variance2 = "Additional investigations ordered flag";
                    }
                }
                #endregion

                #region Calculation for Variance3

                tempflag = false;
                if ((FrameworkElement)Form.FindName("RMToHospital") != null && ((ComboBox)Form.FindName("RMToHospital")).SelectedItem == null)
                {
                    //AlarmDiarrheaFever
                    if (Form.FindName("AlarmDiarrheaFever") is FrameworkElement)
                    {
                        CheckBox chkAlarmDiarrheaFever = (CheckBox)Form.FindName("AlarmDiarrheaFever");
                        if (chkAlarmDiarrheaFever.IsChecked == true && chkAlarmDiarrheaFever.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Diarrhea accompanied with fever above 102.2 F, ";
                        }
                    }
                    //AlarmAbdominalDistension
                    if (Form.FindName("AlarmAbdominalDistension") is FrameworkElement)
                    {
                        CheckBox chkAlarmAbdominalDistension = (CheckBox)Form.FindName("AlarmAbdominalDistension");
                        if (chkAlarmAbdominalDistension.IsChecked == true && chkAlarmAbdominalDistension.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Abdominal distension, ";
                        }
                    }
                    //AlarmProlongedSymptomsOfDiarrhea14Days
                    if (Form.FindName("AlarmProlongedSymptomsOfDiarrhea14Days") is FrameworkElement)
                    {
                        CheckBox chkAlarmProlongedSymptomsOfDiarrhea14Days = (CheckBox)Form.FindName("AlarmProlongedSymptomsOfDiarrhea14Days");
                        if (chkAlarmProlongedSymptomsOfDiarrhea14Days.IsChecked == true && chkAlarmProlongedSymptomsOfDiarrhea14Days.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Prolonged symptoms of diarrhea(>14 days), ";
                        }
                    }
                    //AlarmPersistentVomiting
                    if (Form.FindName("AlarmPersistentVomiting") is FrameworkElement)
                    {
                        CheckBox chkAlarmPersistentVomiting = (CheckBox)Form.FindName("AlarmPersistentVomiting");
                        if (chkAlarmPersistentVomiting.IsChecked == true && chkAlarmPersistentVomiting.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Persistent vomiting, ";
                        }
                    }
                    //AlarmPassingOfBlackTarryStool
                    if (Form.FindName("AlarmPassingOfBlackTarryStool") is FrameworkElement)
                    {
                        CheckBox chkAlarmPassingOfBlackTarryStool = (CheckBox)Form.FindName("AlarmPassingOfBlackTarryStool");
                        if (chkAlarmPassingOfBlackTarryStool.IsChecked == true && chkAlarmPassingOfBlackTarryStool.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Passing of black tarry stool, ";
                        }
                    }
                    //AlarmSevereDehydration
                    if (Form.FindName("AlarmSevereDehydration") is FrameworkElement)
                    {
                        CheckBox chkAlarmSevereDehydration = (CheckBox)Form.FindName("AlarmSevereDehydration");
                        if (chkAlarmSevereDehydration.IsChecked == true && chkAlarmSevereDehydration.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Severe dehydration, ";
                        }
                    }
                    //AlarmSeverePainInTheAbdomenOrRectum
                    if (Form.FindName("AlarmSeverePainInTheAbdomenOrRectum") is FrameworkElement)
                    {
                        CheckBox chkAlarmSeverePainInTheAbdomenOrRectum = (CheckBox)Form.FindName("AlarmSeverePainInTheAbdomenOrRectum");
                        if (chkAlarmSeverePainInTheAbdomenOrRectum.IsChecked == true && chkAlarmSeverePainInTheAbdomenOrRectum.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Severe pain in the abdomen or rectum, ";
                        }
                    }
                    //AlarmDiarrheaWithSevereAbdominalPain
                    if (Form.FindName("AlarmDiarrheaWithSevereAbdominalPain") is FrameworkElement)
                    {
                        CheckBox chkAlarmDiarrheaWithSevereAbdominalPain = (CheckBox)Form.FindName("AlarmDiarrheaWithSevereAbdominalPain");
                        if (chkAlarmDiarrheaWithSevereAbdominalPain.IsChecked == true && chkAlarmDiarrheaWithSevereAbdominalPain.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Diarrhea with severe abdominal pain, ";
                        }
                    }
                    //AlarmDrowsinessOrListlessness
                    if (Form.FindName("AlarmDrowsinessOrListlessness") is FrameworkElement)
                    {
                        CheckBox chkAlarmDrowsinessOrListlessness = (CheckBox)Form.FindName("AlarmDrowsinessOrListlessness");
                        if (chkAlarmDrowsinessOrListlessness.IsChecked == true && chkAlarmDrowsinessOrListlessness.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Drowsiness or listlessness, ";
                        }
                    }
                    //AlarmConvulsionsOrLossOfConsciousness
                    if (Form.FindName("AlarmConvulsionsOrLossOfConsciousness") is FrameworkElement)
                    {
                        CheckBox chkAlarmConvulsionsOrLossOfConsciousness = (CheckBox)Form.FindName("AlarmConvulsionsOrLossOfConsciousness");
                        if (chkAlarmConvulsionsOrLossOfConsciousness.IsChecked == true && chkAlarmConvulsionsOrLossOfConsciousness.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Convulsions or loss of consciousness, ";
                        }
                    }
                    //AlarmInabilityToAdministerOralRehydrationTherapy
                    if (Form.FindName("AlarmInabilityToAdministerOralRehydrationTherapy") is FrameworkElement)
                    {
                        CheckBox chkAlarmInabilityToAdministerOralRehydrationTherapy = (CheckBox)Form.FindName("AlarmInabilityToAdministerOralRehydrationTherapy");
                        if (chkAlarmInabilityToAdministerOralRehydrationTherapy.IsChecked == true && chkAlarmInabilityToAdministerOralRehydrationTherapy.Visibility == Visibility.Visible)
                        {
                            tempflag = true;
                            variance.ListVariance3 += "Inability to administer oral rehydration therapy";
                        }
                    }
                }

                if (tempflag == true)
                {
                    variance.Variance3 = "Referral not made flag";
                }
                #endregion

                #region Calculation for Variance4

                // 4.	Medication protocol non adherence flag

                tempflag = false;
                tempStr1 = "";
                temp1 = false;
                temp2 = false;
                bool temp3 = false;
                bool temp4 = false;
                bool temp5 = false;
                #region 1
                if (Form.FindName("PDDysentry") is FrameworkElement)
                {
                    CheckBox chkPDDysentry = (CheckBox)Form.FindName("PDDysentry");
                    if (chkPDDysentry.IsChecked == true)
                    {
                        //ManagementLactobacillus                                                                                   
                        if (Form.FindName("ManagementLactobacillus") is FrameworkElement)
                        {
                            ListBox lstManagementLactobacillus = (ListBox)Form.FindName("ManagementLactobacillus");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementLactobacillus.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    break;
                                }
                            }
                            if (temp1 == false)
                                tempStr1 += "Lactobacillus, ";
                        }
                        //ManagementOtherAntidiarrheals                                                                                   
                        if (Form.FindName("ManagementOtherAntidiarrheals") is FrameworkElement)
                        {
                            ListBox lstManagementOtherAntidiarrheals = (ListBox)Form.FindName("ManagementOtherAntidiarrheals");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementOtherAntidiarrheals.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    break;
                                }
                            }
                            if (temp2 == false)
                                tempStr1 += "Other Antidiarrheals, ";
                        }
                        //ManagementQuinolones
                        if (Form.FindName("ManagementQuinolones") is FrameworkElement)
                        {
                            ListBox lstManagementQuinolones = (ListBox)Form.FindName("ManagementQuinolones");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementQuinolones.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp3 = true;
                                    break;
                                }
                            }
                            if (temp3 == false)
                                tempStr1 += "Quinolones, ";
                        }

                        //ManagementTetracyclines
                        if (Form.FindName("ManagementTetracyclines") is FrameworkElement)
                        {
                            ListBox lstManagementTetracyclines = (ListBox)Form.FindName("ManagementTetracyclines");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementTetracyclines.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp4 = true;
                                    break;
                                }
                            }
                            if (temp4 == false)
                                tempStr1 += "Tetracyclines, ";
                        }

                        //ManagementOtherAntibiotics
                        if (Form.FindName("ManagementOtherAntibiotics") is FrameworkElement)
                        {
                            ListBox lstbxManagementOtherAntibiotics = (ListBox)Form.FindName("ManagementOtherAntibiotics");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherAntibiotics.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp5 = true;
                                    break;
                                }
                            }
                            if (temp5 == false)
                                tempStr1 += "Other Antibiotics, ";
                        }

                        if (temp1 == false && temp2 == false && temp3 == false && temp4 == false && temp5 == false)
                        {
                            variance.Variance4 = "Medication protocol non adherence flag";
                            variance.ListVariance4 = tempStr1;
                        }
                        else
                        {
                            variance.ListVariance4 = "";
                        }
                    }
                }
                #endregion
                #endregion

                #region Calculation for Variance5

                // 5.	Unwarranted drug prescription flag

                tempflag = false;
                tempStr1 = "";
                temp1 = false;
                temp2 = false;
                temp3 = false;
                temp4 = false;


                if (Form.FindName("PDAcuteWateryDiarrhea") is FrameworkElement)
                {
                    CheckBox chkPDAcuteWateryDiarrhea = (CheckBox)Form.FindName("PDAcuteWateryDiarrhea");
                    if (chkPDAcuteWateryDiarrhea.IsChecked == true)
                    {
                        //ManagementQuinolones                                                                                   
                        if (Form.FindName("ManagementQuinolones") is FrameworkElement)
                        {
                            ListBox lstManagementQuinolones = (ListBox)Form.FindName("ManagementQuinolones");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementQuinolones.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp1 = true;
                                    tempStr1 += "Quinolones, ";
                                    break;
                                }
                            }
                        }
                        //ManagementTetracyclines                                                                                   
                        if (Form.FindName("ManagementTetracyclines") is FrameworkElement)
                        {
                            ListBox lstManagementTetracyclines = (ListBox)Form.FindName("ManagementTetracyclines");

                            MedicationFieldSetting medSetting = ((MedicationFieldSetting)((FieldDetail)lstManagementTetracyclines.DataContext).Settings);
                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                            {
                                if (((Medication)medSetting.ItemsSource[i]).Drug != null)
                                {
                                    temp2 = true;
                                    tempStr1 += "Tetracyclines, ";
                                    break;
                                }
                            }
                        }
                        //ManagementOtherAntibiotics
                        if (Form.FindName("ManagementOtherAntibiotics") is FrameworkElement)
                        {
                            ListBox lstbxManagementOtherAntibiotics = (ListBox)Form.FindName("ManagementOtherAntibiotics");

                            OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherAntibiotics.DataContext).Settings);
                            for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                            {
                                if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                                {
                                    temp3 = true;
                                    tempStr1 += "Others Antibiotics, ";
                                    break;
                                }
                            }
                        }
                    }
                }

                //ManagementOtherDrugs
                if (Form.FindName("ManagementOtherDrugs") is FrameworkElement)
                {
                    ListBox lstbxManagementOtherDrugs = (ListBox)Form.FindName("ManagementOtherDrugs");

                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)((FieldDetail)lstbxManagementOtherDrugs.DataContext).Settings);
                    for (int i = 0; i < OthermedSetting.ItemsSource.Count; i++)
                    {
                        if (((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != null && ((OtherMedication)OthermedSetting.ItemsSource[i]).Drug != "")
                        {
                            temp4 = true;
                            tempStr1 += "Other Drugs";
                            break;
                        }
                    }
                }

                if (temp1 == true || temp2 == true || temp3 == true || temp4 == true)
                {
                    variance.Variance5 = "Unwarranted drug prescription flag";
                    variance.ListVariance5 = tempStr1;
                }
                #endregion
            }

        }

        private string IsMandatorySet()
        {
            string listofMandatoryFields = "";

            int i = 1;

            #region Calculation of Mandatory Fields which are not filled


            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null)
            {
                foreach (var section in SelectedFormStructure.SectionList)
                {
                    foreach (var item in section.FieldList)
                    {
                        if (item.IsRequired)
                        {
                            switch (item.DataType.Id)
                            {
                                case 1:
                                    if (((TextFieldSetting)item.Settings).Value == null || ((TextFieldSetting)item.Settings).Value == "")
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 2:
                                    if (((BooleanFieldSetting)item.Settings).Value == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 3:
                                    if (((DateFieldSetting)item.Settings).Date == null)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 4:
                                    if (((ListFieldSetting)item.Settings).ChoiceMode == SelectionMode.Single)
                                    {
                                        if (((ListFieldSetting)item.Settings).SelectedItem == null)
                                        {
                                            listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                        }
                                    }
                                    else if (((ListFieldSetting)item.Settings).ChoiceMode == SelectionMode.Multiples)
                                    {
                                        if (((ListFieldSetting)item.Settings).SelectedItems != null)
                                        {
                                            if (((ListFieldSetting)item.Settings).SelectedItems.Count == 0)
                                                listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                        }
                                    }
                                    break;
                                case 5:
                                    if (((DecimalFieldSetting)item.Settings).Value == null || ((DecimalFieldSetting)item.Settings).Value == Convert.ToDecimal(0))
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 8:
                                    if (((ListFieldSetting)item.Settings).ChoiceMode == SelectionMode.Single)
                                    {
                                        if (((LookUpFieldSetting)item.Settings).SelectedItem == null)
                                        {
                                            listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                        }
                                    }
                                    break;
                                case 9:
                                    bool tempformedication = false;
                                    MedicationFieldSetting medSetting = ((MedicationFieldSetting)item.Settings);
                                    for (int k = 0; k < medSetting.ItemsSource.Count; k++)
                                    {
                                        if (((Medication)medSetting.ItemsSource[k]).Drug != null)
                                        {
                                            tempformedication = true;
                                            break;
                                        }
                                    }
                                    if (tempformedication == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 11:
                                    bool tempForOtherInvest = false;
                                    OtherInvestigationFieldSetting InvestSetting = (OtherInvestigationFieldSetting)item.Settings;
                                    for (int k = 0; k < InvestSetting.ItemsSource.Count; k++)
                                    {
                                        if (((OtherInvestigation)InvestSetting.ItemsSource[k]).Investigation != null && ((OtherInvestigation)InvestSetting.ItemsSource[k]).Investigation != "--Select--")
                                        {
                                            tempForOtherInvest = true;
                                            break;
                                        }
                                    }
                                    if (tempForOtherInvest == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 12:
                                    bool tempForListCheckBox = false;
                                    ListOfCheckBoxesFieldSetting listSetting1 = (ListOfCheckBoxesFieldSetting)item.Settings;
                                    for (int k = 0; k < listSetting1.ItemsSource.Count; k++)
                                    {
                                        if (listSetting1.SelectedItems[k])
                                        {
                                            tempForListCheckBox = true;
                                            break;
                                        }
                                    }
                                    if (tempForListCheckBox == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 13:
                                    bool tempForAutomatedListBox = false;
                                    AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)item.Settings);
                                    switch (AutolistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (AutolistSetting.SelectedItem != null)
                                            {
                                                tempForAutomatedListBox = true;
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (AutolistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (AutolistSetting.SelectedItem != null)
                                                    {
                                                        tempForAutomatedListBox = true;
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                    {
                                                        tempForAutomatedListBox = true;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < AutolistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)AutolistSetting.ItemSource[k]).Status)
                                                {
                                                    tempForAutomatedListBox = true;
                                                    break;
                                                }
                                            }
                                            break;
                                    }
                                    if (tempForAutomatedListBox == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 14:
                                    bool tempForOtherMedication = false;
                                    OtherMedicationFieldSetting OthermedSetting = ((OtherMedicationFieldSetting)item.Settings);
                                    for (int k = 0; k < OthermedSetting.ItemsSource.Count; k++)
                                    {
                                        if (((OtherMedication)OthermedSetting.ItemsSource[k]).Drug != null)
                                        {
                                            tempForOtherMedication = true;
                                            break;
                                        }
                                    }
                                    if (tempForOtherMedication == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                                case 15:
                                    bool tempForInvest = false;
                                    InvestigationFieldSetting InvestlistSetting = ((InvestigationFieldSetting)item.Settings);
                                    switch (InvestlistSetting.ControlType)
                                    {
                                        case AutoListControlType.ComboBox:
                                            if (InvestlistSetting.SelectedItem != null)
                                            {
                                                tempForInvest = true;
                                            }
                                            break;
                                        case AutoListControlType.ListBox:
                                            switch (InvestlistSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    if (InvestlistSetting.SelectedItem != null)
                                                    {
                                                        tempForInvest = true;
                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (InvestlistSetting.SelectedItems != null && InvestlistSetting.SelectedItems.Count > 0)
                                                    {
                                                        tempForInvest = true;
                                                    }
                                                    break;
                                            }
                                            break;
                                        case AutoListControlType.CheckListBox:
                                            for (int k = 0; k < InvestlistSetting.ItemSource.Count; k++)
                                            {
                                                if (((MasterListItem)InvestlistSetting.ItemSource[k]).Status)
                                                {
                                                    tempForInvest = true;
                                                }
                                            }
                                            break;
                                    }
                                    if (tempForInvest == false)
                                    {
                                        listofMandatoryFields += i++ + ". " + item.Title + ", ";
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            #endregion

            return listofMandatoryFields;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            SavePhysicalExam();
        }

        private void SavePhysicalExam()
        {
            string msgText = "Palash";
            Indicatior = new WaitIndicator();
            try
            {
                #region Add Patient EMR Template
                Indicatior.Show();

                listPatientEMRDetails.Clear();

                SaveFieldWiseData();

                Indicatior.Close();
                IsSaveRecord = true;

                Form.RowDefinitions.Clear();
                Form.Children.Clear();
                SelectedFormStructure = null;
                scroolForm.ScrollToTop();
                GC.Collect();
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());












                #endregion
            }
            catch (Exception)
            {
                Indicatior.Close();
                msgText = "Error occurred while saving record.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgText, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }
        }

        private void SaveFieldWiseData()
        {
            #region Consultation
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null)
            {
                FieldDetail target = null;
                foreach (var section in SelectedFormStructure.SectionList)
                {
                    foreach (var field in section.FieldList)
                    {
                        target = field;
                        target.Parent = section;
                        if (target != null)
                        {
                            string tempControlName = "";
                            string tempControlCaption = "";
                            string tempControlValue = null;
                            string tempControlType = null;
                            DateTime? tempDt = null;
                            if ((((SectionDetail)target.Parent).Tab == "Consultation") || (((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
                            {
                                tempControlName = target.Name;
                                tempControlCaption = target.Title;
                                Tab = "Consultation";
                                switch (target.DataType.Id)
                                {
                                    case 1:
                                        tempControlType = "Text";
                                        tempControlValue = ((TextFieldSetting)target.Settings).Value;
                                        break;
                                    case 2:
                                        if (((FrameworkElement)target.Control != null))
                                        {
                                            tempControlType = "Boolean";
                                            if (((BooleanFieldSetting)target.Settings).Value && ((FrameworkElement)target.Control).Visibility == Visibility.Visible)
                                            {
                                                tempControlValue = "True";
                                            }
                                            else
                                            {
                                                tempControlValue = "False";
                                            }
                                        }
                                        break;
                                    case 3:
                                        tempControlType = "DateTime";
                                        tempControlValue = ((DateFieldSetting)target.Settings).Date.ToString();
                                        break;
                                    case 16:
                                        tempControlType = "DateTime";
                                        //tempControlValue = Convert.ToDateTime(((DateFieldSetting)target.Settings).Date).ToShortTimeString();
                                        tempControlValue = Convert.ToDateTime(((TimeFieldSetting)target.Settings).Time).ToShortTimeString();
                                        break;
                                    case 5:
                                        tempControlType = "Numeric";
                                        if (((DecimalFieldSetting)target.Settings).Value != null)
                                        {
                                            tempControlValue = ((DecimalFieldSetting)target.Settings).Value.ToString();
                                        }
                                        break;
                                    case 6:
                                        tempControlType = "Hyperlink";
                                        tempControlValue = ((HyperlinkFieldSetting)target.Settings).Url;
                                        break;
                                    case 8:
                                        tempControlType = "LookUp";
                                        LookUpFieldSetting LookUpSetting = ((LookUpFieldSetting)target.Settings);
                                        switch (LookUpSetting.ChoiceMode)
                                        {
                                            case SelectionMode.Single:

                                                if (LookUpSetting.SelectedItem != null)
                                                {
                                                    tempControlValue = ((DynamicListItem)LookUpSetting.SelectedItem).Title;
                                                }
                                                if (LookUpSetting.IsAlternateText == true && LookUpSetting.AlternateText != null && LookUpSetting.AlternateText != "")
                                                    tempControlValue = (tempControlValue == "" ? "" : (tempControlValue + " OR ")) + LookUpSetting.AlternateText;
                                                break;
                                        }
                                        break;

                                }

                                if (tempControlType != null)
                                {
                                    clsPatientEMRDetailsVO objPatEMRDetail = new clsPatientEMRDetailsVO();
                                    objPatEMRDetail.ControlCaption = tempControlCaption;
                                    objPatEMRDetail.ControlName = tempControlName;
                                    objPatEMRDetail.ControlType = tempControlType;
                                    objPatEMRDetail.Value = tempControlValue;
                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                    objPatEMRDetail.TemplateID = TemplateID;
                                    if (tempControlValue != "")
                                        listPatientEMRDetails.Add(objPatEMRDetail);
                                }
                                else
                                {
                                    clsPatientEMRDetailsVO objPatEMRDetail = null;
                                    switch (target.DataType.Id)
                                    {
                                        case 4:
                                            ListFieldSetting listSetting = (ListFieldSetting)target.Settings;
                                            switch (listSetting.ChoiceMode)
                                            {
                                                case SelectionMode.Single:
                                                    switch (listSetting.ControlType)
                                                    {
                                                        case ListControlType.ComboBox:
                                                            objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                            objPatEMRDetail.ControlCaption = target.Title;
                                                            objPatEMRDetail.ControlName = target.Name;
                                                            objPatEMRDetail.ControlType = "Combo Box";
                                                            if (listSetting.SelectedItem != null)
                                                                objPatEMRDetail.Value = ((DynamicListItem)listSetting.SelectedItem).Title;
                                                            else
                                                                objPatEMRDetail.Value = null;
                                                            objPatEMRDetail.TemplateID = TemplateID;
                                                            objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                            listPatientEMRDetails.Add(objPatEMRDetail);
                                                            break;

                                                    }
                                                    break;
                                                case SelectionMode.Multiples:
                                                    if (listSetting.SelectedItems != null && listSetting.SelectedItems.Count > 0)
                                                    {
                                                        foreach (var ob in listSetting.SelectedItems)
                                                        {
                                                            if (ob != null)
                                                            {
                                                                objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                                objPatEMRDetail.ControlCaption = target.Title;
                                                                objPatEMRDetail.ControlName = target.Name;
                                                                objPatEMRDetail.ControlType = "List Box - MultiSelect";
                                                                //if (((DynamicListItem)listSetting.SelectedItem) != null)
                                                                //{
                                                                //    objPatEMRDetail.Value = ((DynamicListItem)listSetting.SelectedItem).Title;
                                                                //}
                                                                /////Changed By Nilesh Raut on 11 Sept 2013
                                                                if (ob.Title != "")
                                                                    objPatEMRDetail.Value = ob.Title;
                                                                objPatEMRDetail.TemplateID = TemplateID;
                                                                objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                                listPatientEMRDetails.Add(objPatEMRDetail);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                        objPatEMRDetail.ControlCaption = target.Title;
                                                        objPatEMRDetail.ControlName = target.Name;
                                                        objPatEMRDetail.ControlType = "List Box - MultiSelect";
                                                        objPatEMRDetail.Value = null;
                                                        objPatEMRDetail.TemplateID = TemplateID;
                                                        objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                        listPatientEMRDetails.Add(objPatEMRDetail);
                                                    }
                                                    break;
                                            }
                                            break;
                                        case 12:
                                            ListOfCheckBoxesFieldSetting listSetting1 = (ListOfCheckBoxesFieldSetting)target.Settings;
                                            for (int i = 0; listSetting1.ItemsSource != null && i < listSetting1.ItemsSource.Count; i++)
                                            {
                                                if (listSetting1.SelectedItems[i])
                                                {
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = target.Title;
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.ControlType = "List Of Check Box";
                                                    objPatEMRDetail.Value = (listSetting1.ItemsSource[i]).ToString();
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                            }
                                            if (listSetting1.ItemsSource.Count == 0)
                                            {
                                                objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                objPatEMRDetail.ControlCaption = target.Title;
                                                objPatEMRDetail.ControlName = target.Name;
                                                objPatEMRDetail.ControlType = "List Of Check Box";
                                                objPatEMRDetail.Value = null;
                                                objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                objPatEMRDetail.TemplateID = TemplateID;
                                                listPatientEMRDetails.Add(objPatEMRDetail);
                                            }
                                            break;
                                        case 13:
                                            AutomatedListFieldSetting AutolistSetting = ((AutomatedListFieldSetting)target.Settings);
                                            switch (AutolistSetting.ControlType)
                                            {
                                                case AutoListControlType.ComboBox:
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = target.Title;
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.ControlType = "AutomatedList - Combo Box";
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    if (AutolistSetting.SelectedItem != null)
                                                    {
                                                        objPatEMRDetail.Value = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                    }
                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                    break;
                                                case AutoListControlType.ListBox:
                                                    switch (AutolistSetting.ChoiceMode)
                                                    {
                                                        case SelectionMode.Single:
                                                            objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                            objPatEMRDetail.ControlCaption = target.Title;
                                                            objPatEMRDetail.ControlName = target.Name;
                                                            objPatEMRDetail.ControlType = "AutomatedList - List Box - SigleSelect";
                                                            objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                            if (AutolistSetting.SelectedItem != null)
                                                            {
                                                                objPatEMRDetail.Value = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                            }
                                                            objPatEMRDetail.TemplateID = TemplateID;
                                                            listPatientEMRDetails.Add(objPatEMRDetail);
                                                            break;
                                                        case SelectionMode.Multiples:
                                                            if (AutolistSetting.SelectedItems != null && AutolistSetting.SelectedItems.Count > 0)
                                                            {
                                                                foreach (var obList in AutolistSetting.SelectedItems)
                                                                {
                                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                                    objPatEMRDetail.ControlCaption = target.Title;
                                                                    objPatEMRDetail.ControlName = target.Name;
                                                                    objPatEMRDetail.ControlType = "AutomatedList - List Box - MultiSelect";
                                                                    objPatEMRDetail.Value = obList.Description.ToString();
                                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                                objPatEMRDetail.ControlCaption = target.Title;
                                                                objPatEMRDetail.ControlName = target.Name;
                                                                objPatEMRDetail.ControlType = "AutomatedList - List Box - MultiSelect";
                                                                objPatEMRDetail.Value = null;
                                                                objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                                objPatEMRDetail.TemplateID = TemplateID;
                                                                listPatientEMRDetails.Add(objPatEMRDetail);
                                                            }
                                                            break;
                                                    }
                                                    break;
                                                case AutoListControlType.CheckListBox:
                                                    for (int k = 0; AutolistSetting.ItemSource != null && k < AutolistSetting.ItemSource.Count; k++)
                                                    {
                                                        if (((MasterListItem)AutolistSetting.ItemSource[k]).Status)
                                                        {
                                                            objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                            objPatEMRDetail.ControlCaption = target.Title;
                                                            objPatEMRDetail.ControlName = target.Name;
                                                            objPatEMRDetail.ControlType = "AutomatedList - CheckList Box";
                                                            objPatEMRDetail.Value = ((MasterListItem)AutolistSetting.ItemSource[k]).Description.ToString();
                                                            objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                            objPatEMRDetail.TemplateID = TemplateID;
                                                            listPatientEMRDetails.Add(objPatEMRDetail);
                                                        }
                                                    }
                                                    if (AutolistSetting.ItemSource.Count == 0)
                                                    {
                                                        objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                        objPatEMRDetail.ControlCaption = target.Title;
                                                        objPatEMRDetail.ControlName = target.Name;
                                                        objPatEMRDetail.ControlType = "AutomatedList - CheckList Box";
                                                        objPatEMRDetail.Value = null;
                                                        objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                        objPatEMRDetail.TemplateID = TemplateID;
                                                        listPatientEMRDetails.Add(objPatEMRDetail);
                                                    }
                                                    break;
                                            }
                                            break;
                                        //Added by Saily P on 03.12.13 Purpose - New control.
                                        case 18:
                                            IsBPControl = true;
                                            if (objBPDetails == null)
                                                objBPDetails = new clsBPControlVO();
                                            BPControlSetting BPSetting = ((BPControlSetting)target.Settings);
                                            for (int i = 0; i < BPSetting.ItemsSource.Count; i++)
                                            {
                                                if (((BPControl)BPSetting.ItemsSource[i]).BP1 != null)
                                                {
                                                    objBPDetails = new clsBPControlVO()
                                                    {
                                                        BP1 = ((BPControl)BPSetting.ItemsSource[i]).BP1,
                                                        BP2 = ((BPControl)BPSetting.ItemsSource[i]).BP2,
                                                    };
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = "BPControl";
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.ControlType = "Boolean";
                                                    objPatEMRDetail.Value = "True";
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                            }

                                            break;
                                        case 19:
                                            IsVisionControl = true;
                                            if (objVisionDetails == null)
                                                objVisionDetails = new clsVisionVO();
                                            VisionControlSetting VisionSetting = ((VisionControlSetting)target.Settings);
                                            for (int i = 0; i < VisionSetting.ItemsSource.Count; i++)
                                            {
                                                if (((VisionControl)VisionSetting.ItemsSource[i]).SPH != null)
                                                {
                                                    objVisionDetails = new clsVisionVO()
                                                    {
                                                        SPH = ((VisionControl)VisionSetting.ItemsSource[i]).SPH,
                                                        CYL = ((VisionControl)VisionSetting.ItemsSource[i]).CYL,
                                                        Axis = ((VisionControl)VisionSetting.ItemsSource[i]).Axis,
                                                    };
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = "VisionControl";
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.ControlType = "Boolean";
                                                    objPatEMRDetail.Value = "True";
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                            }

                                            break;
                                        case 20:
                                            IsGPControl = true;
                                            if (objGlassPower == null)
                                                objGlassPower = new clsGlassPowerVO();
                                            GlassPowerControlSetting GPSetting = ((GlassPowerControlSetting)target.Settings);
                                            for (int i = 0; i < GPSetting.ItemsSource.Count; i++)
                                            {
                                                if (((GlassPower)GPSetting.ItemsSource[i]).SPH1 != null)
                                                {
                                                    objGlassPower = new clsGlassPowerVO()
                                                    {
                                                        SPH1 = ((GlassPower)GPSetting.ItemsSource[i]).SPH1,
                                                        CYL1 = ((GlassPower)GPSetting.ItemsSource[i]).CYL1,
                                                        Axis1 = ((GlassPower)GPSetting.ItemsSource[i]).Axis1,
                                                        VA1 = ((GlassPower)GPSetting.ItemsSource[i]).VA1,
                                                        SPH2 = ((GlassPower)GPSetting.ItemsSource[i]).SPH2,
                                                        CYL2 = ((GlassPower)GPSetting.ItemsSource[i]).CYL2,
                                                        Axis2 = ((GlassPower)GPSetting.ItemsSource[i]).Axis2,
                                                        VA2 = ((GlassPower)GPSetting.ItemsSource[i]).VA2,
                                                    };
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = "GlassPower";
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.ControlType = "Boolean";
                                                    objPatEMRDetail.Value = "True";
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                            }

                                            break;
                                        case 17:
                                            if (listPatientEMRUploadedFiles == null)
                                                listPatientEMRUploadedFiles = new List<clsPatientEMRUploadedFilesVO>();
                                            FileUploadFieldSetting medSetting = ((FileUploadFieldSetting)target.Settings);
                                            for (int i = 0; i < medSetting.ItemsSource.Count; i++)
                                            {
                                                if (((FileUpload)medSetting.ItemsSource[i]).FileName != null && ((FileUpload)medSetting.ItemsSource[i]).FileName != "")
                                                {
                                                    listPatientEMRUploadedFiles.Add(new clsPatientEMRUploadedFilesVO()
                                                    {
                                                        ControlCaption = target.Title,
                                                        ControlName = target.Name,
                                                        ControlType = "FileUpload",
                                                        Value = ((FileUpload)medSetting.ItemsSource[i]).Data,
                                                        ControlIndex = i
                                                    });

                                                    clsPatientEMRDetailsVO objPatEMRDetail1 = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail1.ControlCaption = target.Title;
                                                    objPatEMRDetail1.ControlName = target.Name;
                                                    objPatEMRDetail1.ControlType = "FileUpload";
                                                    objPatEMRDetail1.Value = ((FileUpload)medSetting.ItemsSource[i]).FileName;
                                                    objPatEMRDetail1.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail1.TemplateID = TemplateID;
                                                    listPatientEMRDetails.Add(objPatEMRDetail1);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            #endregion
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (IsSaveRecord == true)
                this.DialogResult = true;
            else
                this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ScrollViewer_KeyUp(object sender, KeyEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            FrameworkElement focusedElement = FocusManager.GetFocusedElement() as FrameworkElement;
            GeneralTransform focusedVisualTransform = focusedElement.TransformToVisual(scrollViewer);
            Rect rectangle = focusedVisualTransform.TransformBounds(new Rect(new Point(focusedElement.Margin.Left, focusedElement.Margin.Top), focusedElement.RenderSize));
            double newOffset = scrollViewer.VerticalOffset + (rectangle.Bottom - scrollViewer.ViewportHeight);
            scrollViewer.ScrollToVerticalOffset(newOffset);
        }

        private void LoadTemplate()
        {

            if (TemplateID > 0)
            {

                GetEMRDetails(TemplateID);
            }
            else
            {
                MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Select The Sheet.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbx.Show();
            }

        }
    }
}



