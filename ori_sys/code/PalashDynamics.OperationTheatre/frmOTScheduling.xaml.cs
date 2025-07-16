using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PalashDynamics.Animations;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Input;
using PalashDynamic.Localization;
//using PalashDynamics.ValueObjects.RSIJ;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.RSIJ;
using System.IO;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTScheduling : UserControl
    {
        #region Variable Declaration
        List<clsPatientProcedureVO> procList1 = new List<clsPatientProcedureVO>();
        bool flagChangeDoc = false;
        bool flagChangeStaff = false;
        private SwivelAnimation objAnimation;
        long VisitAdmID;
        long VisitAdmUnitID;
        bool IsOPDIPD;
        public long scheduleID1 = 0;
        public long scheduleUnitID1 = 0;
        bool IsCancel = true;
        public PagedSortableCollectionView<clsPatientProcedureScheduleVO> DataList { get; private set; }
        public double OTTimeSlots;
        public List<clsSchedule> SelectedCurrentDateSchedule { get; set; }
        public List<clsSchedule> SelectedCurrentDateAppointment { get; set; }
        bool IsPageLoad = false;
        string msgText;
        private bool IsCallFromCurrentUncheckEvent = false;
        bool isViewChecklist = false;
        bool isView = false;
        long PatientID = 0;
        long PatientUnitID = 0;
        public ObservableCollection<clsPatientProcDocScheduleDetailsVO> DocList { get; set; }
        public List<clsPatientProcDocScheduleDetailsVO> ChkFromTimeList { get; set; }
        public ObservableCollection<clsPatientProcedureScheduleVO> OTList1 { get; set; }
        List<clsPatientProcedureChecklistDetailsVO> PatientWiseProcCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
        PagedCollectionView pcv = null;
        private PalashDynamics.OperationTheatre.clsSchedule schedulerOutput;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        public List<long> procedureIDList = new List<long>();
        List<clsSchedule> ChecklistSchedule = new List<clsSchedule>();
        List<clsPatientProcedureChecklistDetailsVO> myList = new List<clsPatientProcedureChecklistDetailsVO>();
        WaitIndicator waitIndicator = null;

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
        public ObservableCollection<clsPatientProcStaffDetailsVO> StaffList1 { get; set; }
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient as clsPatientGeneralVO;
        LocalizationManager objLocalizationManager = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public frmOTScheduling()
        {
            this.schedulerOutput = null;
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsPatientProcedureScheduleVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            SelectedCurrentDateAppointment = new List<clsSchedule>();
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            SearchDate.SelectedDate = DateTime.Now;
            ToSearchDate.SelectedDate = DateTime.Now;
            FillOTcmb();
            IsPageLoad = true;
            procDate1.DisplayDateStart = DateTime.Today;
            DoctorSchedule.IsEnabled = false;
            //StaffSchedule.IsEnabled = false;
        }
        #endregion

        #region OnRefresh
        /// <summary>
        /// Used when refresh event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        #endregion

        #region Loaded
        /// <summary>
        /// User Control loaded
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.grdDemographicDetails.MinWidth = this.ActualWidth;
                OTTimeSlots = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OTSlot;
                OTTimeSlots = 15;
                this.DataContext = new clsPatientProcedureScheduleVO();
                StaffList1 = new ObservableCollection<clsPatientProcStaffDetailsVO>();
                DocList = new ObservableCollection<clsPatientProcDocScheduleDetailsVO>();
                OTList1 = new ObservableCollection<clsPatientProcedureScheduleVO>();
                dgSchedule.SelectedItem = null;
                FetchData();
                FillOperationTheatre();
                if (IsPageLoad)
                {
                    SetCommandButtonState("Load");
                }

                IsPageLoad = false;
                SelectedCurrentDateSchedule = new List<clsSchedule>();
                cmdModifyDoc.IsEnabled = false;
                cmdModifyStaff.IsEnabled = false;
                if (this.SelectedPatient != null)
                {
                    if (this.SelectedPatient.IsEMR && this.SelectedPatient.PatientID > 0)
                    {
                        IsCancel = false;
                        btnSearchCriteria.IsEnabled = false;
                        CmdPatientSearch.IsEnabled = false;
                        txtMRNo.IsEnabled = false;
                        cmdCancel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        IsCancel = false;
                        btnSearchCriteria.IsEnabled = true;
                        CmdPatientSearch.IsEnabled = true;
                        txtMRNo.IsEnabled = true;
                        //cmdCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
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

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    cmdCancelOTBooking.IsEnabled = true;
                    if (SelectedPatient.IsEMR)
                        cmdCancel.Visibility = Visibility.Collapsed;
                    //else
                    //    cmdCancel.Visibility = Visibility.Visible;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdCancelOTBooking.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdConsent.IsEnabled = false;
                    IsCancel = false;
                    cmdViewConsent.IsEnabled = false;
                    cmdCancel.Visibility = Visibility.Visible;
                    break;
                case "Save":
                    cmdModify.IsEnabled = false;
                    cmdAdd.IsEnabled = true;
                    cmdCancelOTBooking.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    cmdCancel.Visibility = Visibility.Collapsed;
                    break;
                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdCancelOTBooking.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdCancel.Visibility = Visibility.Collapsed;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdAdd.IsEnabled = true;
                    //cmdCancelOTBooking.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdCancel.Visibility = Visibility.Collapsed;
                    cmdConsent.IsEnabled = true;
                    cmdViewConsent.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdCancel.Visibility = Visibility.Visible;
                    cmdCancelOTBooking.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdConsent.IsEnabled = false;
                    cmdViewConsent.IsEnabled = false;
                    cmdAddDoc.IsEnabled = true;
                    cmdModifyDoc.IsEnabled = false;
                    cmdAddStaff.IsEnabled = true;
                    cmdModifyStaff.IsEnabled = false;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Button Click Event

        /// <summary>
        /// Adds staff
        /// </summary>
        private void cmdAddStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateStaff())
                {
                    clsPatientProcStaffDetailsVO tempStaff = new clsPatientProcStaffDetailsVO();

                    var staff = from r in StaffList1
                                where (r.DesignationID == Convert.ToInt32(((MasterListItem)CmbDesignation.SelectedItem).Code) && r.StaffID == ((MasterListItem)CmbStaff.SelectedItem).ID && r.ProcedureID == ((MasterListItem)CmbStaffProcedure.SelectedItem).ID)
                                select new clsPatientProcStaffDetailsVO
                                {
                                    Status = r.Status,
                                    DesignationID = r.DesignationID,
                                    DesignationCode = r.DesignationCode,
                                    designationDesc = ((MasterListItem)CmbDesignation.SelectedItem).Description,
                                    StaffID = r.StaffID,
                                    Quantity = r.Quantity,
                                    stffDesc = ((MasterListItem)CmbStaff.SelectedItem).Description
                                };

                    var Designation = (from r in StaffList1
                                       where (r.DesignationCode == ((MasterListItem)CmbDesignation.SelectedItem).Code)
                                       select r);
                    if (staff.ToList().Count == 0)
                    {
                        //if (Designation.ToList().Count < Convert.ToInt64(txtStaffQuantity.Text))
                        //{
                        tempStaff.DesignationID = Convert.ToInt64(((MasterListItem)CmbDesignation.SelectedItem).Code);
                        tempStaff.DesignationCode = ((MasterListItem)CmbDesignation.SelectedItem).Code;
                        tempStaff.designationDesc = ((MasterListItem)CmbDesignation.SelectedItem).Description;
                        tempStaff.StaffID = ((MasterListItem)CmbStaff.SelectedItem).ID;
                        tempStaff.stffDesc = ((MasterListItem)CmbStaff.SelectedItem).Description;
                        tempStaff.ProcedureID = ((MasterListItem)CmbStaffProcedure.SelectedItem).ID;
                        tempStaff.ProcedureName = ((MasterListItem)CmbStaffProcedure.SelectedItem).Description;
                        //tempStaff.Quantity = (float)Convert.ToDouble(txtStaffQuantity.Text);
                        tempStaff.Status = true;
                        StaffList1.Add(tempStaff);
                        dgProcedureStaff.ItemsSource = null;
                        dgProcedureStaff.ItemsSource = StaffList1;
                        CmbStaffProcedure.SelectedValue = (long)0;
                        CmbDesignation.SelectedValue = (long)0;
                        CmbStaff.SelectedValue = (long)0;
                        txtStaffQuantity.Text = "";
                        //}
                        //else
                        //{
                        //    if (objLocalizationManager != null)
                        //    {
                        //        msgText = objLocalizationManager.GetValue("StaffCountExceed_Msg");
                        //    }
                        //    else
                        //    {
                        //        msgText = "Staff count exceed.";
                        //    }
                        //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //}
                    }
                    else
                    {
                        msgText = "Staff already added for the procedure.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    CmbStaffProcedure.SelectedValue = (long)0;
                    CmbDesignation.SelectedValue = (long)0;
                    CmbStaff.SelectedValue = (long)0;
                    txtStaffQuantity.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Get Procedure button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetProcedure_Click(object sender, RoutedEventArgs e)
        {
            if (procDate1.SelectedDate != null)
            {
                isView = false;
                frmProcedureSearch winProcedureSearch = new frmProcedureSearch();
                //winProcedureSearch.Height = ActualHeight - 150;
                //winProcedureSearch.Width = ActualWidth - 250;
                winProcedureSearch.Width = this.ActualWidth * 0.60;
                winProcedureSearch.Height = this.ActualHeight * 0.70;
                winProcedureSearch.OnAddButton_Click += new RoutedEventHandler(winProcedureSearch_OnSaveButton_Click);
                winProcedureSearch.Show();
            }
            else
            {
                string strMsg = "Please select OT Date !";
                //string strMsg = PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("SelectOTDate_Msg");
                ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// Search Procedure Window Ok Button Click
        /// </summary>
        void winProcedureSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            frmProcedureSearch procwin = (frmProcedureSearch)sender;
            if (procwin.procedureList != null)
            {
                List<clsProcedureMasterVO> lstProc = procwin.procedureList.Where(z => z.Status == true).ToList();
                foreach (var item in lstProc)
                {
                    if (item.Status == true)
                    {
                        if (procList1.Where(procItems => procItems.ProcedureID == item.ID).Any() == false)
                        {
                            procList1.Add(
                            new clsPatientProcedureVO()
                            {
                                ProcedureID = item.ID,
                                ProcDesc = item.Description,
                                Status = item.Status
                            });
                            DoctorSchedule.IsEnabled = true;
                            //StaffSchedule.IsEnabled = true;
                        }
                        else
                        {
                            msgText = "Procedure is already added in the list.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                }
                grdProcedure.ItemsSource = null;
                grdProcedure.ItemsSource = procList1;
                FetchOTbyProcedure();
                FillCheckListOnProcedureClick();
            }
        }

        long ScheduleID = 0;

        /// <summary>
        /// New button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CmdPatientSearch.IsEnabled = true;
                ClrFoot.Visibility = Visibility.Visible;
                cmdModifyDoc.IsEnabled = false;
                cmdAddDoc.IsEnabled = true;
                cmdModifyStaff.IsEnabled = false;
                cmdAddStaff.IsEnabled = true;
                blChkDocSchedule = false;
                DoctorSchedule.IsEnabled = false;
                this.SetCommandButtonState("New");
                ClearUI();
                CleartimeSlot();
                ScheduleID = 0;
                btnSearchCriteria.IsEnabled = true;
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "New OT Schedule Details";
                objAnimation.Invoke(RotationType.Forward);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Search button on front panel click
        /// </summary>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void FillCheckListOnProcedureClick()
        {
            procedureIDList = new List<long>();
            //if (isViewChecklist == false)
            //{
            if (grdProcedure.ItemsSource != null)
            {
                //frmPatientWiseChecklist chkListWindow = new frmPatientWiseChecklist();
                int i = 0;
                for (i = 0; i < procList1.Count; i++)
                {
                    procedureIDList.Add(procList1[i].ProcedureID);
                }
                FillChecklistGrid();
                //chkListWindow.OnSaveButton_Click += new RoutedEventHandler(PatientWiseChecklist_OnSaveButton_Click);
                //chkListWindow.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWin =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Procedure First.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
            }
            //}
            //else
            //    FetchChecklistForShcedule(scheduleID1, scheduleUnitID1);
        }

        private void FillChecklistGrid()
        {
            try
            {
                clsGetCheckListByProcedureIDBizActionVO BizActionVo = new clsGetCheckListByProcedureIDBizActionVO();
                BizActionVo.procedureIDList = this.procedureIDList;

                //for (int i = 0; i < procedureIDList.Count; i++)
                //{
                //BizActionVo.ProcedureID = procedureIDList[i];
              //  BizActionVo.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetCheckListByProcedureIDBizActionVO)e.Result).ChecklistDetails != null)
                        {
                            clsGetCheckListByProcedureIDBizActionVO result = e.Result as clsGetCheckListByProcedureIDBizActionVO;

                            if (result.ChecklistDetails != null)
                            {
                                PatientWiseProcCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                                myList.Clear();
                                myList = result.ChecklistDetails;

                                foreach (var item in myList)
                                
                                    
                                        PatientWiseProcCheckList.Add(item);
                                        pcv = new PagedCollectionView(myList);
                                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                                        grdchecklist.ItemsSource = pcv;
                                
                            }

                        }
                    }
                };

                Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdGetCheckList_Click(object sender, RoutedEventArgs e)
        {
            //if (isViewChecklist == false)
            //{
            if (grdProcedure.ItemsSource != null)
            {
                frmPatientWiseChecklist chkListWindow = new frmPatientWiseChecklist();
                //chkListWindow.Height = ActualHeight - 150;
                //chkListWindow.Width = ActualWidth - 250;
                chkListWindow.Width = this.ActualWidth * 0.60;
                chkListWindow.Height = this.ActualHeight * 0.70;
                int i = 0;
                for (i = 0; i < procList1.Count; i++)
                {
                    chkListWindow.procedureIDList.Add(procList1[i].ProcedureID);
                }
                chkListWindow.OnSaveButton_Click += new RoutedEventHandler(PatientWiseChecklist_OnSaveButton_Click);
                chkListWindow.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWin =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Procedure First.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
            }
            //}
            //else
            //    FetchChecklistForShcedule(scheduleID1, scheduleUnitID1);
        }

        /// <summary>
        /// Patientwise checklist OK Button Click
        /// </summary>
        void PatientWiseChecklist_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmPatientWiseChecklist PatientWiseChecklistWin = (frmPatientWiseChecklist)sender;
                if (PatientWiseChecklistWin.resultCheckList != null)
                {
                    PatientWiseProcCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                    foreach (var item in PatientWiseChecklistWin.resultCheckList)
                    {
                        PatientWiseProcCheckList.Add(item);
                        //if ((PatientWiseProcCheckList.Where(S => S.ID == item.ID)).ToList().Count < 1)
                        //    PatientWiseProcCheckList.Add(item);
                    }
                    pcv = new PagedCollectionView(PatientWiseProcCheckList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                    grdchecklist.ItemsSource = null;
                    grdchecklist.ItemsSource = pcv;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdCancelOTBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msgTitle = "";
                msgText = "Are you sure you want to cancel OT booking ?";
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed2);
                msgW2.Show();

            }
            catch (Exception)
            {
                throw;
            }
        }

        void msgW2_OnMessageBoxClosed2(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (dgSchedule.SelectedItem != null)
                {
                    frmCancelBooking winCancelBooking = new frmCancelBooking();
                    winCancelBooking.OnSaveButton_Click += new RoutedEventHandler(winCancelBooking_OnSaveButton_Click);
                    winCancelBooking.Show();
                }
            }
        }

        /// <summary>
        /// Purpose: For Cancel OT Booking And Add OT booking cancellation reason
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void winCancelBooking_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsCancelOTBookingBizActionVO BizAction = new clsCancelOTBookingBizActionVO();
                BizAction.patientProcScheduleID = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).ID;
                BizAction.CancelledBy = 0;
                //BizAction.CancelledBy = ((MasterListItem)(((frmCancelBooking)sender).cmbCancelledBy).SelectedItem).ID;
                BizAction.CancelledReason = ((frmCancelBooking)sender).txtCancelledReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            WaitIndicator Indicatior = new WaitIndicator();
                            FetchData();
                            msgText = "OT booking cancelled successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            msgText = "Error occured while processing.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Save button click
        /// </summary>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool SaveTest = true;
                ClrFoot.Visibility = Visibility.Collapsed;
                SaveTest = ValidateOnSave();
                if (SaveTest == true)
                {
                    Save();

                    //string msgTitle = "";
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("SaveVerification_Msg");
                    //}
                    //else
                    //{
                    //    msgText = "Are you sure you want to save ?";
                    //}
                    //MessageBoxControl.MessageBoxChildWindow msgSave =
                    //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    //msgSave.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgSave_OnMessageBoxClosed);
                    //msgSave.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function get called if user clicks ok on save message
        /// </summary>
        /// <param name="result">MessageBoxResult</param>
        void msgSave_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                IsOPDIPD = true;
                Save();
            }
        }

        /// <summary>
        /// Modify Button Click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ModifyTest = true;
                ClrFoot.Visibility = Visibility.Collapsed;
                ModifyTest = ValidateOnSave();
                if (ModifyTest == true)
                {
                    Modify();

                    //string msgTitle = "";
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("UpdateVerification_Msg");
                    //}
                    //else
                    //{
                    //    msgText = "Are you sure you want to update ?";
                    //}
                    //MessageBoxControl.MessageBoxChildWindow msgUpdate =
                    //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    //msgUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgUpdate_OnMessageBoxClosed);

                    //msgUpdate.Show();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  This function get called if user clicks ok on modify message
        /// </summary>
        /// <param name="result">MessageBoxResult </param>
        void msgUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Modify();
            }
        }

        /// <summary>
        /// Clears UI
        /// </summary>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearUI();
                ClrFoot.Visibility = Visibility.Collapsed;
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = "  ";
                IsCancel = true;
                FetchData();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// View hyperlink on front panel grid click
        /// </summary>
        /// l
        bool blView = false;
        bool blChkDocSchedule = false;
        DateTime StrTime;
        DateTime EndTime;
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            isView = true;
            blView = true;
            blChkDocSchedule = true;
            ClrFoot.Visibility = Visibility.Visible;
            clsPatientProcedureScheduleVO objProcedureDetails = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem);
            try
            {
                SetCommandButtonState("View");
                if (SelectedPatient.IsEMR)
                    IsCancel = true;
                btnSearchCriteria.IsEnabled = false;
                CmdPatientSearch.IsEnabled = false;
                StrTime = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime;
                EndTime = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).EndTime;
                if (dgSchedule.SelectedItem != null)
                {
                    DateTime dt = (DateTime)((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).Date;
                    if (dt.Date.Equals(DateTime.Now.Date))
                    {
                        if ((((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime.TimeOfDay) > DateTime.Now.TimeOfDay)
                            cmdModify.IsEnabled = true;
                        else
                            cmdModify.IsEnabled = false;
                    }
                    else
                        cmdModify.IsEnabled = true;

                    this.DataContext = objProcedureDetails;
                    if (objProcedureDetails.ID > 0)
                    {
                        DoctorSchedule.IsEnabled = true;
                        GetPatientData(objProcedureDetails.MRNO);
                        FillDetailTablesOfProcedureSchedule(objProcedureDetails.ID, objProcedureDetails.UnitID);
                    }
                    objAnimation.Invoke(RotationType.Forward);
                    TabControlMain.SelectedIndex = 0;
                }
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : On Date " + ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).Date;

            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Doctoe Details Add Button Click Event Get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddDoc_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDOC())
            {
                AddDoctor();
                //GetDoctorSchedule();
            }
        }

        private void GetDoctorSchedule()
        {
            GetDoctorScheduleTimeVO BizAction = new GetDoctorScheduleTimeVO();
            BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
            if (CmbDoctor.SelectedItem != null)
                BizAction.DoctorId = ((MasterListItem)CmbDoctor.SelectedItem).ID;
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.AppointmentType = 1;
            string objDay = null;
            if (procDate1.SelectedDate != null)
            {
                objDay = procDate1.SelectedDate.Value.DayOfWeek.ToString();
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList != null && ((GetDoctorScheduleTimeVO)arg.Result).DoctorScheduleDetailsList.Count != 0)
                    {
                        GetDoctorScheduleTimeVO DetailsVO = new GetDoctorScheduleTimeVO();
                        DetailsVO = (GetDoctorScheduleTimeVO)arg.Result;
                        List<clsSchedule> listSchedule = new List<clsSchedule>();
                        if (DetailsVO.DoctorScheduleDetailsList != null)
                        {
                            int IsNextDaySchedule = 0;
                            for (int DayCount = 0; DayCount <= 7; DayCount++)
                            {
                                string objNextDay = procDate1.SelectedDate.Value.AddDays(DayCount).DayOfWeek.ToString();
                                DateTime Date = procDate1.SelectedDate.Value.AddDays(DayCount);
                                List<clsDoctorScheduleDetailsVO> DoctorScheduleCount = (from ScheduleObject in DetailsVO.DoctorScheduleDetailsList
                                                                                        where ScheduleObject.Day.Equals(objNextDay) && ScheduleObject.DoctorID.Equals(BizAction.DoctorId)
                                                                                        select ScheduleObject).ToList<clsDoctorScheduleDetailsVO>();
                                if (DoctorScheduleCount.Count != 0)
                                {
                                    int ScheduleCount = 1;
                                    foreach (clsDoctorScheduleDetailsVO item in DoctorScheduleCount)
                                    {
                                        if (item.StartTime != null && item.EndTime != null)
                                        {
                                            double ScheduleHrs = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime)).TotalMinutes / OTTimeSlots;
                                            DateTime? Dt = null;
                                            if (item.StartTime != null)
                                                Dt = item.StartTime;
                                            for (int Schedulecount = 0; Schedulecount < ScheduleHrs; Schedulecount++)
                                            {
                                                clsSchedule _listItem = new clsSchedule();
                                                _listItem.Day = "Schedule" + ScheduleCount;

                                                if (DayCount.Equals(0))
                                                {
                                                    _listItem.IsCurrentSchedule = true;
                                                    _listItem.CurrentAppointmentDate = Date;
                                                }
                                                else
                                                {
                                                    _listItem.IsCurrentSchedule = false;

                                                }
                                                _listItem.ScheduleFromTime = Dt.Value;
                                                _listItem.ScheduleToTime = (Dt.Value.AddMinutes(OTTimeSlots));
                                                _listItem.CurrentapColor = new SolidColorBrush(Colors.Green);
                                                listSchedule.Add(_listItem);
                                                Dt = Dt.Value.AddMinutes(OTTimeSlots);
                                            }
                                        }
                                        ScheduleCount++;
                                    }
                                    IsNextDaySchedule++;
                                }
                                else
                                {
                                    if (DayCount.Equals(0))
                                    {
                                        IsNextDaySchedule++;
                                    }
                                }
                                if (IsNextDaySchedule.Equals(1))
                                    break;
                            }
                            CheckDoctorScheduleAvailable(listSchedule);
                        }
                    }
                    else
                    {
                        msgText = "Doctor is not available.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    }
                }

                else
                {
                    msgText = "Error occured while processing.";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        /// <summary>
        /// modified doctor information
        /// </summary>
        private void cmdModifyDoc_Click(object sender, RoutedEventArgs e)
        {
            bool Flag = true;
            try
            {
                //if (DocList.Count > 0)
                //{
                //if (((MasterListItem)CmbDocType.SelectedItem).ID != 0 && ((MasterListItem)CmbDocType.SelectedItem).ID != 0)
                if (!((MasterListItem)CmbDocType.SelectedItem).Code.Equals("0") && !((MasterListItem)CmbDocType.SelectedItem).Code.Equals("0"))
                {
                    if (ValidateDOC())
                    {
                        clsGetDocScheduleByDocIDBizActionVO BizActionobj = new clsGetDocScheduleByDocIDBizActionVO();
                        BizActionobj.DocID = ((MasterListItem)CmbDoctor.SelectedItem).ID;
                        BizActionobj.DocTableID = ((MasterListItem)CmbDocType.SelectedItem).ID;
                        BizActionobj.procDate = (DateTime)procDate1.SelectedDate;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (sa, arg1) =>
                        {
                            if (arg1.Error == null)
                            {
                                if (((clsGetDocScheduleByDocIDBizActionVO)arg1.Result).DocScheduleListDetails != null)
                                {
                                    ChkFromTimeList = ((clsGetDocScheduleByDocIDBizActionVO)arg1.Result).DocScheduleListDetails;
                                    for (int i = 0; i < ChkFromTimeList.Count; i++)
                                    {
                                        DateTime AlreadyFromDatetime = new DateTime(procDate1.SelectedDate.Value.Year, procDate1.SelectedDate.Value.Month, procDate1.SelectedDate.Value.Day, Convert.ToInt16(ChkFromTimeList[i].StartTime.Hour), Convert.ToInt16(ChkFromTimeList[i].EndTime.Minute), 0);
                                        DateTime AlreadyToDatetime = new DateTime(procDate1.SelectedDate.Value.Year, procDate1.SelectedDate.Value.Month, procDate1.SelectedDate.Value.Day, Convert.ToInt16(ChkFromTimeList[i].EndTime.Hour), Convert.ToInt16(ChkFromTimeList[i].EndTime.Minute), 0);

                                        DateTime FromDatetime = new DateTime(procDate1.SelectedDate.Value.Year, procDate1.SelectedDate.Value.Month, procDate1.SelectedDate.Value.Day, Convert.ToInt16(tpFromTime.Value.Value.Hour), Convert.ToInt16(tpFromTime.Value.Value.Minute), 0);
                                        DateTime ToDatetime = new DateTime(procDate1.SelectedDate.Value.Year, procDate1.SelectedDate.Value.Month, procDate1.SelectedDate.Value.Day, Convert.ToInt16(tpToTime.Value.Value.Hour), Convert.ToInt16(tpToTime.Value.Value.Minute), 0);
                                        if (FromDatetime == AlreadyFromDatetime)
                                        {
                                            Flag = false;
                                        }
                                        else if (ToDatetime == AlreadyToDatetime)
                                        {
                                            Flag = false;
                                        }
                                        else if (FromDatetime < AlreadyFromDatetime && ToDatetime > AlreadyToDatetime)
                                        {
                                            Flag = false;
                                        }
                                        else
                                        {

                                            while (ToDatetime >= AlreadyFromDatetime && ToDatetime <= AlreadyToDatetime)
                                            {
                                                if (ToDatetime > AlreadyFromDatetime && ToDatetime < AlreadyToDatetime)
                                                {
                                                    Flag = false;
                                                    break;
                                                }
                                                ToDatetime = ToDatetime.AddMinutes(OTTimeSlots);
                                            }
                                            while (FromDatetime >= AlreadyFromDatetime && FromDatetime <= AlreadyToDatetime)
                                            {
                                                if (FromDatetime > AlreadyFromDatetime && FromDatetime < AlreadyToDatetime)
                                                {
                                                    Flag = false;
                                                    break;
                                                }
                                                FromDatetime = FromDatetime.AddMinutes(OTTimeSlots);
                                            }
                                        }
                                    }
                                    if (Flag == false)
                                    {
                                        msgText = "Time slots is already booked.";
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW1.Show();
                                    }
                                }

                                else
                                {
                                    clsPatientProcDocScheduleDetailsVO tempDoc = new clsPatientProcDocScheduleDetailsVO();

                                    //foreach (var item in DocList)
                                    //{
                                    //    if (item.SpecializationCode == ((MasterListItem)CmbDocType.SelectedItem).Code && item.DocID == ((MasterListItem)CmbDoctor.SelectedItem).ID && item.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                                    //    {
                                    //        item.StartTime = (DateTime)tpFromTime.Value;
                                    //        item.EndTime = (DateTime)tpToTime.Value;
                                    //        item.StrStartTime = item.StartTime.ToShortTimeString();
                                    //        item.StrEndTime = item.EndTime.ToShortTimeString();
                                    //        dgDocAddedTimeSlots1.ItemsSource = null;
                                    //        dgDocAddedTimeSlots1.ItemsSource = DocList;
                                    //        cmdModifyDoc.IsEnabled = false;
                                    //        cmdAddDoc.IsEnabled = true;
                                    //    }
                                    //}



                                    var doc = from r in DocList
                                              //where (r.SpecializationCode == ((MasterListItem)CmbDocType.SelectedItem).Code && r.DoctorCode == ((MasterListItem)CmbDoctor.SelectedItem).Code && r.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                                              where (r.SpecializationCode == ((MasterListItem)CmbDocType.SelectedItem).Code && r.DocID == ((MasterListItem)CmbDoctor.SelectedItem).ID && r.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                                              select new clsPatientProcDocScheduleDetailsVO
                                              {
                                                  Status = r.Status,
                                                  DocTypeID = r.DocTypeID,
                                                  SpecializationCode = ((MasterListItem)CmbDocType.SelectedItem).Code,
                                                  docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description,
                                                  Specialization = ((MasterListItem)CmbDocType.SelectedItem).Description,
                                                  DocID = r.DocID,
                                                  DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code,
                                                  ProcedureID = ((MasterListItem)CmbDocProcedure.SelectedItem).ID,
                                                  ProcedureName = ((MasterListItem)CmbDocProcedure.SelectedItem).Description,
                                                  docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description,
                                                  DayID = (long?)procDate1.SelectedDate.Value.DayOfWeek,
                                                  StartTime = r.StartTime,
                                                  EndTime = r.EndTime,
                                                  StrStartTime = r.StartTime.ToShortTimeString(),
                                                  StrEndTime = r.EndTime.ToShortTimeString()
                                              };
                                    if (doc.ToList().Count == 0)
                                    {
                                        tempDoc.DocTypeID = Convert.ToInt64(((MasterListItem)CmbDocType.SelectedItem).Code);
                                        tempDoc.SpecializationCode = ((MasterListItem)CmbDocType.SelectedItem).Code;
                                        tempDoc.docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description;
                                        tempDoc.DocID = ((MasterListItem)CmbDoctor.SelectedItem).ID;
                                        tempDoc.Specialization = ((MasterListItem)CmbDocType.SelectedItem).Description;
                                        tempDoc.DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code;
                                        tempDoc.docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description;
                                        tempDoc.StartTime = (DateTime)tpFromTime.Value;
                                        tempDoc.EndTime = (DateTime)tpToTime.Value;
                                        tempDoc.StrStartTime = tempDoc.StartTime.ToShortTimeString();
                                        tempDoc.StrEndTime = tempDoc.EndTime.ToShortTimeString();
                                        tempDoc.ProcedureID = ((MasterListItem)CmbDocProcedure.SelectedItem).ID;
                                        tempDoc.ProcedureName = ((MasterListItem)CmbDocProcedure.SelectedItem).Description;
                                        tempDoc.DayID = (long)procDate1.SelectedDate.Value.DayOfWeek;
                                        tempDoc.Status = true;
                                        DocList.Add(tempDoc);
                                        dgDocAddedTimeSlots1.ItemsSource = null;
                                        dgDocAddedTimeSlots1.ItemsSource = DocList;
                                        CmbDocProcedure.SelectedValue = (long)0;
                                        CmbDocType.SelectedValue = (long)0;
                                        CmbDoctor.SelectedValue = (long)0;
                                        tpFromTime.Value = tpFromtime1.Value;
                                        tpToTime.Value = tpToTime1.Value;
                                        cmdModifyDoc.IsEnabled = false;
                                        cmdAddDoc.IsEnabled = true;
                                    }
                                    //clsPatientProcDocScheduleDetailsVO tempDoc = new clsPatientProcDocScheduleDetailsVO();
                                    //var doc = from r in DocList
                                    //          where (r.DocTypeID == ((MasterListItem)CmbDocType.SelectedItem).ID && r.DocID == ((MasterListItem)CmbDoctor.SelectedItem).ID && r.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                                    //          select new clsPatientProcDocScheduleDetailsVO
                                    //          {
                                    //              Status = r.Status,
                                    //              DocTypeID = r.DocTypeID,
                                    //              docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description,
                                    //              DocID = r.DocID,
                                    //              docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description,
                                    //              DayID = (long?)procDate1.SelectedDate.Value.DayOfWeek,
                                    //              StartTime = r.StartTime,
                                    //              EndTime = r.EndTime

                                    //          };


                                    //if (doc.ToList().Count == 0)
                                    //{
                                    //    tempDoc.DocTypeID = ((MasterListItem)CmbDocType.SelectedItem).ID;
                                    //    tempDoc.docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description;


                                    //    tempDoc.ProcedureID = ((MasterListItem)CmbDocProcedure.SelectedItem).ID;
                                    //    tempDoc.ProcedureName = ((MasterListItem)CmbDocProcedure.SelectedItem).Description;

                                    //    tempDoc.DocID = ((MasterListItem)CmbDoctor.SelectedItem).ID;
                                    //    tempDoc.docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description;

                                    //    tempDoc.StartTime = (DateTime)tpFromTime.Value;
                                    //    tempDoc.EndTime = (DateTime)tpToTime.Value;

                                    //    tempDoc.DayID = (long)procDate1.SelectedDate.Value.DayOfWeek;

                                    //    clsPatientProcDocScheduleDetailsVO var = (clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem;
                                    //    DocList.Remove(var);

                                    //    tempDoc.Status = true;
                                    //    DocList.Add(tempDoc);

                                    //    dgDocAddedTimeSlots1.ItemsSource = null;
                                    //    dgDocAddedTimeSlots1.ItemsSource = DocList;

                                    //    cmdModifyDoc.IsEnabled = false;
                                    //    cmdAddDoc.IsEnabled = true;

                                    //    CmbDocType.SelectedValue = (long)0;
                                    //    CmbDoctor.SelectedValue = (long)0;
                                    //    CmbDocProcedure.SelectedValue = (long)0;

                                    //    tpFromTime.Value = null;
                                    //    tpToTime.Value = null;
                                    //}


                                    else
                                    {
                                        msgText = "Doctor already added for given procedure.";
                                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW2.Show();
                                    }


                                    CmbDocProcedure.SelectedValue = (long)0;
                                    CmbDocType.SelectedValue = (long)0;
                                    CmbDoctor.SelectedValue = (long)0;
                                    tpFromTime.Value = tpFromtime1.Value;
                                    tpToTime.Value = tpToTime1.Value;
                                    cmdModifyDoc.IsEnabled = false;
                                    cmdAddDoc.IsEnabled = true;
                                }
                            }
                        };
                        client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }

                }

                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Consent Click buttton event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdConsent_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.ItemsSource != null)
            {
                if (dgSchedule.SelectedItem != null)
                {
                    clsPatientProcedureScheduleVO selectedPatient = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem);
                    frmPatientConsentDetails win = new frmPatientConsentDetails(selectedPatient.ID, selectedPatient.PatientID, selectedPatient.PatientUnitID, selectedPatient.VisitAdmID, selectedPatient.UnitID);
                    ChildWindow winChild = new ChildWindow();
                    //winChild.Height = ActualHeight - 150;
                    //winChild.Width = ActualWidth - 250;
                    winChild.Width = this.ActualWidth * 0.60;
                    winChild.Height = this.ActualHeight * 0.70;
                    //winChild.Height = 500;
                    //winChild.Width = 800;
                    winChild.Content = win;
                    winChild.Show();
                    winChild.Closing += new EventHandler<CancelEventArgs>(winChild_Closing);
                }
                else
                {
                    msgText = "Please Select OT Booking .";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
        }

        void winChild_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            //throw new NotImplementedException();
            FetchData();
        }

        private void cmdViewConsent_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.ItemsSource != null)
            {
                if (dgSchedule.SelectedItem != null)
                {
                    clsPatientProcedureScheduleVO selectedPatient = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem);
                    frmViewConsent win = new frmViewConsent(selectedPatient.ID, selectedPatient.PatientID, selectedPatient.ConsentID);
                    //win.Height = ActualHeight - 150;
                    //win.Width = ActualWidth - 250;
                    win.Width = this.ActualWidth * 0.60;
                    win.Height = this.ActualHeight * 0.70;
                    ChildWindow winChild = new ChildWindow();
                    //winChild.Height = ActualHeight - 150;
                    //winChild.Width = ActualWidth - 250;
                    winChild.Width = this.ActualWidth * 0.60;
                    winChild.Height = this.ActualHeight * 0.70;
                    //winChild.Height = 500;
                    //winChild.Width = 800;
                    winChild.Content = win;
                    winChild.Show();
                    winChild.Closing += new EventHandler<CancelEventArgs>(winChild_Closing);
                }
                else
                {
                    msgText = "Please Select OT Booking .";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
        }


        #endregion

        #region Private Method

        /// <summary>
        /// validates staff
        /// </summary>
        /// <returns></returns>
        private bool ValidateStaff()
        {
            bool result = true;
            try
            {
                if (CmbDesignation.SelectedItem == null || ((MasterListItem)CmbDesignation.SelectedItem).Code == "0")
                {
                    msgText = "Please select designation";
                    CmbDesignation.TextBox.SetValidation(msgText);
                    CmbDesignation.TextBox.RaiseValidationError();
                    CmbDesignation.Focus();
                    result = false;
                }
                else if (CmbStaffProcedure.SelectedItem == null || ((MasterListItem)CmbStaffProcedure.SelectedItem).ID == 0)
                {
                    msgText = "Please select procedure";
                    CmbStaffProcedure.TextBox.SetValidation(msgText);
                    CmbStaffProcedure.TextBox.RaiseValidationError();
                    CmbStaffProcedure.Focus();
                    result = false;
                }
                else if (CmbStaff.SelectedItem == null || ((MasterListItem)CmbStaff.SelectedItem).ID == 0)
                {
                    msgText = "Please select staff";
                    CmbStaff.TextBox.SetValidation(msgText);
                    CmbStaff.TextBox.RaiseValidationError();
                    CmbStaff.Focus();
                    result = false;
                }

                else
                {
                    CmbStaffProcedure.TextBox.ClearValidationError();
                    CmbStaff.TextBox.ClearValidationError();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void AddDoctor()
        {
            clsPatientProcDocScheduleDetailsVO tempDoc = new clsPatientProcDocScheduleDetailsVO();
            var doc = from r in DocList
                      //where (r.SpecializationCode == ((MasterListItem)CmbDocType.SelectedItem).Code && r.DoctorCode == ((MasterListItem)CmbDoctor.SelectedItem).Code && r.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                      where (r.SpecializationCode == ((MasterListItem)CmbDocType.SelectedItem).Code && r.DocID == ((MasterListItem)CmbDoctor.SelectedItem).ID && r.ProcedureID == ((MasterListItem)CmbDocProcedure.SelectedItem).ID)
                      select new clsPatientProcDocScheduleDetailsVO
                      {
                          Status = r.Status,
                          DocTypeID = r.DocTypeID,
                          SpecializationCode = ((MasterListItem)CmbDocType.SelectedItem).Code,
                          docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description,
                          Specialization = ((MasterListItem)CmbDocType.SelectedItem).Description,
                          DocID = r.DocID,
                          DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code,
                          ProcedureID = ((MasterListItem)CmbDocProcedure.SelectedItem).ID,
                          ProcedureName = ((MasterListItem)CmbDocProcedure.SelectedItem).Description,
                          docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description,
                          DayID = (long?)procDate1.SelectedDate.Value.DayOfWeek,
                          StartTime = r.StartTime,
                          EndTime = r.EndTime,
                          StrStartTime = r.StartTime.ToShortTimeString(),
                          StrEndTime = r.EndTime.ToShortTimeString()
                      };
            if (doc.ToList().Count == 0)
            {
                tempDoc.DocTypeID = Convert.ToInt64(((MasterListItem)CmbDocType.SelectedItem).Code);
                tempDoc.SpecializationCode = ((MasterListItem)CmbDocType.SelectedItem).Code;
                tempDoc.docTypeDesc = ((MasterListItem)CmbDocType.SelectedItem).Description;
                tempDoc.DocID = ((MasterListItem)CmbDoctor.SelectedItem).ID;
                tempDoc.DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code;
                tempDoc.Specialization = ((MasterListItem)CmbDocType.SelectedItem).Description;
                tempDoc.docDesc = ((MasterListItem)CmbDoctor.SelectedItem).Description;
                tempDoc.StartTime = (DateTime)tpFromTime.Value;
                tempDoc.EndTime = (DateTime)tpToTime.Value;
                tempDoc.StrStartTime = tempDoc.StartTime.ToShortTimeString();
                tempDoc.StrEndTime = tempDoc.EndTime.ToShortTimeString();
                tempDoc.ProcedureID = ((MasterListItem)CmbDocProcedure.SelectedItem).ID;
                tempDoc.ProcedureName = ((MasterListItem)CmbDocProcedure.SelectedItem).Description;
                tempDoc.DayID = (long)procDate1.SelectedDate.Value.DayOfWeek;
                tempDoc.Status = true;
                DocList.Add(tempDoc);
                dgDocAddedTimeSlots1.ItemsSource = null;
                dgDocAddedTimeSlots1.ItemsSource = DocList;
                CmbDocProcedure.SelectedValue = (long)0;
                CmbDocType.SelectedValue = (long)0;
                CmbDoctor.SelectedValue = (long)0;
                tpFromTime.Value = tpFromtime1.Value;
                tpToTime.Value = tpToTime1.Value;
            }
            else
            {
                msgText = "Doctor already added for given procedure.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            CmbDocProcedure.SelectedValue = (long)0;
            CmbDocType.SelectedValue = (long)0;
            CmbDoctor.SelectedValue = (long)0;
            tpFromTime.Value = tpFromtime1.Value;
            tpToTime.Value = tpToTime1.Value;

        }

        /// <summary>
        /// Validates doctor information before adding doctor
        /// </summary>
        private bool ValidateDOC()
        {
            bool result = true;
            try
            {
                if (tpToTime.Value == null)
                {
                    msgText = "Please select to date.";
                    tpToTime.SetValidation(msgText);
                    tpToTime.RaiseValidationError();
                    tpToTime.BorderBrush = new SolidColorBrush(Colors.Red);
                    result = false;
                }
                else
                {
                    tpToTime.BorderBrush = new SolidColorBrush(Colors.Black);
                    tpToTime.ClearValidationError();
                }

                if (tpFromTime.Value == null)
                {
                    msgText = "Please select from date.";
                    tpFromTime.SetValidation(msgText);
                    tpFromTime.RaiseValidationError();
                    tpFromTime.BorderBrush = new SolidColorBrush(Colors.Red);
                    result = false;
                }
                else
                {
                    tpFromTime.BorderBrush = new SolidColorBrush(Colors.Black);
                    tpFromTime.ClearValidationError();
                }

                if ((MasterListItem)CmbDocProcedure.SelectedItem == null || ((MasterListItem)CmbDocProcedure.SelectedItem).ID == 0)
                {
                    msgText = "Please select procedure.";
                    CmbDocProcedure.TextBox.SetValidation(msgText);
                    CmbDocProcedure.TextBox.RaiseValidationError();
                    CmbDocProcedure.Focus();
                    result = false;
                }
                else
                    CmbDocProcedure.TextBox.ClearValidationError();

                if (CmbDoctor.SelectedItem == null || ((MasterListItem)CmbDoctor.SelectedItem).Code == "0")
                {
                    msgText = "Please select doctor.";
                    CmbDoctor.TextBox.SetValidation(msgText);
                    CmbDoctor.TextBox.RaiseValidationError();
                    CmbDoctor.Focus();
                    result = false;
                }
                else
                    CmbDoctor.TextBox.ClearValidationError();

                if (CmbDocType.SelectedItem == null || ((MasterListItem)CmbDocType.SelectedItem).Code == "0")
                {
                    msgText = "Please select doctor Specialization.";
                    CmbDocType.TextBox.SetValidation(msgText);
                    CmbDocType.TextBox.RaiseValidationError();
                    CmbDocType.Focus();
                    result = false;
                }
                else
                    CmbDocType.TextBox.ClearValidationError();
                if (tpFromTime.Value != null && tpToTime.Value != null && tpFromtime1.Value != null && tpToTime1.Value != null)
                {
                    if (tpFromtime1.Value > tpFromTime.Value || tpToTime1.Value < tpToTime.Value)
                    {
                        msgText = "Please enter time within given time range.";
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                              new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                        return false;

                    }
                    else if (tpFromTime.Value > tpToTime.Value)
                    {
                        msgText = "From time can not be greater than to time.";
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                        return false;
                    }
                    else if (tpFromTime.Value == tpToTime.Value)
                    {
                        msgText = "From time and to time can not be equal.";
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                                  new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                        return false;
                    }

                }
                else
                {
                    msgText = "First you need to select OT schedule.";
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                                    new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                    return false;
                }
                if (procDate1.SelectedDate == null)
                {
                    msgText = "Please select the date.";
                    procDate1.SetValidation(msgText);
                    procDate1.RaiseValidationError();
                    procDate1.Focus();
                    result = false;
                    return result;
                }
                else
                    procDate1.ClearValidationError();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Check Weather The Doctor Having the Schedule or Not.
        /// </summary>
        /// <param name="ListSchedule"></param>
        private void CheckDoctorScheduleAvailable(List<clsSchedule> ListSchedule)
        {
            DateTime NextDay = DateTime.Now;
            GetDoctorScheduleTimeVO BizAction = new GetDoctorScheduleTimeVO();
            BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (CmbDoctor.SelectedItem != null)
                BizAction.DoctorId = ((MasterListItem)CmbDoctor.SelectedItem).ID;
            List<clsSchedule> listSchedule = new List<clsSchedule>();
            List<clsSchedule> CurrentScheduleList = (from Obj in ListSchedule
                                                     where Obj.IsCurrentSchedule.Equals(true)
                                                     select Obj).ToList<clsSchedule>();
            if (CurrentScheduleList.Count > 0)
            {
                AddDoctor();
            }
            else
            {
                msgText = "Schedule not available for given day.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// MEthod Used to Generate the Schedule for the Doctor.
        /// </summary>
        /// <param name="ListSchedule"></param>
        /// <param name="IsFromDoctorSchedule"></param>
        //private void BindDoctorScheduleData(List<clsSchedule> ListSchedule, bool IsFromDoctorSchedule)
        //{
        //    DateTime NextDay = DateTime.Now, 
        //    CurrentDate = DateTime.Now;
        //    List<clsSchedule> listSchedule = new List<clsSchedule>();
        //    List<clsSchedule> CurrentScheduleList = (from Obj in ListSchedule
        //                                             where Obj.IsCurrentSchedule.Equals(true)
        //                                             select Obj).ToList<clsSchedule>();

        //    if (CurrentScheduleList.Count > 0)
        //    {
        //        for (int index = 0; index <= CurrentScheduleList.Count; index++)
        //        {
        //            clsSchedule _NewObject = new clsSchedule();
        //            _NewObject.RowID = index + 1;
        //            if (CurrentScheduleList.Count > index)
        //            {
        //                _NewObject.NoOfPatient = CurrentScheduleList[index].NoOfPatient;
        //                _NewObject.CurrentScheduleDate = CurrentScheduleList[index].CurrentScheduleDate;
        //                _NewObject.ScheduleFromTime = CurrentScheduleList[index].ScheduleFromTime;
        //                _NewObject.ScheduleToTime = CurrentScheduleList[index].ScheduleToTime;
        //                _NewObject.ScheduleFromTimeString = CurrentScheduleList[index].ScheduleFromTime.ToString();
        //                _NewObject.ScheduleToTimeString = CurrentScheduleList[index].ScheduleToTime.ToString();
        //                CurrentDate = CurrentScheduleList[index].CurrentScheduleDate;
        //                if (CurrentDate != null)
        //                {
        //                    DateTime AppTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _NewObject.ScheduleFromTime.Hour, _NewObject.ScheduleFromTime.Minute, 0);
        //                    if (DateTime.Now > AppTime)
        //                    {
        //                        _NewObject.CurrentapColor = new SolidColorBrush(Colors.Gray);
        //                        _NewObject.VisibleCurrentSchedule = "Collapsed";
        //                    }
        //                    else
        //                    {
        //                        _NewObject.CurrentapColor = new SolidColorBrush(Colors.Green);
        //                        _NewObject.VisibleCurrentSchedule = "Visible";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                _NewObject.ScheduleFromTimeString = string.Empty;
        //                _NewObject.ScheduleToTimeString = string.Empty;
        //            }

        //            if (listSchedule == null)
        //            {
        //                listSchedule = new List<clsSchedule>();
        //            }

        //            if (this.schedulerOutput != null)
        //            {
        //                if (this.schedulerOutput.ScheduleFromTime.ToLocalTime() <= _NewObject.ScheduleFromTime.ToLocalTime() &&
        //                    this.schedulerOutput.ScheduleToTime.ToLocalTime() >= _NewObject.ScheduleToTime.ToLocalTime())
        //                {
        //                    _NewObject.CurrentIsChecked = true;
        //                    SelectedCurrentDateAppointment.Add(_NewObject);
        //                    tpFromTime.Value = SelectedCurrentDateAppointment.Min(S => S.ScheduleFromTime);
        //                    tpToTime.Value = SelectedCurrentDateAppointment.Max(S => S.ScheduleToTime);
        //                }
        //            }
        //            listSchedule.Add(_NewObject);
        //        }
        //    }

        //    clsGetAppointmentByDoctorAndAppointmentDateBizActionVO BizActionobj = new clsGetAppointmentByDoctorAndAppointmentDateBizActionVO();
        //    BizActionobj.DoctorId = ((MasterListItem)CmbDoctor.SelectedItem).ID;
        //    BizActionobj.DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code;
        //    //BizActionobj.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    BizActionobj.AppointmentDate = CurrentDate;
        //    BizActionobj.NextAppointmentDate = null;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (sa, arg1) =>
        //    {
        //        if (arg1.Error == null)
        //        {
        //            if (arg1.Result != null)
        //            {
        //                if (((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList != null)
        //                {

        //                    List<clsAppointmentVO> AppointmentList = ((clsGetAppointmentByDoctorAndAppointmentDateBizActionVO)arg1.Result).AppointmentDetailsList;
        //                    Color C = new Color() { R = 255, B = 0, G = 0, A = 255 };
        //                    bool IsAllReadyAppointmentCheck = false;
        //                    //For already booked time
        //                    foreach (clsAppointmentVO a in AppointmentList)
        //                    {
        //                        if (listSchedule != null)
        //                        {
        //                            for (int ListCount = 0; ListCount < listSchedule.Count; ListCount++)
        //                            {
        //                                if (CurrentDate.Date == a.AppointmentDate)
        //                                {

        //                                    int AppointmentCount = AppointmentList.Where(S => S.FromTime >= ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime && S.ToTime <= ((clsSchedule)listSchedule[ListCount]).ScheduleToTime).Count();
        //                                    if (((clsSchedule)listSchedule[ListCount]).NoOfPatient.Equals(AppointmentCount))
        //                                    {
        //                                        IsAllReadyAppointmentCheck = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        IsAllReadyAppointmentCheck = true;
        //                                    }

        //                                    if (IsAllReadyAppointmentCheck)
        //                                    {
        //                                        if (((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay <= a.ToTime.TimeOfDay || ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.TimeOfDay)
        //                                        {
        //                                            if (a.FromTime.TimeOfDay == ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay)
        //                                            {
        //                                                listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
        //                                                listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
        //                                            }
        //                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.FromTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.ToTime.TimeOfDay)
        //                                            {
        //                                                listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
        //                                                listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
        //                                            }
        //                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.FromTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay > a.FromTime.TimeOfDay)
        //                                            {
        //                                                listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
        //                                                listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
        //                                            }
        //                                            else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.FromTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.ToTime.TimeOfDay)
        //                                            {
        //                                                listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
        //                                                listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            break;
        //                                        }
        //                                        IsAllReadyAppointmentCheck = false;
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //        else
        //        {
        //            if (objLocalizationManager != null)
        //            {
        //                msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
        //            }
        //            else
        //            {
        //                msgText = "Error occured while processing.";
        //            }
        //            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //        }


        //    };

        //    client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //    dgTimeSlots.ItemsSource = null;
        //    dgTimeSlots.ItemsSource = listSchedule;
        //    dgTimeSlots.SelectedIndex = -1;
        //}

        /// <summary>
        /// Fetches staff by designation
        /// </summary>
        /// 
        List<MasterListItem> StaffList = null;
        private void FetchStaffByDesignation(string designationID)
        {
            try
            {
                clsStaffByDesignationIDBizActionVO bizActionVo = new clsStaffByDesignationIDBizActionVO();

                MasterListItem firstObj = new MasterListItem();
                firstObj.ID = 0;
                firstObj.Description = "--Select--";

                StaffList = new List<MasterListItem>();
                StaffList.Add(firstObj);

                //if (procList1 != null && CmbStaffProcedure.SelectedItem != null)
                //{
                //    bizActionVo.ProcedureID = ((MasterListItem)CmbStaffProcedure.SelectedItem).ID;
                bizActionVo.DesignationID = Convert.ToInt64(designationID);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        foreach (var item in ((clsStaffByDesignationIDBizActionVO)e.Result).StaffDetails)
                        {
                            if ((StaffList.Where(S => S.ID == item.ID)).ToList().Count < 1)
                                StaffList.Add(item);
                        }

                        CmbStaff.ItemsSource = null;
                        CmbStaff.ItemsSource = StaffList;
                        if (StaffList.Count == 2)
                            CmbStaff.SelectedItem = StaffList[1];
                        else
                            CmbStaff.SelectedItem = StaffList[0];

                        //if (CmbDesignation.SelectedItem != null)
                        //{
                        //    if (((MasterListItem)CmbDesignation.SelectedItem).Code != "0")
                        //    {
                        //        txtStaffQuantity.Text = ((clsStaffByDesignationIDBizActionVO)e.Result).staffQuantity.ToString();
                        //    }
                        //    else
                        //        txtStaffQuantity.Text = "0";
                        //}
                        //else
                        //    txtStaffQuantity.Text = "0";

                        if (flagChangeStaff)
                        {
                            foreach (var item in StaffList)
                            {
                                if (item.ID == staffId)
                                    CmbStaff.SelectedItem = item;
                            }
                            //txtStaffQuantity.Text = quantity;
                            flagChangeStaff = false;
                        }
                    }
                };

                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                //}
                //else
                //    txtStaffQuantity.Text = "0";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private void FetchDoctorByDocSpecialization(string SpecialCode)
        //{
        //    clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.IsForReferral = true;
        //    if ((MasterListItem)CmbDocType.SelectedItem != null)
        //        BizAction.SpecialCode = SpecialCode;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem("0", "-- Select --"));
        //            objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
        //            CmbDoctor.ItemsSource = null;
        //            CmbDoctor.ItemsSource = objList;

        //            CmbDoctor.SelectedItem = objList[0];
        //            //if (this.DataContext != null)
        //            //{
        //            //    CmbDoctor.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ID;
        //            //}
        //            if (blEditDoc)
        //            {
        //                foreach (var item in objList)
        //                {
        //                    if (item.Code == DCode)
        //                        CmbDoctor.SelectedItem = item;
        //                }
        //                blEditDoc = false;
        //            }


        //            //CmbDoctor.ItemsSource = null;
        //            //if (((MasterListItem)CmbDocType.SelectedItem).Description.Trim() == CurrentVisit.DoctorSpecialization.Trim())
        //            //{
        //            //    MasterListItem objDoc = objList.Where(z => z.Code.Trim() == CurrentVisit.DoctorCode.Trim()).FirstOrDefault();
        //            //    objList.Remove(objDoc);
        //            //}
        //            //CmbDoctor.ItemsSource = objList;
        //            //if (this.DataContext != null)
        //            //{
        //            //    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
        //            //    {
        //            //        CmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
        //            //    }
        //            //    CmbDoctor.SelectedItem = objList[0];
        //            //}
        //            //else
        //            //    CmbDoctor.SelectedItem = objList[0];
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}


        /// <summary>
        /// Gets doctors from doctor  classification
        /// </summary>
        /// <param name="docClassification"></param>
        private void FetchDoctorByDocSpecialization(string sSpecialization)
        {
            try
            {
                //clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
                //BizAction.MasterList = new List<MasterListItem>();
                //BizAction.MasterTable = MasterTableNameList.SPESIAL_DOC;
                //BizAction.CodeColumn = "KODEDOKTER";
                //BizAction.DescriptionColumn = "NAMADOKTER";
                //BizAction.Parent = new KeyValue();
                //BizAction.Parent.Value = "KDSPESIAL";
                //BizAction.Parent.Key = sSpecialization;
                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //client.ProcessCompleted += (s, arg) =>
                //{
                //    if (arg.Error == null && arg.Result != null)
                //    {
                //        List<MasterListItem> objList = new List<MasterListItem>();
                //        objList.Add(new MasterListItem("0", "-- Select --"));
                //        objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                //        CmbDoctor.ItemsSource = null;
                //        CmbDoctor.ItemsSource = objList;

                //        CmbDoctor.SelectedItem = objList[0];
                //        //if (this.DataContext != null)
                //        //{
                //        //    CmbDoctor.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ID;
                //        //}
                //        if (blEditDoc)
                //        {
                //            foreach (var item in objList)
                //            {
                //                if (item.Code == DCode)
                //                    CmbDoctor.SelectedItem = item;
                //            }
                //            blEditDoc = false;
                //        }
                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                //client.CloseAsync();







                clsGetDoctorListBySpecializationBizActionVO BizActionVo = new clsGetDoctorListBySpecializationBizActionVO();
                BizActionVo.SpecializationCode = sSpecialization;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "-- Select --"));
                        objList.AddRange(((clsGetDoctorListBySpecializationBizActionVO)e.Result).DocDetails);
                        //if (!flagChangeDoc)
                        //{
                        CmbDoctor.ItemsSource = null;
                        CmbDoctor.ItemsSource = objList;
                        //if (dgDocAddedTimeSlots1.SelectedItem != null)
                        //{
                        //    CmbDoctor.SelectedValue = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).DocID;
                        //}
                        //else
                        CmbDoctor.SelectedItem = objList[0];
                        if (blEditDoc)
                        {
                            foreach (var item in objList)
                            {
                                if (item.ID == docId)
                                    CmbDoctor.SelectedItem = item;
                            }
                            blEditDoc = false;
                        }
                        //}
                        //else
                        //{
                        //    flagChangeDoc = false;
                        //}


                    }

                };

                Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the Patient Data By MRNo.
        /// </summary>
        public void GetPatientData(string sMRNo)
        {
            if (waitIndicator == null)
                waitIndicator = new WaitIndicator();
            if (waitIndicator.Visibility == Visibility.Collapsed)
                waitIndicator.Show();
            else
            {
                waitIndicator.Close();
                waitIndicator.Show();
            }

            clsGetOTPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetOTPatientGeneralDetailsListBizActionVO();
            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
            BizActionObject.MRNo = sMRNo;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsGetOTPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetOTPatientGeneralDetailsListBizActionVO;
                    if (result.PatientDetailsList.Count > 0)
                    {
                        SelectedPatient = result.PatientDetailsList[0];
                        BindSelectedPatient();
                    }
                    else
                    {
                        msgText = "Please check MR number.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        txtMRNo.Text = string.Empty;
                    }
                    waitIndicator.Close();
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void BindSelectedPatient()
        {
            txtMRNo.Text = SelectedPatient.MRNo;
            // Comman.SetPatientDetailHeader(PatientVO.PatientDetails);
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            // TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            // mElement.Text = " : " + SelectedPatient.PatientName;
            // mElement.Text += " - " + SelectedPatient.MRNo + " : " + SelectedPatient.Gender;

            txtFirstName.Text = SelectedPatient.PatientName;
            txtGender.Text = SelectedPatient.Gender;
            txtMaritalStatus.Text = SelectedPatient.MaritalStatus;
            txtEducation.Text = SelectedPatient.Education;
            txtReligion.Text = SelectedPatient.Religion;
            txtContactNO1.Text = SelectedPatient.ContactNO1;

            //if (SelectedPatient.Photo != null)
            //if (SelectedPatient.ImageName.Length > 0)
            //{
            //    //commented by neena
            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
            //    //bmp.FromByteArray(SelectedPatient.Photo);
            //    //imgPhoto.Source = bmp;
            //    //

            //    //added by neena
            //    imgPhoto.Source = new BitmapImage(new Uri(SelectedPatient.ImageName, UriKind.Absolute));
            //    //
            //}

            if (SelectedPatient.ImageName != null && SelectedPatient.ImageName.Length > 0)
            {
                imgPhoto.Source = new BitmapImage(new Uri(SelectedPatient.ImageName, UriKind.Absolute));
            }
            else if (SelectedPatient.Photo != null)
            {
                byte[] imageBytes = SelectedPatient.Photo;
                BitmapImage img = new BitmapImage();
                img.SetSource(new MemoryStream(imageBytes, false));
                imgPhoto.Source = img;
            }
            else
                imgPhoto.Source = null;

            btnAttachPhoto.IsEnabled = true;

            if (SelectedPatient.DateOfBirth != null)
            {
                txtDOB.Text = Convert.ToDateTime(SelectedPatient.DateOfBirth).ToString("dd/MM/yyyy");
                txtAge.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY") + " years";
            }

        }

        private void GetSchedule()
        {
            SelectedCurrentDateSchedule = new List<clsSchedule>();
            if (procDate1.SelectedDate == null)
            {
                msgText = "Please select the date.";
                procDate1.SetValidation(msgText);
                procDate1.RaiseValidationError();
                procDate1.Focus();
                return;
            }
            else
                procDate1.ClearValidationError();
            if (procDate1.SelectedDate < DateTime.Now.Date.Date && isView != true)
            {
                msgText = "Date should be greater than current date.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            else if (CmbOT1.SelectedItem == null)
            {
                msgText = "Please select operation theatre.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            else if (((MasterListItem)CmbOT1.SelectedItem).ID == 0)
            {
                msgText = "Please select operation theatre.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            else if (CmbOTTableBack1.SelectedItem == null || ((MasterListItem)CmbOTTableBack1.SelectedItem).ID == 0)
            {
                msgText = "Please select OT table.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            else
                FetchOTSchedule();
        }

        /// <summary>
        /// Bind the data.
        /// </summary>
        /// <param name="ListSchedule"></param>
        /// <param name="IsFromOTSchedule"></param>
        private void BindOTScheduleData(List<clsSchedule> ListSchedule, bool IsFromOTSchedule)
        {
            DateTime CurrentDate = DateTime.Now;
            clsGetOTScheduleTimeVO BizAction = new clsGetOTScheduleTimeVO();
            BizAction.OTScheduleDetailsList = new List<clsOTScheduleDetailsVO>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (CmbOT1.SelectedItem != null)
                BizAction.OTID = ((MasterListItem)CmbOT1.SelectedItem).ID;
            if (CmbOTTableBack1.SelectedItem != null)
                BizAction.OTTabelID = ((MasterListItem)CmbOTTableBack1.SelectedItem).ID;
            List<clsSchedule> listSchedule = null;
            List<clsSchedule> CurrentScheduleList = (from Obj in ListSchedule
                                                     where Obj.IsCurrentSchedule.Equals(true)
                                                     select Obj).ToList<clsSchedule>();
            if (CurrentScheduleList.Count > 0)
            {
                for (int index = 0; index <= CurrentScheduleList.Count; index++)
                {
                    clsSchedule _NewObject = new clsSchedule();
                    _NewObject.RowID = index + 1;
                    if (CurrentScheduleList.Count > index)
                    {
                        _NewObject.NoOfPatient = CurrentScheduleList[index].NoOfPatient;
                        _NewObject.CurrentScheduleDate = CurrentScheduleList[index].CurrentScheduleDate;
                        _NewObject.ScheduleFromTime = CurrentScheduleList[index].ScheduleFromTime;
                        _NewObject.ScheduleToTime = CurrentScheduleList[index].ScheduleToTime;
                        _NewObject.ScheduleFromTimeString = CurrentScheduleList[index].ScheduleFromTime.ToString();
                        _NewObject.ScheduleToTimeString = CurrentScheduleList[index].ScheduleToTime.ToString();
                        CurrentDate = CurrentScheduleList[index].CurrentScheduleDate;
                        if (CurrentDate != null)
                        {
                            DateTime AppTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, _NewObject.ScheduleFromTime.Hour, _NewObject.ScheduleFromTime.Minute, 0);
                            if (DateTime.Now > AppTime)
                            {
                                _NewObject.CurrentapColor = new SolidColorBrush(Colors.Gray);
                                _NewObject.VisibleCurrentSchedule = "Collapsed";
                            }
                            else
                            {
                                _NewObject.CurrentapColor = new SolidColorBrush(Colors.Green);
                                _NewObject.VisibleCurrentSchedule = "Visible";
                            }
                        }
                    }
                    else
                    {
                        _NewObject.ScheduleFromTimeString = string.Empty;
                        _NewObject.ScheduleToTimeString = string.Empty;
                    }
                    if (listSchedule == null)
                    {
                        listSchedule = new List<clsSchedule>();
                    }
                    if (this.schedulerOutput != null)
                    {
                        if (this.schedulerOutput.ScheduleFromTime.ToLocalTime() <= _NewObject.ScheduleFromTime.ToLocalTime() &&
                            this.schedulerOutput.ScheduleToTime.ToLocalTime() >= _NewObject.ScheduleToTime.ToLocalTime())
                        {
                            _NewObject.CurrentIsChecked = true;
                            SelectedCurrentDateSchedule.Add(_NewObject);
                            tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);
                        }
                    }
                    listSchedule.Add(_NewObject);
                }
            }
            else
            {
                if (objLocalizationManager != null)
                {
                    msgText = objLocalizationManager.GetValue("ScheduleForGivenDay_Msg");
                }
                else
                {
                    msgText = "Schedule not available for given day.";
                }

                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
            clsPatientProcedureScheduleVO objPatientDetails = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem);

            CurrentScheduleHeader.Text = "Current Schedule (" + CurrentDate.ToString("dd/MM/yyyy") + " )";
            clsGetOTBookingByOTTablebookingDateBizActionVO BizActionobj = new clsGetOTBookingByOTTablebookingDateBizActionVO();
            if (blView == true)
            {
                ScheduleID = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).ID;
                blView = false;
            }
            BizActionobj.OTID = ((MasterListItem)CmbOT1.SelectedItem).ID;
            BizActionobj.OTTableID = ((MasterListItem)CmbOTTableBack1.SelectedItem).ID;
            BizActionobj.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionobj.OTDate = CurrentDate;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (sa, arg1) =>
            {
                if (arg1.Error == null)
                {
                    if (arg1.Result != null)
                    {
                        if (((clsGetOTBookingByOTTablebookingDateBizActionVO)arg1.Result).bookingDetailsList != null)
                        {
                            List<clsPatientProcedureScheduleVO> bookingList = ((clsGetOTBookingByOTTablebookingDateBizActionVO)arg1.Result).bookingDetailsList;
                            Color C = new Color() { R = 255, B = 0, G = 0, A = 255 };
                            //Color Y = new Color() { R = 255, B = 0, G =255 , A = 255 };
                            Color Y = new Color() { R = 218, B = 32, G = 165, A = 255 };
                            //Color Y = new Color() { R = 204, B = 0, G = 204, A = 204 };
                            bool IsAllReadybookedCheck = false;
                            //For already booked time
                            if (bookingList.Count > 0)
                            {
                                foreach (clsPatientProcedureScheduleVO a in bookingList)
                                {
                                    if (listSchedule != null)
                                    {
                                        for (int ListCount = 0; ListCount < listSchedule.Count; ListCount++)
                                        {
                                            if (CurrentDate.Date == a.Date)
                                            {
                                                int bookingCount = bookingList.Where(S => S.StartTime >= ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime && S.EndTime <= ((clsSchedule)listSchedule[ListCount]).ScheduleToTime).Count();
                                                IsAllReadybookedCheck = true;
                                                if (IsAllReadybookedCheck)
                                                {
                                                    if (((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay < a.EndTime.TimeOfDay || ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.EndTime.TimeOfDay || ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay == a.EndTime.TimeOfDay)
                                                    {
                                                        if (a.StartTime.TimeOfDay == ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            // listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            if (a.Date > DateTime.Now.Date)
                                                            {
                                                                listSchedule[ListCount].PatientName = a.PatientName;

                                                                if (ScheduleID == a.ID)
                                                                {
                                                                   // listSchedule[ListCount - 1].VisibleCurrentSchedule = "Collapsed";
                                                                    listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                                    //listSchedule[ListCount].CurrentIsChecked = true;
                                                                    //ChecklistSchedule[ListCount].CurrentIsChecked = true;
                                                                }
                                                                else
                                                                {
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                                    listSchedule[ListCount - 1].VisibleCurrentSchedule = "Collapsed";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                listSchedule[ListCount].PatientName = a.PatientName;
                                                                if (a.StartTime.TimeOfDay < DateTime.Now.TimeOfDay)
                                                                {
                                                                    if (ScheduleID == a.ID)
                                                                    {
                                                                        listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                        listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                                    }
                                                                    else
                                                                    {
                                                                        listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";                                                                      
                                                                    }

                                                                }
                                                                else if (ScheduleID == a.ID)
                                                                {
                                                                    listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                                }
                                                                else
                                                                {
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                                    listSchedule[ListCount - 1].VisibleCurrentSchedule = "Collapsed";
                                                                }
                                                            }
                                                        }

                                                        else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.StartTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.EndTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            //listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            if (a.Date > DateTime.Now.Date)
                                                            {
                                                                listSchedule[ListCount].PatientName = a.PatientName;
                                                                if (ScheduleID == a.ID)
                                                                {
                                                                    listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                                    //listSchedule[ListCount].CurrentIsChecked = true;
                                                                    //ChecklistSchedule[ListCount].CurrentIsChecked = true;
                                                                }
                                                                else
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";                                                                 
                                                                     
                                                            }
                                                            else
                                                            {
                                                                listSchedule[ListCount].PatientName = a.PatientName;
                                                                if (a.StartTime.TimeOfDay < DateTime.Now.TimeOfDay)
                                                                {
                                                                    if (ScheduleID == a.ID)
                                                                    {
                                                                        listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                        listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                                    }
                                                                    else
                                                                        listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                                }
                                                                else if (ScheduleID == a.ID)
                                                                {
                                                                    listSchedule[ListCount].CurrentapColor = new SolidColorBrush(Y);
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                                }
                                                                else
                                                                    listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            }
                                                        }
                                                        else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.StartTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleToTime.TimeOfDay > a.StartTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            //listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                        }
                                                        else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay >= a.StartTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay <= a.EndTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            //listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                        }
                                                        else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.StartTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.EndTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            //listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                        }
                                                        else if (((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay > a.StartTime.TimeOfDay && ((clsSchedule)listSchedule[ListCount]).ScheduleFromTime.TimeOfDay < a.EndTime.TimeOfDay)
                                                        {
                                                            listSchedule[ListCount].CurrentapColor = new SolidColorBrush(C);
                                                            listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                            //listSchedule[ListCount].VisibleCurrentSchedule = "Visible";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (listSchedule[ListCount - 1].VisibleCurrentSchedule == "Collapsed")
                                                        {
                                                            listSchedule[ListCount].VisibleCurrentSchedule = "Collapsed";
                                                        }
                                                        break;
                                                    }
                                                    IsAllReadybookedCheck = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            dgTimeSlots1.ItemsSource = null;
                            dgTimeSlots1.ItemsSource = listSchedule;
                            dgTimeSlots1.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    if (objLocalizationManager != null)
                    {
                        msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                    }
                    else
                    {
                        msgText = "Error occured while processing.";
                    }
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            };
            client.ProcessAsync(BizActionobj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        /// <summary>
        /// Fetches Ot Schedule irrespective of PatientProcSceduleID
        /// </summary>
        /// 
       
        private void FetchOTSchedule()
        {
            try
            {
                if (procDate1.SelectedDate != null)
                {
                    if (CmbOT1.SelectedItem != null && ((MasterListItem)CmbOT1.SelectedItem).ID != 0)
                    {
                        if (CmbOTTableBack1.SelectedItem != null && ((MasterListItem)CmbOTTableBack1.SelectedItem).ID != 0)
                        {
                            clsGetOTScheduleTimeVO BizAction = new clsGetOTScheduleTimeVO();
                            BizAction.OTScheduleDetailsList = new List<clsOTScheduleDetailsVO>();
                            if (CmbOT1.SelectedItem != null)
                                BizAction.OTID = ((MasterListItem)CmbOT1.SelectedItem).ID;

                            if (CmbOTTableBack1.SelectedItem != null)
                                BizAction.OTTabelID = ((MasterListItem)CmbOTTableBack1.SelectedItem).ID;

                            string objDay = null;
                            if (this.schedulerOutput != null)
                            {
                                if (this.schedulerOutput.OTDate != null)
                                {
                                    objDay = this.schedulerOutput.OTDate.DayOfWeek.ToString();
                                    procDate1.SelectedDate = this.schedulerOutput.OTDate;
                                }
                                else
                                {
                                    objDay = this.schedulerOutput.CurrentScheduleDate.DayOfWeek.ToString();
                                    procDate1.SelectedDate = this.schedulerOutput.CurrentScheduleDate;
                                }
                            }
                            else if (procDate1.SelectedDate != null)
                            {
                                objDay = procDate1.SelectedDate.Value.DayOfWeek.ToString();
                            }
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            dgTimeSlots1.ItemsSource = null;
                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null && (clsGetOTScheduleTimeVO)arg.Result != null)
                                {
                                    if (((clsGetOTScheduleTimeVO)arg.Result).OTScheduleDetailsList != null && ((clsGetOTScheduleTimeVO)arg.Result).OTScheduleDetailsList.Count != 0)
                                    {
                                        clsGetOTScheduleTimeVO DetailsVO = new clsGetOTScheduleTimeVO();
                                        DetailsVO = (clsGetOTScheduleTimeVO)arg.Result;
                                        List<clsSchedule> listSchedule = new List<clsSchedule>();
                                        ChecklistSchedule = new List<clsSchedule>();
                                        if (DetailsVO.OTScheduleDetailsList != null)
                                        {
                                            if (DetailsVO.OTScheduleDetailsList.Where(S => S.Day.Equals(procDate1.SelectedDate.Value.DayOfWeek.ToString())).Count() > 0)
                                            {
                                                int DCount = 0;
                                                DateTime AvailableFirstScheduleDate = DateTime.Now;
                                                foreach (clsOTScheduleDetailsVO item in DetailsVO.OTScheduleDetailsList)
                                                {
                                                    string objNextDay = procDate1.SelectedDate.Value.AddDays(DCount).DayOfWeek.ToString();
                                                    DateTime Date = procDate1.SelectedDate.Value.AddDays(DCount);

                                                    List<clsOTScheduleDetailsVO> OTScheduleCount = (from ScheduleObject in DetailsVO.OTScheduleDetailsList
                                                                                                    where ScheduleObject.Day.Equals(objNextDay) && ScheduleObject.OTTableID.Equals(BizAction.OTTabelID)
                                                                                                    select ScheduleObject).ToList<clsOTScheduleDetailsVO>();
                                                    if (OTScheduleCount.Count != 0)
                                                    {
                                                        AvailableFirstScheduleDate = Date;
                                                        break;
                                                    }
                                                    DCount++;
                                                }
                                                int IsNextDaySchedule = 0;
                                                for (int DayCount = 0; DayCount <= 7; DayCount++)
                                                {
                                                    string objNextDay = AvailableFirstScheduleDate.DayOfWeek.ToString();
                                                    DateTime Date = AvailableFirstScheduleDate; //procDate1.SelectedDate.Value.AddDays(DayCount);
                                                    List<clsOTScheduleDetailsVO> OTScheduleCount = (from ScheduleObject in DetailsVO.OTScheduleDetailsList
                                                                                                    where ScheduleObject.Day.Equals(objNextDay) && ScheduleObject.OTTableID.Equals(BizAction.OTTabelID)
                                                                                                    select ScheduleObject).ToList<clsOTScheduleDetailsVO>();
                                                    if (OTScheduleCount.Count != 0)
                                                    {
                                                        int ScheduleCount = 1;
                                                        foreach (clsOTScheduleDetailsVO item in OTScheduleCount)
                                                        {
                                                            if (item.StartTime != null && item.EndTime != null)
                                                            {
                                                                double ScheduleHrs = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime)).TotalMinutes / OTTimeSlots;
                                                                DateTime? Dt = null;
                                                                if (item.StartTime != null)
                                                                    Dt = item.StartTime;
                                                                for (int Schedulecount = 0; Schedulecount < ScheduleHrs; Schedulecount++)
                                                                {
                                                                    clsSchedule _listItem = new clsSchedule();
                                                                    _listItem.Day = "Schedule" + ScheduleCount;

                                                                    if (DayCount.Equals(0))
                                                                    {
                                                                        _listItem.IsCurrentSchedule = true;
                                                                        _listItem.CurrentScheduleDate = Date;
                                                                    }
                                                                    else
                                                                    {
                                                                        _listItem.IsCurrentSchedule = false;
                                                                    }
                                                                    _listItem.ScheduleFromTime = Dt.Value;
                                                                    _listItem.ScheduleToTime = (Dt.Value.AddMinutes(OTTimeSlots));
                                                                    if (_listItem.CurrentapColor != null)
                                                                        _listItem.CurrentapColor = new SolidColorBrush(Colors.Green);
                                                                    listSchedule.Add(_listItem);
                                                                    ChecklistSchedule.Add(_listItem);
                                                                    Dt = Dt.Value.AddMinutes(OTTimeSlots);
                                                                }
                                                            }
                                                            ScheduleCount++;
                                                        }
                                                        IsNextDaySchedule++;
                                                    }
                                                    else
                                                    {
                                                        if (DayCount.Equals(0))
                                                        {
                                                            IsNextDaySchedule++;
                                                        }
                                                    }
                                                    if (IsNextDaySchedule.Equals(1))
                                                        break;
                                                }
                                                BindOTScheduleData(listSchedule, true);
                                            }
                                            else
                                            {
                                                msgText = "Schedule not available for selected OT table";
                                                //msgText = objLocalizationManager.GetValue("ScheduleForSelectedOTTable_Msg");
                                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        msgText = "Schedule not available for selected OT table.";
                                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    msgText = "Error occured while processing.";
                                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        else
                        {
                            msgText = "Please select operation theatre.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        msgText = "Please select OT table.";
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                }
                else
                {
                    msgText = "Please select OT Schedule date.";
                    //msgText = objLocalizationManager.GetValue("lblSelectOTScheduleDate");
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Fetch OTTable according to OT
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTTable(long OtTableID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTableMaster;
                BizAction.Parent = new KeyValue { Value = "OTTheatreID", Key = OtTableID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOTTableBack1.ItemsSource = null;
                        CmbOTTableBack1.ItemsSource = objList;

                        if (schedulerOutput != null)
                        {
                            CmbOTTableBack1.SelectedItem = ((List<MasterListItem>)CmbOTTableBack1.ItemsSource).SingleOrDefault(S => S.ID.Equals(this.schedulerOutput.OTTableID));
                            FetchOTSchedule();
                        }
                        else if (isView == true)
                        {
                            CmbOTTableBack1.SelectedValue = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).OTTableID;
                            FetchOTSchedule();
                        }
                        else
                        {
                            CmbOTTableBack1.SelectedItem = objList[0];
                        }
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

        /// <summary>
        /// Clears UI element
        /// </summary>
        private void ClearUI()
        {
            isView = false;
            try
            {
                this.DataContext = new clsPatientProcedureScheduleVO();
                CmbOT1.SelectedValue = Convert.ToInt64(0);
                SelectedPatient = new clsPatientGeneralVO();
                CmbOTTableBack1.SelectedValue = Convert.ToInt64(0);
                dgTimeSlots1.ItemsSource = null;
                OTList1 = new ObservableCollection<clsPatientProcedureScheduleVO>();
                SelectedCurrentDateSchedule = new List<clsSchedule>();
                CmbDocType.SelectedValue = Convert.ToInt64(0);
                CmbDoctor.SelectedValue = Convert.ToInt64(0);
                tpFromTime.Value = null;
                tpToTime.Value = null;
                procList1 = new List<clsPatientProcedureVO>();
                dgDocAddedTimeSlots1.ItemsSource = null;
                dgProcedureStaff.ItemsSource = null;
                DocList = new ObservableCollection<clsPatientProcDocScheduleDetailsVO>();
                StaffList1 = new ObservableCollection<clsPatientProcStaffDetailsVO>();
                CmbDocProcedure.SelectedItem = null;
                CmbStaffProcedure.SelectedItem = null;
                CmbDesignation.SelectedValue = Convert.ToInt64(0);
                txtStaffQuantity.Text = string.Empty;
                CmbStaff.SelectedValue = new MasterListItem("0", "--Select--");
                CmbStaff.Text = string.Empty;
                if (txtProcDuration != null)
                    txtProcDuration.Text = string.Empty;
                grdProcedure.ItemsSource = null;
                PatientWiseProcCheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                grdchecklist.ItemsSource = null;
                TabControlMain.SelectedItem = procDetails;
                PatientID = 0;
                txtFirstName.Text = string.Empty;
                txtGender.Text = string.Empty;
                txtDOB.Text = string.Empty;
                txtAge.Text = string.Empty;
                txtMaritalStatus.Text = string.Empty;
                txtEducation.Text = string.Empty;
                txtReligion.Text = string.Empty;
                txtContactNO1.Text = string.Empty;
                imgPhoto.Source = null;
                rdIPD.IsChecked = true;
                chkEmergency.IsChecked = false;
                chkExcel.IsChecked = false;
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method used to bind the values to the controls
        /// </summary>
        /// <param name="PatientVO"></param>
        /// <param name="Indicatior"></param>
        private void BindSelectedPatientDetails(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            txtMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
            PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;
            // Comman.SetPatientDetailHeader(PatientVO.PatientDetails);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            mElement.Text = " : " + PatientVO.PatientDetails.GeneralDetails.FirstName + " " + PatientVO.PatientDetails.GeneralDetails.MiddleName + " " + PatientVO.PatientDetails.GeneralDetails.LastName;
            mElement.Text += " - " + PatientVO.PatientDetails.GeneralDetails.MRNo + " : " + PatientVO.PatientDetails.GeneralDetails.Gender;
            Indicatior.Close();
        }

        /// <summary>
        /// Fills from grid
        /// </summary>
        private void FetchData()
        {
            WaitIndicator indicator = new WaitIndicator();

            bool res = true;

            if ((SearchDate.SelectedDate != null)  && (ToSearchDate.SelectedDate != null))
            {
                if (SearchDate.SelectedDate.Value.Date > ToSearchDate.SelectedDate.Value.Date)
                {                  
                    msgText = "From Date Should Be Less Than To Date";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    res = false;
                }
                else
                {
                    SearchDate.ClearValidationError();
                }
            }

            if ((SearchDate.SelectedDate != null) && (SearchDate.SelectedDate.Value.Date > DateTime.Now.Date && res == true))
            {               
                msgText = "From Date Should Not Be Greater Than Today's Date";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                res = false;               
            }
            else
            {
                SearchDate.ClearValidationError();
            }
            if (res)
            {
                try
                {
                    clsGetPatientProcedureScheduleBizActionVO BizAction = new clsGetPatientProcedureScheduleBizActionVO();

                    indicator.Show();
                    BizAction.patientProcScheduleDetails = new List<clsPatientProcedureScheduleVO>();
                   BizAction.OTUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    BizAction.IsPagingEnabled = true;
                    BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                    BizAction.MaximumRows = DataList.PageSize;
                    if (CmbOtTheatre.SelectedItem != null)
                        BizAction.OTID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;
                    else
                        BizAction.OTID = 0;
                    if (CmbOTTable.SelectedItem != null)
                        BizAction.OTTableID = ((MasterListItem)CmbOTTable.SelectedItem).ID;
                    else
                        BizAction.OTTableID = 0;
                    if (SearchDate.SelectedDate != null)
                        BizAction.OTBookingDate = SearchDate.SelectedDate.Value.Date;
                    if (ToSearchDate.SelectedDate != null)
                        BizAction.OTTODate = ToSearchDate.SelectedDate.Value.Date;
                    if (txtSearchCriteria.Text.Trim() != null)
                        BizAction.FirstName = txtSearchCriteria.Text.Trim();
                    if (txtLastName.Text.Trim() != null)
                        BizAction.LastName = txtLastName.Text.Trim();
                    if (txtMRNO.Text.Trim() != null)
                        BizAction.MRNo = txtMRNO.Text.Trim();
                    if (this.SelectedPatient != null)
                    {
                        if (this.SelectedPatient.IsEMR)
                        {
                            BizAction.PatientID = this.SelectedPatient.PatientID;
                            BizAction.PatientUnitID = this.SelectedPatient.PatientUnitID;
                        }

                    }

                    if ((bool)chkIsEmergency.IsChecked)
                        BizAction.IsEmergency = true;
                    else
                        BizAction.IsEmergency = false;

                    if ((bool)chkIsCancelled.IsChecked)
                        BizAction.IsCancelled = true;
                    else
                        BizAction.IsCancelled = false;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsGetPatientProcedureScheduleBizActionVO)arg.Result).patientProcScheduleDetails != null)
                            {

                                clsGetPatientProcedureScheduleBizActionVO result = arg.Result as clsGetPatientProcedureScheduleBizActionVO;
                                DataList.TotalItemCount = result.TotalRows;
                                if (result.patientProcScheduleDetails != null)
                                {
                                    DataList.Clear();
                                    foreach (var item in result.patientProcScheduleDetails)
                                    {
                                        
                                            DataList.Add(item);
                                        
                                    }
                                    dgSchedule.ItemsSource = null;
                                    dgSchedule.ItemsSource = DataList;
                                    dataGrid2Pager.Source = null;
                                    dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                    dataGrid2Pager.Source = DataList;
                                    if (DataList.Count == 0)
                                    {
                                        cmdOk.IsEnabled = false;
                                        cmdCancelOTBooking.IsEnabled = false;
                                       
                                    }
                                    else if (DataList.Count > 1)
                                    {
                                        cmdOk.IsEnabled = true;                                        
                                    }
                                    else
                                        cmdCancelOTBooking.IsEnabled = true;


                                    if (DataList.Count > 0)
                                    {
                                        chkExcel.IsEnabled = true;
                                        cmdPrint.IsEnabled = true;
                                    }
                                    //if ((bool)chkIsCancelled.IsChecked)
                                    //{
                                    //    cmdCancelOTBooking.IsEnabled = false;
                                    //    foreach (DataGridColumn item in dgSchedule.Columns)
                                    //    {
                                    //        if (item.Header.ToString() == "View")
                                    //            item.IsReadOnly = true;

                                    //    }
                                    //}
                                }
                            }
                        }
                        else
                        {
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                            //}
                            //else
                            //{
                            msgText = "Error occured while processing.";
                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    indicator.Close();
                }
            }

        }

        /// <summary>
        /// Fetch the Operation Theatre as per the Procedure.
        /// </summary>
        /// 

        List<MasterListItem> docTypeList = null;
        List<MasterListItem> designationList = null;
        private void FetchOTbyProcedure()
        {
            try
            {
                clsGetOTForProcedureBizActionVO bizActionVo = new clsGetOTForProcedureBizActionVO();
                bizActionVo.procedureIDList = new List<long>();
                docTypeList = new List<MasterListItem>();
                designationList = new List<MasterListItem>();


                //MasterListItem firstObj = new MasterListItem();
                //firstObj.ID = 0;
                //firstObj.Description = "-- Select --";
                //designationList.Add(firstObj);

                docTypeList.Add(new MasterListItem("0", "-- Select --"));
                designationList.Add(new MasterListItem(0, "--Select--"));


                if (procList1 != null)
                {
                    //for (int i = 0; i < procList1.Count; i++)
                    //{
                    //    bizActionVo.procedureID = procList1[i].ProcedureID;

                    for (int i = 0; i < procList1.Count; i++)
                        bizActionVo.procedureIDList.Add(procList1[i].ProcedureID);

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, e) =>
                    {
                        if (e.Error == null && e.Result != null)
                        {
                            foreach (MasterListItem item in ((clsGetOTForProcedureBizActionVO)e.Result).DocDetails)
                            {
                                docTypeList.Add(item);
                            }
                            CmbDocType.ItemsSource = null;
                            if (docTypeList.Count > 1)
                            {
                                if (docTypeList.Count == 2)
                                {
                                    CmbDocType.ItemsSource = docTypeList;
                                    CmbDocType.SelectedItem = docTypeList[1];
                                }
                                else
                                {
                                    CmbDocType.ItemsSource = docTypeList;
                                    CmbDocType.SelectedItem = docTypeList[0];
                                }
                                objDocClsList = new List<MasterListItem>();
                                objDocClsList = docTypeList.ToList();
                            }
                            else
                                FetchDoctorClassification();



                            foreach (MasterListItem item in ((clsGetOTForProcedureBizActionVO)e.Result).DesignationDetails)
                            {
                                designationList.Add(item);
                            }
                            CmbDesignation.ItemsSource = null;
                            if (designationList.Count > 1)
                            {
                                if (designationList.Count == 2)
                                {
                                    CmbDesignation.ItemsSource = designationList; // designationList;
                                    CmbDesignation.SelectedItem = designationList[1];
                                }
                                else
                                {
                                    CmbDesignation.ItemsSource = designationList; // designationList;
                                    CmbDesignation.SelectedItem = designationList[0];
                                }
                                objStaffDesgList = new List<MasterListItem>();
                                objStaffDesgList = designationList.ToList();
                            }
                            else
                                FetchDesignations();

                            //foreach (var item in dgDocAddedTimeSlots1.ItemsSource)
                            //{
                            //    if (!objDocClsList.Where(z => z.Code.Contains(((clsPatientProcDocScheduleDetailsVO)item).DoctorCode)).Any())
                            //    {                                   
                            //    }

                            //}
                            //dgDocAddedTimeSlots1.ItemsSource = null;
                            //dgDocAddedTimeSlots1.ItemsSource = DocList;
                            //dgDocAddedTimeSlots1.UpdateLayout();

                        }
                    };
                    Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                    //}
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        List<MasterListItem> objStaffDesgList = null;
        private void FetchDesignations()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        objStaffDesgList = new List<MasterListItem>();
                        objStaffDesgList.Add(new MasterListItem("0", "- Select -"));
                        objStaffDesgList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objStaffDesgList.ForEach(z => z.Code = z.ID.ToString());
                        CmbDesignation.ItemsSource = null;
                        CmbDesignation.ItemsSource = objStaffDesgList;
                        CmbDesignation.SelectedItem = objStaffDesgList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Fetches Checklist for schedule id
        /// </summary>
        /// <param name="scheduleID"></param>
        private void FetchChecklistForShcedule(long scheduleID, long scheduleUnitID1)
        {
            try
            {
                frmPatientWiseChecklist chkListWindow = new frmPatientWiseChecklist();
                chkListWindow.ViewChecklist = true;
                chkListWindow.ScheduleID = scheduleID;
                chkListWindow.ScheduleUnieID = scheduleUnitID1;
                chkListWindow.OnSaveButton_Click += new RoutedEventHandler(PatientWiseChecklist_OnSaveButton_Click);
                chkListWindow.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Validates Back Panel
        /// </summary>
        private bool ValidateOnSave()
        {
            bool result = true;
            try
            {
                if (!string.IsNullOrEmpty(txtMRNo.Text))
                {
                    if (procList1 != null && grdProcedure.ItemsSource != null)
                    {
                        if (procList1.Count > 0)
                        {
                            if (procDate1.SelectedDate == null)
                            {
                                //if (objLocalizationManager != null)
                                //{
                                //    msgText = objLocalizationManager.GetValue("OTDateEmpty_Msg");
                                //}
                                //else
                                //{
                                msgText = "OT Date can't be empty";
                                //}
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                return false;
                            }

                            if (tpFromtime1.Value == null || tpToTime1.Value == null)
                            {
                                //if (objLocalizationManager != null)
                                //{
                                //    msgText = objLocalizationManager.GetValue("SelectTimeSlot_Msg");
                                //}
                                //else
                                //{
                                msgText = "Please select time slot";
                                //}
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                return false;
                            }

                            if (txtProcDuration.Text == null || txtProcDuration.Text == "")
                            {
                                //if (objLocalizationManager != null)
                                //{
                                //    msgText = objLocalizationManager.GetValue("EstimatedDurationValidation_Msg");
                                //}
                                //else
                                //{
                                msgText = "Please enter estimated procedure duration";
                                //}
                                txtProcDuration.SetValidation(msgText);
                                txtProcDuration.RaiseValidationError();
                                txtProcDuration.Focus();
                                result = false;
                                return result;
                            }
                            else
                                txtProcDuration.ClearValidationError();

                            if (dgDocAddedTimeSlots1.ItemsSource != null)
                            {
                                if (DocList != null)
                                {
                                    if (DocList.Count == 0)
                                    {
                                        //if (objLocalizationManager != null)
                                        //{
                                        //    msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                        //    //msgText = objLocalizationManager.GetValue("EnterDoctorDetails_Msg");
                                        //}
                                        //else
                                        //{
                                        msgText = "Please enter doctor schedule";
                                        //msgText = "Please enter doctor details";
                                        //}
                                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                else
                                {
                                    //if (objLocalizationManager != null)
                                    //{
                                    //    msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                    //    //msgText = objLocalizationManager.GetValue("EnterDoctorDetails_Msg");
                                    //}
                                    //else
                                    //{
                                    msgText = "Please enter doctor schedule";
                                    //msgText = "Please enter doctor details";
                                    //}
                                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    return false;
                                }

                                if (blChkDocSchedule)
                                {
                                    DateTime valueStart = (DateTime)tpFromtime1.Value;
                                    DateTime valueEnd = (DateTime)tpToTime1.Value;
                                    if (!(StrTime.TimeOfDay == valueStart.TimeOfDay) || !(EndTime.TimeOfDay == valueEnd.TimeOfDay))
                                    {
                                        StrTime = valueStart;
                                        EndTime = valueEnd;
                                        dgDocAddedTimeSlots1.ItemsSource = null;
                                        DocList.Clear();
                                        StaffList1.Clear();
                                        //if (objLocalizationManager != null)
                                        //{
                                        //    msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                        //}
                                        //else
                                        //{
                                        msgText = "Please enter doctor schedule";
                                        //}
                                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        return false;
                                    }
                                    //else
                                    //{
                                    //foreach (var item in dgDocAddedTimeSlots1.ItemsSource)
                                    //{
                                    //    DateTime dtStart = Convert.ToDateTime(((clsPatientProcDocScheduleDetailsVO)item).StartTime);
                                    //    DateTime dt1FrmTime = Convert.ToDateTime(tpFromtime1.Value);

                                    //    DateTime dtEnd = Convert.ToDateTime(((clsPatientProcDocScheduleDetailsVO)item).EndTime);
                                    //    DateTime dt1ToTime = Convert.ToDateTime(tpToTime1.Value);
                                    //    bool value = false;

                                    //    if ((dtStart.TimeOfDay >= dt1FrmTime.TimeOfDay && dtStart.TimeOfDay <= dt1ToTime.TimeOfDay) && (dtEnd.TimeOfDay >= dt1FrmTime.TimeOfDay && dtEnd.TimeOfDay <= dt1ToTime.TimeOfDay))
                                    //    {
                                    //        value = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (objLocalizationManager != null)
                                    //        {
                                    //            msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                    //        }
                                    //        else
                                    //        {
                                    //            msgText = "Please enter doctor schedule";
                                    //        }
                                    //        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //        return false;
                                    //    }


                                    ////if (!(dtStart.TimeOfDay >= dt1FrmTime.TimeOfDay))
                                    //    //&& !(dtStart.TimeOfDay <= dtEnd.TimeOfDay)
                                    //if(!(dtStart.TimeOfDay>=dt1FrmTime.TimeOfDay && dtStart.TimeOfDay<=dt1ToTime.TimeOfDay))
                                    //{
                                    //    if (objLocalizationManager != null)
                                    //    {
                                    //        msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                    //    }
                                    //    else
                                    //    {
                                    //        msgText = "Please enter doctor schedule";
                                    //    }
                                    //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //    return false;

                                    //}

                                    //if (dtStart.TimeOfDay >= dt1FrmTime.TimeOfDay && dtEnd.TimeOfDay >= dt1ToTime.TimeOfDay)
                                    //{

                                    //}
                                    //}
                                    //}
                                    //blChkDocSchedule = false;
                                }

                            }
                            else
                            {
                                //if (objLocalizationManager != null)
                                //{
                                //    msgText = objLocalizationManager.GetValue("EnterDocTime_Msg");
                                //    //msgText = objLocalizationManager.GetValue("EnterDoctorDetails_Msg");
                                //}
                                //else
                                //{
                                msgText = "Please enter doctor schedule";
                                //msgText = "Please enter doctor details";
                                //}
                                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("SelectProcedure_Msg");
                            //}
                            //else
                            //{
                            msgText = "Please select procedure.";
                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("SelectProcedure_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please select procedure.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("PatientValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Please select patient.";
                    //}
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Saves OT Scehduling
        /// </summary>
        private void Save()
        {
            try
            {
                clsAddupdatePatientProcedureSchedulebizActionVO BizAction = new clsAddupdatePatientProcedureSchedulebizActionVO();
                BizAction.patientProcScheduleDetails = new clsPatientProcedureScheduleVO();
                BizAction.patientProcScheduleDetails.PatientID = SelectedPatient.PatientID;
                BizAction.patientProcScheduleDetails.PatientUnitID = PatientUnitID;
                BizAction.patientProcScheduleDetails.OTID = ((MasterListItem)CmbOT1.SelectedItem).ID;
                BizAction.patientProcScheduleDetails.OTTableID = ((MasterListItem)CmbOTTableBack1.SelectedItem).ID;
                BizAction.patientProcScheduleDetails.StartTime = (DateTime)tpFromtime1.Value;
                BizAction.patientProcScheduleDetails.EndTime = (DateTime)tpToTime1.Value;
                BizAction.patientProcScheduleDetails.Date = procDate1.SelectedDate;
                BizAction.patientProcScheduleDetails.VisitAdmID = VisitAdmID;
                BizAction.patientProcScheduleDetails.VisitAdmUnitID = VisitAdmUnitID;
                BizAction.patientProcScheduleDetails.Opd_Ipd = IsOPDIPD;
                BizAction.patientProcScheduleDetails.Status = true;
                BizAction.patientProcScheduleDetails.Remarks = txtRemark1.Text;
                BizAction.patientProcScheduleDetails.SpecialRequirement = txtSpecialRequirement.Text;
                BizAction.patientProcScheduleDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if ((bool)chkEmergency.IsChecked)
                    BizAction.patientProcScheduleDetails.IsEmergency = true;
                else
                    BizAction.patientProcScheduleDetails.IsEmergency = false;

                foreach (var item in dgDocAddedTimeSlots1.ItemsSource)
                {
                    TimeSpan ts = new TimeSpan(00, 0, 0);
                    if (tpFromtime1.Value.Value.TimeOfDay <= ts && tpToTime1.Value.Value.TimeOfDay >= ts)
                    {
                        ((clsPatientProcDocScheduleDetailsVO)item).DayID = (long)procDate1.SelectedDate.Value.DayOfWeek + 1;
                    }
                    else
                        ((clsPatientProcDocScheduleDetailsVO)item).DayID = (long)procDate1.SelectedDate.Value.DayOfWeek;

                    BizAction.patientProcScheduleDetails.DocScheduleList.Add((clsPatientProcDocScheduleDetailsVO)item);
                }
                if (dgProcedureStaff.ItemsSource != null)
                {
                    foreach (var item in dgProcedureStaff.ItemsSource)
                    {
                        BizAction.patientProcScheduleDetails.StaffList.Add((clsPatientProcStaffDetailsVO)item);
                    }
                }
                foreach (var item in PatientWiseProcCheckList)
                {
                    BizAction.patientProcScheduleDetails.PatientProcCheckList.Add((clsPatientProcedureChecklistDetailsVO)item);
                }

                foreach (var item in procList1)
                {
                    BizAction.patientProcScheduleDetails.PatientProcList.Add((clsPatientProcedureVO)item);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsAddupdatePatientProcedureSchedulebizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Save");
                            if (SelectedPatient.IsEMR)
                            {
                                cmdCancel.Visibility = Visibility.Visible;
                            }
                            objAnimation.Invoke(RotationType.Backward);
                            if (objLocalizationManager != null)
                            {
                                msgText = objLocalizationManager.GetValue("RecordSaved_Msg");
                            }
                            else
                            {
                                msgText = "Record saved successfully.";
                            }
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Modified Patient Procedure Schedule Master
        /// </summary>
        private void Modify()
        {
            try
            {
                clsAddupdatePatientProcedureSchedulebizActionVO BizAction = new clsAddupdatePatientProcedureSchedulebizActionVO();
                BizAction.patientProcScheduleDetails = (clsPatientProcedureScheduleVO)dgSchedule.DataContext;
                BizAction.patientProcScheduleDetails.Status = true;
                BizAction.patientProcScheduleDetails.OTID = ((MasterListItem)CmbOT1.SelectedItem).ID;
                BizAction.patientProcScheduleDetails.OTTableID = ((MasterListItem)CmbOTTableBack1.SelectedItem).ID;
                BizAction.patientProcScheduleDetails.VisitAdmID = VisitAdmID;
                BizAction.patientProcScheduleDetails.VisitAdmUnitID = VisitAdmUnitID;
                BizAction.patientProcScheduleDetails.Opd_Ipd = IsOPDIPD;
                if ((bool)chkEmergency.IsChecked)
                    BizAction.patientProcScheduleDetails.IsEmergency = true;
                else
                    BizAction.patientProcScheduleDetails.IsEmergency = false;
                foreach (var item in dgDocAddedTimeSlots1.ItemsSource)
                {
                    TimeSpan ts = new TimeSpan(00, 0, 0);
                    if (tpFromtime1.Value.Value.TimeOfDay <= ts && tpToTime1.Value.Value.TimeOfDay >= ts)
                    {
                        ((clsPatientProcDocScheduleDetailsVO)item).DayID = (long)procDate1.SelectedDate.Value.DayOfWeek + 1;
                    }
                    else
                        ((clsPatientProcDocScheduleDetailsVO)item).DayID = (long)procDate1.SelectedDate.Value.DayOfWeek;
                    BizAction.patientProcScheduleDetails.DocScheduleList.Add((clsPatientProcDocScheduleDetailsVO)item);
                }

                foreach (clsPatientProcStaffDetailsVO item in dgProcedureStaff.ItemsSource)
                {
                    BizAction.patientProcScheduleDetails.StaffList.Add(item);
                }

                foreach (var item in OTList1)
                {
                    item.ApColor = null;
                    BizAction.patientProcScheduleDetails.OTScedhuleList.Add((clsPatientProcedureScheduleVO)item);
                }

                foreach (var item in PatientWiseProcCheckList)
                {

                    BizAction.patientProcScheduleDetails.PatientProcCheckList.Add(item);
                }

                foreach (var item in procList1)
                {

                    BizAction.patientProcScheduleDetails.PatientProcList.Add(item);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null)
                    {
                        if (((clsAddupdatePatientProcedureSchedulebizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            FetchData();
                            ClearUI();
                            SetCommandButtonState("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            if (objLocalizationManager != null)
                            {
                                msgText = objLocalizationManager.GetValue("RecordModify_Msg");
                            }
                            else
                            {
                                msgText = "Record updated successfully.";
                            }
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches Details of procedure schedule
        /// </summary>
        private void FillDetailTablesOfProcedureSchedule(long ScheduleID, long ScheduleUnitID)
        {
            try
            {
                if (waitIndicator == null)
                    waitIndicator = new WaitIndicator();
                if (waitIndicator.Visibility == Visibility.Collapsed)
                    waitIndicator.Show();
                else
                {
                    waitIndicator.Close();
                    waitIndicator.Show();
                }

                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                BizAction.ScheduleID = ScheduleID;
                BizAction.ScheduleUnitID = ScheduleUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                            DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;
                            if (CmbOT1.ItemsSource != null)
                            {
                                CmbOT1.SelectedValue = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).OTID;
                            }
                            procDate1.SelectedDate = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).Date;
                            if (DetailsVO.PatientProcList != null)
                            {
                                procList1.Clear();
                                foreach (var item in DetailsVO.PatientProcList)
                                {
                                    procList1.Add(item);
                                }
                                grdProcedure.ItemsSource = null;
                                grdProcedure.ItemsSource = procList1;
                                grdProcedure.Focus();
                                grdProcedure.UpdateLayout();
                            }
                            FetchOTbyProcedure();

                            if (DetailsVO.CheckList != null)
                            {
                                PatientWiseProcCheckList.Clear();
                                foreach (var item in DetailsVO.CheckList)
                                {
                                    PatientWiseProcCheckList.Add(item);
                                }
                                grdchecklist.ItemsSource = null;
                                pcv = new PagedCollectionView(PatientWiseProcCheckList);
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory"));
                                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
                                grdchecklist.ItemsSource = pcv;
                                grdchecklist.UpdateLayout();
                                grdchecklist.Focus();
                            }
                            if (DetailsVO.DocScheduleDetails != null)
                            {
                                foreach (var item in DetailsVO.DocScheduleDetails)
                                {
                                    DocList.Add(item);
                                }
                                dgDocAddedTimeSlots1.ItemsSource = null;
                                dgDocAddedTimeSlots1.ItemsSource = DocList;
                                dgDocAddedTimeSlots1.Focus();
                                dgDocAddedTimeSlots1.UpdateLayout();
                            }

                            if (DetailsVO.StaffDetailList != null)
                            {
                                foreach (var item in DetailsVO.StaffDetailList)
                                {
                                    StaffList1.Add(item);
                                }
                                dgProcedureStaff.ItemsSource = null;
                                dgProcedureStaff.ItemsSource = StaffList1;
                                dgProcedureStaff.Focus();
                                dgProcedureStaff.UpdateLayout();
                            }
                            if (DetailsVO.OTScheduleList != null)
                            {
                                List<clsSchedule> objScheduleList = new List<clsSchedule>();
                                foreach (var item in DetailsVO.OTScheduleList)
                                {
                                    procDate1.SelectedDate = item.Date;
                                    txtRemark1.Text = item.Remarks;
                                    txtSpecialRequirement.Text = item.SpecialRequirement;
                                    OTList1.Add(item);
                                }
                                dgTimeSlots1.ItemsSource = null;
                                dgTimeSlots1.ItemsSource = OTList1;
                                dgTimeSlots1.Focus();
                                dgTimeSlots1.UpdateLayout();
                            }
                            if (DetailsVO.patientInfoObject != null)
                            {
                                PatientID = DetailsVO.patientInfoObject.pateintID;
                            }
                            chkEmergency.IsChecked = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).IsEmergency;
                            isViewChecklist = true;
                            scheduleID1 = ScheduleID;
                            scheduleUnitID1 = ScheduleUnitID;
                            tpFromtime1.Value = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime;
                            tpToTime1.Value = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).EndTime;
                            tpFromTime.Maximum = tpToTime1.Value;
                            tpFromTime.Minimum = tpFromtime1.Value;
                            tpToTime.Maximum = tpToTime1.Value;
                            tpToTime.Minimum = tpFromtime1.Value;
                            tpFromTime.Value = tpFromtime1.Value;
                            tpToTime.Value = tpToTime1.Value;

                            txtProcDuration.Text = (((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).EndTime - ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime).ToString();
                        }
                        else
                        {
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                            //}
                            //else
                            //{
                            msgText = "Error occured while processing.";
                            //}

                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                        waitIndicator.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CleartimeSlot()
        {
            dgTimeSlots1.ItemsSource = null;
            tpFromtime1.Value = null;
            tpToTime1.Value = null;
            txtProcDuration.Text = "";
        }
        #endregion

        #region Fill ComboBox

        #region Front Panel Combo
        /// <summary>
        /// Fetch Operation Theatre For Front Panel
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FillOperationTheatre()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];

                        List<MasterListItem> objList1 = new List<MasterListItem>();
                        objList1.Add(new MasterListItem(0, "-- Select --"));
                        CmbOTTable.ItemsSource = null;
                        CmbOTTable.SelectedValue = objList1[0];
                        CmbOTTable.ItemsSource = objList1;
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

        /// <summary>
        /// Fill the OT Table by Operation Theatre Type.
        /// </summary>
        /// <param name="OtTypeID"></param>
        private void FillOTTableByOTType(long OtTypeID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTableMaster;
                BizAction.Parent = new KeyValue { Value = "OTTheatreID", Key = OtTypeID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOTTable.ItemsSource = null;
                        CmbOTTable.ItemsSource = objList;
                        CmbOTTable.SelectedItem = objList[0];
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
        #endregion

        #region Back Panel Combo

        private void FillOTcmb()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        CmbOT1.ItemsSource = null;
                        CmbOT1.ItemsSource = objList;
                        if (this.schedulerOutput != null)
                        {
                            CmbOT1.SelectedItem = ((List<MasterListItem>)CmbOT1.ItemsSource).SingleOrDefault(S => S.ID.Equals(this.schedulerOutput.OTID));
                        }
                        else if (isView == true)
                        {
                            CmbOT1.SelectedValue = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).OTID;
                        }
                        else
                        {
                            CmbOT1.SelectedItem = objList[0];
                        }
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

        List<MasterListItem> MasterProcedureList = null;
        private void FillProcedure()
        {
            MasterProcedureList = new List<MasterListItem>();
            MasterProcedureList.Add(new MasterListItem(0, "-- Select --"));
            if (this.grdProcedure.ItemsSource != null)
            {
                foreach (clsPatientProcedureVO item in this.grdProcedure.ItemsSource)
                {
                    MasterProcedureList.Add(new MasterListItem(item.ProcedureID, item.ProcDesc));
                }
                CmbDocProcedure.ItemsSource = null;
                CmbDocProcedure.ItemsSource = MasterProcedureList;

                CmbStaffProcedure.ItemsSource = null;
                CmbStaffProcedure.ItemsSource = MasterProcedureList;
                if (dgDocAddedTimeSlots1.SelectedItem != null)
                {
                    CmbDocProcedure.SelectedValue = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).ProcedureID;
                    CmbStaffProcedure.SelectedValue = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).ProcedureID;
                }
                else if (MasterProcedureList.Count == 2)
                {
                    CmbDocProcedure.SelectedItem = MasterProcedureList[1];
                    CmbStaffProcedure.SelectedItem = MasterProcedureList[1];
                }
                else
                {
                    CmbDocProcedure.SelectedItem = MasterProcedureList[0];
                    CmbStaffProcedure.SelectedItem = MasterProcedureList[0];
                }
                if (blEditDoc)
                {
                    foreach (var item in MasterProcedureList)
                    {
                        if (item.ID == procedureID)
                            CmbDocProcedure.SelectedItem = item;
                    }
                    //blEditDoc = false;
                }
            }
        }

        #endregion

        #endregion

        #region Child Click From OT Details Form
        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.ItemsSource != null)
            {
                if (dgSchedule.SelectedItem != null)
                {
                    ((IApplicationConfiguration)App.Current).SelectedOTBooking = (clsPatientProcedureScheduleVO)dgSchedule.SelectedItem;
                }
            }
            ChildWindow objOTScheduling = ((ChildWindow)((frmOTScheduling)this).Parent);
            objOTScheduling.DialogResult = true;
        }

        private void cmdChildCancel_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow objOTScheduling = ((ChildWindow)((frmOTScheduling)this).Parent);
            ((IApplicationConfiguration)App.Current).SelectedOTBooking = null;
            objOTScheduling.DialogResult = false;
        }
        #endregion

        #region Selection Changes Event
        /// <summary>
        /// Fetches Operation Theatre Tables According to Operation Theatre.
        /// </summary>
        private void CmbOtTheatre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (CmbOtTheatre.SelectedItem != null && ((MasterListItem)CmbOtTheatre.SelectedItem).ID > 0)
                {
                    FillOTTableByOTType(((MasterListItem)CmbOtTheatre.SelectedItem).ID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetches the OT Table using OT ID.
        /// </summary>
        private void CmbOT1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbOT1.SelectedItem != null && ((MasterListItem)CmbOT1.SelectedItem).ID > 0)
                {
                    FetchOTTable(((MasterListItem)CmbOT1.SelectedItem).ID);
                }
                else
                {
                    CleartimeSlot();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CmbOTTableBack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbOT1.ItemsSource != null && CmbOT1.SelectedItem != null && !((MasterListItem)CmbOT1.SelectedItem).ID.Equals(0))
            {
                if (CmbOTTableBack1.ItemsSource != null && CmbOTTableBack1.SelectedItem != null && !((MasterListItem)CmbOTTableBack1.SelectedItem).ID.Equals(0))
                {
                    GetSchedule();
                }
                else
                {
                    CleartimeSlot();
                }
            }
            else
            {
                CleartimeSlot();
            }
        }

        /// <summary>
        /// Fetches doctor according to doctor classification
        /// </summary>
        /// 
        private void CmbDocType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CmbDocType.SelectedItem != null && ((MasterListItem)CmbDocType.SelectedItem).Code != "0")
                {
                    FetchDoctorByDocSpecialization(((MasterListItem)CmbDocType.SelectedItem).Code);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Procedure Selection Changes occures for the Staff Details tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbStaffProcedure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //txtStaffQuantity.Text = string.Empty;
            //if (CmbDesignation.SelectedItem != null && ((MasterListItem)CmbDesignation.SelectedItem).Code != "0" && CmbStaffProcedure.SelectedItem != null && ((MasterListItem)CmbStaffProcedure.SelectedItem).ID != 0)
            //{
            //    try
            //    {
            //        if (!flagChangeStaff)
            //        //if (!flagChangeDoc)
            //        {
            //            FetchStaffByDesignation(((MasterListItem)CmbDesignation.SelectedItem).Code);
            //        }
            //        //else
            //        //    flagChangeDoc = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}

        }

        /// <summary>
        /// Doctor on Doctor Details Tab Selection Changes Event get Handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillProcedure();
            //if (CmbDoctor.SelectedItem != null && ((MasterListItem)CmbDoctor.SelectedItem).ID != 0)
            //{
            //    GetDoctorSchedule();
            //}
        }

        //private void GetDoctorSchedule()
        // {
        //     try
        //     {
        //         clsGetRSIJDoctorScheduleTimeVO BizAction = new clsGetRSIJDoctorScheduleTimeVO();
        //         BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
        //         BizAction.DoctorCode = ((MasterListItem)CmbDoctor.SelectedItem).Code;
        //         BizAction.Date = procDate1.SelectedDate;
        //         BizAction.SpecializationCode = ((MasterListItem)CmbDocType.SelectedItem).Code;

        //         Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //         PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //         dgTimeSlots.ItemsSource = null;
        //         client.ProcessCompleted += (s, arg) =>
        //         {
        //             if (arg.Error == null && arg.Result != null)
        //             {
        //                 clsGetRSIJDoctorScheduleTimeVO objResult = (clsGetRSIJDoctorScheduleTimeVO)arg.Result;
        //                 if (objResult != null && objResult.DoctorScheduleDetailsList != null && objResult.DoctorScheduleDetailsList.Count != 0)
        //                 {
        //                     clsGetRSIJDoctorScheduleTimeVO DetailsVO = new clsGetRSIJDoctorScheduleTimeVO();
        //                     DetailsVO = objResult;
        //                     List<clsSchedule> listSchedule = new List<clsSchedule>();

        //                     if (DetailsVO.DoctorScheduleDetailsList != null)
        //                     {
        //                         int DCount = 0;
        //                         DateTime AvailableFirstScheduleDate = DateTime.Now;

        //                         int IsNextDaySchedule = 0;
        //                         for (int DayCount = 0; DayCount <= 7; DayCount++)
        //                         {
        //                             string objNextDay = AvailableFirstScheduleDate.DayOfWeek.ToString();
        //                             DateTime Date = AvailableFirstScheduleDate; //procDate1.SelectedDate.Value.AddDays(DayCount);
        //                             List<clsDoctorScheduleDetailsVO> DoctorScheduleCount = (from ScheduleObject in DetailsVO.DoctorScheduleDetailsList
        //                                                                                     where ScheduleObject.Day.Equals(objNextDay) && ScheduleObject.DoctorID.Equals(BizAction.DoctorId)
        //                                                                                     select ScheduleObject).ToList<clsDoctorScheduleDetailsVO>();
        //                             if (DoctorScheduleCount.Count != 0)
        //                             {
        //                                 int ScheduleCount = 1;
        //                                 foreach (clsDoctorScheduleDetailsVO item in DoctorScheduleCount)
        //                                 {
        //                                     if (item.StartTime != null && item.EndTime != null)
        //                                     {
        //                                         double ScheduleHrs = ((DateTime)(item.EndTime)).Subtract((DateTime)(item.StartTime)).TotalMinutes / OTTimeSlots;
        //                                         DateTime? Dt = null;
        //                                         if (item.StartTime != null)
        //                                             Dt = item.StartTime;
        //                                         for (int Schedulecount = 0; Schedulecount < ScheduleHrs; Schedulecount++)
        //                                         {
        //                                             clsSchedule _listItem = new clsSchedule();
        //                                             _listItem.Day = "Schedule" + ScheduleCount;

        //                                             if (DayCount.Equals(0))
        //                                             {
        //                                                 _listItem.IsCurrentSchedule = true;
        //                                                 _listItem.CurrentScheduleDate = Date;
        //                                             }
        //                                             else
        //                                             {
        //                                                 _listItem.IsCurrentSchedule = false;
        //                                             }
        //                                             _listItem.ScheduleFromTime = Dt.Value;
        //                                             _listItem.ScheduleToTime = (Dt.Value.AddMinutes(OTTimeSlots));
        //                                             if (_listItem.CurrentapColor != null)
        //                                                 _listItem.CurrentapColor = new SolidColorBrush(Colors.Green);
        //                                             listSchedule.Add(_listItem);
        //                                             Dt = Dt.Value.AddMinutes(OTTimeSlots);
        //                                         }
        //                                     }
        //                                     ScheduleCount++;
        //                                 }
        //                                 IsNextDaySchedule++;
        //                             }
        //                             else
        //                             {
        //                                 if (DayCount.Equals(0))
        //                                 {
        //                                     IsNextDaySchedule++;
        //                                 }
        //                             }
        //                             if (IsNextDaySchedule.Equals(1))
        //                                 break;
        //                         }
        //                         BindDoctorScheduleData(listSchedule, true);
        //                     }

        //                 }
        //             }
        //             else
        //             {
        //                 if (objLocalizationManager != null)
        //                 {
        //                     msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
        //                 }
        //                 else
        //                 {
        //                     msgText = "Error occured while processing.";
        //                 }
        //                 ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //             }
        //         };

        //         client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //         client.CloseAsync();
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }

        private void ScrollViewer_KeyUp(object sender, KeyEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            FrameworkElement focusedElement = FocusManager.GetFocusedElement() as FrameworkElement;
            GeneralTransform focusedVisualTransform = focusedElement.TransformToVisual(scrollViewer);
            Rect rectangle = focusedVisualTransform.TransformBounds(new Rect(new Point(focusedElement.Margin.Left, focusedElement.Margin.Top), focusedElement.RenderSize));
            double newOffset = scrollViewer.VerticalOffset + (rectangle.Bottom - scrollViewer.ViewportHeight);
            scrollViewer.ScrollToVerticalOffset(newOffset);
        }
        #endregion

        #region CheckBox

        private void chkCurrentSchedule_Checked(object sender, RoutedEventArgs e)
        {
            //dgDocAddedTimeSlots1.ItemsSource = null;
            // DocList.Clear();           

            SelectedCurrentDateAppointment = new List<clsSchedule>();
            if (!IsCallFromCurrentUncheckEvent)
            {
                clsSchedule SelectedScheduleObject = ((clsSchedule)dgTimeSlots1.SelectedItem);
                bool IsCurrectAvailableScheduleSlot = false;
                if (SelectedCurrentDateSchedule != null && SelectedCurrentDateSchedule.Count != 0)
                {
                    if (SelectedScheduleObject != null)
                    {
                        foreach (clsSchedule Obj in SelectedCurrentDateSchedule)
                        {
                            if ((Obj.RowID - 1).Equals(SelectedScheduleObject.RowID) || (Obj.RowID + 1).Equals(SelectedScheduleObject.RowID))
                            {
                                IsCurrectAvailableScheduleSlot = true;
                            }
                        }
                        if (IsCurrectAvailableScheduleSlot)
                        {
                            //tpToTime.Value = null;
                            SelectedCurrentDateSchedule.Add(SelectedScheduleObject);
                            procDate1.SelectedDate = SelectedScheduleObject.CurrentScheduleDate;



                            //tpToTime.DataContext = new List<clsSchedule>();
                            //tpToTime.DataContext = SelectedCurrentDateSchedule;
                            //var rohit = ((DateTime)(SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime))).DeepCopy();

                            tpFromtime1.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime1.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                            tpFromTime.Minimum = tpFromtime1.Value;
                            tpFromTime.Maximum = tpToTime1.Value;
                            tpToTime.Minimum = tpFromtime1.Value;
                            tpToTime.Maximum = tpToTime1.Value;

                            tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);
                        }
                        else
                        {
                            ((CheckBox)sender).IsChecked = false;
                            //if (objLocalizationManager != null)
                            //{
                            //    msgText = objLocalizationManager.GetValue("ProperSlot_Msg");
                            //}
                            //else
                            //{
                            msgText = "Please select proper slot.";
                            //}
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        tpFromtime1.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                        tpToTime1.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                        tpFromTime.Minimum = tpFromtime1.Value;
                        tpFromTime.Maximum = tpToTime1.Value;
                        tpToTime.Minimum = tpFromtime1.Value;
                        tpToTime.Maximum = tpToTime1.Value;

                        tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                        tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                    }
                }
                else
                {
                    if (SelectedScheduleObject != null)
                    {
                        SelectedCurrentDateSchedule.Add(SelectedScheduleObject);
                        procDate1.SelectedDate = SelectedScheduleObject.CurrentScheduleDate;

                        tpFromtime1.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                        tpToTime1.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                        tpFromTime.Minimum = tpFromtime1.Value;
                        tpFromTime.Maximum = tpToTime1.Value;
                        tpToTime.Minimum = tpFromtime1.Value;
                        tpToTime.Maximum = tpToTime1.Value;

                        tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                        tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);
                    }
                }

                IsCallFromCurrentUncheckEvent = false;
            }
            else
            {
                IsCallFromCurrentUncheckEvent = false;
            }
            if (SelectedCurrentDateSchedule.Count == 0)
            {
                tpFromtime1.Value = null;
                tpToTime1.Value = null;
            }
            txtProcDuration.Text = (tpToTime1.Value - tpFromtime1.Value).ToString();
        }

        private void chkCurrentSchedule_Unchecked(object sender, RoutedEventArgs e)
        {
            //dgDocAddedTimeSlots1.ItemsSource = null;
            // DocList.Clear();
                  
            //List<clsSchedule> CurrentScheduleList = (from Obj in ChecklistSchedule
            //                                         where Obj.CurrentIsChecked.Equals(true)
            //                                         select Obj).ToList<clsSchedule>();
            //if (CurrentScheduleList.Count() > 0 )
            //{
            //    foreach (var item in CurrentScheduleList)
            //    {
            //        item.CurrentIsChecked = false;
            //    }
            //}


            clsSchedule SelectedScheduleObject = (clsSchedule)dgTimeSlots1.SelectedItem;
            SelectedCurrentDateAppointment = new List<clsSchedule>();
            if (SelectedCurrentDateSchedule != null)
            {
                if (SelectedScheduleObject != null)
                {
                    clsSchedule ImmediateUpSchedule = SelectedCurrentDateSchedule.SingleOrDefault(S => S.RowID.Equals(SelectedScheduleObject.RowID + 1));
                    clsSchedule ImmediateDownSchedule = SelectedCurrentDateSchedule.SingleOrDefault(S => S.RowID.Equals(SelectedScheduleObject.RowID - 1));
                    if (ImmediateUpSchedule != null && ImmediateDownSchedule != null)
                    {
                        IsCallFromCurrentUncheckEvent = true;
                        ((CheckBox)sender).IsChecked = true;
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("UnselectProperSlot_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please unselect proper slot.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    else
                    {
                        SelectedCurrentDateSchedule.Remove(SelectedScheduleObject);
                        if (SelectedCurrentDateSchedule.Count > 0)
                        {

                            tpFromtime1.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime1.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                            tpFromTime.Minimum = tpFromtime1.Value;
                            tpFromTime.Maximum = tpToTime1.Value;
                            tpToTime.Minimum = tpFromtime1.Value;
                            tpToTime.Maximum = tpToTime1.Value;

                            tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                        }
                    }
                }
                else
                {
                    tpFromtime1.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                    tpToTime1.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                    tpFromTime.Minimum = tpFromtime1.Value;
                    tpFromTime.Maximum = tpToTime1.Value;
                    tpToTime.Minimum = tpFromtime1.Value;
                    tpToTime.Maximum = tpToTime1.Value;

                    tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                    tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);

                }
            }
            if (SelectedCurrentDateSchedule.Count == 0)
            {
                tpFromtime1.Value = null;
                tpToTime1.Value = null;
                tpFromTime.Value = null;
                tpToTime.Value = null;
            }
            txtProcDuration.Text = (tpToTime1.Value - tpFromtime1.Value).ToString();
        }

        private void chkCurrentSchedule_Click(object sender, RoutedEventArgs e)
        {
            bool chkbox = (bool)((CheckBox)sender).IsChecked;
            CheckUncheckCurrentSchedule(sender, chkbox);

        }

        private void CheckUncheckCurrentSchedule(object sender, bool chkbox)
        {
            clsSchedule SelectedScheduleObject = new clsSchedule();//(clsSchedule)dgTimeSlots.SelectedItem;

            if (chkbox.Equals(true))
            {
                if (!IsCallFromCurrentUncheckEvent)
                {
                    bool IsCurrectAvailableAppintmentSlot = false;
                    if (SelectedCurrentDateSchedule != null && SelectedCurrentDateSchedule.Count != 0)
                    {
                        foreach (clsSchedule Obj in SelectedCurrentDateSchedule)
                        {
                            if (Obj.ScheduleFromTime >= SelectedScheduleObject.ScheduleFromTime
                                && Obj.ScheduleToTime >= SelectedScheduleObject.ScheduleToTime)
                            {
                                IsCurrectAvailableAppintmentSlot = true;
                            }
                        }
                        if (IsCurrectAvailableAppintmentSlot)
                        {
                            SelectedCurrentDateAppointment.Add(SelectedScheduleObject);
                            tpFromTime.Value = SelectedCurrentDateAppointment.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateAppointment.Max(S => S.ScheduleToTime);
                        }
                        else
                        {
                            ((CheckBox)sender).IsChecked = false;
                            msgText = "Please select time slot in range of procedure time.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        SelectedCurrentDateAppointment.Add(SelectedScheduleObject);
                        if (SelectedCurrentDateAppointment.Min(S => S.ScheduleFromTime).Equals(SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime)) && SelectedCurrentDateAppointment.Max(S => S.ScheduleToTime).Equals(SelectedCurrentDateSchedule.Min(S => S.ScheduleToTime)))
                        {
                            tpFromTime.Value = SelectedCurrentDateAppointment.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateAppointment.Max(S => S.ScheduleToTime);
                        }
                        else
                        {
                            msgText = "Please select time slot in range of procedure time.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    IsCallFromCurrentUncheckEvent = false;
                }
                else
                {
                    IsCallFromCurrentUncheckEvent = false;
                }
            }
            else
            {
                if (SelectedCurrentDateSchedule != null && SelectedCurrentDateSchedule.Count > 0)
                {
                    clsSchedule ImmediateUpSchedule = SelectedCurrentDateSchedule.SingleOrDefault(S => S.RowID.Equals(SelectedScheduleObject.RowID + 1));
                    clsSchedule ImmediateDownSchedule = SelectedCurrentDateSchedule.SingleOrDefault(S => S.RowID.Equals(SelectedScheduleObject.RowID - 1));
                    if (ImmediateUpSchedule != null && ImmediateDownSchedule != null)
                    {
                        IsCallFromCurrentUncheckEvent = true;
                        ((CheckBox)sender).IsChecked = true;
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("UnselectProperSlot_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please unselect proper slot";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    else
                    {
                        SelectedCurrentDateAppointment.Remove(SelectedScheduleObject);
                        SelectedCurrentDateSchedule.Remove(SelectedScheduleObject);
                        if (SelectedCurrentDateSchedule.Count > 0)
                        {
                            tpFromTime.Value = SelectedCurrentDateSchedule.Min(S => S.ScheduleFromTime);
                            tpToTime.Value = SelectedCurrentDateSchedule.Max(S => S.ScheduleToTime);
                        }
                        else
                        {
                            tpFromTime.Value = null;
                            tpToTime.Value = null;
                        }
                    }
                }
            }
        }
        #endregion

        #region ImageClick

        /// <summary>
        /// Patient Image click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMRNo.Text) && txtMRNo.Text.Trim() != String.Empty)
            {
                GetPatientData(txtMRNo.Text.Trim());
            }
            else
            {
                //if (objLocalizationManager != null)
                //{
                //    msgText = objLocalizationManager.GetValue("MRNoValidation_Msg");
                //}
                //else
                //{
                msgText = "Please enter MR number.";
                //}
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Edits doctor
        /// </summary>
        /// 
        bool blEditDoc = false;
        long docId, procedureID;
        string DCode;
        private void hlbEditDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flagChangeDoc = true;
                blEditDoc = true;
                docId = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).DocID;
                //DCode = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).DoctorCode;
                procedureID = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).ProcedureID;
                foreach (var item in objDocClsList)
                {
                    if (item.Code == ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).SpecializationCode)
                        CmbDocType.SelectedItem = item;
                }

                //CmbDocType.SelectedValue = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).DocTypeID;
                //FetchDoctorByDocSpecialization(((MasterListItem)CmbDocType.SelectedItem).Code);

                //CmbDoctor.SelectedValue = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).DocID;


                tpFromTime.Value = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).StartTime;
                tpToTime.Value = ((clsPatientProcDocScheduleDetailsVO)dgDocAddedTimeSlots1.SelectedItem).EndTime;
                cmdModifyDoc.IsEnabled = true;
                cmdAddDoc.IsEnabled = false;


                int index = dgDocAddedTimeSlots1.SelectedIndex;
                DocList.RemoveAt(index);
                dgDocAddedTimeSlots1.ItemsSource = null;
                dgDocAddedTimeSlots1.ItemsSource = DocList;
                dgDocAddedTimeSlots1.UpdateLayout();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Date Selection Changed

        private void procDate1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker Date = (DatePicker)sender;
            if (Date.SelectedDate != null)
            {
                tpFromtime1.Value = null;
                tpToTime1.Value = null;
                if (this.schedulerOutput != null)
                {

                    if (this.schedulerOutput.OTDate != null)
                    {
                        if (this.schedulerOutput.OTDate.Date.Equals(Date.SelectedDate.Value.Date))
                        {
                            Date.SelectedDate = this.schedulerOutput.OTDate;
                        }
                    }
                    else if (this.schedulerOutput.CurrentScheduleDate != null)
                    {
                        if (this.schedulerOutput.CurrentScheduleDate.Date.Equals(Date.SelectedDate.Value.Date))
                        {
                            Date.SelectedDate = this.schedulerOutput.CurrentScheduleDate;
                        }
                    }
                }
            }
            else
            {
                if (this.schedulerOutput != null)
                {
                    if (this.schedulerOutput.OTDate != null)
                    {
                        Date.SelectedDate = this.schedulerOutput.OTDate;
                    }
                    else
                    {
                        Date.SelectedDate = this.schedulerOutput.CurrentScheduleDate;
                    }
                }
            }

            if (CmbOT1.ItemsSource != null && CmbOT1.SelectedItem != null && !((MasterListItem)CmbOT1.SelectedItem).ID.Equals(0))
            {
                if (CmbOTTableBack1.ItemsSource != null && CmbOTTableBack1.SelectedItem != null && !((MasterListItem)CmbOTTableBack1.SelectedItem).ID.Equals(0))
                {
                    GetSchedule();
                }
                else
                {
                    CleartimeSlot();
                }
            }
            else
            {
                CleartimeSlot();
            }
        }
        #endregion

        #region HyperlinkClickEvent
        /// <summary>
        /// Deletes doctor
        /// </summary>
        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgDocAddedTimeSlots1.SelectedItem != null)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("DeleteValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are you sure you want to delete the record ?";
                    //}
                    int index = dgDocAddedTimeSlots1.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            DocList.RemoveAt(index);
                        }
                    };

                    msgWD.Show();
                    dgDocAddedTimeSlots1.ItemsSource = null;
                    dgDocAddedTimeSlots1.ItemsSource = DocList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ask permission to delete the procedure record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeleteProcedure_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            //if (objLocalizationManager != null)
            //{
            //    msgText = objLocalizationManager.GetValue("DeleteValidation_Msg");
            //}
            //else
            //{
            msgText = "Are you sure you want to delete the record ?";
            //}
            MessageBoxControl.MessageBoxChildWindow msgW2 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW2_OnMessageBoxClosed3);
            msgW2.Show();
        }
        /// <summary>
        /// On User consent delete the Procedure entry.
        /// </summary>
        /// <param name="result"></param>

        void msgW2_OnMessageBoxClosed3(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                long ProcId = ((clsPatientProcedureVO)(grdProcedure.SelectedItem)).ProcedureID;
                procList1.RemoveAt(grdProcedure.SelectedIndex);
                grdProcedure.ItemsSource = null;
                grdProcedure.ItemsSource = procList1;

                FetchOTbyProcedure();
                FillCheckListOnProcedureClick();

                //grdchecklist.ItemsSource = null;
                if (procList1 == null || procList1.Count == 0)
                {
                    DoctorSchedule.IsEnabled = false;
                    //StaffSchedule.IsEnabled = false;
                }



                for (int i = DocList.Count - 1; i > -1; i--)
                {
                    if (DocList[i].ProcedureID == ProcId)
                        DocList.RemoveAt(i);
                }
                dgDocAddedTimeSlots1.ItemsSource = null;
                dgDocAddedTimeSlots1.ItemsSource = DocList;
                dgDocAddedTimeSlots1.UpdateLayout();

                for (int i = StaffList1.Count - 1; i > -1; i--)
                {
                    if (StaffList1[i].ProcedureID == ProcId)
                        StaffList1.RemoveAt(i);
                }
                dgProcedureStaff.ItemsSource = null;
                dgProcedureStaff.ItemsSource = StaffList1;
                dgProcedureStaff.UpdateLayout();
            }
        }

        /// <summary>
        /// deletes checklist item
        /// </summary>
        private void cmdDeleteCheckListItem_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            //if (objLocalizationManager != null)
            //{
            //    msgText = objLocalizationManager.GetValue("DeleteValidation_Msg");
            //}
            //else
            //{
            msgText = "Are you sure you want to delete the record ?";
            //}
            MessageBoxControl.MessageBoxChildWindow msgWDel =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWDel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWDel_OnMessageBoxClosed3);
            msgWDel.Show();
        }

        void msgWDel_OnMessageBoxClosed3(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                PatientWiseProcCheckList.RemoveAt(grdchecklist.SelectedIndex);
                grdchecklist.ItemsSource = null;
                pcv = new PagedCollectionView(PatientWiseProcCheckList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SubCategory1"));
                grdchecklist.ItemsSource = pcv;
                grdchecklist.UpdateLayout();
            }
        }

        #endregion
        List<MasterListItem> objDocClsList = null;

        private void FetchDoctorClassification()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem("0", "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        objList.ForEach(z => z.Code = z.ID.ToString());
                        CmbDocType.ItemsSource = null;
                        CmbDocType.ItemsSource = objList;
                        CmbDocType.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //private void FetchDoctorClassification()
        //{
        //    clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
        //    BizAction.DescriptionColumn = "NMSPESIAL";
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.MasterTable = MasterTableNameList.SPESIAL;
        //    BizAction.CodeColumn = "KDSPESIAL";

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            objDocClsList = new List<MasterListItem>();
        //            objDocClsList.Add(new MasterListItem("0", "-- Select --"));
        //            objDocClsList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
        //            CmbDocType.ItemsSource = null;
        //            CmbDocType.ItemsSource = objDocClsList;
        //            CmbDocType.SelectedItem = objDocClsList[0];
        //            //if (this.DataContext != null)
        //            //{
        //            //    CmbDoctor.SelectedValue = ((clsProcedureMasterVO)this.DataContext).ID;
        //            //}
        //        }
        //    };
        //    client.ProcessAsync(BizAction, new clsUserVO());
        //}







        /// <summary>
        /// Fills Slected OT booking
        /// </summary>
        private void dgSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgSchedule.SelectedItem != null)
                {
                    cmdConsent.IsEnabled = true;
                    cmdViewConsent.IsEnabled = true;
                    ((IApplicationConfiguration)App.Current).SelectedOTBooking = (clsPatientProcedureScheduleVO)dgSchedule.SelectedItem;

                    if (((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).OTDetailsID == 0)
                    {
                        DateTime dt = (DateTime)((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).Date;
                        if (dt.Date.Equals(DateTime.Now.Date))
                        {
                            if ((((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime.TimeOfDay) > DateTime.Now.TimeOfDay)
                                cmdCancelOTBooking.IsEnabled = true;
                            else
                                cmdCancelOTBooking.IsEnabled = false;
                        }
                        else if (dt.Date < DateTime.Now.Date)
                            cmdCancelOTBooking.IsEnabled = false;
                        else
                            cmdCancelOTBooking.IsEnabled = true;

                        if ((bool)chkIsCancelled.IsChecked)
                        {
                            cmdCancelOTBooking.IsEnabled = false;
                            foreach (DataGridColumn item in dgSchedule.Columns)
                            {
                                if (item.Header.ToString() == "View")
                                    item.Visibility = Visibility.Collapsed;

                            }
                        }
                        //if (dt.Date.Equals(DateTime.Now.Date) && ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime.TimeOfDay > DateTime.Now.TimeOfDay)
                        //    //if (((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).StartTime > DateTime.Now)
                        //    cmdCancelOTBooking.IsEnabled = true;
                        //else
                        //    cmdCancelOTBooking.IsEnabled = false;
                        //cmdCancelOTBooking.IsEnabled = true;
                    }
                    else
                    {
                        cmdCancelOTBooking.IsEnabled = false;
                    }
                }
                else
                {
                    cmdCancelOTBooking.IsEnabled = false;
                    cmdConsent.IsEnabled = false;
                    cmdViewConsent.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();

            if (IsOpdIpd == (int)PatientTypes.IPD)
            {
                Win.Initiate(PatientTypes.IPD.ToString());
            }
            else if (IsOpdIpd == (int)PatientTypes.OPD)
            {
                Win.Initiate(PatientTypes.OPD.ToString());
            }
            else if (IsOpdIpd == (int)PatientTypes.All)
            {
                Win.Initiate(PatientTypes.All.ToString());
            }

            //Win.Height = ActualHeight - 150;
            //Win.Width = ActualWidth - 250;
            Win.Width = this.ActualWidth * 0.60;
            Win.Height = this.ActualHeight * 0.70;
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnPatientSearch_Click);
            Win.Show();
        }

        void Win_OnPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");           
            //mElement.Text = " : " + SelectedPatient.PatientName;
            //mElement.Text += " - " + SelectedPatient.MRNo + " : " + SelectedPatient.Gender;

            txtMRNo.Text = SelectedPatient.MRNo;
            PatientID = SelectedPatient.PatientID;
            txtFirstName.Text = SelectedPatient.PatientName;
            txtGender.Text = SelectedPatient.Gender;
            txtMaritalStatus.Text = SelectedPatient.MaritalStatus;
            txtEducation.Text = SelectedPatient.Education;
            txtReligion.Text = SelectedPatient.Religion;
            txtContactNO1.Text = SelectedPatient.ContactNO1;

            //if (SelectedPatient.Photo != null)
            //if (SelectedPatient.ImageName.Length > 0)
            //{
            //    //commented by neena
            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
            //    //bmp.FromByteArray(SelectedPatient.Photo);
            //    //imgPhoto.Source = bmp;
            //    //

            //    //added by neena
            //    imgPhoto.Source = new BitmapImage(new Uri(SelectedPatient.ImageName, UriKind.Absolute));
            //    //

            //}
            if (SelectedPatient.ImageName != null && SelectedPatient.ImageName.Length > 0)
            {
                imgPhoto.Source = new BitmapImage(new Uri(SelectedPatient.ImageName, UriKind.Absolute));
            }
            else if (SelectedPatient.Photo != null)
            {
                byte[] imageBytes = SelectedPatient.Photo;
                BitmapImage img = new BitmapImage();
                img.SetSource(new MemoryStream(imageBytes, false));
                imgPhoto.Source = img;
            }
            else
                imgPhoto.Source = null;

            btnAttachPhoto.IsEnabled = true;

            if (SelectedPatient.DateOfBirth != null)
            {
                txtDOB.Text = Convert.ToDateTime(SelectedPatient.DateOfBirth).ToString("dd/MM/yyyy");
                txtAge.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY") + " years";
                //txtMonths.Text = ConvertDate(PatientInfo.DOB, "MM");
                //txtDays.Text = ConvertDate(PatientInfo.DOB, "DD");
            }

        }

        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);
                    DateTime age = DateTime.MinValue + difference;
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }


        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProcedureStaff.SelectedItem != null)
                {
                    //if (objLocalizationManager != null)
                    //{
                    //    msgText = objLocalizationManager.GetValue("DeleteValidation_Msg");
                    //}
                    //else
                    //{
                    msgText = "Are you sure you want to delete the record ?";
                    //}
                    int index = dgProcedureStaff.SelectedIndex;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            StaffList1.RemoveAt(index);
                        }
                    };

                    msgWD.Show();
                    dgProcedureStaff.ItemsSource = null;
                    dgProcedureStaff.ItemsSource = StaffList1;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void txtMRNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CmdPatientSearch_Click(sender, e);
            }
        }

        private void SearchDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem != null)
            {
                clsUpdateOTScheduleStatusBizActionVO BizAction = new clsUpdateOTScheduleStatusBizActionVO();
                BizAction.UpdateStatusField = new clsPatientProcedureScheduleVO();

                if (dgSchedule.SelectedItem != null)
                {
                    BizAction.UpdateStatusField.ID = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).ID;
                }
                BizAction.UpdateStatusField.Status = (bool)((CheckBox)sender).IsChecked;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("lblStatusUpdatedSucessfully");
                        //}
                        //else
                        //{
                        msgText = "Status Updated Successfully";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.SelectedItem != null)
            {
                clsUpdateOTScheduleStatusBizActionVO BizAction = new clsUpdateOTScheduleStatusBizActionVO();
                BizAction.IsCalledForPAC = true;
                BizAction.UpdateStatusField = new clsPatientProcedureScheduleVO();

                if (dgSchedule.SelectedItem != null)
                {
                    BizAction.UpdateStatusField.ID = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem).ID;
                }
                BizAction.UpdateStatusField.Status = (bool)((CheckBox)sender).IsChecked;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("AnesthesiaCheckupStatus_Msg");
                        //}
                        //else
                        //{
                        msgText = "Anesthesia checkup status updated successfully.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("ErrorWhileSaveModify_Msg");
                        //}
                        //else
                        //{
                        msgText = "Error occured while processing";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearch_Click(sender, e);
            }
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtSearchCriteria_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSearchCriteria.Text = txtSearchCriteria.Text.ToTitleCase();
        }

        private void CmbDesignation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CmbStaffProcedure_SelectionChanged(sender, e);
            if (CmbDesignation.SelectedItem != null)
            //if (CmbDesignation.SelectedItem != null && ((MasterListItem)CmbDesignation.SelectedItem).Code != "0" && CmbStaffProcedure.SelectedItem != null && ((MasterListItem)CmbStaffProcedure.SelectedItem).ID != 0)
            {
                try
                {
                    //if (!flagChangeDoc)
                    //{
                    FetchStaffByDesignation(((MasterListItem)CmbDesignation.SelectedItem).Code);
                    FillProcedure();
                    //}
                    //else
                    //    flagChangeDoc = false;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

        int IsOpdIpd;
        private void rdAll_Checked(object sender, RoutedEventArgs e)
        {
            IsOpdIpd = (int)PatientTypes.All;
        }


        private void IPD_Checked(object sender, RoutedEventArgs e)
        {
            IsOpdIpd = (int)PatientTypes.IPD;
        }

        private void OPD_Checked(object sender, RoutedEventArgs e)
        {
            IsOpdIpd = (int)PatientTypes.OPD;
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            frmUpdatePhoto UpdatePhoto = new frmUpdatePhoto();
            if (this.DataContext != null)
                UpdatePhoto.MyPhoto = null;
            UpdatePhoto.OnSaveButton_Click += new RoutedEventHandler(UpdatePhoto_OnSaveButton_Click);
            UpdatePhoto.Show();
        }

        void UpdatePhoto_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmUpdatePhoto)sender).DialogResult == true)
            {
                imgPhoto.Source = ((frmUpdatePhoto)sender).imgPhoto.Source;
                clsSavePhotoBizActionVO BizAction = new clsSavePhotoBizActionVO();
                BizAction.PatientID = PatientID;
                //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.Photo = ((frmUpdatePhoto)sender).MyPhoto;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    ShowMessageBox("Image Save Successfully !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
        }

        private void cmdModifyStaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateStaff())
                {
                    clsPatientProcStaffDetailsVO tempStaff = new clsPatientProcStaffDetailsVO();

                    var staff = from r in StaffList1
                                where (r.DesignationID == Convert.ToInt32(((MasterListItem)CmbDesignation.SelectedItem).Code) && r.StaffID == ((MasterListItem)CmbStaff.SelectedItem).ID && r.ProcedureID == ((MasterListItem)CmbStaffProcedure.SelectedItem).ID)
                                select new clsPatientProcStaffDetailsVO
                                {
                                    Status = r.Status,
                                    DesignationID = r.DesignationID,
                                    DesignationCode = r.DesignationCode,
                                    designationDesc = ((MasterListItem)CmbDesignation.SelectedItem).Description,
                                    StaffID = r.StaffID,
                                    Quantity = r.Quantity,
                                    stffDesc = ((MasterListItem)CmbStaff.SelectedItem).Description
                                };

                    var Designation = (from r in StaffList1
                                       where (r.DesignationCode == ((MasterListItem)CmbDesignation.SelectedItem).Code)
                                       select r);
                    if (staff.ToList().Count == 0)
                    {
                        //if (Designation.ToList().Count < Convert.ToInt64(txtStaffQuantity.Text))
                        //{
                        tempStaff.DesignationID = Convert.ToInt64(((MasterListItem)CmbDesignation.SelectedItem).Code);
                        tempStaff.DesignationCode = ((MasterListItem)CmbDesignation.SelectedItem).Code;
                        tempStaff.designationDesc = ((MasterListItem)CmbDesignation.SelectedItem).Description;
                        tempStaff.StaffID = ((MasterListItem)CmbStaff.SelectedItem).ID;
                        tempStaff.stffDesc = ((MasterListItem)CmbStaff.SelectedItem).Description;
                        tempStaff.ProcedureID = ((MasterListItem)CmbStaffProcedure.SelectedItem).ID;
                        tempStaff.ProcedureName = ((MasterListItem)CmbStaffProcedure.SelectedItem).Description;
                        //tempStaff.Quantity = (float)Convert.ToDouble(txtStaffQuantity.Text);
                        tempStaff.Status = true;
                        StaffList1.Add(tempStaff);
                        dgProcedureStaff.ItemsSource = null;
                        dgProcedureStaff.ItemsSource = StaffList1;
                        CmbStaffProcedure.SelectedValue = (long)0;
                        CmbDesignation.SelectedValue = (long)0;
                        CmbStaff.SelectedValue = (long)0;
                        txtStaffQuantity.Text = "";
                        //}
                        //else
                        //{
                        //    if (objLocalizationManager != null)
                        //    {
                        //        msgText = objLocalizationManager.GetValue("StaffCountExceed_Msg");
                        //    }
                        //    else
                        //    {
                        //        msgText = "Staff count exceed.";
                        //    }
                        //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        //}
                    }
                    else
                    {
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("StaffAddInProcedure_Msg");
                        //}
                        //else
                        //{
                        msgText = "Staff already added for the procedure.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                    CmbStaffProcedure.SelectedValue = (long)0;
                    CmbDesignation.SelectedValue = (long)0;
                    CmbStaff.SelectedValue = (long)0;
                    txtStaffQuantity.Text = "";
                    cmdModifyStaff.IsEnabled = false;
                    cmdAddStaff.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        long staffId;
        string quantity;
        private void hlbEditstaff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flagChangeStaff = true;
                staffId = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).StaffID;
                quantity = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).Quantity.ToString();
                foreach (var item in objStaffDesgList)
                {
                    if (item.Code == ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).DesignationID.ToString())
                        CmbDesignation.SelectedItem = item;
                }
                //FillProcedure();
                //foreach (clsPatientProcedureVO item in this.grdProcedure.ItemsSource)
                //{
                //    MasterProcedureList.Add(new MasterListItem(item.ProcedureID, item.ProcDesc));
                //}
                foreach (var item in MasterProcedureList)
                {
                    if (item.ID == ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).ProcedureID)
                        CmbStaffProcedure.SelectedItem = item;
                }



                //CmbDesignation.SelectedValue = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).DesignationID;
                //CmbStaffProcedure.SelectedValue = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).ProcedureID;                
                //CmbStaff.SelectedValue = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).StaffID;
                //FetchStaffByDesignation(((MasterListItem)CmbDesignation.SelectedItem).Code);

                //txtStaffQuantity.Text = ((clsPatientProcStaffDetailsVO)dgProcedureStaff.SelectedItem).Quantity.ToString();

                int index = dgProcedureStaff.SelectedIndex;
                StaffList1.RemoveAt(index);
                dgProcedureStaff.ItemsSource = null;
                dgProcedureStaff.ItemsSource = StaffList1;
                dgProcedureStaff.UpdateLayout();
                cmdModifyStaff.IsEnabled = true;
                cmdAddStaff.IsEnabled = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdBillClearance_Click(object sender, RoutedEventArgs e)
        {
            if (dgSchedule.ItemsSource != null)
            {
                if (dgSchedule.SelectedItem != null)
                {
                    clsPatientProcedureScheduleVO selectedPatient = ((clsPatientProcedureScheduleVO)dgSchedule.SelectedItem);
                    BillClearance win = new BillClearance();
                    win.ScheduleID = selectedPatient.ID;
                    win.ScheduleUnitID = selectedPatient.UnitID;
                    win.MRNO = selectedPatient.MRNO;
                    win.BillClearanceID = selectedPatient.BillClearanceID;
                    win.BillClearanceUnitID = selectedPatient.BillClearanceUnitID;
                    win.BillClearanceIsFreezed = selectedPatient.BillClearanceIsFreezed;
                    win.Show();
                    win.Closing += new EventHandler<CancelEventArgs>(winChild_Closing);

                }
                else
                {
                    msgText = "Please Select OT Booking .";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }            
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            if (DataList.IsEmpty)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "There are no reports to print", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

            }
            else
            {                

                if (SearchDate.SelectedDate != null)
                {
                    dtFT = SearchDate.SelectedDate.Value.Date.Date;
                }
                if (ToSearchDate.SelectedDate != null)
                {
                    dtTT = ToSearchDate.SelectedDate.Value.Date.Date.AddDays(1);
                }

                long UnitID = 0;              
                string MRNO = null;
                long OTID = 0;
                long OTTableID = 0;
                string FirstName = null;              
                string LastName = null;
                bool IsEmergency;
                bool IsCancelled;             

                MRNO = txtMRNO.Text;              
                FirstName = txtFirstName.Text;              
                LastName = txtLastName.Text;

                if (CmbOtTheatre.SelectedItem != null)
                    OTID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;
                else
                    OTID = 0;
                if (CmbOTTable.SelectedItem != null)
                    OTTableID = ((MasterListItem)CmbOTTable.SelectedItem).ID;
                else
                    OTTableID = 0;

                if ((bool)chkIsEmergency.IsChecked)
                    IsEmergency = true;
                else
                    IsEmergency = false;

                if ((bool)chkIsCancelled.IsChecked)
                    IsCancelled = true;
                else
                    IsCancelled = false;

                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;             

                string URL;
                if (dtFT != null && dtTT != null)
                {
                    URL = "../Reports/OperationTheatre/OTBookingList.aspx?FromDate=" + dtFT.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtTT.Value.ToString("dd-MMM-yyyy") + "&UnitID=" + UnitID + "&MRNO=" + MRNO + "&OTID=" + OTID + "&OTTableID=" + OTTableID + "&FirstName=" + FirstName + "&LastName=" + LastName + "&IsEmergency=" + IsEmergency + "&IsCancelled=" + IsCancelled + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                   
                }

                if (chkExcel.IsChecked == true)
                {
                    chkExcel.IsChecked = false;
                }
            }

        }
    }

    public partial class clsSchedule : INotifyPropertyChanged
    {
        private string _ControlName;
        public string ControlName
        {
            get { return _ControlName; }
            set
            {
                if (_ControlName != value)
                {
                    _ControlName = value;
                }
            }
        }

        private long _OTID;
        public long OTID
        {
            get { return _OTID; }
            set
            {
                if (_OTID != value)
                {
                    _OTID = value;
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                }
            }
        }

        private long _OTTableID;
        public long OTTableID
        {
            get { return _OTTableID; }
            set
            {
                if (_OTTableID != value)
                {
                    _OTTableID = value;
                }
            }
        }


        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                _RowID = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RowID"));
                }
            }
        }

        private string _IsVisible;
        public string IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
                }
            }
        }

        private long _NoOfPatient;
        public long NoOfPatient
        {
            get { return _NoOfPatient; }
            set
            {
                if (_NoOfPatient != value)
                {
                    _NoOfPatient = value;
                }
            }
        }

        private DateTime _OTDate;
        public DateTime OTDate
        {
            get { return _OTDate; }
            set
            {
                if (_OTDate != value)
                {
                    _OTDate = value;
                }
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime _CurrentAppointmentDate;
        public DateTime CurrentAppointmentDate
        {
            get
            {
                return _CurrentAppointmentDate;
            }
            set
            {
                _CurrentAppointmentDate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentAppointmentDate"));
                }
            }
        }

        private DateTime _NextAppointmentDate;
        public DateTime NextAppointmentDate
        {
            get
            {
                return _NextAppointmentDate;
            }
            set
            {
                _NextAppointmentDate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NextAppointmentDate"));
                }
            }
        }

        private DateTime _CurrentScheduleDate;
        public DateTime CurrentScheduleDate
        {
            get
            {
                return _CurrentScheduleDate;
            }
            set
            {
                _CurrentScheduleDate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentScheduleDate"));
                }

            }
        }

        private DateTime _ScheduleFromTime;
        public DateTime ScheduleFromTime
        {
            get
            {
                return _ScheduleFromTime;
            }
            set
            {
                _ScheduleFromTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleFromTime"));
                }
            }
        }

        private DateTime _ScheduleToTime;
        public DateTime ScheduleToTime
        {
            get
            {
                return _ScheduleToTime;
            }
            set
            {
                _ScheduleToTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleToTime"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private string _ScheduleFromTimeString;
        public string ScheduleFromTimeString
        {
            get
            {
                return _ScheduleFromTimeString;
            }
            set
            {
                _ScheduleFromTimeString = value;
                if (_ScheduleFromTimeString != string.Empty)
                {
                    VisibleCurrentSchedule = "Visible";
                }
                else
                {

                    VisibleCurrentSchedule = "Collapsed";
                }

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleFromTimeString"));
                }
            }
        }

        private string _TableName;
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TableName"));
                }
            }
        }

        private string _ScheduleToTimeString;
        public string ScheduleToTimeString
        {
            get
            {
                return _ScheduleToTimeString;
            }
            set
            {
                _ScheduleToTimeString = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ScheduleToTimeString"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                _PatientName = value;
            }
        }

        private string _VisibleCurrentSchedule;
        public string VisibleCurrentSchedule
        {
            get { return _VisibleCurrentSchedule; }
            set
            {
                _VisibleCurrentSchedule = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VisibleCurrentSchedule"));
                }
            }
        }

        private string _Day;
        public string Day
        {
            get
            {
                return _Day;
            }
            set
            {
                _Day = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Day"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private SolidColorBrush _CurrentapColor;
        public SolidColorBrush CurrentapColor
        {
            get
            {
                return _CurrentapColor;
            }
            set
            {
                _CurrentapColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentapColor"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private SolidColorBrush _apColor;
        public SolidColorBrush apColor
        {
            get
            {
                return _apColor;
            }
            set
            {
                _apColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("apColor"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private SolidColorBrush _NextapColor;
        public SolidColorBrush NextapColor
        {
            get
            {
                return _NextapColor;
            }
            set
            {
                _NextapColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NextapColor"));
                }
                //PropertyChangedHandler("PlayerName");
            }
        }

        private bool _CurrentIsChecked;
        public bool CurrentIsChecked
        {
            get { return _CurrentIsChecked; }
            set
            {
                _CurrentIsChecked = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentIsChecked"));
                }
            }
        }

        private bool _IsCurrentSchedule;
        public bool IsCurrentSchedule
        {
            get { return _IsCurrentSchedule; }
            set
            {
                _IsCurrentSchedule = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsCurrentSchedule"));
                }
            }
        }

        
    }
}

