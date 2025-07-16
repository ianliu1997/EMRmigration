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
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.RSIJ;

namespace EMR
{
    public partial class frmIPDEMRFamilyHistory : UserControl
    {
        #region Data Member
        public long TemplateID { get; set; }
        public clsVisitVO CurrentVisit { get; set; }
        string log = string.Empty;
        string textBefore = string.Empty;
        int selectionStart = 0;
        int selectionLength = 0;
        WaitIndicator Indicatior = new WaitIndicator();
        public Patient SelectedPatient { get; set; }
        public FormDetail SelectedFormStructure { get; set; }
        public string SelectedUser { get; set; }
        public bool IsView = false;
        bool IsFirstTime = false;
        public List<clsPatientEMRDetailsVO> listPatientEMRDetails { get; set; }
        System.Windows.Controls.Primitives.Popup p = null;
        System.Windows.Controls.Primitives.Popup pf = null;
        public clsPCRVO objPCR { get; set; }
        public bool IsEnabledControl = false;
        bool WritePerm = true;
        List<clsPatientEMRDetailsVO> EmrDetails = new List<clsPatientEMRDetailsVO>();
        string strErrMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
        #endregion

        public frmIPDEMRFamilyHistory()
        {
            InitializeComponent();
        }

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
            listPatientEMRDetails = new List<clsPatientEMRDetailsVO>();
            if (SelectedFormStructure == null)
            {
                GetEMRDetails();
            }
            else
            {
                GenratePreview();
                MapRelations();
            }
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnabledControl = false;
            }
            else
            {
                spSpecDoctor.Visibility = Visibility.Visible;
            }
                FillSpecialization();
                FillDoctor();
            cmdSave.IsEnabled = IsEnabledControl;
        }
        #endregion

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor()
        {
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();
            //if (cmbSpecialization.SelectedItem != null)
            //{
            //    BizAction.IsForReferral = true;
            //    BizAction.SpecialCode = sDeptCode;
            //}
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //        cmbDoctor.ItemsSource = null;
            //        cmbDoctor.ItemsSource = objList;
            //        cmbDoctor.SelectedItem = objList[0];
            //        if (this.DataContext != null)
            //        {
            //            cmbDoctor.SelectedValue = objList[0].ID;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
            //                cmbDoctor.IsEnabled = false;
            //            }
            //            else
            //                cmbDoctor.SelectedValue = "0";
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            //try
            //{
            //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            //    BizAction.MasterTable = MasterTableNameList.SPESIAL;
            //    BizAction.CodeColumn = "KDSPESIAL";
            //    BizAction.DescriptionColumn = "NMSPESIAL";
            //    BizAction.MasterList = new List<MasterListItem>();
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null && arg.Result != null)
            //        {
            //            List<MasterListItem> objList = new List<MasterListItem>();
            //            objList.Add(new MasterListItem("0", "-- Select --"));
            //            objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
            //            cmbSpecialization.ItemsSource = null;
            //            cmbSpecialization.ItemsSource = objList;
            //            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            //            {
            //                string sSpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
            //                cmbSpecialization.SelectedItem = objList.Where(z => z.Code == sSpecCode).FirstOrDefault();
            //                cmbSpecialization.IsEnabled = false;
            //            }
            //            else
            //            {
            //                cmbSpecialization.SelectedItem = objList[0];
            //            }
            //        }
            //    };
            //    client.ProcessAsync(BizAction, new clsUserVO());
            //}
            //catch (Exception)
            //{
            //}
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        #endregion

        //private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (cmbSpecialization.SelectedItem != null && ((MasterListItem)cmbSpecialization.SelectedItem).Code != "0")
        //            FillDoctor(((MasterListItem)cmbSpecialization.SelectedItem).Code);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void GetEMRDetails()
        {
            Indicatior.Show();
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            clsGetPatientEMRDetailsBizActionVO BizAction = new clsGetPatientEMRDetailsBizActionVO();
            BizAction.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
            BizAction.PatientID = this.SelectedPatient.PatientId;
            BizAction.PatientUnitID = this.SelectedPatient.patientUnitID;
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.Tab = "History";
            BizAction.TemplateID = this.TemplateID;
            if (IsEnabledControl == true)
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            else
                BizAction.UnitID = 0;
            PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
            {
                if (argsBizActionObjPatientHistoryData.Error == null)
                {
                    if (argsBizActionObjPatientHistoryData.Result != null)
                    {
                        EmrDetails = ((clsGetPatientEMRDetailsBizActionVO)argsBizActionObjPatientHistoryData.Result).EMRDetailsList;
                        FillHistory();
                    }
                    else
                    {
                        FillHistory();
                    }
                }
                else
                {
                    Indicatior.Close();
                    ShowMessageBox(strErrMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                }
            };
            clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientBizActionObjPatientHistoryData.CloseAsync();
        }

        private void FillHistory()
        {
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                clsGetPatientPatientHistoryDataBizActionVO BizActionObjPatientHistoryData = new clsGetPatientPatientHistoryDataBizActionVO();
                BizActionObjPatientHistoryData.PatientID = this.SelectedPatient.PatientId;
                BizActionObjPatientHistoryData.PatientUnitID = this.SelectedPatient.patientUnitID;
                BizActionObjPatientHistoryData.VisitID = CurrentVisit.ID;
                BizActionObjPatientHistoryData.UnitID = CurrentVisit.UnitId;
                BizActionObjPatientHistoryData.TemplateID = this.TemplateID;

                PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
                {
                    if (argsBizActionObjPatientHistoryData.Error == null && argsBizActionObjPatientHistoryData.Result != null)
                    {
                        if (((clsGetPatientPatientHistoryDataBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse != null)
                        {
                            this.SelectedFormStructure = ((clsGetPatientPatientHistoryDataBizActionVO)argsBizActionObjPatientHistoryData.Result).TemplateByNurse.XmlDeserialize<FormDetail>();
                            IsFirstTime = true;
                            GenratePreview();
                            MapRelations();
                            IsFirstTime = false;
                            //IsEditRecord = ((clsGetPatientPatientHistoryDataBizActionVO)argsBizActionObjPatientHistoryData.Result).IsUpdated;
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
                        ShowMessageBox(strErrMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                clientBizActionObjPatientHistoryData.ProcessAsync(BizActionObjPatientHistoryData, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientBizActionObjPatientHistoryData.CloseAsync();

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
                        if ((((SectionDetail)target.Parent).Tab == "History") || (((SectionDetail)target.Parent).Tab == "Follow Up Consultation") || (((SectionDetail)target.Parent).Tab == "Medication"))
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


                if (PItem.Tab == "History")
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

                if (CurrentVisit.VisitTypeID != 1 && PItem.Tab == "History")
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
                    Field.TextWrapping = TextWrapping.Wrap;
                    Field.MaxLength = 500;
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
                        var itemBoolean = (from EDT in EmrDetails
                                           where (EDT.ControlName == PItem.Name)
                                           select EDT).FirstOrDefault();
                        if (itemBoolean != null)
                            ((BooleanFieldSetting)PItem.Settings).Value = Convert.ToBoolean(itemBoolean.Value);

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
                                   where (EDT.ControlName == PItem.Name
                                   )
                                   select EDT;

                    if (itemDate.ToList().Count > 0)
                    {
                        foreach (var item in itemDate)
                        {
                            if (item.ControlName == PItem.Name && item.Value != null)
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
                                    if (PItem.IsToolTip)
                                    {
                                        cmbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
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
                            if (PItem.IsToolTip == true)
                            {
                                lbList.MouseEnter += new MouseEventHandler(Field_MouseEnter);
                            }
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
                                                lbList.SelectedItems.Add(listSetting.ItemSource.Where(i => i.Title == item.Value).Single());
                                            }
                                            catch (Exception) { }
                                        }
                                    }
                                }
                            }
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
                                                           where (EDT.ControlName == PItem.Name
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
            SaveHistory();
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveHistory();
            }
            else
            {
                EMR.frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
                TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
                TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
                clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
                if (SelectedItem.HasItems == true)
                {
                    (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
                }
                else if (objMenu.ParentId == 12)
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

        #region Private Methods

        private void SaveFieldWiseData()
        {
            string Tab = string.Empty;
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

                            tempControlName = target.Name;
                            tempControlCaption = target.Title;
                            Tab = "History";
                            switch (target.DataType.Id)
                            {
                                #region History
                                case 1:
                                    tempControlType = "Text";
                                    tempControlValue = ((TextFieldSetting)target.Settings).Value;
                                    break;
                                case 2:
                                    if ((FrameworkElement)target.Control != null)
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
                                    #region LookUp
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
                                    #endregion
                                #endregion
                            }

                            if (tempControlType != null)
                            {
                                clsPatientEMRDetailsVO objPatEMRDetail = new clsPatientEMRDetailsVO();
                                objPatEMRDetail.ControlCaption = tempControlCaption;
                                objPatEMRDetail.ControlName = tempControlName;
                                objPatEMRDetail.ControlType = tempControlType;
                                objPatEMRDetail.Value = tempControlValue;
                                objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;

                                listPatientEMRDetails.Add(objPatEMRDetail);
                            }
                            else
                            {
                                clsPatientEMRDetailsVO objPatEMRDetail = null;
                                switch (target.DataType.Id)
                                {
                                    case 4:
                                        #region 4
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
                                                        objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                        if (listSetting.SelectedItem != null)
                                                            objPatEMRDetail.Value = ((DynamicListItem)listSetting.SelectedItem).Title;
                                                        else
                                                            objPatEMRDetail.Value = null;
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
                                                            objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                            objPatEMRDetail.Value = ((DynamicListItem)listSetting.SelectedItem).Title;
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
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.Value = null;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                                break;
                                        }
                                        break;
                                        #endregion
                                    case 12:
                                        #region 12
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
                                            listPatientEMRDetails.Add(objPatEMRDetail);
                                        }
                                        break;
                                        #endregion
                                    case 13:
                                        #region 13
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
                                                listPatientEMRDetails.Add(objPatEMRDetail);
                                                break;
                                            case AutoListControlType.ListBox:
                                                switch (AutolistSetting.ChoiceMode)
                                                {
                                                    case SelectionMode.Single:
                                                        objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                        objPatEMRDetail.ControlCaption = target.Title;
                                                        objPatEMRDetail.ControlName = target.Name;
                                                        objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                        objPatEMRDetail.ControlType = "AutomatedList - List Box - SigleSelect";
                                                        if (AutolistSetting.SelectedItem != null)
                                                        {
                                                            objPatEMRDetail.Value = ((MasterListItem)AutolistSetting.SelectedItem).Description.ToString();
                                                        }
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
                                                                objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                                objPatEMRDetail.ControlType = "AutomatedList - List Box - MultiSelect";
                                                                objPatEMRDetail.Value = obList.Description.ToString();
                                                                listPatientEMRDetails.Add(objPatEMRDetail);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                            objPatEMRDetail.ControlCaption = target.Title;
                                                            objPatEMRDetail.ControlName = target.Name;
                                                            objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                            objPatEMRDetail.ControlType = "AutomatedList - List Box - MultiSelect";
                                                            objPatEMRDetail.Value = null;
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
                                                        objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                        objPatEMRDetail.ControlType = "AutomatedList - CheckList Box";
                                                        objPatEMRDetail.Value = ((MasterListItem)AutolistSetting.ItemSource[k]).Description.ToString();
                                                        listPatientEMRDetails.Add(objPatEMRDetail);
                                                    }
                                                }
                                                if (AutolistSetting.ItemSource.Count == 0)
                                                {
                                                    objPatEMRDetail = new clsPatientEMRDetailsVO();
                                                    objPatEMRDetail.ControlCaption = target.Title;
                                                    objPatEMRDetail.ControlName = target.Name;
                                                    objPatEMRDetail.Tab = ((SectionDetail)target.Parent).Tab;
                                                    objPatEMRDetail.ControlType = "AutomatedList - CheckList Box";
                                                    objPatEMRDetail.Value = null;
                                                    listPatientEMRDetails.Add(objPatEMRDetail);
                                                }
                                                break;
                                        }
                                        break;
                                        #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveHistory()
        {
            Indicatior.Show();
            try
            {
                string msgText = "Palash";

                listPatientEMRDetails.Clear();
                SaveFieldWiseData();
                clsAddUpdatePatientHistoryDataAndDetailsBizActionVO BizActionObjPatientEMRData = new clsAddUpdatePatientHistoryDataAndDetailsBizActionVO();
                BizActionObjPatientEMRData.IsOPDIPD = CurrentVisit.OPDIPD;
                BizActionObjPatientEMRData.PatientID = SelectedPatient.PatientId;
                BizActionObjPatientEMRData.PatientUnitID = SelectedPatient.patientUnitID;
                BizActionObjPatientEMRData.VisitID = CurrentVisit.ID;
                BizActionObjPatientEMRData.UnitID = CurrentVisit.UnitId;
                BizActionObjPatientEMRData.DoctorID = CurrentVisit.DoctorID;
                BizActionObjPatientEMRData.DoctorCode = CurrentVisit.DoctorCode;
                BizActionObjPatientEMRData.TemplateID = this.TemplateID;
                BizActionObjPatientEMRData.FalgForAddUpdate = !IsFirstTime;
                BizActionObjPatientEMRData.IsOPDIPD = CurrentVisit.OPDIPD;
                BizActionObjPatientEMRData.Takenby = ((IApplicationConfiguration)App.Current).CurrentUser.ID; 
                clsPatientEMRDataVO OBj = new clsPatientEMRDataVO();
                OBj.PatientID = SelectedPatient.PatientId;
                OBj.PatientUnitID = SelectedPatient.patientUnitID;
                OBj.SavedBy = SelectedUser;
                OBj.VisitID = CurrentVisit.ID;
                OBj.UnitId = CurrentVisit.UnitId;
                OBj.Status = true;
                OBj.DoctorCode = CurrentVisit.DoctorCode;
                OBj.DoctorID = CurrentVisit.DoctorID;
                BizActionObjPatientEMRData.PatientEMRData = OBj;
                if (listPatientEMRDetails != null)
                    BizActionObjPatientEMRData.PatientHistoryDetailsList = listPatientEMRDetails;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient clientObjPatientEMRData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientObjPatientEMRData.ProcessCompleted += (sObjPatientEMRData, argsObjPatientEMRData) =>
                {
                    if (argsObjPatientEMRData.Error == null)
                    {
                        Indicatior.Close();
                        if (argsObjPatientEMRData.Result != null)
                        {
                            if (((clsAddUpdatePatientHistoryDataAndDetailsBizActionVO)argsObjPatientEMRData.Result).SuccessStatus == 1)
                            {

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                //string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //new MessageBoxControl.MessageBoxChildWindow(msgText, strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                //{
                                //    this.Content = null;
                                //    NavigateToNextMenu();
                                //};
                                //msgW1.Show();
                               // NavigateToNextMenu();
                            }
                            else
                            {
                                ShowMessageBox(strErrMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            ShowMessageBox(strErrMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        ShowMessageBox(strErrMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                clientObjPatientEMRData.ProcessAsync(BizActionObjPatientEMRData, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientObjPatientEMRData.CloseAsync();

            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            //if (this.CurrentVisit.VisitTypeID == 2)
            //{
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //}
            //else
            //{
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            //}
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "NST IPD EMR")
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

        #endregion

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
    }
}
