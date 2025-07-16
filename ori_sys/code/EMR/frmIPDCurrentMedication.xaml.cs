using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.RSIJ;

namespace EMR
{
    public partial class frmIPDCurrentMedication : UserControl
    {
        #region Data Members
        ObservableCollection<clsPatientPrescriptionDetailVO> CurrentMedicationList;
        DateConverter dateConverter;
        List<clsPatientPrescriptionDetailVO> DeletedCurrentMedicationList;
        List<FrequencyMaster> FreqList = new List<FrequencyMaster>();
        List<MasterListItem> ItemList = new List<MasterListItem>();
        List<MasterListItem> RouteList = new List<MasterListItem>();
        List<MasterListItem> InstructionList = new List<MasterListItem>();
        WaitIndicator Indicatior = new WaitIndicator();
        public clsVisitVO CurrentVisit { get; set; }
        public bool IsEnabledControl = false;
        clsPatientPrescriptionDetailVO SelectedDrug;
        AutoCompleteComboBox cmbBox;
        TextBox txt;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataList { get; private set; }
        #endregion

        #region Property
        ObservableCollection<clsPatientPrescriptionDetailVO> PastMedicationList { get; set; }
        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }

        #endregion

        public frmIPDCurrentMedication()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        #region Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dateConverter = new DateConverter();
            if (CurrentMedicationList == null)
                CurrentMedicationList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
            DeletedCurrentMedicationList = new List<clsPatientPrescriptionDetailVO>();
            FillRouteList();
            FetchPreviousMedications();
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnabledControl = false;
            }
            else
            {
                if (CurrentVisit.ISIPDDischarge)
                {
                    this.IsEnabledControl = false;
                }
                else
                {
                    this.IsEnabledControl = true;
                }
                spSpecDoctor.Visibility = Visibility.Visible;
            }
            FillSpecialization();
            FillDoctor();
            cmdSave.IsEnabled = IsEnabledControl;
            lnkAddDrugList.IsEnabled = IsEnabledControl;
            lnkAddDrugFromPrecList.IsEnabled = IsEnabledControl;
            dgDrugList.IsReadOnly = !IsEnabledControl;
            btnOtherDrug.IsEnabled = IsEnabledControl;
        }
        #endregion

        #region Private Methods

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbDoctor.ItemsSource = null;
            cmbDoctor.ItemsSource = objList;
            cmbDoctor.SelectedItem = objList[0];
            cmbDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        #endregion

        public Boolean ValidateSave()
        {
            bool blnIsValidate = true;
            if (dgDrugList.ItemsSource == null || CurrentMedicationList == null || CurrentMedicationList.Count == 0)
            {
                blnIsValidate = false;
                ShowMessageBox("Please Add Current Medication Details. !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            //else if (CurrentMedicationList.Where(z => z.Quantity == 0).Any())
            //{
            //    string strErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("QuantityGValidation_msg");
            //    ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //}
            return blnIsValidate;
        }

        private void FetchPreviousMedications()
        {
            try
            {
                clsGetPatientPastMedicationDetailsBizActionVO BizAction = new clsGetPatientPastMedicationDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.VisitID = CurrentVisit.ID;
                    if (CurrentVisit.OPDIPD)
                    {
                        BizAction.DoctorId = CurrentVisit.DoctorID;
                        BizAction.DoctorCode = CurrentVisit.DoctorCode;
                    }
                    else
                    {

                    }
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizAction.IsFromPresc = false;
                    BizAction.PagingEnabled = true;
                    BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                    BizAction.MaximumRows = DataList.PageSize;
                    if (Indicatior == null || Indicatior.Visibility == Visibility.Visible)
                    {

                    }
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsGetPatientPastMedicationDetailsBizActionVO)args.Result).PatientMedicationDetailList != null)
                            {
                                foreach (var item in ((clsGetPatientPastMedicationDetailsBizActionVO)args.Result).PatientMedicationDetailList)
                                {
                                    DataList.Add(item);
                                    item.Datetime = String.Format(item.VisitDate.ToString() + " - " + CurrentVisit.Doctor.Trim() + " - " + CurrentVisit.DoctorSpecialization);
                                }
                            }
                            PastMedicationList = new ObservableCollection<clsPatientPrescriptionDetailVO>(DataList);
                            PagedCollectionView pcvPastMEdications = new PagedCollectionView(PastMedicationList);
                            if (this.CurrentVisit.VisitTypeID == 2)
                                if (this.CurrentVisit.OPDIPD)
                                {
                                    pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                }
                                else
                                {
                                    pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                                }
                            else
                                pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                            dgPrevMedications.ItemsSource = null;
                            dgPrevMedications.ItemsSource = pcvPastMEdications;
                            dgPrevMedications.UpdateLayout();
                            pgrPreMedications.Source = DataList;
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                            string strMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                            ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        public void FillFrequencyList()
        {
            try
            {
                clsGetEMRFrequencyBizActionVO BizAction = new clsGetEMRFrequencyBizActionVO();
                BizAction.FrequencyList = new List<PalashDynamics.ValueObjects.EMR.FrequencyMaster>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<FrequencyMaster> objList = new List<FrequencyMaster>();
                        objList.Add(new FrequencyMaster(0, "-- Select --"));
                        objList.AddRange(((clsGetEMRFrequencyBizActionVO)args.Result).FrequencyList);
                        FreqList = objList;
                        ShowCurrentMedication();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                ShowCurrentMedication();
            }
        }

        private void FillRouteList()
        {
            try
            {
                Indicatior.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Route;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        RouteList = objList;
                        FillInstruction();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                FillInstruction();
            }
        }

        private void FillInstruction()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_EMRInstructionMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                        InstructionList = objList;
                        FillFrequencyList();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                FillFrequencyList();
            }
        }

        private void ShowCurrentMedication()
        {
            try
            {
                clsGetPatientCurrentMedicationDetailListBizActionVO BizActionCurMed = new clsGetPatientCurrentMedicationDetailListBizActionVO();
                if (CurrentVisit != null)
                {
                    BizActionCurMed.VisitID = CurrentVisit.ID;
                    BizActionCurMed.PatientID = CurrentVisit.PatientId;
                    BizActionCurMed.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionCurMed.UnitID = CurrentVisit.UnitId;
                    BizActionCurMed.IsOPDIPD = CurrentVisit.OPDIPD;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientCurrentMedicationDetailListBizActionVO)args.Result).PatientCurrentMedicationDetailList != null)
                        {
                            foreach (var item in ((clsGetPatientCurrentMedicationDetailListBizActionVO)args.Result).PatientCurrentMedicationDetailList)
                            {
                                //if (RouteList != null)
                                //    item.RouteName = RouteList;
                                //if (FreqList != null)
                                //    item.FrequencyName = FreqList;
                                //if (InstructionList != null)
                                //    item.InstructionName = InstructionList;
                                CurrentMedicationList.Add(item);
                            }
                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = CurrentMedicationList;
                        dgDrugList.UpdateLayout();
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                        string strMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                        ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void SaveCurrentMedication()
        {
            try
            {
                string msgText = "Palash";
                if (CurrentMedicationList != null && CurrentMedicationList.Count > 0)
                {
                    clsAddUpdatePatientCurrentMedicationDetailBizActionVO BizActionObjCurrentMedication = new clsAddUpdatePatientCurrentMedicationDetailBizActionVO();
                    Indicatior.Show();
                    BizActionObjCurrentMedication.PatientID = CurrentVisit.PatientId;
                    BizActionObjCurrentMedication.VisitID = CurrentVisit.ID;
                    BizActionObjCurrentMedication.DoctorID = CurrentVisit.DoctorID;
                    BizActionObjCurrentMedication.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionObjCurrentMedication.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizActionObjCurrentMedication.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionObjCurrentMedication.UnitID = CurrentVisit.UnitId;
                    BizActionObjCurrentMedication.TemplateID = 79;
                    if (CurrentMedicationList.Count > 0)
                    {
                        BizActionObjCurrentMedication.PatientCurrentMedicationDetail = CurrentMedicationList.ToList();
                    }
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient clientObjCurrentMedication = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    clientObjCurrentMedication.ProcessCompleted += (sObjPatientCurrentMedication, argsObjPatientCurrentMedication) =>
                    {
                        string strErrorMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                        if (argsObjPatientCurrentMedication.Error == null && argsObjPatientCurrentMedication.Result != null && (((clsAddUpdatePatientCurrentMedicationDetailBizActionVO)argsObjPatientCurrentMedication.Result).SuccessStatus == 1))
                        {
                            Indicatior.Close();
                            //string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                            //MessageBoxControl.MessageBoxChildWindow msgW1 =
                            //new MessageBoxControl.MessageBoxChildWindow(msgText, strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            //{
                            //    this.Content = null;
                            //    NavigateToNextMenu();
                            //};
                            //msgW1.Show();
                            //NavigateToNextMenu();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            Indicatior.Close();
                            ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    };
                    clientObjCurrentMedication.ProcessAsync(BizActionObjCurrentMedication, ((IApplicationConfiguration)App.Current).CurrentUser);
                    clientObjCurrentMedication.CloseAsync();
                }
                else
                {
                    string strMessage = "Please add record to save !";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("EmptyForm_Msg");
                    ShowMessageBox(strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        //private void NavigateToNextMenu()
        //{
        //    UserControl winEMR;
        //    //if (this.CurrentVisit.VisitTypeID == 2)
        //    //{
        //        winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
        //    //}
        //    //else
        //    //{
        //    //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
        //    //}
        //    TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
        //    TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
        //    clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
        //    if (SelectedItem.HasItems == true)
        //    {
        //        (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
        //    }
        //    else if (objMenu.Parent.Trim() == "NST IPD EMR")
        //    {
        //        int iCount = tvEMR.Items.Count;
        //        int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
        //        if (objMenu.MenuOrder < iCount)
        //        {
        //            if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
        //            {
        //                ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
        //            }
        //            else
        //                (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
        //        }
        //    }
        //    else
        //    {
        //        int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
        //        int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
        //        if (iCount > objMenu.MenuOrder)
        //        {
        //            ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
        //        }
        //        else
        //        {
        //            objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
        //            int iIndex = Convert.ToInt32(objMenu.MenuOrder);
        //            (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
        //        }
        //    }
        //}

        #endregion

        #region Events

        private void btnOtherDrug_Click(object sender, RoutedEventArgs e)
        {
            frmOtherMedicationDrug win = new frmOtherMedicationDrug();
            win.OnAddButton_Click += new RoutedEventHandler(win_OnAddButton_Click);
            win.Show();
        }

        void win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmOtherMedicationDrug)sender).DialogResult == true)
            {
                var item1 = from r in CurrentMedicationList
                            where (r.DrugName == ((frmOtherMedicationDrug)sender).NewDrug)
                            select r;
                if (item1.ToList().Count == 0)
                {
                    clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                    OBj.DrugName = ((frmOtherMedicationDrug)sender).NewDrug;
                    if (RouteList != null)
                        OBj.RouteName = RouteList;
                    if (FreqList != null)
                        OBj.FrequencyName = FreqList;
                    if (InstructionList != null)
                        OBj.InstructionName = InstructionList;
                    CurrentMedicationList.Add(OBj);
                }
                else
                {
                    string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OtherDrugAdded_Msg");
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                dgDrugList.ItemsSource = CurrentMedicationList;
                dgDrugList.UpdateLayout();
                dgDrugList.Focus();
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            FetchPreviousMedications();
        }

        private void lnkAddDrugList_Click(object sender, RoutedEventArgs e)
        {
            frmEMRMedicationDrugSelectionList Win = new frmEMRMedicationDrugSelectionList();
           // Win.FromCurrntMed = true;
            dgDrugList.SelectedIndex = -1;
            UserControl winEMR;
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.OnAddButton_Click += new RoutedEventHandler(WinDrug_OnAddButton_Click);
            Win.Show();
        }

        void WinDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmEMRMedicationDrugSelectionList)sender).DialogResult == true)
            {
                foreach (var item in (((frmEMRMedicationDrugSelectionList)sender).SelectedDrugList))
                {
                    if (CurrentMedicationList.Count > 0)
                    {

                        var item1 = from r in CurrentMedicationList
                                    where (r.DrugID == item.ItemID
                                   )
                                    select new clsPatientPrescriptionDetailVO
                                    {
                                        DrugID = r.DrugID,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                            OBj.DrugID = item.ItemID;
                            OBj.DrugName = item.ItemName;
                            OBj.FrequencyName = FreqList;
                            OBj.RouteName = RouteList;
                            OBj.NewAdded = true;
                            OBj.SelectedRoute.ID = item.RouteID;
                            OBj.SelectedRoute.Description = item.Route;
                            OBj.InstructionName = InstructionList;
                            CurrentMedicationList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                        OBj.DrugID = item.ItemID;
                        OBj.DrugName = item.ItemName;
                        OBj.FrequencyName = FreqList;
                        OBj.RouteName = RouteList;
                        OBj.NewAdded = true;
                        OBj.SelectedRoute.ID = item.RouteID;
                        OBj.SelectedRoute.Description = item.Route;
                        OBj.InstructionName = InstructionList;
                        CurrentMedicationList.Add(OBj);
                    }
                }
                dgDrugList.ItemsSource = null;
                dgDrugList.ItemsSource = CurrentMedicationList;
                dgDrugList.UpdateLayout();
            }
        }

        private void dgDrugList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                #region
                if (IsEnabledControl == false)
                {
                    DataGridRow dataGrdrow = e.Row;
                    foreach (DataGridColumn grdColumn in dgDrugList.Columns)
                    {
                        if (grdColumn.Header != null)
                        {
                            if (grdColumn.Header.Equals("Route"))
                            {
                                FrameworkElement cellContent = grdColumn.GetCellContent(dataGrdrow);
                                cmbBox = cellContent as AutoCompleteComboBox;
                                if (cmbBox != null)
                                {
                                    cmbBox.IsEnabled = false;
                                }
                            }
                            else if (grdColumn.Header.Equals("Frequency"))
                            {
                                FrameworkElement cellContent = grdColumn.GetCellContent(dataGrdrow);
                                cmbBox = cellContent as AutoCompleteComboBox;
                                if (cmbBox != null)
                                {
                                    cmbBox.IsEnabled = false;
                                }
                            }
                            else if (grdColumn.Header.Equals("Dose"))
                            {
                                FrameworkElement cellContent = grdColumn.GetCellContent(dataGrdrow);
                                txt = cellContent as TextBox;
                                if (txt != null)
                                {
                                    txt.IsReadOnly = true;
                                }
                            }
                            else if (grdColumn.Header.Equals("Days"))
                            {
                                FrameworkElement cellContent = grdColumn.GetCellContent(dataGrdrow);
                                txt = cellContent as TextBox;
                                if (txt != null)
                                {
                                    txt.IsReadOnly = true;
                                }
                            }
                            else if (grdColumn.Header.Equals("Instruction"))
                            {
                                FrameworkElement cellContent = grdColumn.GetCellContent(dataGrdrow);
                                cmbBox = cellContent as AutoCompleteComboBox;
                                if (cmbBox != null)
                                {
                                    cmbBox.IsEnabled = false;
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            { }
        }

        public void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateSave())
            {
                //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
                //MessageBoxControl.MessageBoxChildWindow msgW =
                //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //msgW.Show();

                SaveCurrentMedication();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveCurrentMedication();
            }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbFrequency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                if (((AutoCompleteComboBox)sender).SelectedItem != null)
                {
                    try
                    {
                        double Days = Convert.ToDouble(((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Days);
                        ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Quantity = (((FrequencyMaster)((AutoCompleteComboBox)sender).SelectedItem).Quantity * Days);
                    }
                    catch (Exception) { }

                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "")
                {
                    try
                    {
                        double Days = Convert.ToDouble(((TextBox)sender).Text);
                        double Qty = 0;
                        if (FreqList != null)
                        {
                            if (SelectedDrug != null)
                            {
                                var item1 = from r in FreqList
                                            where (r.Description == ((clsPatientPrescriptionDetailVO)SelectedDrug).SelectedFrequency.Description
                                               )
                                            select r;

                                foreach (FrequencyMaster myitem in item1)
                                {
                                    Qty = myitem.Quantity;
                                    break;
                                }
                                ((clsPatientPrescriptionDetailVO)SelectedDrug).Quantity = Qty * (Days);
                            }
                            else
                            {
                                var item1 = from r in FreqList
                                            where (r.Description == ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).SelectedFrequency.Description
                                               )
                                            select r;

                                foreach (FrequencyMaster myitem in item1)
                                {
                                    Qty = myitem.Quantity;
                                    break;
                                }
                                ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Quantity = Qty * (Days);

                            }
                        }
                        else
                            ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Quantity = Qty * (Days);
                    }
                    catch (Exception) { }

                }
            }
        }

        private void lnkAddDrugFromPrecList_Click(object sender, RoutedEventArgs e)
        {
            frmEMRPrescribedMedication Win = new frmEMRPrescribedMedication(true);
            Win.CurrentVisit = this.CurrentVisit;
            dgDrugList.SelectedIndex = -1;
            UserControl winEMR;
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.OnAddButton_Click += new RoutedEventHandler(WinCurrentDrug_OnAddButton_Click);
            Win.Show();
        }

        void WinCurrentDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmEMRPrescribedMedication)sender).DialogResult == true)
            {
                try
                {
                    foreach (var item in (((frmEMRPrescribedMedication)sender).SelectedDrugList))
                    {
                        if (CurrentMedicationList.Count > 0)
                        {
                            var item1 = from r in CurrentMedicationList
                                        where (r.DrugName == item.DrugName)
                                        select new clsPatientPrescriptionDetailVO
                                        {
                                            DrugName = r.DrugName,
                                            Status = r.Status
                                        };
                            if (item1.ToList().Count == 0)
                            {
                                clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                                OBj.DrugID = item.DrugID;
                                OBj.DrugName = item.DrugName;
                                OBj.Frequency = item.Frequency;
                                OBj.Quantity = item.Quantity;
                                OBj.Instruction = item.Instruction;
                                OBj.NewAdded = true;
                                CurrentMedicationList.Add(OBj);
                            }
                        }
                        else
                        {
                            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                            OBj.DrugID = item.DrugID;
                            OBj.DrugName = item.DrugName;
                            OBj.Frequency = item.Frequency;
                            OBj.Instruction = item.Instruction;
                            OBj.Quantity = item.Quantity;
                            OBj.NewAdded = true;
                            CurrentMedicationList.Add(OBj);
                        }
                    }
                    dgDrugList.ItemsSource = null;
                    dgDrugList.ItemsSource = CurrentMedicationList;
                    dgDrugList.UpdateLayout();
                }
                catch (Exception)
                { }
            }
        }

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                try
                {
                    string msgText = "Are you sure you want to Delete the selected Drug ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsPatientPrescriptionDetailVO objVo = (clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem;
                            objVo.Status = false;
                            CurrentMedicationList.RemoveAt(dgDrugList.SelectedIndex);
                            dgDrugList.ItemsSource = null;
                            dgDrugList.ItemsSource = CurrentMedicationList;
                            dgDrugList.UpdateLayout();
                            dgDrugList.Focus();
                        }
                    };
                    msgWD.Show();
                }
                catch (Exception)
                { }
            }
        }

        private void dgDrugList_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
                SelectedDrug = (clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem;
        }

        private void dgDrugList_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
                SelectedDrug = (clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem;
        }

        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

    }
}
