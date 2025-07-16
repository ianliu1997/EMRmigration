using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.ValueObjects.Master;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Data;
using PalashDynamic.Localization;
using System.Windows.Input;
//using PalashDynamics.SearchResultLists;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmOTDetails : UserControl
    {
        #region Variable Declaration
        bool IsCancel = true;
        public long lOTDetailsID { get; set; }     
        public bool IsView { get; set; }
        clsPatientGeneralVO patientDetails = new clsPatientGeneralVO();
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient as clsPatientGeneralVO;
        LocalizationManager objLocalizationManager = null;
        WaitIndicator ObjWiaitIndicator = null;
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASH";
        string msgText = string.Empty;
        public string ModuleName { get; set; }
        public long PatientID = 0;
        public List<clsOtDetailsProcedureDetailsVO> procedureList;
        List<MasterListItem> PostInstructionList = new List<MasterListItem>();
        List<MasterListItem> ProcedureList = new List<MasterListItem>();
        public List<clsOTDetailsDocDetailsVO> docList;
        public List<clsOTDetailsStaffDetailsVO> StaffList;
        public List<clsOTDetailsPostInstructionDetailsVO> PostInsList;
        public List<clsOTDetailsItemDetailsVO> OTDetailsItemList;
        public List<clsOTDetailsSurgeryDetailsVO> OTDetailsSurgeyNotesList;
        public List<clsOtDetailsAnesthesiaNotesDetailsVO> OTDetailsAnesthesiaNotesList;
        public PagedSortableCollectionView<clsOTDetailsVO> DataList { get; private set; }
        public long ScheduleID = 0;
        public long OTDetailsIDView = 0;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        #endregion

        #region Properties

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
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Constructor And Load Function
        public frmOTDetails()
        {
            InitializeComponent();
            FetchOTForFrontPanel();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsOTDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            objLocalizationManager = ((IApplicationConfiguration)App.Current).LocalizedManager;
            accOT.IsSelected = true;
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            //LayoutRoot.Width = this.ActualWidth;

        }

        /// <summary>
        /// Used when refresh event occurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        /// <summary>
        /// User Control loaded
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.grdDemographicDetails.MinWidth = this.ActualWidth;
            cmdNew.Click += new RoutedEventHandler(cmdNew_Click);
            SetCommandButtonState("Load");
            FetchData();
            //PalashDynamics.SearchResultLists.DisplayPatientDetails winDisplay = new PalashDynamics.SearchResultLists.DisplayPatientDetails();
            //winDisplay.MinWidth = this.ActualWidth;
            //ResultListContent.Content = winDisplay;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Fills front panel OT details grid
        /// </summary>
        private void FetchData()
        {
            ObjWiaitIndicator = new WaitIndicator();

            bool res = true;

            if ((dtpFromDate.SelectedDate != null) && (dtpToDate.SelectedDate != null))
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                {
                    msgText = "From Date Should Be Less Than To Date";
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if ((dtpFromDate.SelectedDate != null) && (dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date && res == true))
            {
                msgText = "From Date Should Not Be Greater Than Today's Date";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }
            if (res)
            {
                try
                {
                    clsGetOTDetailsListizActionVO BizAction = new clsGetOTDetailsListizActionVO();
                    BizAction.objOTDetails = new List<clsOTDetailsVO>();
                    BizAction.IsPagingEnabled = true;
                    BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                    BizAction.MaximumRows = DataList.PageSize;
                    if (CmbOtTheatre.SelectedItem != null)
                        BizAction.OTID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;
                    else
                        BizAction.OTID = 0;
                    if (dtpFromDate.SelectedDate != null)
                        BizAction.FromDate = dtpFromDate.SelectedDate;
                    if (dtpToDate.SelectedDate != null)
                        BizAction.ToDate = dtpToDate.SelectedDate;
                    if (txtSearchCriteria.Text.Trim() != null)
                        BizAction.FirstName = txtSearchCriteria.Text.Trim();
                    if (txtLastName.Text.Trim() != null)
                        BizAction.LastName = txtLastName.Text.Trim();
                    if (txtMRNO.Text.Trim() != null)
                        BizAction.MRNo = txtMRNO.Text.Trim();

                    ObjWiaitIndicator.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && ((clsGetOTDetailsListizActionVO)arg.Result).objOTDetails != null)
                        {
                            clsGetOTDetailsListizActionVO result = arg.Result as clsGetOTDetailsListizActionVO;
                            DataList.TotalItemCount = Convert.ToInt32(result.TotalRows);

                            if (result.objOTDetails != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.objOTDetails)
                                {
                                    if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId || item.UnitID==1)
                                    {
                                        DataList.Add(item);
                                    }
                                }
                                PagedCollectionView collection = new PagedCollectionView(DataList);
                                dgSchedule.ItemsSource = null;
                                dgSchedule.ItemsSource = collection;
                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.Source = DataList;
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
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    ObjWiaitIndicator.Close();
                }
            }
        }

        /// <summary>
        /// Clears UI
        /// </summary>
        private void ClearUI()
        {
            try
            {
                procedureList = new List<clsOtDetailsProcedureDetailsVO>();
                docList = new List<clsOTDetailsDocDetailsVO>();
                ProcedureList = new List<MasterListItem>();
                PostInstructionList = new List<MasterListItem>();
                PostInsList = new List<clsOTDetailsPostInstructionDetailsVO>();
                StaffList = new List<clsOTDetailsStaffDetailsVO>();
                OTDetailsItemList = new List<clsOTDetailsItemDetailsVO>();
                grdBackPanel.DataContext = new clsOTDetailsVO();
                txtFirstName.Text = string.Empty;
                txtGender.Text = string.Empty;
                txtDOB.Text = string.Empty;
                txtAge.Text = string.Empty;
                txtMaritalStatus.Text = string.Empty;
                txtEducation.Text = string.Empty;
                txtReligion.Text = string.Empty;
                txtContactNO1.Text = string.Empty;
                //txtYears.Text = string.Empty;
                //txtMonths.Text = string.Empty;
                //txtDays.Text = string.Empty;
                //txtDOB.Text = string.Empty;
                ((IApplicationConfiguration)App.Current).SelectedOTBooking = new clsPatientProcedureScheduleVO();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetPatientData(string sMRNo)
        {
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
                        //if (objLocalizationManager != null)
                        //{
                        //    msgText = objLocalizationManager.GetValue("MRChkValidation_Msg");
                        //}
                        //else
                        //{
                        msgText = "Please check MR number.";
                        //}
                        ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //txtMrNo.Text = string.Empty;
                    }
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();



        }

        /// <summary>
        /// splits date into month,day & year
        /// </summary>
        /// <param name="Datevalue"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
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

        private void BindSelectedPatient()
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + SelectedPatient.PatientName;
            mElement.Text += " - " + SelectedPatient.MRNo + " : " + SelectedPatient.Gender;
            //txtMrNo.Text = SelectedPatient.MRNo;
            txtFirstName.Text = SelectedPatient.PatientName;
            txtAge.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY") + " years";
            txtGender.Text = SelectedPatient.Gender;
            txtMaritalStatus.Text = SelectedPatient.MaritalStatus;
            txtContactNO1.Text = SelectedPatient.ContactNO1;
            txtEducation.Text = SelectedPatient.Education;
            txtReligion.Text = SelectedPatient.Religion;

            //if (SelectedPatient.Photo != null)
            //if (SelectedPatient.ImageName.Length > 0)
            //{
            //    //commented by neena
            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
            //    //bmp.FromByteArray(SelectedPatient.Photo);
            //    ////bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedPatient.Photo);
            //    //imgPhoto.Source = bmp;
            //    //

            //    //added by neena
            //    imgPhoto.Source = new BitmapImage(new Uri(((IApplicationConfiguration)App.Current).SelectedPatient.ImageName, UriKind.Absolute));
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
            //txtPatientName.Text = SelectedPatient.PatientName;
            if (SelectedPatient.DateOfBirth != null)
            {
                txtDOB.Text = Convert.ToDateTime(SelectedPatient.DateOfBirth).ToString("dd/MM/yyyy");
                //txtYears.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY");
                //txtMonths.Text = ConvertDate(SelectedPatient.DateOfBirth, "MM");
                //txtDays.Text = ConvertDate(SelectedPatient.DateOfBirth, "DD");
            }
            //txtSex.Text = SelectedPatient.Gender;     
            ScheduleID = 0;
            FillContentDetails();
        }

        private void FillDetailsOfProcedureSchedule(long ScheduleID)
        {
            try
            {
                if (ObjWiaitIndicator == null)
                    ObjWiaitIndicator = new WaitIndicator();
                if (ObjWiaitIndicator.Visibility == Visibility.Collapsed)
                {
                    ObjWiaitIndicator.Show();
                }
                else
                {
                    ObjWiaitIndicator.Close();
                    ObjWiaitIndicator.Show();
                }
                clsGetProcScheduleDetailsByProcScheduleIDBizActionVO BizAction = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                //BizAction.PatientProcList = new List<clsPatientProcedureVO>();
                //BizAction.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                //BizAction.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                //BizAction.OTScheduleList = new List<clsPatientProcedureScheduleVO>();
                //BizAction.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                BizAction.ScheduleID = ScheduleID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        ClearUI();
                        clsGetProcScheduleDetailsByProcScheduleIDBizActionVO DetailsVO = new clsGetProcScheduleDetailsByProcScheduleIDBizActionVO();
                        DetailsVO = (clsGetProcScheduleDetailsByProcScheduleIDBizActionVO)arg.Result;

                        if (DetailsVO.patientInfoObject != null && DetailsVO.patientInfoObject.pateintID > 0)
                        {
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                            mElement.Text = " : " + DetailsVO.patientInfoObject.PatientName;
                            mElement.Text += " - " + DetailsVO.patientInfoObject.MRNO + " : " + DetailsVO.patientInfoObject.Gender;

                            PatientID = DetailsVO.patientInfoObject.pateintID;
                            txtFirstName.Text = DetailsVO.patientInfoObject.PatientName;
                            //txtMrNo.Text = DetailsVO.patientInfoObject.MRNO;
                            txtGender.Text = DetailsVO.patientInfoObject.Gender;

                            if (DetailsVO.patientInfoObject.DOB.ToString().Contains("1/0001"))
                            {
                                txtDOB.Text = "";
                                //txtYears.Text = "";
                                //txtMonths.Text = "";
                                //txtDays.Text = "";
                            }
                            else
                            {
                                int len = DetailsVO.patientInfoObject.DOB.ToString().IndexOf(" ", 0);
                                txtDOB.Text = DetailsVO.patientInfoObject.DOB.ToString().Substring(0, len);
                                txtAge.Text = ConvertDate(DetailsVO.patientInfoObject.DOB, "YY") + " years";
                                //txtMonths.Text = ConvertDate(DetailsVO.patientInfoObject.DOB, "MM");
                                //txtDays.Text = ConvertDate(DetailsVO.patientInfoObject.DOB, "DD");
                            }
                            (tvOTDetails.Items[0] as TreeViewItem).IsSelected = true;
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
                    ObjWiaitIndicator.Close();

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Fill Combo Box

        /// <summary>
        /// Fetch OT For front panel
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTForFrontPanel()
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
        /// Fetches Doctor
        /// </summary>
        private void FetchDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    //CmbDoctor.ItemsSource = null;
                    //CmbDoctor.ItemsSource = objList;
                    //CmbDoctor.SelectedItem = objList[0];
                    //if (this.DataContext != null)
                    //{
                    //    CmbDoctor.SelectedValue = ((clsOTDetailsVO)this.DataContext).DoctorID;
                    //}
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
                    cmdNew.IsEnabled = true;
                    this.IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Search Click

        private void btnPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnPatientSearch_Click);
            Win.Show();
        }

        void Win_OnPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + SelectedPatient.PatientName;
            mElement.Text += " - " + SelectedPatient.MRNo + " : " + SelectedPatient.Gender;
            //txtMrNo.Text = SelectedPatient.MRNo;
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
            //    ////bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedPatient.Photo);
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

            //txtPatientName.Text = SelectedPatient.PatientName;
            if (SelectedPatient.DateOfBirth != null)
            {
                txtDOB.Text = Convert.ToDateTime(SelectedPatient.DateOfBirth).ToString("dd/MM/yyyy");
                txtAge.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY") + "Years";
                //txtYears.Text = ConvertDate(SelectedPatient.DateOfBirth, "YY");
                //txtMonths.Text = ConvertDate(SelectedPatient.DateOfBirth, "MM");
                //txtDays.Text = ConvertDate(SelectedPatient.DateOfBirth, "DD");
            }
            //txtSex.Text = SelectedPatient.Gender;
            ScheduleID = 0;
            FillContentDetails();
        }

        private void CmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            //if (!String.IsNullOrEmpty(txtMrNo.Text) && txtMrNo.Text.Trim() != String.Empty)
            //{
            //    GetPatientData(txtMrNo.Text.Trim());
            //}
            //else
            //{
            //    if (objLocalizationManager != null)
            //    {
            //        msgText = objLocalizationManager.GetValue("MRNoValidation_Msg");
            //    }
            //    else
            //    {
            //        msgText = "Please Enter MR Number";
            //    }
            //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //}
        }

        #endregion

        #region Button Click Event
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if ((tvOTDetails.Items[0] as TreeViewItem).IsSelected)
            {
                (tvOTDetails.Items[0] as TreeViewItem).IsSelected = false;
                (tvOTDetails.Items[0] as TreeViewItem).IsSelected = true;
            }
            else
                (tvOTDetails.Items[0] as TreeViewItem).IsSelected = true;
            this.SetCommandButtonState("New");
            ClearUI();
            objAnimation.Invoke(RotationType.Forward);
            NewCancelBrd.Visibility = Visibility.Collapsed;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearUI();
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = "  ";
                IsCancel = true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdOTSchedule_Click(object sender, RoutedEventArgs e)
        {


            frmOTScheduling win_ObjSchedule = new frmOTScheduling();
            win_ObjSchedule.MainFooter.Visibility = Visibility.Collapsed;
            win_ObjSchedule.ChildFooter.Visibility = Visibility.Visible;

            var hyLink = win_ObjSchedule.dgSchedule.Columns[0];
            foreach (DataGridColumn item in win_ObjSchedule.dgSchedule.Columns)
            {
                if (item.Header.ToString() == "View")
                    item.Visibility = Visibility.Collapsed;
                //if (item.Header.ToString() == "Is Emergency")
                //    item.IsReadOnly = true;
                //if (item.Header.ToString() == "Status")
                //    item.IsReadOnly = true;

            }

            ChildWindow ObjChildOTSchedule = new ChildWindow();
            ObjChildOTSchedule.Content = win_ObjSchedule;

            ObjChildOTSchedule.Width = this.ActualWidth * 0.85;
            ObjChildOTSchedule.Height = this.ActualHeight * 0.85;

            ObjChildOTSchedule.Closed += new EventHandler(ObjChildOTSchedule_Closed);
            ObjChildOTSchedule.Show();
        }

        bool IsEmergency = false;
        private void ObjChildOTSchedule_Closed(object sender, EventArgs e)
        {
            if ((bool)((ChildWindow)sender).DialogResult)
            {
                this.SetCommandButtonState("New");

                if (((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking) != null)
                {
                    if (((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking) != null)
                    {
                        clsPatientProcedureScheduleVO PatientInfo = ((IApplicationConfiguration)App.Current).SelectedOTBooking as clsPatientProcedureScheduleVO;

                        ScheduleID = PatientInfo.ID;
                        PatientID = PatientInfo.PatientID;

                        //UserControl rootPage = Application.Current.RootVisual as UserControl;
                        //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        //mElement.Text = " : " + PatientInfo.PatientName;
                        //mElement.Text += " - " + PatientInfo.MRNO + " : " + PatientInfo.Gender;

                        //txtMrNo.Text = PatientInfo.MRNO;
                        txtFirstName.Text = PatientInfo.PatientName;
                        txtGender.Text = PatientInfo.Gender;
                        txtMaritalStatus.Text = PatientInfo.MaritalStatus;
                        txtEducation.Text = PatientInfo.Education;
                        txtReligion.Text = PatientInfo.Religion;
                        txtContactNO1.Text = PatientInfo.ContactNo1;
                        IsEmergency = PatientInfo.IsEmergency;

                        //MemoryStream stream = new MemoryStream(PatientInfo.Photo);                        
                        //BitmapImage b = new BitmapImage();
                        //b.SetSource(stream);
                        //imgPhoto.Source = b;

                        //if (PatientInfo.Photo != null)
                        //if (PatientInfo.ImageName.Length > 0)
                        //{
                        //    //commented by neena
                        //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                        //    //bmp.FromByteArray(PatientInfo.Photo);
                        //    ////bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedPatient.Photo);
                        //    //imgPhoto.Source = bmp;
                        //    //

                        //    //added by neena
                        //    imgPhoto.Source = new BitmapImage(new Uri(PatientInfo.ImageName, UriKind.Absolute));
                        //    //
                        //}                     



                        if (PatientInfo.ImageName != null && PatientInfo.ImageName.Length > 0)
                        {
                            imgPhoto.Source = new BitmapImage(new Uri(PatientInfo.ImageName, UriKind.Absolute));
                        }
                        else if (PatientInfo.Photo != null)
                        {
                            byte[] imageBytes = PatientInfo.Photo;
                            BitmapImage img = new BitmapImage();
                            img.SetSource(new MemoryStream(imageBytes, false));
                            imgPhoto.Source = img;
                        }
                        else
                            imgPhoto.Source = null;


                        btnAttachPhoto.IsEnabled = true;

                        if (((IApplicationConfiguration)App.Current).SelectedOTBooking.DOB != null)
                        {
                            txtDOB.Text = Convert.ToDateTime(PatientInfo.DOB).ToString("dd/MM/yyyy");
                            txtAge.Text = ConvertDate(PatientInfo.DOB, "YY") + " years";
                            //txtMonths.Text = ConvertDate(PatientInfo.DOB, "MM");
                            //txtDays.Text = ConvertDate(PatientInfo.DOB, "DD");
                        }

                        FillContentDetails();
                        //switch ((this.dpnlFormDetails as ContentControl).Content.GetType().Name)
                        //{
                        //    case "frmOTSheetDetails":
                        //        frmOTSheetDetails winOTSheetDetails = (this.dpnlFormDetails as ContentControl).Content as frmOTSheetDetails;
                        //        winOTSheetDetails.lScheduleID = ScheduleID;
                        //        winOTSheetDetails.PatientID = PatientID;
                        //        winOTSheetDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                        //        break;

                        //    case "frmSurgeryDetails":
                        //        frmOTSurgeryDetails winOTSurgeryDetails = (this.dpnlFormDetails as ContentControl).Content as frmOTSurgeryDetails;
                        //        winOTSurgeryDetails.lScheduleID = ScheduleID;
                        //        winOTSurgeryDetails.lPatientID = PatientID;
                        //        winOTSurgeryDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                        //        break;

                        //    case "frmDocEmpDetails":
                        //        frmDocEmpDetails winDocEmpDetails = (this.dpnlFormDetails as ContentControl).Content as frmDocEmpDetails;
                        //        winDocEmpDetails.lScheduleID = ScheduleID;
                        //        winDocEmpDetails.lPatientID = PatientID;
                        //        winDocEmpDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                        //        break;

                        //    case "frmService":
                        //        frmOTDetailsService winOTDetailsService = (this.dpnlFormDetails as ContentControl).Content as frmOTDetailsService;
                        //        winOTDetailsService.lScheduleID = ScheduleID;
                        //        winOTDetailsService.lPatientID = PatientID;
                        //        winOTDetailsService.FillDetailsOfProcedureSchedule(ScheduleID);
                        //        break;

                        //    case "frmOTNotes":
                        //        frmOTNotes winOTNotes = (this.dpnlFormDetails as ContentControl).Content as frmOTNotes;
                        //        winOTNotes.lScheduleID = ScheduleID;
                        //        winOTNotes.lPatientID = PatientID;
                        //        winOTNotes.FillDetailsOfProcedureScheduleNotes(ScheduleID);
                        //        break;

                        //    case "frmPostInstructionDetails":
                        //        frmPostInstructionDetails winPostInstructionDetails = (this.dpnlFormDetails as ContentControl).Content as frmPostInstructionDetails;
                        //        winPostInstructionDetails.lScheduleID = ScheduleID;
                        //        winPostInstructionDetails.lPatientID = PatientID;
                        //        winPostInstructionDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                        //        break;
                        //}
                    }
                }
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void FillContentDetails()
        {
            string name = (this.dpnlFormDetails as ContentControl).Content.GetType().Name;
            switch (name)
            {
                case "frmOTSheetDetails":
                    frmOTSheetDetails winOTSheetDetails = (this.dpnlFormDetails as ContentControl).Content as frmOTSheetDetails;
                    winOTSheetDetails.lScheduleID = ScheduleID;
                    winOTSheetDetails.PatientID = PatientID;
                    winOTSheetDetails.lIsEmergency = IsEmergency;
                    winOTSheetDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

                case "frmOTSurgeryDetails":
                    frmOTSurgeryDetails winOTSurgeryDetails = (this.dpnlFormDetails as ContentControl).Content as frmOTSurgeryDetails;
                    winOTSurgeryDetails.lScheduleID = ScheduleID;
                    winOTSurgeryDetails.lPatientID = PatientID;
                    winOTSurgeryDetails.lIsEmergency = IsEmergency;
                    winOTSurgeryDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

                case "frmDocEmpDetails":
                    frmDocEmpDetails winDocEmpDetails = (this.dpnlFormDetails as ContentControl).Content as frmDocEmpDetails;
                    winDocEmpDetails.lScheduleID = ScheduleID;
                    winDocEmpDetails.lPatientID = PatientID;
                    winDocEmpDetails.lIsEmergency = IsEmergency;
                    winDocEmpDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

                case "frmOTDetailsService":
                    frmOTDetailsService winOTDetailsService = (this.dpnlFormDetails as ContentControl).Content as frmOTDetailsService;
                    winOTDetailsService.lScheduleID = ScheduleID;
                    winOTDetailsService.lPatientID = PatientID;
                    winOTDetailsService.lIsEmergency = IsEmergency;
                    winOTDetailsService.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

                case "frmOTNotes":
                    frmOTNotes winOTNotes = (this.dpnlFormDetails as ContentControl).Content as frmOTNotes;
                    winOTNotes.lScheduleID = ScheduleID;
                    winOTNotes.lPatientID = PatientID;
                    winOTNotes.lIsEmergency = IsEmergency;
                    winOTNotes.FillDetailsOfProcedureScheduleNotes(ScheduleID);
                    break;

                case "frmPostInstructionDetails":
                    frmPostInstructionDetails winPostInstructionDetails = (this.dpnlFormDetails as ContentControl).Content as frmPostInstructionDetails;
                    winPostInstructionDetails.lScheduleID = ScheduleID;
                    winPostInstructionDetails.lPatientID = PatientID;
                    winPostInstructionDetails.lIsEmergency = IsEmergency;
                    winPostInstructionDetails.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

                case "frmOTItemDetails":
                    frmOTItemDetails winItemNotes = (this.dpnlFormDetails as ContentControl).Content as frmOTItemDetails;
                    winItemNotes.lScheduleID = ScheduleID;
                    winItemNotes.lPatientID = PatientID;
                    winItemNotes.lIsEmergency = IsEmergency;
                    winItemNotes.FillDetailsOfProcedureSchedule(ScheduleID);
                    break;

            }
        }

        private void hlkView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("View");
                ClearUI();
                if (dgSchedule.SelectedItem != null)
                {
                    grdBackPanel.DataContext = ((clsOTDetailsVO)dgSchedule.SelectedItem);
                    txtFirstName.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).PatientName;
                    //txtMrNo.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).MRNo;
                    txtAge.Text = ConvertDate(((clsOTDetailsVO)grdBackPanel.DataContext).DOB, "YY") + " years";
                    txtGender.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).Gender;
                    txtMaritalStatus.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).MaritalStatus;
                    txtEducation.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).Education;
                    txtReligion.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).Religion;
                    txtContactNO1.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).ContactNo1;
                    OTDetailsIDView = (dgSchedule.SelectedItem as clsOTDetailsVO).ID;
                    PatientID = (dgSchedule.SelectedItem as clsOTDetailsVO).PatientID;
                    ScheduleID = (dgSchedule.SelectedItem as clsOTDetailsVO).ScheduleID;
                    IsEmergency = (dgSchedule.SelectedItem as clsOTDetailsVO).IsEmergency;

                    //if ((dgSchedule.SelectedItem as clsOTDetailsVO).Photo != null)
                    //if ((dgSchedule.SelectedItem as clsOTDetailsVO).ImageName.Length > 0)
                    //{
                    //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                    //    //bmp.FromByteArray((dgSchedule.SelectedItem as clsOTDetailsVO).Photo);
                    //    //imgPhoto.Source = bmp;

                    //    //added by neena
                    //    imgPhoto.Source = new BitmapImage(new Uri((dgSchedule.SelectedItem as clsOTDetailsVO).ImageName, UriKind.Absolute));
                    //    //

                    //}

                    if ((dgSchedule.SelectedItem as clsOTDetailsVO).ImageName != null && (dgSchedule.SelectedItem as clsOTDetailsVO).ImageName.Length > 0)
                    {
                        imgPhoto.Source = new BitmapImage(new Uri((dgSchedule.SelectedItem as clsOTDetailsVO).ImageName, UriKind.Absolute));
                    }
                    else if ((dgSchedule.SelectedItem as clsOTDetailsVO).Photo != null)
                    {
                        byte[] imageBytes = (dgSchedule.SelectedItem as clsOTDetailsVO).Photo;
                        BitmapImage img = new BitmapImage();
                        img.SetSource(new MemoryStream(imageBytes, false));
                        imgPhoto.Source = img;
                    }

                    if (((clsOTDetailsVO)grdBackPanel.DataContext).DOB.ToString().Contains("1/0001"))
                    {
                        txtDOB.Text = string.Empty;
                        //txtYears.Text = string.Empty;
                        //txtMonths.Text = string.Empty;
                        //txtDays.Text = string.Empty;
                    }
                    else
                    {
                        int len = ((clsOTDetailsVO)grdBackPanel.DataContext).DOB.ToString().IndexOf(" ", 0);
                        txtDOB.Text = ((clsOTDetailsVO)grdBackPanel.DataContext).DOB.ToString().Substring(0, len);
                        //txtYears.Text = ConvertDate(((clsOTDetailsVO)grdBackPanel.DataContext).DOB, "YY");
                        //txtMonths.Text = ConvertDate(((clsOTDetailsVO)grdBackPanel.DataContext).DOB, "MM");
                        //txtDays.Text = ConvertDate(((clsOTDetailsVO)grdBackPanel.DataContext).DOB, "DD");
                    }
                    //CmdPatientSearch.IsEnabled = false;
                    //btnPatientSearch.IsEnabled = false;
                    cmdOTSchedule.IsEnabled = false;
                    //txtMrNo.IsReadOnly = true;
                    objAnimation.Invoke(RotationType.Forward);

                    if ((tvOTDetails.Items[0] as TreeViewItem).IsSelected)
                    {
                        (tvOTDetails.Items[0] as TreeViewItem).IsSelected = false;
                        (tvOTDetails.Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvOTDetails.Items[0] as TreeViewItem).IsSelected = true;

                }

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : On Date " + ((clsOTDetailsVO)dgSchedule.SelectedItem).Date;

                btnAttachPhoto.IsEnabled = true;
                NewCancelBrd.Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        #endregion

        #region SelectionChanged Events
        private void tvOTDetails_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            string strOTDetails = (sender as TreeViewItem).Name;
            switch (strOTDetails)
            {
                case "frmOTSheetDetails":
                    frmOTSheetDetails winSheetDetails = new frmOTSheetDetails();
                    winSheetDetails.lScheduleID = this.ScheduleID;
                    winSheetDetails.PatientID = this.PatientID;                   
                    winSheetDetails.lOTDetailsID = this.OTDetailsIDView;                           
                    winSheetDetails.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winSheetDetails;
                    break;
                case "frmSurgeryDetails":
                    frmOTSurgeryDetails winSurgeryDetails = new frmOTSurgeryDetails();
                    winSurgeryDetails.lScheduleID = this.ScheduleID;
                    winSurgeryDetails.lPatientID = this.PatientID;
                    if (this.OTDetailsIDView > 0)
                    {
                        winSurgeryDetails.lOTDetailsIDView = this.OTDetailsIDView; 
                    }
                    else
                    {
                        this.OTDetailsIDView = lOTDetailsID;
                    }
                    winSurgeryDetails.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winSurgeryDetails;
                    break;
                case "frmDocEmpDetails":
                    frmDocEmpDetails winDocEmpDetails = new frmDocEmpDetails();
                    winDocEmpDetails.lScheduleID = this.ScheduleID;
                    winDocEmpDetails.lPatientID = this.PatientID;
                    winDocEmpDetails.lODetailsIDView = this.OTDetailsIDView;
                    winDocEmpDetails.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winDocEmpDetails;
                    break;
                case "frmService":
                    frmOTDetailsService winService = new frmOTDetailsService();
                    winService.lScheduleID = this.ScheduleID;
                    winService.lPatientID = this.PatientID;
                    winService.lOTDetailsIDView = this.OTDetailsIDView;
                    winService.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winService;
                    break;
                case "frmPostInstructionDetails":
                    frmPostInstructionDetails winPostInstruction = new frmPostInstructionDetails();
                    winPostInstruction.lScheduleID = this.ScheduleID;
                    winPostInstruction.lPatientID = this.PatientID;
                    winPostInstruction.lOTDetailsIDView = this.OTDetailsIDView;
                    winPostInstruction.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winPostInstruction;
                    break;
                case "frmOTNotes":
                    frmOTNotes winNotes = new frmOTNotes();
                    winNotes.lScheduleID = this.ScheduleID;
                    winNotes.lPatientID = this.PatientID;
                    winNotes.lOTDetailsIDView = this.OTDetailsIDView;
                    winNotes.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winNotes;
                    break;

                case "frmItemDetails":
                    frmOTItemDetails winItemNotes = new frmOTItemDetails();
                    winItemNotes.lScheduleID = this.ScheduleID;
                    winItemNotes.lPatientID = this.PatientID;
                    winItemNotes.lODetailsIDView = this.OTDetailsIDView;
                    winItemNotes.lIsEmergency = this.IsEmergency;
                    this.dpnlFormDetails.Content = winItemNotes;
                    break;
                case "FrmOTOtherDetails":
                    FrmOTOtherDetails WIn = new FrmOTOtherDetails();
                    WIn.lScheduleID = this.ScheduleID;
                    WIn.lPatientID = SelectedPatient.PatientID;
                    this.dpnlFormDetails.Content = WIn;
                    break;


            }

        }

        private void txtMrNo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CmdPatientSearch_Click(sender, e);
            }
        }
        #endregion

        private void cmdPatientList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearUI();
                SetCommandButtonState("Cancel");
                objAnimation.Invoke(RotationType.Backward);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = "  ";
                IsCancel = true;
                NewCancelBrd.Visibility = Visibility.Visible;
                this.ScheduleID = 0;
                btnAttachPhoto.IsEnabled = false;
                //CmdPatientSearch.IsEnabled = true;
                //btnPatientSearch.IsEnabled = true;
                cmdOTSchedule.IsEnabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void cmdPrintSummary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //msgText = "Record saved successfully.";
                //clsPatientProcedureScheduleVO PatientInfo = ((IApplicationConfiguration)App.Current).SelectedOTBooking as clsPatientProcedureScheduleVO;
                if (PatientID > 0 && ScheduleID>0)
                {
                    string unitid = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString();
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/OperationTheatre/OTSchedulesDetailsReport.aspx?PatientID=" + PatientID + "&ScheduleID=" + ScheduleID + "&OTDetailsID=" + this.OTDetailsIDView + "&UnitID=" + unitid), "_blank");
                }
                else
                {

                    msgText = "Please select schedule.";
                
                    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
           
                
                }
                
                }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            frmUpdatePhoto UpdatePhoto = new frmUpdatePhoto();
            if (this.DataContext != null)
                UpdatePhoto.MyPhoto = null;
            UpdatePhoto.OnSaveButton_Click += new RoutedEventHandler(UpdatePhoto_OnSaveButton_Click);
            UpdatePhoto.Show();


            //PhotoWindow phWin = new PhotoWindow();
            //if (this.DataContext != null)
            //    phWin.MyPhoto = null; //((clsPatientVO)this.DataContext).Photo;
            //phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            //phWin.Show();
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

        private void frmPostInstructionDetails_Selected(object sender, RoutedEventArgs e)
        {

        }

        //void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (((PhotoWindow)sender).DialogResult == true)
        //    {

        //        imgPhoto.Source = ((PhotoWindow)sender).imgPhoto.Source;
        //        clsSavePhotoBizActionVO BizAction = new clsSavePhotoBizActionVO();
        //        BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //        BizAction.Photo = ((PhotoWindow)sender).MyPhoto;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, args) =>
        //        {
        //            ShowMessageBox("Image Save Successfully !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }            
        //}




    }

}
