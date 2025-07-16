using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.CompoundDrug;
using PalashDynamics.SearchResultLists;
using PalashDynamics.ValueObjects.RSIJ;
using System.Windows.Media;
namespace EMR
{
    public partial class frmEMRCompoundMedication : UserControl
    {
        #region Data Member
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
        ObservableCollection<clsPatientPrescriptionDetailVO> CompoundMedicationList;
        ObservableCollection<clsPatientPrescriptionDetailVO> PastCompdPrescriptionList { get; set; }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        DateConverter dateConverter;
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsEnableControl { get; set; }
        public string sErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
        WaitIndicator Indicatior = new WaitIndicator();
        ObservableCollection<clsPatientPrescriptionDetailVO> CompoundList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
        List<FrequencyMaster> FreqList = new List<FrequencyMaster>();
        List<MasterListItem> ItemList = new List<MasterListItem>();
        List<MasterListItem> RouteList = new List<MasterListItem>();
        List<MasterListItem> InstructionList = new List<MasterListItem>();
        clsPatientPrescriptionDetailVO DeletePrescriptionList;
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataList { get; private set; }
        public Boolean IsPageLoad { get; set; }
        #endregion

        #region Constructor
        public frmEMRCompoundMedication()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            this.Loaded += new RoutedEventHandler(frmEMRCompoundMedication_Loaded);
        }
        void frmEMRCompoundMedication_Loaded(object sender, RoutedEventArgs e)
        {
            dateConverter = new DateConverter();
            if (CompoundMedicationList == null)
                CompoundMedicationList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
            if (!IsPageLoad)
            {
                IsPageLoad = true;
                ShowPrescriptionAndServices();
                FetchPreviousPrescriptions();
                if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
                {
                    spSpecDoctor.Visibility = Visibility.Collapsed;
                    this.IsEnableControl = false;
                }
                else if (this.CurrentVisit.VisitTypeID == 1)
                {
                    spSpecDoctor.Visibility = Visibility.Collapsed;
                }
                else
                {
                    spSpecDoctor.Visibility = Visibility.Visible;
                    FillSpecialization();
                    FillDoctor();
                }
                cmdSave.IsEnabled = IsEnableControl;
                btnCompdDrug.IsEnabled = IsEnableControl;
            }
        }
        #endregion

        #region Refresh

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchPreviousPrescriptions();
        }

        #endregion

        #region Fill Methods

        private void FetchPreviousPrescriptions()
        {
            try
            {
                clsGetPatientPastMedicationDetailsBizActionVO BizAction = new clsGetPatientPastMedicationDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.DoctorCode = CurrentVisit.DoctorCode;
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizAction.IsFromPresc = true;
                    BizAction.PagingEnabled = true;
                    BizAction.IsForCompound = true;
                    BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                    BizAction.MaximumRows = DataList.PageSize;
                    if (Indicatior == null || Indicatior.Visibility == Visibility.Collapsed)
                    {
                        Indicatior = new WaitIndicator();
                        Indicatior.Show();
                    }

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsGetPatientPastMedicationDetailsBizActionVO)args.Result).PatientMedicationDetailList != null)
                            {
                                DataList.TotalItemCount = ((clsGetPatientPastMedicationDetailsBizActionVO)args.Result).TotalRows;
                                DataList.Clear();
                                foreach (var item in ((clsGetPatientPastMedicationDetailsBizActionVO)args.Result).PatientMedicationDetailList)
                                {
                                    DataList.Add(item);
                                    if (this.CurrentVisit.OPDIPD)
                                        item.Datetime = String.Format(item.VisitDate.ToString() + " - " + CurrentVisit.Doctor.Trim() + " - " + CurrentVisit.DoctorSpecialization);
                                }
                            }
                            PastCompdPrescriptionList = new ObservableCollection<clsPatientPrescriptionDetailVO>(DataList);
                            PagedCollectionView pcvPastMEdications = new PagedCollectionView(PastCompdPrescriptionList);
                            if (CurrentVisit.VisitTypeID == 2)
                                if (this.CurrentVisit.OPDIPD)
                                {
                                    pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                }
                                else
                                {
                                    pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                                }
                            else
                            {
                                pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                                pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("CompoundDrug"));
                            }

                            dgPreviousPrescriptions.ItemsSource = null;
                            dgPreviousPrescriptions.ItemsSource = pcvPastMEdications;
                            dgPreviousPrescriptions.UpdateLayout();
                            pgrPrevPrescriptions.Source = DataList;
                            
                            Indicatior.Close();
                        }
                        else
                        {
                            ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                        Indicatior.Close();

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

        private void ShowPrescriptionAndServices()
        {
            try
            {
                clsGetCompoundPrescriptionBizActionVO BizActionCPOE = new clsGetCompoundPrescriptionBizActionVO();
                if (CurrentVisit != null)
                {
                    BizActionCPOE.VisitID = CurrentVisit.ID;
                    BizActionCPOE.PatientID = CurrentVisit.PatientId;
                    BizActionCPOE.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionCPOE.IsOPDIPD = CurrentVisit.OPDIPD;
                }
                if (Indicatior == null)
                {
                    Indicatior = new WaitIndicator();
                }
                else if (Indicatior.Visibility == Visibility.Collapsed)
                {
                    Indicatior.Show();
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetCompoundPrescriptionBizActionVO)args.Result).CoumpoundDrugList != null)
                        {
                            CompoundList.Clear();
                            foreach (var item in ((clsGetCompoundPrescriptionBizActionVO)args.Result).CoumpoundDrugList)
                            {
                                CompoundList.Add(item);
                            }
                        }
                        PagedCollectionView pcv = new PagedCollectionView(CompoundList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("CompoundDrug"));
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = pcv;
                        dgDrugList.UpdateLayout();
                        dgDrugList.Focus();
                        if (IsEnableControl == false)
                        {
                            dgDrugList.IsReadOnly = true;
                        }
                        dgDrugList.SelectedIndex = -1;
                    }
                    else
                    {
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizActionCPOE, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
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
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void FillInstructionList()
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
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
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
                       // ShowCurrentMedication();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
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

        #region ChildWindow
        private void btnCompdDrug_Click(object sender, RoutedEventArgs e)
        {
            CompoundDrugList Win = new CompoundDrugList();
            Win.OnAddButton_Click += new RoutedEventHandler(WinCompoundDrug1_OnAddButton_Click);
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            }
            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.Show();
        }

        void WinCompoundDrug1_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CompoundDrugList)sender).DialogResult == true)
            {
                string sCompoundName = string.Empty;
                foreach (var item in (((CompoundDrugList)sender).DrugList))
                {
                    if (CompoundList.Count > 0)
                    {
                        var item1 = from r in CompoundList
                                    where ((r.CompoundDrug == item.PrintName || r.CompoundDrug == ((CompoundDrugList)sender).txtCName.Text) && r.DrugCode == item.Code)
                                    select r;

                        if (item1.ToList().Count == 0)
                        {
                            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                            OBj.DrugName = item.Description;
                            OBj.DrugCode = item.Code;
                            //OBj.FrequencyName = FreqList;
                            //OBj.RouteName = RouteList;
                            //OBj.SelectedRoute.ID = item.FilterID;
                            //OBj.SelectedRoute.Description = item.PrintDescription;
                            //OBj.InstructionName = InstructionList;
                            OBj.AvailableStock = item.AvailableStock;
                            if (((CompoundDrugList)sender).txtCName.Text != null && ((CompoundDrugList)sender).txtCName.Text != "")
                            {
                                OBj.CompoundDrug = ((CompoundDrugList)sender).txtCName.Text;
                            }
                            else
                            {
                                OBj.CompoundDrug = item.PrintName;
                            }
                            OBj.CompoundDrugID = item.SelectedID;
                            //OBj.CompoundDrugUnitID = item.SelectedID1;
                            OBj.Frequency = string.Empty;
                            OBj.Instruction = String.Empty;
                            CompoundList.Add(OBj);
                        }
                        else
                        {
                            if (sCompoundName == string.Empty || sCompoundName != item.PrintName)
                            {
                                sCompoundName = item1.ToList()[0].CompoundDrug;
                                ShowMessageBox("Already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                        OBj.DrugName = item.Description;
                        OBj.DrugCode = item.Code;
                        //OBj.SelectedRoute.ID = item.FilterID;
                        //OBj.SelectedRoute.Description = item.PrintDescription;
                        //OBj.FrequencyName = FreqList;
                        //OBj.RouteName = RouteList;
                        //OBj.InstructionName = InstructionList;
                        OBj.AvailableStock = item.AvailableStock;
                        if (((CompoundDrugList)sender).txtCName.Text != null && ((CompoundDrugList)sender).txtCName.Text != "")
                        {
                            OBj.CompoundDrug = ((CompoundDrugList)sender).txtCName.Text;
                        }
                        else
                        {
                            OBj.CompoundDrug = item.PrintName;
                        }
                        OBj.CompoundDrugID = item.SelectedID;
                        //OBj.CompoundDrugUnitID = item.SelectedID1;
                        OBj.Frequency = string.Empty;
                        OBj.Instruction = String.Empty;
                        CompoundList.Add(OBj);
                    }
                }
                PagedCollectionView pcv = new PagedCollectionView(CompoundList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("CompoundDrug"));

                dgDrugList.ItemsSource = null;
                dgDrugList.ItemsSource = pcv;

                dgDrugList.UpdateLayout();
                dgDrugList.Focus();
            }
        }
        #endregion

        #region Events
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

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
                //MessageBoxControl.MessageBoxChildWindow msgW =
                //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                //msgW.Show();
                #region UpdateEncounterList
                if (this.CurrentVisit.VisitTypeID == 2)
                {
                    frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                    ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                    DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                    DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                    clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                    objPatientHeader.IsCompdPrescRight = "Visible";
                    objPatientHeader.IsCompdPrescWrong = "Collapsed";
                    dgEncounterList.ItemsSource = null;
                    dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                }
                #endregion
                SaveCompoundMedication();
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            Indicatior.Show();
            if (result == MessageBoxResult.Yes)
            {
                SaveCompoundMedication();
            }
            else
            {
                Indicatior.Close();
            }
        }

        private void SaveCompoundMedication()
        {
            try
            {
                clsAddUpdateCompoundPrescriptionBizActionVO BizAction = new clsAddUpdateCompoundPrescriptionBizActionVO();
                BizAction.PatientID = CurrentVisit.PatientId;
                BizAction.VisitID = CurrentVisit.ID;
                BizAction.DoctorID = CurrentVisit.DoctorID;
                BizAction.DoctorCode = CurrentVisit.DoctorCode;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                var results = from a in CompoundList
                              group a by a.CompoundDrug into grouped
                              select grouped.First();

                foreach (var item in (results.ToList()))
                {
                    clsCompoundDrugMasterVO obj = new clsCompoundDrugMasterVO();
                    obj.Description = ((clsPatientPrescriptionDetailVO)item).CompoundDrug;
                    BizAction.CompoundDrugMaster.Add(obj);
                }
                foreach (var item in CompoundList)
                {
                    clsPatientCompoundPrescriptionVO ObjPatientPrescriptionDetail = new clsPatientCompoundPrescriptionVO();
                    ObjPatientPrescriptionDetail.CompoundDrug = item.CompoundDrug;
                    ObjPatientPrescriptionDetail.Frequency = item.Frequency;
                    ObjPatientPrescriptionDetail.Instruction = item.Instruction;
                    ObjPatientPrescriptionDetail.sComponentQuantity = item.sComponentQuantity;
                    ObjPatientPrescriptionDetail.sCompoundQuantity = item.sCompoundQuantity;
                    ObjPatientPrescriptionDetail.ItemCode = item.DrugCode;
                    ObjPatientPrescriptionDetail.ItemName = item.DrugName;
                    ObjPatientPrescriptionDetail.SelectedFrequency = item.SelectedFrequency;
                    //ObjPatientPrescriptionDetail.SelectedInstruction = item.SelectedInstruction;
                    //ObjPatientPrescriptionDetail.SelectedRoute = item.SelectedRoute;
                    //ObjPatientPrescriptionDetail.Dose = item.Dose;
                    ObjPatientPrescriptionDetail.CompoundDrugID = item.CompoundDrugID;
                    //ObjPatientPrescriptionDetail.CompoundDrugUnitID = item.CompoundDrugUnitID;

                    ObjPatientPrescriptionDetail.Days = Convert.ToInt32(item.Days);
                    BizAction.CoumpoundDrugList.Add(ObjPatientPrescriptionDetail);
                }
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient clientObjPatientEMRData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientObjPatientEMRData.ProcessCompleted += (sObjPatientEMRData, argsObjPatientEMRData) =>
                {
                    if (argsObjPatientEMRData.Error == null)
                    {
                        Indicatior.Close();
                        //string strSaveMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("RecordSaved_Msg");
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        //{
                        //    NavigateToNextMenu();
                        //};
                        //msgW1.Show();
                        NavigateToNextMenu();
                    }
                    else
                    {
                        Indicatior.Close();
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                clientObjPatientEMRData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientObjPatientEMRData.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = "Are you sure you want to discard the Changes ?";
            //msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            NavigatetoDashBoard();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                NavigatetoDashBoard();
            }
        }

        private void NavigatetoDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (IsEnableControl == true && dgDrugList.SelectedItem != null)
            {
                string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteDrugConfrm_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsPatientPrescriptionDetailVO objSelected = dgDrugList.SelectedItem as clsPatientPrescriptionDetailVO;
                        List<clsPatientPrescriptionDetailVO> lstDeletedCompd = CompoundList.Where(z => z.CompoundDrugID == objSelected.CompoundDrugID).ToList();
                        if (lstDeletedCompd != null)
                        {
                            foreach (var item in lstDeletedCompd)
                            {
                                CompoundList.Remove(item);
                            }
                        }

                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = CompoundList;
                        dgDrugList.UpdateLayout();
                        dgDrugList.Focus();
                        string sErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeletVerify_Msg");
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                msgWD.Show();
            }
        }

        #endregion

        #region Methods

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


        private bool Validation()
        {
            bool result = true;

            if (CompoundList != null && CompoundList.Count > 0)
            {
                //if (CompoundList.Where(Items => Items.CompoundQuantity == 0).Any() == true)
                //{
                //    ShowMessageBox("Please Enter Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    result = false;
                //    return result;
                //}
                if (CompoundList.Where(Items => Items.sCompoundQuantity == string.Empty && Items.sComponentQuantity == string.Empty).Any() == true)
                {
                    ShowMessageBox("Please Enter Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    result = false;
                    return result;
                }
                //var lstCompoundItem = CompoundList.GroupBy(z => z.DrugCode).ToList();
                //for (int iCount = 0; iCount < lstCompoundItem.Count; iCount++)
                //{
                //    double dQty = lstCompoundItem[iCount].ToList().Sum(item => item.CompoundQuantity);
                //    if (dQty > lstCompoundItem[iCount].ToList()[0].AvailableStock)
                //    {
                //        ShowMessageBox("Quantity must be less than or equal to Available Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        result = false;
                //        return result;
                //    }
                //}


            }
            return result;
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            TreeView tvEMR = null;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            }
            else
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
                tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            }

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

        #region ACB Drop Down

        private void cmbFrequency_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox Combo = (AutoCompleteComboBox)sender;
            Combo.ItemsSource = FreqList;

            var grp = (((CollectionViewGroup)Combo.DataContext).Items).ToList();
            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                Combo.SelectedItem = item.SelectedFrequency;

            }
        }

        private void cmbGroupFrequency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteComboBox Ch = (AutoCompleteComboBox)sender;
            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                item.SelectedFrequency = (FrequencyMaster)Ch.SelectedItem;
            }
        }

        private void cmbGrpInstruction_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox Combo = (AutoCompleteComboBox)sender;
            Combo.ItemsSource = InstructionList;

            var grp = (((CollectionViewGroup)Combo.DataContext).Items).ToList();
            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                Combo.SelectedItem = item.SelectedInstruction;
            }
        }
        private void cmbGrpInstruction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteComboBox Ch = (AutoCompleteComboBox)sender;
            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                item.SelectedInstruction = (MasterListItem)Ch.SelectedItem;
            }
        }
        private void cmbFrequency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                if (((AutoCompleteComboBox)sender).SelectedItem != null)
                {
                    double Days = Convert.ToDouble(((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Days);
                    ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Quantity = (((FrequencyMaster)((AutoCompleteComboBox)sender).SelectedItem).Quantity * Days);
                }
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox Ch = (TextBox)sender;
            if (Ch.Text != string.Empty)
            {
                var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();
                foreach (clsPatientPrescriptionDetailVO item in grp)
                {
                    if (Ch.Name == "txtGrpQuantity")
                        item.sCompoundQuantity = Ch.Text;
                    if (Ch.Name == "txtGrpFrequency")
                        item.Frequency = Ch.Text;
                    if (Ch.Name == "txtGrpInstruction")
                        item.Instruction = Ch.Text;
                }
            }
        }
        private void cmbRouteGrp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteComboBox Ch = (AutoCompleteComboBox)sender;

            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                item.SelectedRoute = (MasterListItem)Ch.SelectedItem;
            }
        }
        private void cmbRoute_Loaded(object sender, RoutedEventArgs e)
        {
            AutoCompleteComboBox Combo = (AutoCompleteComboBox)sender;
            Combo.ItemsSource = RouteList;

            var grp = (((CollectionViewGroup)Combo.DataContext).Items).ToList();
            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                Combo.SelectedItem = item.SelectedRoute;
            }

        }
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox Ch = (TextBox)sender;
            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();
            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                if (Ch.Name == "txtGrpQuantity")
                    Ch.Text = item.sCompoundQuantity;
                if (Ch.Name == "txtGrpFrequency")
                    Ch.Text = item.Frequency;
                if (Ch.Name == "txtGrpInstruction")
                    Ch.Text = item.Instruction;
            }
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
            else if ((dgDrugList.CurrentColumn != null) && dgDrugList.CurrentColumn.Header != null && dgDrugList.CurrentColumn.Header.Equals("Dose") && !String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                int Dose = Convert.ToInt32(((TextBox)sender).Text);
                Dose = Dose != null && Dose > 0 ? Dose : 1;
                ((TextBox)sender).Text = Dose.ToString();
                int Days = Convert.ToInt32(((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Days);
                Days = Days != null && Days > 0 ? Days : 1;
                ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).CompoundQuantity = ((((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).SelectedFrequency).Quantity * Days * Dose);
            }
        }
        private void txtGrpDays_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            var grp = (((CollectionViewGroup)txt.DataContext).Items).ToList();
            foreach (clsPatientPrescriptionDetailVO item in grp)
            {
                if (item.Days > 0)
                {
                    txt.Text = item.Days.ToString();
                }
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        private void TextBox_LoadedPrv(object sender, RoutedEventArgs e)
        {
            TextBox Ch = (TextBox)sender;
            var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

            if (grp[0] is clsPatientPrescriptionDetailVO)
            {
                foreach (clsPatientPrescriptionDetailVO item in grp)
                {
                    if (Ch.Name == "txtGrpQuantityPrv")
                        Ch.Text = item.sCompoundQuantity;
                    if (Ch.Name == "txtGrpFrequencyPrv")
                        Ch.Text = item.Frequency;
                    if (Ch.Name == "txtGrpInstructionPrv")
                        Ch.Text = item.Instruction;
                }
            }
            else
            {
                TextBox Ch1 = (TextBox)sender;
                var grp1 = (((CollectionViewGroup)Ch.DataContext).Items).ToList();
                if (Ch1.Name == "txtGrpQuantityPrv")
                {
                    Ch1.Visibility = Visibility.Collapsed;
                }
                if (Ch1.Name == "txtGrpFrequencyPrv")
                    Ch1.Visibility = Visibility.Collapsed;
                if (Ch1.Name == "txtGrpInstructionPrv")
                    Ch1.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
        private void lblQuantity_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock TB = (TextBlock)sender;
            var grp1 = (((CollectionViewGroup)TB.DataContext).Items).ToList();
            if (grp1[0] is clsPatientPrescriptionDetailVO)
            {
                if (TB.Name == "lblQuantity")
                    TB.Visibility = Visibility.Visible;
                if (TB.Name == "lblFrequency")
                    TB.Visibility = Visibility.Visible;
                if (TB.Name == "lblInstruction")
                    TB.Visibility = Visibility.Visible;
            }
            else
            {
                if (TB.Name == "lblQuantity")
                    TB.Visibility = Visibility.Collapsed;
                if (TB.Name == "lblFrequency")
                    TB.Visibility = Visibility.Collapsed;
                if (TB.Name == "lblInstruction")
                    TB.Visibility = Visibility.Collapsed;
                (TB.Parent as StackPanel).Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
