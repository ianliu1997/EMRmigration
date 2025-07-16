using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using System.Collections.ObjectModel;
using MessageBoxControl;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Controls;
using System.Windows.Data;
using PalashDynamics.Collections;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.SearchResultLists;

namespace EMR
{
    public partial class frmIPDEMRProcedure : UserControl
    {
        #region DataMember
        public clsVisitVO CurrentVisit { get; set; }
        WaitIndicator Indicatior = null;
        string msgTitle = "Palash";
        public Patient SelectedPatient { get; set; }
        public ObservableCollection<clsEMRAddDiagnosisVO> DiagnosisList { get; set; }
        List<clsEMRAddDiagnosisVO> PatientDiagnosiDeletedList = new List<clsEMRAddDiagnosisVO>();
        public string SelectedUser { get; set; }
        int TotalRows, CurrentStartRow;
        public event RoutedEventHandler OnAddButton_Click;
        public bool IsEnabledControl = false;
        DateConverter dateConverter;
        #endregion

        public frmIPDEMRProcedure()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        #region Paging

        public PagedSortableCollectionView<clsEMRAddDiagnosisVO> DataList { get; private set; }

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetPatientDiagnosisHistory();
        }

        #endregion

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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DiagnosisList == null)
                DiagnosisList = new ObservableCollection<clsEMRAddDiagnosisVO>();
            dgDiagnosisList.ItemsSource = DiagnosisList;

            FillDiagnosisType();
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
                spSpecDoctor.Visibility = Visibility.Visible;
            }
                FillSpecialization();
                FillDoctor();
            cmdSave.IsEnabled = IsEnabledControl;
            lnkAddDiagnosis.IsEnabled = IsEnabledControl;
            lnkNewDiagnosis.IsEnabled = IsEnabledControl;
            dgDiagnosisList.IsReadOnly = !IsEnabledControl;
            dgDiagnosisList.Columns[0].IsReadOnly = !IsEnabledControl;

            TotalRows = CurrentStartRow = 0;
            dateConverter = new DateConverter();


        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            lnkSave_Click(sender, e);
        }
        private void lnkSave_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //msgW.Show();

            string strMsg = "Add the Procedure details.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("AddDiagnosisDtls_Msg");
            if (DiagnosisList == null || DiagnosisList.Count <= 0)
            {
                ShowMessageBox(strMsg, MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else
            {
                SaveDiagnosis();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            string msgText = "";
            if (result == MessageBoxResult.Yes)
            {
                bool IsPrinciple = false;
                string strMsg = "Add the Procedure details.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("AddDiagnosisDtls_Msg");
                if (DiagnosisList == null || DiagnosisList.Count <= 0)
                {
                    ShowMessageBox(strMsg, MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    SaveDiagnosis();
                }
                //else
                //{
                //    IsPrinciple = false;
                //    int IsPrimaryCount = 0;
                //    foreach (var item in DiagnosisList)
                //    {
                //        if (item.SelectedDiagnosisType.ID == 0)
                //        {
                //            string sMsgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectDgnssType_Msg");
                //            ShowMessageBox(sMsgText, MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            IsPrinciple = false;
                //            break;
                //        }
                //        else
                //        {
                //            IsPrinciple = true;
                //        }
                //        if (item.SelectedDiagnosisType.ID == 1)
                //        {
                //            IsPrimaryCount++;
                //        }
                //    }
                //    if (IsPrinciple)
                //    {
                //        if (IsPrimaryCount <= 1)
                //        {
                //            SaveDiagnosis();
                //        }
                //        else
                //        {
                //            string sMsgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("OnlyOnePrimary_Msg");
                //            ShowMessageBox(sMsgText, MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        }
                //    }
                //}
            }
        }

        private void cmdAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            frmProcedureDiagnosisSelection Win = new frmProcedureDiagnosisSelection();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            Win.Width = winEMR.ActualWidth * 0.8;
            Win.Height = winEMR.ActualHeight * 0.8;
            Win.Show();
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmProcedureDiagnosisSelection)sender).DialogResult == true)
            {
                foreach (var item in (((frmProcedureDiagnosisSelection)sender).DiagnosisList))
                {
                    if (DiagnosisList.Count > 0)
                    {
                        var item1 = from r in DiagnosisList
                                    where (r.ServiceCode == item.ServiceCode)
                                    select r;
                        if (item1.ToList().Count == 0)
                        {
                            clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                            OBj.Diagnosis = item.Diagnosis;
                            OBj.Code = item.Categori;
                            OBj.Class = item.Class;
                            OBj.ServiceCode = item.Categori;
                            // OBj.IsICOPIMHead = item.IsICOPIMHead;
                            OBj.IsEnabled = false;
                            OBj.DiagnosisTypeList = DiagnosisTypeList;
                            OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                            OBj.IsICD9 = true;
                            OBj.TemplateID = item.TemplateID;
                            OBj.TemplateName = item.TemplateName;
                            DiagnosisList.Add(OBj);
                        }
                        else
                        {
                            string sMsgText = "Procedure already added"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Diagnosis already added");
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                        OBj.Diagnosis = item.Diagnosis;
                        OBj.Code = item.Categori;
                        OBj.Diagnosis = item.Diagnosis;
                        OBj.Class = item.Class;
                        OBj.ServiceCode = item.Categori;
                        // OBj.IsICOPIMHead = item.IsICOPIMHead;
                        OBj.DiagnosisTypeId = item.DiagnosisTypeId;
                        OBj.IsEnabled = false;
                        OBj.DiagnosisTypeList = DiagnosisTypeList;
                        OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                        OBj.IsICD9 = true;
                        OBj.TemplateID = item.TemplateID;
                        OBj.TemplateName = item.TemplateName;
                        DiagnosisList.Add(OBj);
                    }
                }
                dgDiagnosisList.ItemsSource = DiagnosisList;
                dgDiagnosisList.UpdateLayout();
                dgDiagnosisList.Focus();
            }
        }

        private void cmdDeleteDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            if (dgDiagnosisList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Procedure ?";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ConfirmDeleteDiagnosis_Msg");

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsEMRAddDiagnosisVO objVo = (clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem;
                            objVo.Status = false;
                            objVo.IsDeleted = true;
                            DiagnosisList.RemoveAt(dgDiagnosisList.SelectedIndex);
                            if (objVo.ID != null && objVo.ID > 0)
                                PatientDiagnosiDeletedList.Add(objVo);
                            dgDiagnosisList.ItemsSource = null;
                            dgDiagnosisList.ItemsSource = DiagnosisList;
                            dgDiagnosisList.UpdateLayout();
                        }
                    }
                };
                msgWD.Show();
            }
        }

        private void chkIsprimaryDiag_Click(object sender, RoutedEventArgs e)
        {
            if (dgDiagnosisList.SelectedItem != null)
            {
                clsEMRAddDiagnosisVO selItem = new clsEMRAddDiagnosisVO();
                selItem = (clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem;
                foreach (clsEMRAddDiagnosisVO item in DiagnosisList)
                {
                    if (item == selItem)
                    {
                        item.IsSelected = true;
                    }
                    else
                    {
                        item.IsSelected = false;
                    }
                }
                dgDiagnosisList.ItemsSource = null;
                dgDiagnosisList.ItemsSource = DiagnosisList;
                dgDiagnosisList.UpdateLayout();
            }
        }

        public void SaveDiagnosis()
        {
            string strMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
            WaitIndicator IndicatiorDiag = new WaitIndicator();
            try
            {
                IndicatiorDiag.Show();
                List<clsEMRAddDiagnosisVO> SavePatientDiagnosiList = new List<clsEMRAddDiagnosisVO>();

                foreach (clsEMRAddDiagnosisVO item in DiagnosisList)
                {
                    SavePatientDiagnosiList.Add(item);
                }
                foreach (clsEMRAddDiagnosisVO item in PatientDiagnosiDeletedList)
                {
                    item.Status = false;
                    SavePatientDiagnosiList.Add(item);
                }
                clsAddUpdateDeleteDiagnosisDetailsBizActionVO BizActionVO = new clsAddUpdateDeleteDiagnosisDetailsBizActionVO();
                BizActionVO.UnitID = CurrentVisit.UnitId;
                BizActionVO.PatientID = CurrentVisit.PatientId;
                BizActionVO.VisitID = CurrentVisit.ID;
                BizActionVO.PatientUnitID = CurrentVisit.PatientUnitId;
                BizActionVO.DiagnosisDetails = SavePatientDiagnosiList;
                BizActionVO.IsICD10 = false;
                BizActionVO.DoctorID = CurrentVisit.DoctorID;
                BizActionVO.IsOPDIPD = CurrentVisit.OPDIPD;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddUpdateDeleteDiagnosisDetailsBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            IndicatiorDiag.Close();
                            //string msgText = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                            //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            //{
                            //    this.Content = null;
                            //    NavigateToNextMenu();
                            //};
                            //msgWindow.Show();

                            #region UpdateEncounterList
                            if (this.CurrentVisit.VisitTypeID == 2)
                            {
                                frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                                ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                                DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                                clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                                objPatientHeader.IsProcedure = "Visible";
                                objPatientHeader.IsNonProcedure = "Collapsed";
                                dgEncounterList.ItemsSource = null;
                                dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                            }
                            #endregion

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();

                            //this.Content = null;
                            //NavigateToNextMenu();
                        }
                        else
                        {
                            IndicatiorDiag.Close();
                            ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        IndicatiorDiag.Close();
                        ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                IndicatiorDiag.Close();
                ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }

      

        private void GetPatientDiagnosis()
        {
            try
            {
                GetPatientDiagnosisHistory();
                WaitIndicator IndicatiorGet = new WaitIndicator();
                IndicatiorGet.Show();
                clsGetPatientDiagnosisDataBizActionVO BizAction = new clsGetPatientDiagnosisDataBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                }
                BizAction.PatientID = CurrentVisit.PatientId;
                BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                BizAction.UnitID = CurrentVisit.UnitId;
                BizAction.Ishistory = false;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                //BizAction.DoctorCode = CurrentVisit.DoctorCode;
                BizAction.DoctorID = CurrentVisit.DoctorID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();
                    if ((clsGetPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            foreach (var item in ((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                if (IsEnabledControl == false)
                                    item.IsEnabled = true;
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DiagnosisList.Add(item);
                            }
                        }
                        dgDiagnosisList.ItemsSource = null;
                        dgDiagnosisList.ItemsSource = DiagnosisList;
                        dgDiagnosisList.UpdateLayout();
                        IndicatiorGet.Close();

                    }
                    else
                    {
                        string sMsgText = "Error ocurred while processing.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
                        IndicatiorGet.Close();
                        ShowMessageBox(sMsgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        public List<clsEMRAddDiagnosisVO> DiagnosisListHistory { get; set; }

        private void GetPatientDiagnosisHistory()
        {
            try
            {
                WaitIndicator IndicatiorGet = new WaitIndicator();
                IndicatiorGet.Show();
                DiagnosisListHistory = new List<clsEMRAddDiagnosisVO>();
                clsGetPatientDiagnosisDataBizActionVO BizAction = new clsGetPatientDiagnosisDataBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                }
                BizAction.PatientID = CurrentVisit.PatientId;
                BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                BizAction.UnitID = CurrentVisit.UnitId;
                BizAction.Ishistory = true;
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                BizAction.ISDashBoard = false;
               // BizAction.DoctorCode = CurrentVisit.DoctorCode;
                BizAction.DoctorID = CurrentVisit.DoctorID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if ((clsGetPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {

                        if (((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DataList.TotalItemCount = ((clsGetPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;
                            DataList.Clear();
                            foreach (var item in ((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                if (IsEnabledControl == false)
                                    item.IsEnabled = true;
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                if (this.CurrentVisit.OPDIPD)
                                    item.Datetime = String.Format(item.Date.ToString("dd/MM/yyyy") + " - " + item.DoctorName.Trim() + " - " + item.DocSpec);
                                //DiagnosisListHistory.Add(item);
                                DataList.Add(item);
                            }
                        }
                        DiagnosisListHistory = DataList.ToList();
                        PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(DiagnosisListHistory);
                        if (this.CurrentVisit.VisitTypeID == 2)
                            if (this.CurrentVisit.OPDIPD)
                            {
                                pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                            }
                            else
                            {
                                pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                            }
                        else
                            pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));
                        dgPreviousDiagnosisList.ItemsSource = null;
                        dgPreviousDiagnosisList.ItemsSource = pcvDiagnosisListHistory;
                        dgPreviousDiagnosisList.UpdateLayout();
                        pgrPatientDiagnosis.Source = DataList;
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        string sMsgText = "Error ocurred while processing.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
                        IndicatiorGet.Close();
                        ShowMessageBox(sMsgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void cmdNewDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            frmAddDiagnosis AddWin = new frmAddDiagnosis();
            AddWin.lblDiagnosisName.Text = "Procedure Name";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblProcedureName");
            AddWin.Title = "New Procedure";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblNewProcedure");
            AddWin.OnAddButton_Click += new RoutedEventHandler(AddWin_OnAddButton_Click);
            AddWin.Show();
        }

        void AddWin_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmAddDiagnosis)sender).DialogResult == true)
            {
                var item1 = from r in DiagnosisList
                            where (r.Diagnosis == ((frmAddDiagnosis)sender).NewDiagnosis)
                            select r;
                if (item1.ToList().Count == 0)
                {
                    clsEMRAddDiagnosisVO OBj = new clsEMRAddDiagnosisVO();
                    OBj.Diagnosis = ((frmAddDiagnosis)sender).NewDiagnosis;
                    OBj.DiagnosisTypeList = DiagnosisTypeList;
                    OBj.SelectedDiagnosisType = new MasterListItem(0, "-- Select --");
                    DiagnosisList.Add(OBj);
                }
                else
                {
                    string sMsgText = "Procedure already added";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DiagnosisAdded_Msg");
                    ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                dgDiagnosisList.ItemsSource = DiagnosisList;
                dgDiagnosisList.UpdateLayout();
                dgDiagnosisList.Focus();
            }
        }

        List<MasterListItem> DiagnosisTypeList = new List<MasterListItem>();
        private void FillDiagnosisType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DiagnosisMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        DiagnosisTypeList = new List<MasterListItem>();
                        DiagnosisTypeList.Add(new MasterListItem(0, "-- Select --"));
                        DiagnosisTypeList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    }
                    GetPatientDiagnosis();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
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
        private void cmbDiagnosis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((AutoCompleteComboBox)sender).SelectedItem != null)
            {
                if ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem != null)
                {
                    int index = dgDiagnosisList.SelectedIndex;
                    if (index != -1)
                    {
                        for (int i = 0; i < DiagnosisList.Count; i++)
                        {
                            if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID == 1)
                            {
                                DiagnosisList[index].Image = "../Icons/green.png";
                            }
                            else if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID == 2)
                            {
                                DiagnosisList[index].Image = "../Icons/yellow.png";
                            }
                            else if (((MasterListItem)((AutoCompleteComboBox)sender).SelectedItem).ID == 3)
                            {
                                DiagnosisList[index].Image = "../Icons/orange.png";
                            }
                            break;
                        }
                    }

                    dgDiagnosisList.ItemsSource = null;
                    dgDiagnosisList.ItemsSource = DiagnosisList;
                    dgDiagnosisList.UpdateLayout();
                }
            }
        }

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).TemplateID != 0)
            {
                frmDiagnosisEMRTemplate DiagnosisEMRTemplate = new frmDiagnosisEMRTemplate();
                DiagnosisEMRTemplate.TemplateID = (int)((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).TemplateID;
                DiagnosisEMRTemplate.SelectedPatient = SelectedPatient;
                DiagnosisEMRTemplate.DiagnosisName = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).Diagnosis;
                DiagnosisEMRTemplate.DiagnosisCode = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).Code.ToString();
                DiagnosisEMRTemplate.SelectedUser = SelectedUser;
                DiagnosisEMRTemplate.CurrentVisit = CurrentVisit;
                DiagnosisEMRTemplate.IsEnabledControl = IsEnabledControl;
                if (((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).listPatientEMRDetails != null)
                    DiagnosisEMRTemplate.EmrDetails = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).listPatientEMRDetails;
                else
                    DiagnosisEMRTemplate.TemplateLoad = true;
                DiagnosisEMRTemplate.OnAddButton_Click += new RoutedEventHandler(DiagnosisEMRTemplate_OnAddButton_Click);
                DiagnosisEMRTemplate.Show();
            }
        }

        void DiagnosisEMRTemplate_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmDiagnosisEMRTemplate)sender).DialogResult == true)
            {
                for (int i = 0; i < DiagnosisList.Count; i++)
                {
                    if (DiagnosisList[i].Diagnosis == ((frmDiagnosisEMRTemplate)sender).DiagnosisName)
                    {
                        DiagnosisList[i].listPatientEMRDetails = ((frmDiagnosisEMRTemplate)sender).listPatientEMRDetails;
                    }
                }

                dgDiagnosisList.ItemsSource = DiagnosisList;
                dgDiagnosisList.UpdateLayout();
                dgDiagnosisList.Focus();
            }
        }
    }
}
