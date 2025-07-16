using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using System.ComponentModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.ValueObjects.Master;
namespace EMR
{
    public partial class frmPatientEMRDashboard : UserControl
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool DelayNotification { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (DelayNotification) return;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region Paging
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataListCurrentMedication { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        void DataListCurrentMedication_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetCurrentMedications();
        }
        public PagedSortableCollectionView<clsEMRAddDiagnosisVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsEMRAddDiagnosisVO> DataListProcedure { get; private set; }
        public PagedSortableCollectionView<clsVitalChartVO> DataListVital { get; private set; }

        public int DataListPageSizeforDiagnosis
        {
            get
            {
                return DataListProcedure.PageSize;
            }
            set
            {
                if (value == DataListProcedure.PageSize) return;
                DataListProcedure.PageSize = value;
            }
        }
        public int DataListPageSizeforVital
        {
            get
            {
                return DataListVital.PageSize;
            }
            set
            {
                if (value == DataListVital.PageSize) return;
                DataListVital.PageSize = value;
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetProcedures();
        }

        void DataListVital_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetVitals();
        }

        #endregion

        #region Data Members
        bool IsPageLoded = false;
        List<clsEMRAddDiagnosisVO> DiagnosisList = null;
        List<clsEMRVitalsVO> VitalsList = null;
        List<clsPatientPrescriptionDetailVO> MedicationList;
        List<clsDoctorSuggestedServiceDetailVO> ReferralList;
        PalashDynamics.Converters.DateConverter objConverter;
        List<MasterListItem> Priority;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return PageSize;
            }
            set
            {
                RaisePropertyChanged("PageSize");
            }
        }
        public clsVisitVO CurrentVisit { get; set; }
        public clsPatientGeneralVO SelectedPatient { get; set; }
        #endregion

        #region Constructor
        public frmPatientEMRDashboard()
        {
            InitializeComponent();
            PageSize = 10;

            DiagnosisList = new List<clsEMRAddDiagnosisVO>();
            VitalsList = new List<clsEMRVitalsVO>();
            MedicationList = new List<clsPatientPrescriptionDetailVO>();
            ReferralList = new List<clsDoctorSuggestedServiceDetailVO>();
            SelectedPatient = new clsPatientGeneralVO();
            this.Loaded += new RoutedEventHandler(frmPatientEMRDashboard_Loaded);
        }
        #endregion

        #region Loaded
        void frmPatientEMRDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded == false)
            {
                FillReferenceType();

                DataList = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 15;

                DataListProcedure = new PagedSortableCollectionView<clsEMRAddDiagnosisVO>();
                DataListProcedure.OnRefresh += new EventHandler<RefreshEventArgs>(DataListProcedure_OnRefresh);
                DataListPageSizeforDiagnosis = 15;

                DataListVital = new PagedSortableCollectionView<clsVitalChartVO>();
                DataListVital.OnRefresh += new EventHandler<RefreshEventArgs>(DataListVital_OnRefresh);
                DataListPageSizeforVital = 15;

                objConverter = new DateConverter();
                SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                GetVitals();
                //GetCurrentMedications();
               // GetProcedures();
                GetDiagnosis();
                IsPageLoded = true;

            }
        }

        void DataListProcedure_OnRefresh(object sender, RefreshEventArgs e)
        {
            //throw new NotImplementedException();
            GetDiagnosis();
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
                    PlannedTreatment = objList;
                    GetProcedures();

                }
                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }
                //    fillExternalStimulation();
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

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

                            fillPlannedTreatment();
                        }

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
        //
        #endregion

        #region Maximize Events
        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {
        }

        private void PatientVitals_Maximized(object sender, EventArgs e)
        {
        }

        private void PatientReferral_Maximized(object sender, EventArgs e)
        {
        }

        private void PatientCurrentMedication_Maximized(object sender, EventArgs e)
        {
        }

        private void PatientPhysicalExam_Maximized(object sender, EventArgs e)
        {
        }

        private void PatientDiagnosis_Maximized(object sender, EventArgs e)
        {
        }

        #endregion

        #region Private MEthod
        private void GetVitals()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                try
                {
                    List<clsVitalChartVO> VitalChartList = new List<clsVitalChartVO>();
                    clsGetPatientVitalChartBizActionVO BizAction = new clsGetPatientVitalChartBizActionVO();
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.UnitID = CurrentVisit.UnitId;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.IsFromDashBoard = true;
                    BizAction.StartRowIndex = DataListVital.PageIndex * DataListVital.PageSize;
                    BizAction.MaximumRows = DataListVital.PageSize;
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient clientBizActionObjPatientVitalChartData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    clientBizActionObjPatientVitalChartData.ProcessCompleted += (sBizActionObjPatientVitalChartData, argsBizActionObjPatientVitalChartData) =>
                    {
                        VitalChartList.Clear();
                        if (argsBizActionObjPatientVitalChartData.Result != null && ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).PatientVitalChartlst != null)
                        {
                            DataListVital.TotalItemCount = ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).TotalRows;
                            DataListVital.Clear();
                                foreach (var item in ((clsGetPatientVitalChartBizActionVO)argsBizActionObjPatientVitalChartData.Result).PatientVitalChartlst)
                                {
                                    if (item.BMI < Convert.ToDouble(17.0))
                                    {
                                        item.NutritionStatus = "SCEM";
                                    }
                                    else if (item.BMI > Convert.ToDouble(27.0))
                                    {
                                        item.NutritionStatus = "Obease";
                                    }
                                    else if (item.BMI >= Convert.ToDouble(17.0) && item.BMI <= Convert.ToDouble(18.4))
                                    {
                                        item.NutritionStatus = "MCEM";
                                    }
                                    else if (item.BMI >= Convert.ToDouble(18.5) && item.BMI <= Convert.ToDouble(25.0))
                                    {
                                        item.NutritionStatus = "Normal";
                                    }
                                    else if (item.BMI >= Convert.ToDouble(25.1) && item.BMI <= Convert.ToDouble(25.0))
                                    {
                                        item.NutritionStatus = "Overweight";
                                    }
                                    if (this.CurrentVisit.VisitTypeID == 2)
                                    {
                                        if (this.CurrentVisit.OPDIPD)
                                            if (!String.IsNullOrEmpty(CurrentVisit.Doctor))
                                            {
                                                CurrentVisit.Doctor = CurrentVisit.Doctor.Trim();
                                            }
                                        item.Datetime = Convert.ToString(String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor + " - " + CurrentVisit.DoctorSpecialization));
                                    }
                                    VitalChartList.Add(item);
                                    DataListVital.Add(item);
                                }
                            PagedCollectionView pcvVitals = new PagedCollectionView(VitalChartList);
                            //if (this.CurrentVisit.VisitTypeID == 2)
                            //{
                            //    //if (this.CurrentVisit.OPDIPD)
                            //    //{
                            //    //    pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                            //    //}
                            //    //else
                            //    //{
                            //        pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                            //    //}
                            //}
                            //else
                            //{
                                pcvVitals.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                            //}
                            dgPatientVitals.ItemsSource = pcvVitals;
                            dpgrPatientVitals.Source = DataListVital;
                        }
                        Indicatior.Close();
                    };
                    clientBizActionObjPatientVitalChartData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    clientBizActionObjPatientVitalChartData.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }
        private void GetProcedures()
        {
            try
            {
                List<clsEMRAddDiagnosisVO> DiagnosisListHistory = new List<clsEMRAddDiagnosisVO>();
                List<MasterListItem> DiagnosisTypeList = new List<MasterListItem>();
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetPatientDiagnosisDataBizActionVO BizAction = new clsGetPatientDiagnosisDataBizActionVO();
                if (SelectedPatient != null)
                {
                    BizAction.VisitID = SelectedPatient.VisitID;
                }
                BizAction.UnitID = CurrentVisit.UnitId;
                BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                BizAction.PatientID = SelectedPatient.PatientID;
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                BizAction.Ishistory = true;
                BizAction.ISDashBoard = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    Indicatior.Close();
                    if ((clsGetPatientDiagnosisDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DataList.TotalItemCount = ((clsGetPatientDiagnosisDataBizActionVO)arg.Result).TotalRows;
                            DataList.Clear();
                            foreach (var item in ((clsGetPatientDiagnosisDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
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
                                //
                              

                                DiagnosisListHistory.Add(item);
                                DataList.Add(item);
                                if (this.CurrentVisit.VisitTypeID == 2)
                                {
                                    if(this.CurrentVisit.OPDIPD)
                                        if (!String.IsNullOrEmpty(CurrentVisit.Doctor))
                                        {
                                            CurrentVisit.Doctor = CurrentVisit.Doctor.Trim();
                                        }
                                        item.Datetime = String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor + " - " + CurrentVisit.DoctorSpecialization);
                                }
                            }
                        }
                        PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(DiagnosisListHistory);
                        if (this.CurrentVisit.VisitTypeID == 2)
                        {
                            if (this.CurrentVisit.OPDIPD)
                            {
                                pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                            }
                            else
                            {
                                pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                            }
                        }
                        else
                        {
                            pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                        }
                        dgPatientDiagnosis.ItemsSource = null;
                        dgPatientDiagnosis.ItemsSource = pcvDiagnosisListHistory;
                        dgPatientDiagnosis.UpdateLayout();
                        pgrPatientDiagnosis.Source = DataList;
                        Indicatior.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }
        private void GetDiagnosis()
        {
            try
            {
                List<MasterListItem> DiagnosisTypeList = new List<MasterListItem>();
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetPatientProcedureDataBizActionVO BizAction = new clsGetPatientProcedureDataBizActionVO();
                if (SelectedPatient != null)
                {
                    BizAction.VisitID = SelectedPatient.VisitID;
                }
                BizAction.PatientID = SelectedPatient.PatientID;
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataListProcedure.PageIndex * DataListProcedure.PageSize;
                BizAction.MaximumRows = DataListProcedure.PageSize;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    Indicatior.Close();
                    if ((clsGetPatientProcedureDataBizActionVO)arg.Result != null)
                    {
                        if (((clsGetPatientProcedureDataBizActionVO)arg.Result).PatientDiagnosisDetails != null)
                        {
                            DataListProcedure.TotalItemCount = ((clsGetPatientProcedureDataBizActionVO)arg.Result).TotalRows;
                            DataListProcedure.Clear();
                            foreach (var item in ((clsGetPatientProcedureDataBizActionVO)arg.Result).PatientDiagnosisDetails)
                            {
                                item.SelectedDiagnosisType = new MasterListItem(item.SelectedDiagnosisType.ID, item.SelectedDiagnosisType.Description);
                                item.DiagnosisTypeList = DiagnosisTypeList;

                                if (item.SelectedDiagnosisType.ID == 2)
                                {
                                    item.Image = "../Icons/green.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 1)
                                {
                                    item.Image = "../Icons/yellow.png";
                                }
                                else if (item.SelectedDiagnosisType.ID == 3)
                                {
                                    item.Image = "../Icons/orange.png";
                                }
                                DataListProcedure.Add(item);
                                if (this.CurrentVisit.VisitTypeID == 2)
                                {
                                    if(this.CurrentVisit.OPDIPD)
                                        if (!String.IsNullOrEmpty(CurrentVisit.Doctor))
                                        {
                                            CurrentVisit.Doctor = CurrentVisit.Doctor.Trim();
                                        }
                                        item.Datetime = String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor + " - " + Convert.ToString(CurrentVisit.DoctorSpecialization));
                                }
                            }
                        }
                        PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(DataListProcedure);
                        //if (this.CurrentVisit.VisitTypeID == 2)
                        //{
                        //    if (this.CurrentVisit.OPDIPD)
                        //    {
                        //        pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                        //    }
                        //    else
                        //    {
                        //        pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                        //    }
                        //}
                        //else
                        //{
                            pcvDiagnosisListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", objConverter));
                        //}
                        dgPatDiagnosis.ItemsSource = null;
                        dgPatDiagnosis.ItemsSource = pcvDiagnosisListHistory;
                        dgPatDiagnosis.UpdateLayout();
                        pgrPatDiagnosis.Source = DataListProcedure;
                        Indicatior.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void GetCurrentMedications()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                clsGetPatientCurrentMedicationDetailsBizActionVO BizActionCurMed = new clsGetPatientCurrentMedicationDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizActionCurMed.VisitID = CurrentVisit.ID;
                    BizActionCurMed.DoctorID = CurrentVisit.DoctorID;
                    BizActionCurMed.PatientID = CurrentVisit.PatientId;
                    BizActionCurMed.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionCurMed.UnitID = CurrentVisit.UnitId;
                }
                BizActionCurMed.IsPrevious = false;
                BizActionCurMed.IsFromPresc = false;
                BizActionCurMed.IsOPDIPD = CurrentVisit.OPDIPD;
                BizActionCurMed.PagingEnabled = true;
                BizActionCurMed.StartRowIndex = DataListCurrentMedication.PageIndex * DataListCurrentMedication.PageSize;
                BizActionCurMed.MaximumRows = DataListCurrentMedication.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList != null)
                        {
                            DataListCurrentMedication.Clear();
                            DataListCurrentMedication.TotalItemCount = ((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).TotalRows;
                            foreach (var item in ((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList)
                            {
                                DataListCurrentMedication.Add(item);
                                MedicationList.Add(item);
                            }
                        }
                        PagedCollectionView pcvMedicationListHistory = new PagedCollectionView(MedicationList);
                        pcvMedicationListHistory.GroupDescriptions.Add(new PropertyGroupDescription("AddedDateTime", objConverter));
                        //dgPatientCurrentMedication.ItemsSource = null;
                        //dgPatientCurrentMedication.ItemsSource = pcvMedicationListHistory;
                        //dgPatientCurrentMedication.UpdateLayout();
                        //dpgrPatientCurrentMedication.Source = DataListCurrentMedication;
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
        private void GetReferrals()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            try
            {
                IndicatiorGet.Show();
                clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = CurrentVisit.PatientUnitId;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if ((clsGetReferralDetailsBizActionVO)arg.Result != null)
                    {
                        if (((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
                        {
                            foreach (var item in ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail)
                            {
                                ReferralList.Add(item);
                            }
                        }
                        PagedCollectionView pcvReferralListHistory = new PagedCollectionView(MedicationList);
                        pcvReferralListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date"));
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                        string strMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");
                        ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                IndicatiorGet.Close();
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

        private void chkArtEnabled_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                dgPatientDiagnosis.Columns[5].Visibility = Visibility.Visible;
                dgPatientDiagnosis.Columns[6].Visibility = Visibility.Visible;
                dgPatientDiagnosis.Columns[7].Visibility = Visibility.Visible;
            }
        }

        private void chkArtEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                dgPatientDiagnosis.Columns[5].Visibility = Visibility.Collapsed;
                dgPatientDiagnosis.Columns[6].Visibility = Visibility.Collapsed;
                dgPatientDiagnosis.Columns[7].Visibility = Visibility.Collapsed;
            }
        }

        private void cmbPlanTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
