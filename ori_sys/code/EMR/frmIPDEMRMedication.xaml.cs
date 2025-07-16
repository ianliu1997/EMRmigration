using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Controls;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.SearchResultLists;
using PalashDynamics.ValueObjects.RSIJ;

namespace EMR
{
    public partial class frmIPDEMRMedication : UserControl
    {
        #region Data Members
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        DateConverter dateConverter;
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsEnableControl { get; set; }
        public string sErrorMsg = "Error ocurred while processing."; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
        WaitIndicator Indicatior = new WaitIndicator();
        ObservableCollection<clsPatientPrescriptionDetailVO> CurrentMedicationList;
        ObservableCollection<clsPatientPrescriptionDetailVO> PrescriptionList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
        List<FrequencyMaster> FreqList = new List<FrequencyMaster>();
        List<MasterListItem> ItemList = new List<MasterListItem>();
        List<MasterListItem> RouteList = new List<MasterListItem>();
        List<MasterListItem> InstructionList = new List<MasterListItem>();
        clsPatientPrescriptionDetailVO DeletePrescriptionList;
        List<clsPatientPrescriptionDetailVO> dgprvpriscriptionchangestatus = new List<clsPatientPrescriptionDetailVO>(); 
        public PagedSortableCollectionView<clsPatientPrescriptionDetailVO> DataList { get; private set; }
        public Boolean IsPageLoad { get; set; }
        #endregion

        #region Property
        ObservableCollection<clsPatientPrescriptionDetailVO> PastPrescriptionList { get; set; }
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

        public frmIPDEMRMedication()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsPatientPrescriptionDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
            this.Loaded += new RoutedEventHandler(frmEMRMedication_Loaded);
        }

        #region Fill Methods

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
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;

        }

      

        private void FetchPreviousPrescriptions()
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
                    }
                    else
                    {
                        BizAction.DoctorCode = CurrentVisit.EncounterListDoctorCode;
                    }
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.UnitID = CurrentVisit.UnitId;
                    BizAction.IsFromPresc = true;
                    BizAction.PagingEnabled = true;
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
                                        item.Datetime = String.Format(item.VisitDate.ToString("dd/MM/yyyy") + " - " + item.DoctorName.Trim() + " - " + item.DoctorSpec);
                                    dgprvpriscriptionchangestatus.Add(item);
                                }
                            }
                            PastPrescriptionList = new ObservableCollection<clsPatientPrescriptionDetailVO>(DataList);
                            PagedCollectionView pcvPastMEdications = new PagedCollectionView(PastPrescriptionList);

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
                                pcvPastMEdications.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
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
                clsGetPatientCPOEDetailsBizActionVO BizActionCPOE = new clsGetPatientCPOEDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizActionCPOE.VisitID = CurrentVisit.ID;
                    BizActionCPOE.DoctorID = CurrentVisit.DoctorID;
                    BizActionCPOE.PatientID = CurrentVisit.PatientId;
                    BizActionCPOE.IsOPDIPD = CurrentVisit.OPDIPD;
                    //BizActionCPOE.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionCPOE.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionCPOE.UnitID = CurrentVisit.UnitId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientCPOEDetailsBizActionVO)args.Result).PatientPrescriptionDetailList != null)
                            foreach (var item in ((clsGetPatientCPOEDetailsBizActionVO)args.Result).PatientPrescriptionDetailList)
                            {
                                item.FrequencyName = FreqList;
                                item.RouteName = RouteList;
                                item.InstructionName = InstructionList;
                                if ((item.SelectedFrequency == null || item.SelectedFrequency.Description == null || item.SelectedFrequency.Description == "-- Select --") && FreqList != null)
                                {
                                    item.SelectedFrequency = item.FrequencyName[0];
                                }
                                if ((item.SelectedInstruction == null || item.SelectedInstruction.Description == null || item.SelectedInstruction.Description == "-- Select --") && InstructionList != null)
                                {
                                    item.SelectedInstruction = item.InstructionName[0];
                                }
                                if ((item.SelectedRoute == null || item.SelectedRoute.Description == null || item.SelectedRoute.Description == "-- Select --") && RouteList != null)
                                {
                                    item.SelectedRoute = item.RouteName[0];
                                }
                                //item.FrequencyName = FreqList;
                                //item.RouteName = RouteList;
                                //item.InstructionName = InstructionList;
                                //item.Frequency = item.Frequency;
                                //item.Instruction = item.Instruction;
                                PrescriptionList.Add(item);
                            }
                        dgDrugList.ItemsSource = PrescriptionList;
                        dgDrugList.UpdateLayout();
                        if (IsEnableControl == false)
                        {
                            dgDrugList.IsReadOnly = true;
                        }
                        dgDrugList.SelectedIndex = -1;
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }

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
                        FillInstructionList();
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
                        FillFrequencyList();
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
                        ShowPrescriptionAndServices();
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

        private void ShowCurrentMedication()
        {
            try
            {
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
                //BizActionCurMed.PrescriptionID = obj.PrescriptionID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList != null)
                        {
                            foreach (var item in ((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList)
                            {
                                if (RouteList != null)
                                    item.RouteName = RouteList;
                                if (FreqList != null)
                                    item.FrequencyName = FreqList;
                                if (InstructionList != null)
                                    item.InstructionName = InstructionList;
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
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchPreviousPrescriptions();
        }

        void frmEMRMedication_Loaded(object sender, RoutedEventArgs e)
        {
            dateConverter = new DateConverter();
            if (CurrentMedicationList == null)
                CurrentMedicationList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
            if (!IsPageLoad)
            {
                FillRouteList();
                //ShowPrescriptionAndServices();
                FetchPreviousPrescriptions();
                IsPageLoad = true;
                if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
                {
                    spSpecDoctor.Visibility = Visibility.Collapsed;
                    this.IsEnableControl = false;
                }
                else
                {
                    if (CurrentVisit.ISIPDDischarge)
                    {
                        this.IsEnableControl = false;
                        cmdSave.IsEnabled = false;
                    }
                    spSpecDoctor.Visibility = Visibility.Visible;
                }
                    FillSpecialization();
                    FillDoctor();
                cmdSave.IsEnabled = IsEnableControl;
                cmdDrug.IsEnabled = IsEnableControl;
                cmdOtherDrug.IsEnabled = IsEnableControl;
                cmdCurrentMedication.IsEnabled = IsEnableControl;
            }
        }

        private void cmdCurrentMedication_Click(object sender, RoutedEventArgs e)
        {
            frmEMRPrescribedMedication Win = new frmEMRPrescribedMedication(false);
            dgDrugList.SelectedIndex = -1;
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
            Win.CurrentVisit = this.CurrentVisit;
            Win.OnAddButton_Click += new RoutedEventHandler(WinCurrentDrug_OnAddButton_Click);
            Win.Show();
        }

        void WinCurrentDrug_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmEMRPrescribedMedication)sender).DialogResult == true)
            {
                foreach (var item in (((frmEMRPrescribedMedication)sender).SelectedDrugList))
                {
                    if (PrescriptionList.Count > 0)
                    {
                        var item1 = from r in PrescriptionList
                                    where (r.DrugName.Trim() == item.DrugName.Trim()
                                   )
                                    select new clsPatientPrescriptionDetailVO
                                    {
                                        DrugID = r.DrugID,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                            OBj.DrugID = item.DrugID;
                            OBj.DrugCode = item.DrugCode;
                            OBj.DrugName = item.DrugName;
                            //OBj.FrequencyName = FreqList;
                            //OBj.RouteName = RouteList;
                            //OBj.SelectedRoute.ID = item.RouteID;
                            //OBj.SelectedRoute.Description = item.Route;
                            // OBj.Dose = item.Code;
                            //OBj.InstructionName = InstructionList;
                            // OBj.Frequency = item.SelectedFrequency.Description; ;
                            // OBj.Instruction = item.SelectedInstruction.Description;
                            OBj.Quantity = item.Quantity;
                            OBj.FrequencyName = FreqList;
                            OBj.RouteName = RouteList;
                            OBj.InstructionName = InstructionList;
                            OBj.Days = item.Days;
                            // OBj.SelectedRoute.Description = item.Route;
                            // OBj.SelectedFrequency.Description = item.Frequency;
                            // OBj.SelectedInstruction.Description = item.Instruction;

                            PrescriptionList.Add(OBj);
                        }
                    }
                    else
                    {
                        clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                        OBj.DrugID = item.DrugID;
                        OBj.DrugName = item.DrugName;
                        OBj.DrugCode = item.DrugCode;
                        //OBj.SelectedRoute.Description = item.Route;
                        //OBj.FrequencyName = FreqList;
                        //OBj.RouteName = RouteList;
                        //OBj.InstructionName = InstructionList;
                        //OBj.Frequency = item.SelectedFrequency.Description;
                        //OBj.Instruction = item.SelectedInstruction.Description;
                        OBj.Quantity = item.Quantity;
                        OBj.FrequencyName = FreqList;
                        OBj.RouteName = RouteList;
                        OBj.InstructionName = InstructionList;
                        OBj.Days = item.Days;
                        //OBj.SelectedRoute.Description = item.Route;
                        //OBj.SelectedFrequency.Description = item.Frequency;
                        //OBj.SelectedInstruction.Description = item.Instruction;
                        PrescriptionList.Add(OBj);
                    }
                }
                dgDrugList.ItemsSource = PrescriptionList;

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
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null && !String.IsNullOrEmpty(((TextBox)sender).Text))
            {
                clsPatientPrescriptionDetailVO SelectedDrug = (clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem;
                try
                {
                    double Days = Convert.ToDouble(((TextBox)sender).Text);
                    double Qty = 0;
                    if (FreqList != null)
                    {
                        //SelectedDrug
                        if (SelectedDrug != null && SelectedDrug.SelectedFrequency != null)
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

        private void lnkAddDrugList_Click(object sender, RoutedEventArgs e)
        {
            frmEMRMedDrugSelectionList WinDrugList = new frmEMRMedDrugSelectionList();
            WinDrugList.CurrentVisit = CurrentVisit;
            dgDrugList.SelectedIndex = -1;
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            }
            WinDrugList.Width = winEMR.ActualWidth * 0.8;
            WinDrugList.Height = winEMR.ActualHeight * 0.8;
            WinDrugList.OnAddButton_Click += new RoutedEventHandler(WinDrugList_OnAddButton_Click);
            WinDrugList.Show();
        }

        void WinDrugList_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmEMRMedDrugSelectionList)sender).DialogResult == true)
            {
                foreach (var item in (((frmEMRMedDrugSelectionList)sender).DrugList))
                {
                    if (PrescriptionList.Count > 0)
                    {
                        var item1 = from r in PrescriptionList
                                    where (r.DrugID  == item.ID
                                   )
                                    select new clsPatientPrescriptionDetailVO
                                    {
                                        DrugID = r.DrugID,
                                        Status = r.Status
                                    };
                        if (item1.ToList().Count == 0)
                        {
                            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                            OBj.DrugID = item.ID;
                            OBj.DrugName = item.Name;
                            OBj.FrequencyName = FreqList;
                            OBj.RouteName = RouteList;
                            //OBj.SelectedRoute.ID = item.FilterID;
                            //OBj.SelectedRoute.Description = item.PrintDescription;
                            OBj.Dose = item.Code;
                            OBj.InstructionName = InstructionList;
                            OBj.Rate = item.Value;
                            OBj.UOM = item.UOM;
                            OBj.UOMID = item.UOMID;
                            PrescriptionList.Add(OBj);
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Drug already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
                        OBj.DrugID = item.ID;
                        OBj.DrugName = item.Name;
                        //OBj.SelectedRoute.ID = item.FilterID;
                        //OBj.SelectedRoute.Description = item.PrintDescription;
                        OBj.Dose = item.Code;
                        OBj.FrequencyName = FreqList;
                        OBj.RouteName = RouteList;
                        OBj.InstructionName = InstructionList;
                        OBj.Rate = item.Value;
                        OBj.UOM = item.UOM;
                        OBj.UOMID = item.UOMID;
                        PrescriptionList.Add(OBj);
                    }
                }
                dgDrugList.ItemsSource = null;
                dgDrugList.ItemsSource = PrescriptionList;
                //dgServiceList.UpdateLayout();
                //dgServiceList.Focus();
            }
        }

        private void lnkAddOtherDrugList_Click(object sender, RoutedEventArgs e)
        {
            frmEMRMedDrugSelectionList WinDrugList = new frmEMRMedDrugSelectionList();
            WinDrugList.CurrentVisit = CurrentVisit;
            WinDrugList.IsInsuraceDrug = true;
            dgDrugList.SelectedIndex = -1;
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            }
            WinDrugList.Width = winEMR.ActualWidth * 0.8;
            WinDrugList.Height = winEMR.ActualHeight * 0.8;
            WinDrugList.IsOtherDrug = true;
            WinDrugList.OnAddButton_Click += new RoutedEventHandler(WinDrugList_OnAddButton_Click);
            WinDrugList.Show();
        }

        void winMedDrugOther_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            //if (((frmEMRMedicationDrugSelectionList)sender).DialogResult == true)
            //{
            //    foreach (var item in (((frmEMRMedicationDrugSelectionList)sender).SelectedDrugList))
            //    {
            //        if (PrescriptionList.Count > 0)
            //        {

            //            var item1 = from r in PrescriptionList
            //                        where (r.DrugName.Trim() == item.DrugName.Trim()
            //                       )
            //                        select new clsPatientPrescriptionDetailVO
            //                        {
            //                            DrugID = r.DrugID,
            //                            Status = r.Status
            //                        };
            //            if (item1.ToList().Count == 0)
            //            {
            //                clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
            //                OBj.DrugID = 0;
            //                OBj.DrugName = item.DrugName;
            //                OBj.DrugCode = item.DrugCode;
            //                OBj.FrequencyName = FreqList;
            //                OBj.RouteName = RouteList;
            //                OBj.InstructionName = InstructionList;
            //                OBj.IsOther = true;
            //                OBj.Rate = 0;
            //                PrescriptionList.Add(OBj);
            //            }
            //        }
            //        else
            //        {
            //            clsPatientPrescriptionDetailVO OBj = new clsPatientPrescriptionDetailVO();
            //            OBj.DrugID = 0;
            //            OBj.DrugName = item.DrugName;
            //            OBj.DrugCode = item.DrugCode;
            //            OBj.FrequencyName = FreqList;
            //            OBj.RouteName = RouteList;
            //            OBj.InstructionName = InstructionList;
            //            OBj.IsOther = true;
            //            OBj.Rate = 0;
            //            PrescriptionList.Add(OBj);
            //        }
            //    }
            //    dgDrugList.ItemsSource = PrescriptionList;
            //    dgDrugList.UpdateLayout();
            //}
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
            //MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //msgW.Show();
            if (PrescriptionList.Where(z => z.Quantity == 0).Any())
            {
                string strErrorMsg = "Please enter quantity greater than zero";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("QuantityGValidation_msg");
                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else if (PrescriptionList.Where(z => z.Days == null).Any())
            {
                string strErrorMsg = "Please enter Days"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("QuantityGValidation_msg");
                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else if (PrescriptionList.Where(z => z.Days == 0).Any())
            {
                string strErrorMsg = "Please enter Days Greater than 0"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("QuantityGValidation_msg");
                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            else
            {
                SaveMedication();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveMedication();
            }
        }

        private void SaveMedication()
        {
            try
            {
                if (PrescriptionList != null && PrescriptionList.Count > 0)
                {
                    clsAddUpdatePatientCurrentMedicationsBizActionVO BizActionObjCurrentMedication = new clsAddUpdatePatientCurrentMedicationsBizActionVO();
                    Indicatior.Show();
                    BizActionObjCurrentMedication.PatientID = CurrentVisit.PatientId;
                    BizActionObjCurrentMedication.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionObjCurrentMedication.VisitID = CurrentVisit.ID;
                    BizActionObjCurrentMedication.UnitID = CurrentVisit.UnitId;
                    //BizActionObjCurrentMedication.DoctorCode = CurrentVisit.DoctorCode;
                    BizActionObjCurrentMedication.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizActionObjCurrentMedication.DoctorID = CurrentVisit.DoctorID;
                    if (PrescriptionList.Count > 0)
                    {
                        BizActionObjCurrentMedication.PatientCurrentMedicationDetailList = PrescriptionList.ToList();
                    }
                    else
                        BizActionObjCurrentMedication.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();

                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient clientObjCurrentMedication = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    clientObjCurrentMedication.ProcessCompleted += (sObjPatientCurrentMedication, argsObjPatientCurrentMedication) =>
                    {
                        string strErrorMsg = DefaultValues.ResourceManager.GetString("ErrorMessage");

                        if (argsObjPatientCurrentMedication.Error == null && argsObjPatientCurrentMedication.Result != null)
                        {
                            Indicatior.Close();
                            if (((clsAddUpdatePatientCurrentMedicationsBizActionVO)argsObjPatientCurrentMedication.Result).SuccessStatus == 1)
                            {
                                //string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //    new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                //{
                                //    this.Content = null;
                                //    NavigateToNextMenu();
                                //};
                                //msgW1.Show();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                #region UpdateEncounterList
                                if (this.CurrentVisit.VisitTypeID == 2)
                                {
                                    frmIPDEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
                                    ContentControl ccPatientDetails = winEMR.ResultListContent as ContentControl;
                                    DisplayIPDPatientDetails winIPDPatientDetails = ccPatientDetails.Content as DisplayIPDPatientDetails;
                                    DataGrid dgEncounterList = winIPDPatientDetails.FindName("dgEncounterList") as DataGrid;
                                    clsPatientConsoleHeaderVO objPatientHeader = dgEncounterList.SelectedItem as clsPatientConsoleHeaderVO;
                                    objPatientHeader.IsPrescriptionRight = "Visible";
                                    objPatientHeader.IsPrescriptionwrong = "Collapsed";
                                    dgEncounterList.ItemsSource = null;
                                    dgEncounterList.ItemsSource = winIPDPatientDetails.MasterList;
                                }
                                #endregion
                              //  NavigateToNextMenu();
                            }
                            else
                            {
                                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
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
                    string sErrorMsg = "No details to save Medication";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("NoMedication_Msg");
                    ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
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

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (IsEnableControl == true && dgDrugList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Drug ?";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeleteDrugConfrm_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        DeletePrescriptionList = (clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem;
                        if (DeletePrescriptionList.ID > 0)
                        {
                            //DeleteCPOEMedicine(DeletePrescriptionList.ID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                        }
                        PrescriptionList.RemoveAt(dgDrugList.SelectedIndex);
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = PrescriptionList;
                        dgDrugList.UpdateLayout();
                        dgDrugList.Focus();
                        string sErrorMsg = "Record deleted Successfully.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("DeletVerify_Msg");
                        ShowMessageBox(sErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                msgWD.Show();
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

        private void chkMultipleDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            clsPatientPrescriptionDetailVO item = dgPreviousPrescriptions.SelectedItem as clsPatientPrescriptionDetailVO;

            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                if (PrescriptionList.Count > 0)
                {
                    var item1 = from r in PrescriptionList
                                where (r.DrugName.Trim() == item.DrugName.Trim()
                               )
                                select new clsPatientPrescriptionDetailVO
                                {
                                    DrugID = r.DrugID,
                                    Status = r.Status,
                                    PrvStatus = r.PrvStatus
                                };
                    if (item1.ToList().Count == 0)
                    {
                        PrescriptionList.Add(item);
                    }
                    else
                    {
                        item.PrvStatus = false;
                        ShowMessageBox("Drug Already exist", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    PrescriptionList.Add(item);
                }
                dgDrugList.ItemsSource = PrescriptionList;
            }
            else
            {
                if (PrescriptionList.Count > 0)
                {
                    var item1 = from r in PrescriptionList
                                where (r.DrugName.Trim() == item.DrugName.Trim()
                               )
                                select new clsPatientPrescriptionDetailVO
                                {
                                    DrugID = r.DrugID,
                                    Status = r.Status,
                                    PrvStatus = r.PrvStatus
                                };
                    if (item1.ToList().Count != 0)
                    {
                        PrescriptionList.Remove(item);
                    }
                }
                dgDrugList.ItemsSource = PrescriptionList;
            }
        }

        private void chkAllStores_Click(object sender, RoutedEventArgs e)
        {
            
            if (chkAllStores.IsChecked == true)
            {
                foreach (var item in dgprvpriscriptionchangestatus)
                {
                    item.PrvStatus = true;
                    var item1 = from r in PrescriptionList
                                where (r.DrugName.Trim() == item.DrugName.Trim()
                               )
                                select new
                                {
                                    item
                                };
                    if (item1.Count() == 0)
                    {
                            PrescriptionList.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in dgprvpriscriptionchangestatus)
                {
                    item.PrvStatus = false;
                    var item1 = from r in PrescriptionList
                                where (r.DrugName.Trim() == item.DrugName.Trim()
                               )
                                select new
                                {
                                    item
                                };
                    if (item1.Count() != 0)
                    {
                        PrescriptionList.Remove(item);
                    }
                }
            }
        }

        private void DaysTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
            else
            {
                string Days = Convert.ToString(((TextBox)sender).Text);
                if (Days == "")
                {
                    ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).Days = null;
                }
            }
        }

        #region Numeric TextBox

        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string strText = ((TextBox)sender).Text;

        //    if ((textBefore != null && !String.IsNullOrEmpty(strText)) && (!strText.IsValueDouble()))
        //    {
        //        ((TextBox)sender).Text = textBefore;
        //        ((TextBox)sender).SelectionStart = selectionStart;
        //        ((TextBox)sender).SelectionLength = selectionLength;
        //        textBefore = string.Empty;
        //        selectionStart = selectionLength = 0;
        //    }
        //    else
        //    {
        //        ((TextBox)sender).Text = strText.Replace("-", "");
        //    }

        //}

        #endregion
    }
}
