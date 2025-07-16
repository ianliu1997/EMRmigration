using PalashDynamics.ValueObjects.EMR;
using System.Windows;
using System.Collections.Generic;
using PalashDynamics.ValueObjects;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Browser;
using System.Windows.Data;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Animations;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.DataVisualization.Charting;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.SearchResultLists;

namespace EMR
{
    public partial class frmEMRVitals : UserControl
    {
        #region Data Member
        DateConverter dateConverter;
        public long TemplateID { get; set; }
        public List<clsEMRVitalsVO> VitalList;
        public clsVisitVO CurrentVisit { get; set; }
        string log = string.Empty;
        string strErrorMsg =string.Empty;
        string textBefore = string.Empty;
        int selectionStart = 0;
        int selectionLength = 0;
        WaitIndicator Indicatior = new WaitIndicator();
        public Patient SelectedPatient { get; set; }
        public FormDetail SelectedFormStructure { get; set; }
        public string SelectedUser { get; set; }
        public bool IsView = false;
        bool IsFirstTime = false;
        public int VisitTypeID { get; set; }

        System.Windows.Controls.Primitives.Popup p = null;
        System.Windows.Controls.Primitives.Popup pf = null;
        public clsPCRVO objPCR { get; set; }
        public bool IsEnabledControl = false;
        List<clsEMRVitalsVO> EmrDetails = new List<clsEMRVitalsVO>();
        clsAppConfigVO ApplicationConfiguration = ((IApplicationConfiguration)App.Current).ApplicationConfigurations;
        public ObservableCollection<clsVitalItems> ListOfVital;
        #endregion

        #region Constructor
        public frmEMRVitals()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            ListOfVital = new ObservableCollection<clsVitalItems>();
            ChartType.ItemsSource = ListOfVital;
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Loaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //strErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
            dateConverter = new DateConverter();
            if (CurrentVisit != null)
            {
                //if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
                //{
                //    this.IsEnabledControl = false;
                //}
                //if (IsEnabledControl == false)
                //{
                //    cmdSave.IsEnabled = false;
                //}
                FillVitalListComboBox();
                FillVitalChartData();
                GetEMRDetails();
                //DateTime d = CurrentVisit.Date;
                //if (d.ToString("d") != DateTime.Now.ToString("d"))
                //{
                //    cmdSave.IsEnabled = false;
                //}

                // EMR Changes Added by Ashish Z. on dated 02062017
                if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
                {
                    cmdSave.IsEnabled = false;
                }
                //End
              
            }
        }
        private void FillVitalListComboBox()
        {
            clsVitalItems objVitalItems = new clsVitalItems();
            objVitalItems.GetVitalList();
            foreach (clsVitalItems item in objVitalItems.lstVitals)
            {
                ListOfVital.Add(item);
            }
            foreach (clsVitalItems item in ListOfVital)
            {
                item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }
            if (ChartType.ItemsSource == null && ListOfVital != null)
            {
                ChartType.ItemsSource = ListOfVital;
            }
            ChartType.SelectionChanged += new SelectionChangedEventHandler(ChartType_SelectionChanged);
        }

        void ChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChartType.SelectedIndex = -1;
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {

            }
        }

        #endregion

        #region Chart

        SwivelAnimation objAnimation;
        List<clsVitalChartVO> VitalChartList = new List<clsVitalChartVO>();
        private void FillVitalChartData()
        {
            clsGetPatientVitalChartBizActionVO BizAction = new clsGetPatientVitalChartBizActionVO();
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.UnitID = CurrentVisit.UnitId;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient clientBizActionObjPatientVitalChartData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientBizActionObjPatientVitalChartData.ProcessCompleted += (sBizActionObjPatientVitalChartData, argsBizActionObjPatientVitalChartData) =>
            {
                VitalChartList.Clear();
                if (argsBizActionObjPatientVitalChartData.Result != null && ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).PatientVitalChartlst != null)
                {
                    VitalChartList = ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).PatientVitalChartlst;
                    var listDesc = from v in VitalChartList
                                   orderby v.Date descending
                                   select v;
                    foreach (var item in ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).PatientVitalChartlst)
                    {
                        if(this.CurrentVisit.OPDIPD)
                        item.Datetime = String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor.Trim() + " - " + CurrentVisit.DoctorSpecialization);
                    }
                    
                    PagedCollectionView pcvVitals = new PagedCollectionView(VitalChartList);
                    if (CurrentVisit.VisitTypeID==2)
                        if (this.CurrentVisit.OPDIPD)
                        {
                            pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                        }
                        else
                        {
                            pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                        }
                        else
                            pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));

                    dgPatientVitals.ItemsSource = pcvVitals;
                    ChartSelectionChange();
                }
            };
            clientBizActionObjPatientVitalChartData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientBizActionObjPatientVitalChartData.CloseAsync();

        }

        private void cmdVitalForward_Click(object sender, RoutedEventArgs e)
        {
            cmdVitalForward.IsEnabled = false;
            cmdVitalBackWord.IsEnabled = true;
            objAnimation.Invoke(RotationType.Forward);
            myChartAll.DataContext = VitalChartList;
        }

        private void cmdVitalBackWord_Click(object sender, RoutedEventArgs e)
        {
            cmdVitalForward.IsEnabled = true;
            cmdVitalBackWord.IsEnabled = false;
            objAnimation.Invoke(RotationType.Backward);
            myChartAll.DataContext = VitalChartList;
        }
        void ChartSelectionChange()
        {
            if (cmdVitalForward.IsEnabled == true)
            {
                dgPatientVitals.Columns[0].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[1].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[2].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[3].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[4].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[5].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[6].Visibility = Visibility.Visible;
                dgPatientVitals.Columns[7].Visibility = Visibility.Visible;
            }
            else
            {
                myChartAll.Visibility = Visibility.Visible;
                myChartAll.DataContext = VitalChartList;
            }

        }

        #endregion
        
        private void GetEMRDetails()
        {
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            clsGetVitalListDetailsBizActionVO BizAction = new clsGetVitalListDetailsBizActionVO();
            BizAction.PatientID = this.SelectedPatient.PatientId;
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.DoctorCode = CurrentVisit.DoctorCode;
            BizAction.PatientUnitID = this.SelectedPatient.patientUnitID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
            {
                if (argsBizActionObjPatientHistoryData.Error == null)
                {
                    if (argsBizActionObjPatientHistoryData.Result != null)
                    {
                        EmrDetails = ((clsGetVitalListDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).vitalListDetail;
                        //this for insert new vitals
                        EmrDetails = null;
                        if (EmrDetails == null)
                            EmrDetails = new List<clsEMRVitalsVO>();
                        FillPhysicalExam();
                    }
                }
                else
                {
                    Indicatior.Close();
                    ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientBizActionObjPatientHistoryData.CloseAsync();

        }

        private void FillPhysicalExam()
        {
            Indicatior.Show();
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                #region Physical Exam Details
                clsGetPatientPhysicalExamDetailsBizActionVO BizAction = new clsGetPatientPhysicalExamDetailsBizActionVO();
                BizAction.PatientID = this.SelectedPatient.PatientId;
                BizAction.PatientUnitID = this.SelectedPatient.patientUnitID;
                BizAction.TemplateID = TemplateID;
                BizAction.VisitID = CurrentVisit.ID;
                BizAction.UnitID = CurrentVisit.UnitId;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                BizAction.UnitID = CurrentVisit.UnitId;
                PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
                {
                    if (argsBizActionObjPatientHistoryData.Error == null)
                    {
                        if (argsBizActionObjPatientHistoryData.Result != null && ((clsGetPatientPhysicalExamDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse != null)
                        {
                            IsFirstTime = true;
                            this.SelectedFormStructure = ((clsGetPatientPhysicalExamDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse.XmlDeserialize<FormDetail>();

                            IsView = false;
                            GenratePreview();
                            MapRelations();
                            IsFirstTime = false;
                            GC.Collect();
                            this.Cursor = Cursors.Arrow;
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                            string strMsg = DefaultValues.ResourceManager.GetString("TemplateNotFound");
                            ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientBizActionObjPatientHistoryData.CloseAsync();

                #endregion
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        private void FillVitalDetails()
        {
            Indicatior.Show();
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                clsGetPatientVitalBizActionVO BizActionObj = new clsGetPatientVitalBizActionVO();
                BizActionObj.PatientID = this.SelectedPatient.PatientId;
                BizActionObj.TemplateID = TemplateID;
                BizActionObj.VisitID = CurrentVisit.ID;
                BizActionObj.IsOPDIPD = CurrentVisit.OPDIPD;
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientVitalBizActionVO)args.Result).VitalDetails != null)
                        {
                            PagedCollectionView pcvVitals = new PagedCollectionView(((clsGetPatientVitalBizActionVO)args.Result).VitalDetails);
                            pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                            Indicatior.Close();
                            dgPatientVitals.ItemsSource = null;
                            dgPatientVitals.ItemsSource = pcvVitals;
                            GC.Collect();
                            this.Cursor = Cursors.Arrow;
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
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
                        if ((((SectionDetail)target.Parent).Tab == "Consultation") || (((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
                        {
                            switch (target.DataType.Id)
                            {
                                case 1:
                                    tempStr = ((TextFieldSetting)target.Settings).Value;

                                    break;
                                case 2:
                                    if (((BooleanFieldSetting)target.Settings).Value && ((FrameworkElement)target.Control).Visibility == Visibility.Visible)
                                    {
                                        tempStr = target.Title;
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


                                case 14:

                                    break;
                                case 15:

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
                #region new added by harish
                ((Border)section.Children[1]).DataContext = PItem.ToolTipText;
                ((Border)section.Children[1]).MouseEnter += new MouseEventHandler(FormDesigner_MouseEnter);


                #endregion
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
                if (ApplicationConfiguration.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ApplicationConfiguration.IsHO == true)
                {
                    if (ApplicationConfiguration.AllowClinicalTransaction == false || ApplicationConfiguration.UnitID != CurrentVisit.UnitId)
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
                    TextBox tbl = new TextBox();
                    tbl.IsEnabled = false;
                    tbl.Text = ((FrameworkElement)sender).DataContext.ToString();
                    tbl.TextWrapping = TextWrapping.Wrap;
                    ((Border)((ScrollViewer)p.Child).Content).Child = tbl;
                    GeneralTransform gt = ((Border)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = gt.Transform(new Point(0, 0));
                    double controlTop = offset.Y + ((Border)sender).ActualHeight;
                    double controlLeft = offset.X;
                    ((ScrollViewer)p.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                    ((ScrollViewer)p.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop - 10;
                    p.VerticalOffset = controlTop;
                    p.HorizontalOffset = controlLeft;
                    p.IsOpen = true;
                }
            }
        }

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
                    if (PItem.IsToolTip)
                    {
                        Field.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }
                    Field.IsEnabled = WritePerm;

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
                                where (EDT.Description == PItem.Name
                                )
                                select EDT;
                    if (item1.ToList().Count > 0)
                    {
                        foreach (var item in item1)
                        {
                            if (item.Description == PItem.Name)
                            {
                                Field.Text = item.Value.ToString();
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
                        if (PItem.IsToolTip == true)
                        {
                            chk.MouseEnter += new MouseEventHandler(Field_MouseEnter);
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
                                        where (EDT.Description == PItem.Name
                                        )
                                        select EDT;

                        if (itemCheck.ToList().Count > 0)
                        {
                            foreach (var item in itemCheck)
                            {
                                if (item.Description == PItem.Name)
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
                        if (PItem.IsToolTip)
                        {
                            panel.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                        }
                        panel.DataContext = PItem;
                        panel.Orientation = Orientation.Horizontal;
                        RadioButton yes = new RadioButton();
                        RadioButton No = new RadioButton();
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

                    if (PItem.IsToolTip)
                    {
                        dtp.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }
                    dtp.IsEnabled = WritePerm;

                    dtp.DataContext = PItem;
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

                    var itemDate = from EDT in EmrDetails
                                   where (EDT.Description == PItem.Name
                                   )
                                   select EDT;

                    if (itemDate.ToList().Count > 0)
                    {
                        foreach (var item in itemDate)
                        {
                            if (item.Description == PItem.Name && item.Value != null)
                            {
                                try
                                {
                                    dtp.SelectedDate = Convert.ToDateTime(item.Value);
                                }
                                catch (Exception) { }
                            }
                        }
                    }

                    Grid.SetRow(dtp, Container.RowDefinitions.Count - 1);
                    Grid.SetColumn(dtp, 1);
                    PItem.Control = dtp;
                    if (IdDependentField)
                        FTitle.Visibility = dtp.Visibility = Visibility.Collapsed;

                    Container.Children.Add(dtp);
                    break;
                    #endregion

                case 5:
                    #region Case 5 - USE
                    StackPanel DecP = new StackPanel();
                    DecP.DataContext = PItem;
                    DecP.Orientation = Orientation.Horizontal;
                    TextBox DecField = new TextBox();
                    if (PItem.IsToolTip == true)
                    {
                        DecP.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }

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
                    }
                    if (PItem.IsRequired)
                    {
                        DecField.SetValidation(PItem.Title + " is required.");
                        DecField.RaiseValidationError();
                    }
                    if (IdDependentField)
                        FTitle.Visibility = DecP.Visibility = Visibility.Collapsed;
                    var itemNumeric = from EDT in EmrDetails
                                      where (EDT.Description == PItem.Name
                                      )
                                      select EDT;
                    if (itemNumeric.ToList().Count > 0)
                    {
                        foreach (var item in itemNumeric)
                        {
                            if (item.Description == PItem.Name)
                            {
                                DecField.Text = item.Value.ToString();
                            }
                        }
                    }
                    PItem.Control = DecP;
                    Container.Children.Add(DecP);
                    break;
                    #endregion
                case 6:
                    #region Case 6 Main -- Use
                    HyperlinkButton HyperBtn = new HyperlinkButton();
                    HyperBtn.VerticalAlignment = VerticalAlignment.Center;
                    HyperBtn.IsTabStop = false;
                    if (PItem.IsToolTip == true)
                    {
                        HyperBtn.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }

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
                        FTitle.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }
                    FTitle.DataContext = PItem;
                    break;
                    #endregion
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

                case 12:
                    #region Case 12 Main
                    ListOfCheckBoxesFieldSetting listSetting1 = ((ListOfCheckBoxesFieldSetting)PItem.Settings);
                    ComboBox lbList1 = new ComboBox();

                    lbList1.SelectionChanged += new SelectionChangedEventHandler(lbList1_SelectionChanged);
                    if (PItem.IsToolTip == true)
                    {
                        lbList1.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                    }
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

                        chk.DataContext = PItem;
                        chk.IsChecked = listSetting1.SelectedItems[j];

                        lbList1.Items.Add(chk);
                        j++;
                    }
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
                            var itemComboSingleSel = from EDT in EmrDetails
                                                     where (EDT.Description == PItem.Name
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

                                        Binding Sourcebinding = new Binding("Settings.ItemSource");
                                        Sourcebinding.Mode = BindingMode.OneWay;
                                        cmbList.SetBinding(ComboBox.ItemsSourceProperty, Sourcebinding);
                                        Binding SIbinding = new Binding("Settings.SelectedItem");
                                        SIbinding.Mode = BindingMode.TwoWay;
                                        cmbList.SetBinding(ComboBox.SelectedItemProperty, SIbinding);

                                        //if (itemComboSingleSel.ToList().Count > 0)
                                        //{
                                        //    foreach (var item in itemComboSingleSel)
                                        //    {
                                        //        if (item.ControlName == PItem.Name)
                                        //        {
                                        //            if (item.Value != null && item.Value != "")
                                        //            {
                                        //                try
                                        //                {

                                        //                    cmbList.SelectedItem = ((List<MasterListItem>)cmbList.ItemsSource).First(dd => dd.Description == item.Value);

                                        //                }
                                        //                catch (Exception) { }
                                        //            }
                                        //        }
                                        //    }
                                        //}
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
                                //if (itemComboSingleSel.ToList().Count > 0)
                                //{
                                //    foreach (var item in itemComboSingleSel)
                                //    {
                                //        if (item.ControlName == PItem.Name)
                                //        {
                                //            if (item.Value != null && item.Value != "")
                                //            {
                                //                try
                                //                {
                                //                    cmbList.SelectedItem = ((List<MasterListItem>)cmbList.ItemsSource).First(dd => dd.Description == item.Value);
                                //                }
                                //                catch (Exception) { }
                                //            }
                                //        }
                                //    }
                                //}
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
                                                           where (EDT.Description == PItem.Name
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


                                                //if (itemSelectSingle.ToList().Count > 0)
                                                //{
                                                //    foreach (var item in itemSelectSingle)
                                                //    {
                                                //        if (item.ControlName == PItem.Name)
                                                //        {
                                                //            if (item.Value != null && item.Value != "")
                                                //            {
                                                //                try
                                                //                {
                                                //                    lbListSingle.SelectedItem = ((List<MasterListItem>)lbListSingle.ItemsSource).First(dd => dd.Description == item.Value);
                                                //                }
                                                //                catch (Exception) { }
                                                //            }
                                                //        }
                                                //    }
                                                //}
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

                                        //if (itemSelectSingle.ToList().Count > 0)
                                        //{
                                        //    foreach (var item in itemSelectSingle)
                                        //    {
                                        //        if (item.ControlName == PItem.Name)
                                        //        {
                                        //            if (item.Value != null && item.Value != "")
                                        //            {
                                        //                try
                                        //                {
                                        //                    lbListSingle.SelectedItem = ((List<MasterListItem>)lbListSingle.ItemsSource).First(dd => dd.Description == item.Value);
                                        //                }
                                        //                catch (Exception) { }
                                        //            }
                                        //        }


                                        //    }
                                        //}
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
                                                           where (EDT.Description == PItem.Name
                                                           )
                                                           select EDT;
                                    /////////END///////////
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


                                                //if (itemMultiSelList.ToList().Count > 0)
                                                //{
                                                //    foreach (var item in itemMultiSelList)
                                                //    {
                                                //        if (item.ControlName == PItem.Name)
                                                //        {
                                                //            if (item.Value != null && item.Value != "")
                                                //            {
                                                //                try
                                                //                {
                                                //                    lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.Description == item.Value).Single());

                                                //                }
                                                //                catch (Exception) { }
                                                //            }
                                                //        }
                                                //    }
                                                //}
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

                                        //if (itemMultiSelList.ToList().Count > 0)
                                        //{
                                        //    foreach (var item in itemMultiSelList)
                                        //    {
                                        //        if (item.ControlName == PItem.Name)
                                        //        {
                                        //            if (item.Value != null && item.Value != "")
                                        //            {
                                        //                try
                                        //                {
                                        //                    lbList.SelectedItems.Add(AutolistSetting.ItemSource.Where(i => i.Description == item.Value).Single());
                                        //                }
                                        //                catch (Exception) { }
                                        //            }
                                        //        }
                                        //    }
                                        //}
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
                                                where (EDT.Description == PItem.Name
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
                                            //if (itemCheckList.ToList().Count > 0)
                                            //{
                                            //    foreach (var item in itemCheckList)
                                            //    {
                                            //        if (item.Value == AutolistSetting.ItemSource[k].Description)
                                            //        {
                                            //            AutolistSetting.ItemSource[k].Status = true;
                                            //            AutolistSetting.ItemSource[k].Selected = true;
                                            //            CLBCI.IsSelected = true;
                                            //        }
                                            //    }
                                            //}
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
                                    //if (itemCheckList.ToList().Count > 0)
                                    //{
                                    //    foreach (var item in itemCheckList)
                                    //    {
                                    //        if (item.Value == AutolistSetting.ItemSource[k].Description)
                                    //        {
                                    //            AutolistSetting.ItemSource[k].Status = true;
                                    //            AutolistSetting.ItemSource[k].Selected = true;
                                    //            CLBCI.IsSelected = true;
                                    //        }
                                    //    }
                                    //}
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
                                   where (EDT.Description == PItem.Name
                                   )
                                   select EDT;

                    if (itemTime.ToList().Count > 0)
                    {
                        foreach (var item in itemTime)
                        {
                            if (item.Description == PItem.Name)
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
                    ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
            }
        }

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
                    TextBox tbl = new TextBox();
                    tbl.IsEnabled = false;
                    tbl.Text = ((FieldDetail)((FrameworkElement)sender).DataContext).ToolTipText;
                    tbl.TextWrapping = TextWrapping.Wrap;

                    ((Border)((ScrollViewer)pf.Child).Content).Child = tbl;
                    GeneralTransform gt = ((FrameworkElement)sender).TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = gt.Transform(new Point(0, 0));
                    double controlTop = offset.Y + ((FrameworkElement)sender).ActualHeight;
                    double controlLeft = offset.X;
                    ((ScrollViewer)pf.Child).MaxWidth = this.ActualWidth - controlLeft + 10;
                    ((ScrollViewer)pf.Child).MaxHeight = (Application.Current.RootVisual as UIElement).DesiredSize.Height - controlTop;
                    pf.VerticalOffset = controlTop;
                    pf.HorizontalOffset = controlLeft;
                    pf.IsOpen = true;
                }
            }
        }

        void lbList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ComboBox)sender).SelectedItem = null;
        }

        void chk_Unchecked(object sender, RoutedEventArgs e)
        {
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

        void dtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
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
            grdAnti.ColumnDefinitions.Add(c);

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
            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);
            grdAnti.Children.Add(txtName);

            TextBox txtRoute = new TextBox();
            txtRoute.Background = a;
            txtRoute.IsReadOnly = true;
            txtRoute.Text = "Route";
            Grid.SetRow(txtRoute, 0);
            Grid.SetColumn(txtRoute, 1);
            grdAnti.Children.Add(txtRoute);

            TextBox txtFrequency = new TextBox();
            txtFrequency.Background = a;
            txtFrequency.IsReadOnly = true;
            txtFrequency.Text = "Frequency";
            Grid.SetRow(txtFrequency, 0);
            Grid.SetColumn(txtFrequency, 2);
            grdAnti.Children.Add(txtFrequency);

            TextBox txtDays = new TextBox();
            txtDays.Background = a;
            txtDays.IsReadOnly = true;
            txtDays.Text = "Days";
            Grid.SetRow(txtDays, 0);
            Grid.SetColumn(txtDays, 3);
            grdAnti.Children.Add(txtDays);

            TextBox txtQty = new TextBox();
            txtQty.Background = a;
            txtQty.IsReadOnly = true;
            txtQty.Text = "Quantity";
            Grid.SetRow(txtQty, 0);
            Grid.SetColumn(txtQty, 4);
            grdAnti.Children.Add(txtQty);

            return grdAnti;
        }

        private Grid GetGridSchema(Grid MainGrid)
        {
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



            lst = null;
            Grid g2 = GetGridHeading();
            //Added at razi
            g2.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntibiotics");
            lst = null;
            Grid g3 = GetGridHeading();

            g3.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntipyretic");

            lst = null;
            Grid g4 = GetGridHeading();
            //Added at razi
            g4.Visibility = Visibility.Collapsed;
            lst = (ListBox)Form.FindName("ManagementAntispasmodic");

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
            HtmlPage.Window.Invoke("alertText", new string[] { ((HyperlinkButton)sender).TargetName });
        }

        void DecField_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        void DecField_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Checking for Valid Decimal Number
            if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsPositiveDoubleValid()) // !((TextBox)sender).Text.IsItDecimal() &&
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

        //void TextField_TextChanged(object sender, RoutedEventArgs e)
        //{

        //}

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
                    TextBox txtDecimal = sender as TextBox;
                    if (txtDecimal.Text != String.Empty && txtDecimal.Text.Trim() != String.Empty)
                    {
                        FieldDetail objFieldDetails = txtDecimal.DataContext as FieldDetail;
                        DecimalFieldSetting objFDSetting = objFieldDetails.Settings as DecimalFieldSetting;
                        if (objFieldDetails != null)
                        {
                            decimal dblValue = Convert.ToDecimal(txtDecimal.Text);
                            string strMEssage = string.Empty;
                            switch (objFieldDetails.Name)
                            {
                                case "Pulse":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        // High Pulse 
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighPulseRate_Msg") + objFDSetting.MaxValue.ToString());
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        // Below Normal Pulse.
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowPulseRate_Msg") + objFDSetting.MinValue.ToString());
                                    }
                                    break;
                                case "Systolic BP":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        // High Pulse 
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighSystolicBP_Msg") + objFDSetting.MaxValue);
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        // Below Normal Pulse.
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowSystolicBP_Msg") + objFDSetting.MinValue.ToString());
                                    }
                                    break;
                                case "Diastolic BP":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighDiastolicBP_Msg") + objFDSetting.MaxValue.ToString());
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowDiastolicBP_Msg") + objFDSetting.MinValue.ToString());
                                    }

                                    break;
                                case "Temperature":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighTemp_Msg") + objFDSetting.MaxValue.ToString() + " C");
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowTemp_Msg") + objFDSetting.MinValue.ToString() + " C");
                                    }
                                    break;
                                case "Respiratory Rate":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighRR_Msg") + objFDSetting.MaxValue);
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowRR_Msg") + objFDSetting.MinValue.ToString());
                                    }
                                    break;
                                case "Height":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighHeight_Msg") + objFDSetting.MaxValue);
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowHeight_Msg") + objFDSetting.MinValue.ToString());
                                    }
                                    break;
                                case "Weight":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighWeight_Msg") + objFDSetting.MaxValue);
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowWeight_Msg") + objFDSetting.MinValue.ToString());
                                    }
                                    break;
                                case "O2":
                                    if (dblValue > objFDSetting.MaxValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("HighO2_Msg") + objFDSetting.MaxValue.ToString());
                                    }
                                    else if (dblValue < objFDSetting.MinValue)
                                    {
                                        strMEssage = String.Format(DefaultValues.ResourceManager.GetString("LowO2_Msg") + objFDSetting.MaxValue.ToString());
                                    }
                                    break;
                            }
                            if (strMEssage != String.Empty)
                            {
                                ShowMessageBox(strMEssage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //msgW.Show();
            SaveFieldWiseData();
            SaveVitalDetails();
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveFieldWiseData();
                SaveVitalDetails();
            }
        }

        private void SaveVitalDetails()
        {
            WaitIndicator Indicator = new WaitIndicator();
            try
            {
                if (VitalList != null && VitalList.Count > 0)
                {
                    clsAddUpdateDeleteVitalDetailsBizActionVO BizActionVO = new clsAddUpdateDeleteVitalDetailsBizActionVO();
                    BizActionVO.PatientID = CurrentVisit.PatientId;
                    BizActionVO.VisitID = CurrentVisit.ID;
                    //BizActionVO.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionVO.DoctorID = CurrentVisit.DoctorID;
                    BizActionVO.PatientVitalDetails = VitalList;
                    BizActionVO.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizActionVO.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionVO.UnitID = CurrentVisit.UnitId;
                    Indicator.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateDeleteVitalDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                Indicator.Close();
                                //frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
                                //ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                //DisplayPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayPatientDetails;
                                //Grid Patient = winIPDPatientDetails.FindName("Patient") as Grid;
                                //clsPatientConsoleHeaderVO Test = Patient.DataContext as clsPatientConsoleHeaderVO;
                                //Test.BMI = 100.00;
                                //Patient.DataContext = null;
                                //Patient.DataContext = winIPDPatientDetails.MasterList;
                                FillVitalChartData();
                                GetEMRDetails();
                                
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                //NavigateToNextMenu();
                            }
                            else
                            {
                                Indicatior.Close();
                                ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            Indicator.Close();
                            ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    };
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    string strEmptyMsg = DefaultValues.ResourceManager.GetString("VitalEmpty_Msg");
                    ShowMessageBox(strEmptyMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                Indicator.Close();
                ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }

        private void SaveFieldWiseData()
        {
            #region Consultation
            if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null)
            {
                FieldDetail target = null;
                VitalList = new List<clsEMRVitalsVO>();

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
                            int tempVitalId = 0;
                            string tempVitalUnit = string.Empty;
                            switch (target.Name)
                            {
                                case "Pulse":
                                    tempVitalId = 1;
                                    break;
                                case "Systolic BP":
                                    tempVitalId = 4;
                                    break;
                                case "Diastolic BP":
                                    tempVitalId = 5;
                                    break;
                                case "Temperature":
                                    tempVitalId = 6;
                                    break;
                                case "Respiratory Rate":
                                    tempVitalId = 16;
                                    break;
                                case "Height":
                                    tempVitalId = 2;
                                    break;
                                case "Weight":
                                    tempVitalId = 3;
                                    break;
                                case "RBS":
                                    tempVitalId = 11;
                                    break;
                                case "O2":
                                    tempVitalId = 12;
                                    break;
                                case "Total Cholesterol":
                                    tempVitalId = 14;
                                    break;
                                case "Head Circumference":
                                    tempVitalId = 17;
                                    break;
                            }
                            DateTime? tempDt = null;
                            if ((((SectionDetail)target.Parent).Tab == "Consultation") || (((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
                            {
                                tempControlName = target.Name;
                                tempControlCaption = target.Title;
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
                                        tempControlType = "Time";
                                        tempControlValue = Convert.ToDateTime(((TimeFieldSetting)target.Settings).Time).ToShortTimeString();
                                        break;
                                    case 5:
                                        tempControlType = "Numeric";
                                        if (((DecimalFieldSetting)target.Settings).Value != null && ((DecimalFieldSetting)target.Settings).Value > 0)
                                        {
                                            tempControlValue = ((DecimalFieldSetting)target.Settings).Value.ToString();
                                            tempVitalUnit = ((DecimalFieldSetting)target.Settings).Unit.ToString();
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

                                if (!String.IsNullOrEmpty(tempControlType) && !String.IsNullOrEmpty(tempControlValue))
                                {
                                    clsEMRVitalsVO Vital = new clsEMRVitalsVO();
                                    Vital.Description = tempControlCaption;
                                    Vital.ID = tempVitalId;
                                    Vital.Unit = tempVitalUnit;
                                    if (target.DataType.Id == 5)
                                        Vital.Value = Convert.ToDouble(tempControlValue);
                                    Vital.ValueDescription = String.Format("{0:0.00}", Convert.ToDouble(tempControlValue));
                                    VitalList.Add(Vital);
                                }
                            }
                        }
                    }
                }

            }
            #endregion
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            NavigateToDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigateToDashBoard();
            }
        }

        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chkVital = sender as CheckBox;
            string strVital = chkVital.Content.ToString();
            if (chkVital.IsChecked == true)
            {
                switch (strVital)
                {
                    case "ALL":
                        ListOfVital.ToList().ForEach(z => z.IsChecked = true);
                        VisibleAllChartSeries(0);
                        break;
                    case "BMI":
                        (myChartAll.Series[2] as LineSeries).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[1].Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[2] as LegendItem).Visibility = Visibility.Visible;
                        break;
                    case "Pulse":
                        (myChartAll.Series[3] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[3] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[2].Visibility = Visibility.Visible;
                        break;
                    case "Systolic BP":
                        (myChartAll.Series[4] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[4] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[3].Visibility = Visibility.Visible;
                        break;
                    case "Diastoliic BP":
                        (myChartAll.Series[5] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[5] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[4].Visibility = Visibility.Visible;
                        break;
                    case "Temperature":
                        (myChartAll.Series[6] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[6] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[5].Visibility = Visibility.Visible;
                        break;
                    case "Height":
                        (myChartAll.Series[0] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[0] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[7].Visibility = Visibility.Visible;
                        break;
                    case "Weight":
                        (myChartAll.Series[1] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[1] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[8].Visibility = Visibility.Visible;
                        break;
                    //case "O2":
                    //    (myChartAll.Series[8] as LineSeries).Visibility = Visibility.Visible;
                    //    (myChartAll.LegendItems[8] as LegendItem).Visibility = Visibility.Visible;
                    //    dgPatientVitals.Columns[7].Visibility = Visibility.Visible;
                    //    break;
                    case "RR":
                        (myChartAll.Series[7] as LineSeries).Visibility = Visibility.Visible;
                        (myChartAll.LegendItems[7] as LegendItem).Visibility = Visibility.Visible;
                        dgPatientVitals.Columns[6].Visibility = Visibility.Visible;
                        break;
                    //case "Head Circumference":
                    //    (myChartAll.Series[7] as LineSeries).Visibility = Visibility.Visible;
                    //    (myChartAll.LegendItems[7] as LegendItem).Visibility = Visibility.Visible;
                    //    dgPatientVitals.Columns[8].Visibility = Visibility.Visible;
                    //    break;
                }
            }

            else
            {
                switch (strVital)
                {
                    case "ALL":
                        ListOfVital.ToList().ForEach(z => z.IsChecked = false);
                        VisibleAllChartSeries(1);
                        break;
                    case "BMI":
                        (myChartAll.Series[2] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[2] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[1].Visibility = Visibility.Collapsed;
                        break;
                    case "Pulse":
                        (myChartAll.Series[3] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[3] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[2].Visibility = Visibility.Collapsed;
                        break;
                    case "Systolic BP":
                        (myChartAll.Series[4] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[4] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[3].Visibility = Visibility.Collapsed;
                        break;
                    case "Diastoliic BP":
                        (myChartAll.Series[5] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[5] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[4].Visibility = Visibility.Collapsed;
                        break;
                    case "Temperature":
                        (myChartAll.Series[6] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[6] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[5].Visibility = Visibility.Collapsed;
                        break;
                    case "Height":
                        (myChartAll.Series[0] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[0] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[7].Visibility = Visibility.Collapsed;
                        break;
                    case "Weight":
                        (myChartAll.Series[1] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[1] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[8].Visibility = Visibility.Collapsed;
                        break;
                    //case "O2":
                    //    (myChartAll.Series[8] as LineSeries).Visibility = Visibility.Collapsed;
                    //    (myChartAll.LegendItems[8] as LegendItem).Visibility = Visibility.Collapsed;
                    //    dgPatientVitals.Columns[7].Visibility = Visibility.Collapsed;
                    //    break;
                    case "RR":
                        (myChartAll.Series[7] as LineSeries).Visibility = Visibility.Collapsed;
                        (myChartAll.LegendItems[7] as LegendItem).Visibility = Visibility.Collapsed;
                        dgPatientVitals.Columns[6].Visibility = Visibility.Collapsed;
                        break;
                    //case "Head Circumference":
                    //    (myChartAll.Series[7] as LineSeries).Visibility = Visibility.Collapsed;
                    //    (myChartAll.LegendItems[7] as LegendItem).Visibility = Visibility.Collapsed;
                    //    dgPatientVitals.Columns[8].Visibility = Visibility.Collapsed;
                    //    break;
                }
            }

            if (ListOfVital.Count == ListOfVital.Where(z => z.IsChecked == true).Count())
            {
                ListOfVital[0].IsChecked = true;
            }
            else
                ListOfVital[0].IsChecked = false;
            ChartType.ItemsSource = null;
            ChartType.ItemsSource = ListOfVital;
        }

        private void VisibleAllChartSeries(int iVisibility)
        {
            Visibility VisibilityStatus = (System.Windows.Visibility)iVisibility;
            (myChartAll.Series[2] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[2] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[1].Visibility = VisibilityStatus;
            (myChartAll.Series[3] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[3] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[2].Visibility = VisibilityStatus;
            (myChartAll.Series[4] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[4] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[3].Visibility = VisibilityStatus;
            (myChartAll.Series[5] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[5] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[4].Visibility = VisibilityStatus;
            (myChartAll.Series[6] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[6] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[5].Visibility = VisibilityStatus;
            (myChartAll.Series[0] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[0] as LegendItem).Visibility = VisibilityStatus;
            //dgPatientVitals.Columns[8].Visibility = VisibilityStatus;
            (myChartAll.Series[1] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[1] as LegendItem).Visibility = VisibilityStatus;
            //dgPatientVitals.Columns[9].Visibility = VisibilityStatus;

            //(myChartAll.Series[7] as LineSeries).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[6].Visibility = VisibilityStatus;
            //(myChartAll.Series[8] as LineSeries).Visibility = VisibilityStatus;
            //(myChartAll.LegendItems[7] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[7].Visibility = VisibilityStatus;
            //(myChartAll.LegendItems[8] as LegendItem).Visibility = VisibilityStatus;

            (myChartAll.Series[7] as LineSeries).Visibility = VisibilityStatus;
            (myChartAll.LegendItems[7] as LegendItem).Visibility = VisibilityStatus;
            dgPatientVitals.Columns[8].Visibility = VisibilityStatus;
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            //if (this.CurrentVisit.VisitTypeID==2)
            //{
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //}
            //else
            //{
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            //}
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    (tvEMR.Items[0] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }
    }
    public class clsVitalItems : INotifyPropertyChanged
    {
        public string VitalDescription { get; set; }
        public int VitalID { get; set; }
        public List<clsVitalItems> lstVitals { get; set; }
        public Boolean IsChecked { get; set; }

        public void GetVitalList()
        {
            lstVitals = new List<clsVitalItems>();
            lstVitals.Add(new clsVitalItems { VitalDescription = "ALL", VitalID = 0, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "BMI", VitalID = 1, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Pulse", VitalID = 2, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Systolic BP", VitalID = 3, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Diastoliic BP", VitalID = 4, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Temperature", VitalID = 5, IsChecked = true });
            //lstVitals.Add(new clsVitalItems { VitalDescription = "Waist Girth", VitalID = 6, IsChecked = true });
            //lstVitals.Add(new clsVitalItems { VitalDescription = "Hip Girth", VitalID = 7, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "RR", VitalID = 8, IsChecked = true });
            //lstVitals.Add(new clsVitalItems { VitalDescription = "O2", VitalID = 9, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Height", VitalID = 10, IsChecked = true });
            lstVitals.Add(new clsVitalItems { VitalDescription = "Weight", VitalID = 11, IsChecked = true });
           //lstVitals.Add(new clsVitalItems { VitalDescription = "Head Circumference", VitalID = 17, IsChecked = true });
            //lstVitals.Add(new clsVitalItems { VitalDescription = "Random Blood Sugar", VitalID = 12, IsChecked = true });
            //lstVitals.Add(new clsVitalItems { VitalDescription = "Fasting Sugar", VitalID = 13, IsChecked = true });
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}

