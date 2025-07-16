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
using PalashDynamics.ValueObjects.DashBoardVO;
//using PalashDynamics.IVF.DashBoard;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace EMR
{
    public partial class frmEMRProcedure : UserControl, IInitiateCIMSIVF
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
        List<MasterListItem> Priority;
        public bool IsArt = false;
        public long PlanTherapyId;
        public long PlanTherapyUnitId;
        public long GenderID;
        #endregion

        #region Constructor
        public frmEMRProcedure()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

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
            CurrentVisit = Item.CurrentVisit;
            IsEnabledControl = Item.CurrentVisit.VisitStatus;
            SelectedPatient = new Patient() { PatientId = PatientID, patientUnitID = PatientUnitID };

        }
        #endregion

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

        private void FillReferenceType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReferenceType;
                BizAction.MasterList = new List<MasterListItem>();
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            Priority = new List<MasterListItem>();
                            Priority.Add(new MasterListItem(0, "-- Select --"));
                            Priority.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
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
            catch (Exception)
            {
                throw;
            }
        }
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
            else if (this.CurrentVisit.VisitTypeID == 1)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
            }
            else
            {
                //spSpecDoctor.Visibility = Visibility.Visible;
                FillSpecialization();
                FillDoctor();
            }
            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdSave.IsEnabled = false;
            //    lnkAddDiagnosis.IsEnabled = false;
            //    lnkNewDiagnosis.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
                lnkAddDiagnosis.IsEnabled = false;
                lnkNewDiagnosis.IsEnabled = false;
            }
            //End

            //cmdSave.IsEnabled = IsEnabledControl;
            //lnkAddDiagnosis.IsEnabled = IsEnabledControl;
            //lnkNewDiagnosis.IsEnabled = IsEnabledControl;
            //dgDiagnosisList.IsReadOnly = !IsEnabledControl;
            dgDiagnosisList.Columns[0].IsReadOnly = !IsEnabledControl;

            TotalRows = CurrentStartRow = 0;
            dateConverter = new DateConverter();

            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8)
            {
                dgDiagnosisList.Columns[9].Visibility = Visibility.Collapsed;
                dgDiagnosisList.Columns[14].Visibility = Visibility.Collapsed;
            }

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

        bool IsSelectDonor = false;
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
            //if (this.CurrentVisit.VisitTypeID == 2)
            //{
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //}
            //else
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

                            //added by neena
                            OBj.PlanTreatmentList = PlannedTreatment;
                            OBj.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == 0);
                            OBj.Priority = Priority;
                            OBj.SelectedPriority = Priority.FirstOrDefault(p => p.ID == 0);
                            OBj.IsArtStatus = true;
                            if (IsArt == true)
                                OBj.IsPACEn = true;
                            else
                                OBj.IsPACEn = false;
                            OBj.PlanTherapyId = PlanTherapyId;
                            OBj.PlanTherapyUnitId = PlanTherapyUnitId;

                            //if (CheckARTEnabled == true)
                            //    OBj.ISARTENABLED = false;
                            //else if (CheckARTEnabled == false)
                            //    OBj.ISARTENABLED = true;
                            //

                            //OBj.ISARTENABLED = false;
                            OBj.IsEnabledArt = true;
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

                        //added by neena
                        OBj.PlanTreatmentList = PlannedTreatment;
                        OBj.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == 0);
                        OBj.Priority = Priority;
                        OBj.SelectedPriority = Priority.FirstOrDefault(p => p.ID == 0);
                        OBj.IsArtStatus = true;
                        if (IsArt == true)
                            OBj.IsPACEn = true;
                        else
                            OBj.IsPACEn = false;
                        OBj.PlanTherapyId = PlanTherapyId;
                        OBj.PlanTherapyUnitId = PlanTherapyUnitId;

                        //if (CheckARTEnabled == true)
                        //    OBj.ISARTENABLED = false;
                        //else if(CheckARTEnabled==false)
                        //    OBj.ISARTENABLED=true;
                        //

                        //OBj.ISARTENABLED = false;
                        OBj.IsEnabledArt = true;
                        DiagnosisList.Add(OBj);
                    }
                }
                if (DiagnosisList.Count > 0)
                {
                    var item = DiagnosisList.Where(x => x.ArtEnabled == true);
                    if (item.ToList().Count > 0)
                    {
                        var singleOne = DiagnosisList.Single(x => x.ArtEnabled == true);
                        if (singleOne != null)
                        {
                            //singleOne.ISARTENABLED = true;
                            singleOne.ARTENABLE = false;
                            singleOne.PlanTreatmentEnabled = true;

                            var theRest = DiagnosisList.Where(x => x.ArtEnabled == false);
                            theRest.ToList().ForEach(x => x.ARTENABLE = true);
                            //theRest.ToList().ForEach(x => x.ISARTENABLED = false);
                        }
                    }
                    else
                    {
                        DiagnosisList.Where(x => x.ArtEnabled == false).ToList().ForEach(x => x.ARTENABLE = false);
                    }
                }

                if (GenderID == 1)
                {
                    DiagnosisList.ToList().ForEach(x => x.ARTENABLE = true);
                }


                //if (CheckARTEnabled)
                //{
                //    //DiagnosisList.Where(x => x.ArtEnabled == true).ToList().ForEach(x => { x.ISARTENABLED = false; x.PlanTreatmentEnabled = false; });
                //    DiagnosisList.Where(x => x.ArtEnabled == true).ToList().ForEach(x => { x.IsEnabledArt = false; x.PlanTreatmentEnabled = false; });
                //}
                //else
                //{
                //    if (DiagnosisList.Count > 1)
                //    {
                //        var item = DiagnosisList.Where(x => x.ArtEnabled == true);
                //        if (item.ToList().Count > 1)
                //        {
                //            var singleOne = DiagnosisList.Single(x => x.ArtEnabled == true);
                //            if (singleOne != null)
                //            {
                //                //singleOne.ISARTENABLED = true;
                //                singleOne.ARTENABLE = false;
                //                singleOne.PlanTreatmentEnabled = true;

                //                var theRest = DiagnosisList.Where(x => x.ArtEnabled == false);
                //                theRest.ToList().ForEach(x => x.ARTENABLE = true);
                //                //theRest.ToList().ForEach(x => x.ISARTENABLED = false);
                //            }
                //        }                      
                //    }
                //    else
                //        DiagnosisList.Where(x => x.ArtEnabled == false).ToList().ForEach(x => x.ARTENABLE = false);
                //   // DiagnosisList.Where(x => x.ArtEnabled == false).ToList().ForEach(x => x.ISARTENABLED = true);
                //    //DiagnosisList.Where(x => x.ArtEnabled == true).ToList().ForEach(x => x.PlanTreatmentEnabled = true);
                //}
                dgDiagnosisList.ItemsSource = null;
                dgDiagnosisList.ItemsSource = DiagnosisList;
                //dgDiagnosisList.UpdateLayout();
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

        //added by neena
        List<MasterListItem> PlannedTreatment = null;
        private void fillPlannedTreatment()
        {
            PlannedTreatment = new List<MasterListItem>();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    PlannedTreatment = objList.DeepCopy();

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8)
                    {
                        PlannedTreatment.Clear();
                        PlannedTreatment.Add(new MasterListItem(0, "-- Select --"));
                        PlannedTreatment.AddRange(objList.Where(x => x.ID == 11).ToList());
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 7)
                    {
                        PlannedTreatment.Clear();
                        PlannedTreatment.AddRange(objList.Where(x => x.ID != 11).ToList());
                    }

                }
                FillReferenceType();
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                //    fillExternalStimulation();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        //

        private bool checkvalidation()
        {
            bool result = true;
            bool blCheckArt = false;
            bool blPlanTreatment = false;
            bool priviousArtClose = false;


            //if (DataList.Count > 0)
            //{
            //    foreach (var item in DataList)
            //    {
            //        if (item.ArtEnabled == true && item.IsClosedEnabled == false)
            //            priviousArtClose = true;
            //        else if (item.ArtEnabled == true && item.IsClosedEnabled == true)
            //            priviousArtClose = false;
            //    }
            //}
            //else if (blCheckArt)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //               new MessageBoxControl.MessageBoxChildWindow("", "Art Cycle is not closed yet.. you can not add other cycle.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW1.Show();
            //    result = false;
            //    //DiagnosisList.RemoveAt(dgDiagnosisList.SelectedIndex);
            //}
            //if (item.ArtEnabled == true)
            //    blCheckArt = true;
            //if (item.ArtEnabled == true)
            //    priviousArtClose = true;
            //else
            //    priviousArtClose = false;
            //foreach (var item in DiagnosisList)
            //{
            //    if (item.ArtEnabled == true && item.SelectedPlanTreatmentId.ID == 0)
            //        blPlanTreatment = true;
            //}

            //if (DiagnosisList.Count >= 2)
            //{
            //    foreach (var item in DiagnosisList)
            //    {
            //        if (item.ArtEnabled == true)
            //            blCheckArt = true;
            //    }
            //}
            //foreach (var item in DiagnosisList)
            //{
            //    if (item.SelectedPlanTreatmentId.ID == 11)
            //    {
            //        if (item.PatientName == null || item.MRNO == null)
            //            IsSelectDonor = true;

            //    }
            //}


            if (DiagnosisList.Count > 0)
            {
                var item1 = DataList.Where(x => x.ArtEnabled == true).Where(x => x.IsClosedEnabled == false);
                if (item1.ToList().Count > 0)
                    priviousArtClose = true;
                else
                    priviousArtClose = false;

                if (priviousArtClose)
                {
                    var SelectArt = DiagnosisList.Where(x => x.ArtEnabled == true);
                    if (SelectArt.ToList().Count > 0)
                        priviousArtClose = true;
                    else
                        priviousArtClose = false;
                }

                var itemPlanTreatment = DiagnosisList.Where(x => x.ArtEnabled == true).Where(x => x.SelectedPlanTreatmentId.ID == 0);
                if (itemPlanTreatment.ToList().Count > 0)
                    blPlanTreatment = true;
                else
                    blPlanTreatment = false;

                var itemDonor = DiagnosisList.Where(x => x.SelectedPlanTreatmentId.ID == 12).Where(x => x.DonorEnabled == true).ToList();
                if (itemDonor.Count > 0)
                {
                    var itemIsSelectDonor = itemDonor.Where(x => x.PatientName == null).Where(x => x.MRNO == null);
                    if (itemIsSelectDonor.ToList().Count > 0)
                        IsSelectDonor = true;
                    else
                        IsSelectDonor = false;
                }


                if (priviousArtClose)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Art Cycle is not closed yet.. you can not add other cycle.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                    //DiagnosisList.RemoveAt(dgDiagnosisList.SelectedIndex);
                }

                else if (blPlanTreatment)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select plan treatment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                    msgW1.Show();
                    result = false;
                }
                else if (IsSelectDonor)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please Select Donor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);
                    msgW1.Show();
                    result = false;
                }


            }


            return result;
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                if (dgDiagnosisList.SelectedItem != null)
                {
                    clsEMRAddDiagnosisVO obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                    obj.IsSelectPatient = true;
                    obj.PlanTreatmentEnabled = true;
                }
            }
        }

        public void SaveDiagnosis()
        {
            if (checkvalidation())
            {
                string strMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                WaitIndicator IndicatiorDiag = new WaitIndicator();
                try
                {
                    IndicatiorDiag.Show();
                    List<clsEMRAddDiagnosisVO> SavePatientDiagnosiList = new List<clsEMRAddDiagnosisVO>();

                    foreach (clsEMRAddDiagnosisVO item in DiagnosisList)
                    {
                        item.PlanTreatmentId = item.SelectedPlanTreatmentId.ID;
                        item.PriorityId = item.SelectedPriority.ID;
                        if (item.SelectedPlanTreatmentId.ID == 12) //11
                            if (item.DonorEnabled)
                                item.IsDonorCycle = true;
                        if (item.IsDonorCycle == true)
                        {
                            item.DonorID = SelectedPatientDonorObj.PatientID;
                            item.DonarUnitID = SelectedPatientDonorObj.UnitId;
                        }

                        if (SelectedSurrogatedPatient != null && SelectedSurrogatedPatient.Count > 0 && item.SurrogateMRNO != null)
                            item.IsSurrogate = true;

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
                    //added by neena
                    if (SelectedSurrogatedPatient != null && SelectedSurrogatedPatient.Count > 0)
                        BizActionVO.objSurrogatedPatient = SelectedSurrogatedPatient;
                    //
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
        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            //if (this.CurrentVisit.VisitTypeID == 2)
            //    winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            //else
            winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;

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

        WaitIndicator IndicatiorGet = new WaitIndicator();
        bool CheckARTEnabled = false;
        private void GetPatientDiagnosis()
        {
            try
            {

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

                                //added by neena
                                item.PlanTreatmentList = PlannedTreatment;
                                if (Convert.ToInt64(item.PlanTreatmentId) > 0)
                                {
                                    item.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PlanTreatmentId));
                                }
                                else
                                {
                                    item.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == 0);
                                }

                                item.Priority = Priority;
                                if (Convert.ToInt64(item.PriorityId) > 0)
                                {
                                    item.SelectedPriority = Priority.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PriorityId));
                                }
                                else
                                {
                                    item.SelectedPriority = Priority.FirstOrDefault(p => p.ID == 0);
                                }

                                if (item.ArtEnabled == true && item.BilledEnabled == true)
                                {
                                    item.PlanTreatmentEnabled = false;
                                    item.IsEnabled = true;
                                }
                                else if (item.PACEnabled == true && item.BilledEnabled == true)
                                    item.IsEnabled = true;
                                else
                                {
                                    item.PlanTreatmentEnabled = false;
                                    item.IsEnabled = false;
                                }

                                //if (item.ArtEnabled == true)
                                //    CheckARTEnabled = true;


                                //
                                if (item.ArtEnabled == true)
                                {
                                    CheckARTEnabled = true;
                                    dgDiagnosisList.Columns[5].Visibility = Visibility.Visible;
                                    dgDiagnosisList.Columns[6].Visibility = Visibility.Visible;
                                    dgDiagnosisList.Columns[7].Visibility = Visibility.Visible;
                                }

                                if (item.SelectedPlanTreatmentId.ID == 12) //11
                                {
                                    if (item.IsDonorCycle)
                                    {
                                        item.DonorEnabled = true;
                                        item.IsSelectPatient = false;
                                        dgDiagnosisList.Columns[9].Visibility = Visibility.Visible;
                                        dgDiagnosisList.Columns[10].Visibility = Visibility.Visible;
                                        //dgDiagnosisList.Columns[11].Visibility = Visibility.Visible;
                                        dgDiagnosisList.Columns[12].Visibility = Visibility.Visible;
                                    }
                                }

                                if (item.SelectedPlanTreatmentId.ID == 11)
                                    dgDiagnosisList.Columns[13].Visibility = Visibility.Visible;
                                if (item.IsSurrogate)
                                    dgDiagnosisList.Columns[15].Visibility = Visibility.Visible;

                                DiagnosisList.Add(item);
                            }

                            if (DiagnosisList.Count > 0)
                            {
                                var item = DiagnosisList.Where(x => x.ArtEnabled == true);
                                if (item.ToList().Count > 0)
                                {
                                    var singleOne = DiagnosisList.Single(x => x.ArtEnabled == true);
                                    if (singleOne != null)
                                    {
                                        //singleOne.ISARTENABLED = true;
                                        if (singleOne.ArtEnabled == true && singleOne.BilledEnabled == true)
                                        {
                                            singleOne.PlanTreatmentEnabled = false;
                                            singleOne.IsEnabled = true;
                                            singleOne.ARTENABLE = true;
                                        }
                                        else
                                        {
                                            singleOne.ARTENABLE = false;
                                            singleOne.PlanTreatmentEnabled = true;
                                        }

                                        var theRest = DiagnosisList.Where(x => x.ArtEnabled == false);
                                        theRest.ToList().ForEach(x => x.ARTENABLE = true);
                                        //theRest.ToList().ForEach(x => x.ISARTENABLED = false);
                                    }
                                }
                                else
                                {
                                    DiagnosisList.Where(x => x.ArtEnabled == false).ToList().ForEach(x => x.ARTENABLE = false);
                                }
                            }

                            if (GenderID == 1)
                            {
                                DiagnosisList.ToList().ForEach(x => x.ARTENABLE = true);
                            }
                        }
                        dgDiagnosisList.ItemsSource = null;
                        dgDiagnosisList.ItemsSource = DiagnosisList;
                        dgDiagnosisList.UpdateLayout();
                        IndicatiorGet.Close();

                        GetPatientDiagnosisHistory();
                    }
                    else
                    {
                        // string sMsgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
                        string sMsgText = "";
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
                                    item.Datetime = String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor.Trim() + " - " + CurrentVisit.DoctorSpecialization);

                                //DiagnosisListHistory.Add(item);

                                //added by neena
                                item.PlanTreatmentList = PlannedTreatment;
                                if (Convert.ToInt64(item.PlanTreatmentId) > 0)
                                {
                                    item.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PlanTreatmentId));
                                }
                                else
                                {
                                    item.SelectedPlanTreatmentId = PlannedTreatment.FirstOrDefault(p => p.ID == 0);
                                }

                                item.Priority = Priority;
                                if (Convert.ToInt64(item.PriorityId) > 0)
                                {
                                    item.SelectedPriority = Priority.FirstOrDefault(p => p.ID == Convert.ToInt64(item.PriorityId));
                                }
                                else
                                {
                                    item.SelectedPriority = Priority.FirstOrDefault(p => p.ID == 0);
                                }
                                //
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
                        //string sMsgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
                        string sMsgText = "";
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
            AddWin.lblDiagnosisName.Text = "Procedure Name";// ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("lblProcedureName");
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
                    fillPlannedTreatment();

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

        long CycleID = 0;
        private void cmbPlanTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (AutoCompleteBox)sender;
            if (dgDiagnosisList.SelectedItem != null)
            {
                CycleID = ((MasterListItem)comboBox.SelectedItem).ID;
                clsEMRAddDiagnosisVO obj;
                obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                obj.IsSelectSurrogate = true;
                // long DecisionID = ((cls_IVFDashboard_GraphicalRepresentationVO)dgDayList.SelectedItem).SelectedDecision.ID;
                if (CycleID == 11)
                {
                    dgDiagnosisList.Columns[13].Visibility = Visibility.Visible;
                    obj.CoupleMRNO = CurrentVisit.CoupleMRNO;
                }
                else if (CycleID == 12)
                    dgDiagnosisList.Columns[9].Visibility = Visibility.Visible;
                else
                {
                    dgDiagnosisList.Columns[9].Visibility = Visibility.Collapsed;
                    dgDiagnosisList.Columns[10].Visibility = Visibility.Collapsed;
                    // dgDiagnosisList.Columns[11].Visibility = Visibility.Collapsed;
                    dgDiagnosisList.Columns[12].Visibility = Visibility.Collapsed;
                    dgDiagnosisList.Columns[13].Visibility = Visibility.Collapsed;
                    obj.PatientName = "";
                    obj.MRNO = "";
                }

                //clsEMRAddDiagnosisVO obj;
                //if (CycleID == 11)
                //{
                //    dgDiagnosisList.Columns[9].Visibility = Visibility.Visible;
                //    obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                //    obj.IsSelectPatient = true;
                //}
                //else
                //    dgDiagnosisList.Columns[9].Visibility = Visibility.Collapsed;
            }
        }

        //added by neena
        private void chkArtEnabled_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //if (chk.IsChecked == true)
            //{
            //    dgDiagnosisList.Columns[5].Visibility = Visibility.Visible;
            //    dgDiagnosisList.Columns[6].Visibility = Visibility.Visible;
            //    dgDiagnosisList.Columns[7].Visibility = Visibility.Visible;
            //}

        }

        private void chkArtEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //if (chk.IsChecked == false)
            //{
            //    dgDiagnosisList.Columns[5].Visibility = Visibility.Collapsed;
            //    dgDiagnosisList.Columns[6].Visibility = Visibility.Collapsed;
            //    dgDiagnosisList.Columns[7].Visibility = Visibility.Collapsed;
            //}
        }

        private void chkArtEnabledP_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                dgPreviousDiagnosisList.Columns[7].Visibility = Visibility.Visible;
                dgPreviousDiagnosisList.Columns[8].Visibility = Visibility.Visible;
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8)
                {
                    dgDiagnosisList.Columns[9].Visibility = Visibility.Collapsed;
                }
                else
                    dgPreviousDiagnosisList.Columns[9].Visibility = Visibility.Visible;
            }
        }

        private void chkArtEnabledP_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == false)
            {
                dgPreviousDiagnosisList.Columns[7].Visibility = Visibility.Collapsed;
                dgPreviousDiagnosisList.Columns[8].Visibility = Visibility.Collapsed;
                dgPreviousDiagnosisList.Columns[9].Visibility = Visibility.Collapsed;
            }
        }

        //private void chkPACEnabled_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void chkPACEnabled_Unchecked(object sender, RoutedEventArgs e)
        //{

        //}

        private void chkPACEnabled_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < DiagnosisList.Count; i++)
            {
                if (i == dgDiagnosisList.SelectedIndex)
                {
                    if (chk.IsChecked == true)
                        DiagnosisList[i].PACEnabled = true;
                    else
                        DiagnosisList[i].PACEnabled = false;
                }
            }
        }

        private void chkArtEnabled_Click(object sender, RoutedEventArgs e)
        {
            clsEMRAddDiagnosisVO obj;
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < DiagnosisList.Count; i++)
            {
                if (i == dgDiagnosisList.SelectedIndex)
                {
                    obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                    if (chk.IsChecked == true)
                    {
                        var singleOne = DiagnosisList.Single(x => x.Code == ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).Code);
                        singleOne.ARTENABLE = false;
                        singleOne.PlanTreatmentEnabled = true;

                        var theRest = DiagnosisList.Where(x => x.Code != ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).Code);
                        theRest.ToList().ForEach(x => { x.ARTENABLE = true; x.PlanTreatmentEnabled = false; });
                        //theRest.ToList().ForEach(x => { x.ISARTENABLED = false; x.PlanTreatmentEnabled = false; });

                        dgDiagnosisList.Columns[5].Visibility = Visibility.Visible;
                        dgDiagnosisList.Columns[6].Visibility = Visibility.Visible;
                        dgDiagnosisList.Columns[7].Visibility = Visibility.Visible;
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 8)
                        {
                            dgDiagnosisList.Columns[14].Visibility = Visibility.Collapsed;
                            obj.IsSelectSurrogate = false;
                        }
                        else
                        {
                            dgDiagnosisList.Columns[14].Visibility = Visibility.Visible;
                            obj.IsSelectSurrogate = true;
                        }
                    }
                    else if (chk.IsChecked == false)
                    {
                        //DiagnosisList.Where(x => x.Code == ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem).Code).ToList().ForEach(x => x.ISARTENABLED = true);
                        //DiagnosisList.ToList().ForEach(x => x.ISARTENABLED = true);
                        //var singleOne = DiagnosisList.Single(x => x.ArtEnabled == true);
                        //singleOne.PlanTreatmentEnabled = true;

                        //var theRest = DiagnosisList.Where(x => x.ArtEnabled == false);
                        //theRest.ToList().ForEach(x => x.ISARTENABLED = true);

                        DiagnosisList.ToList().ForEach(x => x.ARTENABLE = false);

                        dgDiagnosisList.Columns[5].Visibility = Visibility.Collapsed;
                        dgDiagnosisList.Columns[6].Visibility = Visibility.Collapsed;
                        dgDiagnosisList.Columns[7].Visibility = Visibility.Collapsed;
                        dgDiagnosisList.Columns[14].Visibility = Visibility.Collapsed;
                        obj.IsSelectSurrogate = false;
                        obj.SurrogateMRNO = "";
                    }
                }



            }

        }

        private void hlbSelectPatient_Click(object sender, RoutedEventArgs e)
        {
            //FrmPatientListForDashboard WinPatient = new FrmPatientListForDashboard();
            WinPatient = new FrmPtListForDashboard();
            WinPatient.PatientCategoryID = 8;
            WinPatient.OnSaveButton_Click += new RoutedEventHandler(WinPatient_OnSaveButton_Click);
            WinPatient.Show();
        }

        public clsPatientGeneralVO SelectedPatientDonorObj;
        void WinPatient_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            if (WinPatient.SelectedPatientObj != null)
            {
                if (dgDiagnosisList.SelectedItem != null)
                {
                    clsEMRAddDiagnosisVO obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                    SelectedPatientDonorObj = new clsPatientGeneralVO();
                    SelectedPatientDonorObj = WinPatient.SelectedPatientObj;
                    //obj.PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                    //obj.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    obj.PatientName = SelectedPatientDonorObj.PatientName;
                    obj.MRNO = SelectedPatientDonorObj.MRNo;
                    // dgDiagnosisList.Columns[11].Visibility = Visibility.Visible;
                    dgDiagnosisList.Columns[12].Visibility = Visibility.Visible;
                    obj.IsSelectPatient = true;
                    obj.PlanTreatmentEnabled = true;
                    obj.IsSelectSurrogate = true;
                }

            }
        }

        private void chkDonor_Click(object sender, RoutedEventArgs e)
        {
            clsEMRAddDiagnosisVO obj;
            CheckBox chk = sender as CheckBox;
            for (int i = 0; i < DiagnosisList.Count; i++)
            {
                if (i == dgDiagnosisList.SelectedIndex)
                {
                    obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                    if (chk.IsChecked == true)
                    {
                        dgDiagnosisList.Columns[10].Visibility = Visibility.Visible;
                        obj.IsSelectPatient = true;

                    }
                    else if (chk.IsChecked == false)
                    {
                        dgDiagnosisList.Columns[10].Visibility = Visibility.Collapsed;
                        //dgDiagnosisList.Columns[11].Visibility = Visibility.Collapsed;
                        dgDiagnosisList.Columns[12].Visibility = Visibility.Collapsed;
                        obj.PatientName = "";
                        obj.MRNO = "";
                    }
                }
            }
        }

        //FrmPatientListForDashboard WinPatient = null;     
        FrmPtListForDashboard WinPatient = null;
        private void hlbSelectSurrogate_Click(object sender, RoutedEventArgs e)
        {
            //WinPatient = new FrmPatientListForDashboard();
            WinPatient = new FrmPtListForDashboard();
            WinPatient.IsSurrogate = true;
            WinPatient.PatientCategoryID = 10;
            WinPatient.SelectedCheckedPatient = SelectedSurrogatedPatient;
            WinPatient.OnSaveButton_Click += new RoutedEventHandler(WinPatientSurrogate_OnSaveButton_Click);
            WinPatient.OnCloseButton_Click += new RoutedEventHandler(WinPatient_OnCloseButton_Click);
            WinPatient.Show();
        }

        void WinPatient_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {
            //if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            //{
            if (dgDiagnosisList.SelectedItem != null)
            {
                clsEMRAddDiagnosisVO obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                obj.IsSelectSurrogate = true;
                obj.PlanTreatmentEnabled = true;
            }
            //}

        }

        StringBuilder strMrNo = new StringBuilder();
        public List<clsPatientGeneralVO> SelectedSurrogatedPatient = null;
        void WinPatientSurrogate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedSurrogatedPatient = new List<clsPatientGeneralVO>();
            //if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            if (WinPatient.Selectedpatient != null)
            {
                if (dgDiagnosisList.SelectedItem != null)
                {
                    clsEMRAddDiagnosisVO obj = ((clsEMRAddDiagnosisVO)dgDiagnosisList.SelectedItem);
                    dgDiagnosisList.Columns[15].Visibility = Visibility.Visible;
                    obj.IsSelectSurrogate = true;
                    obj.PlanTreatmentEnabled = true;
                    strMrNo = new StringBuilder();

                    foreach (var item in WinPatient.Selectedpatient)
                    {
                        if (strMrNo.ToString().Length > 0)
                            strMrNo.Append(",");
                        strMrNo.Append(item.MRNo);
                        SelectedSurrogatedPatient.Add(item);
                    }
                    obj.SurrogateMRNO = strMrNo.ToString();

                    obj.PlanTreatmentEnabled = true;
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DiagnosisList.Clear();
        }

        //

    }
}

