using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Animations;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmBedUnreservation : UserControl
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion
        #region Variable Declaration AND List Declaration
        PalashDynamics.Animations.SwivelAnimation _flip = null;

        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> ReservedBedList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> FilterDataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> DataListForTransfer { get; private set; }
        public List<clsIPDBedReservationVO> objBedReservationList = null;
        List<clsIPDBedReservationVO> SelectedBedListForUnReserve = new List<clsIPDBedReservationVO>();

        public string ModuleName { get; set; }
        public string Action { get; set; }
        bool IsSearchOnList = false;
        bool IsViewClick;
        string msgText;
        public long PatientID = 0;
        public long UnitID = 0;
        public long IPDAdmissionID = 0;
        public string IPDAdmissionNo = String.Empty;
        private long PatientUnitID = 0, BedID = 0;
        private string MRNO = null;
        private bool FromBedStatus = false;
        public string PatientName = null;
        clsIPDBedReservationVO BedDetails = null;
        clsPatientGeneralVO patientDetails = null;
        bool IsFromAdmission = false;
        bool IsForSelectEvent = false;
        public event RoutedEventHandler OnSaveButton_Click;

        #endregion
        #region OnPropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Page Size

        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        public int PageSizeData
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                OnPropertyChanged("PageSizeData");
            }
        }

        public int PageSizeReservedBed
        {
            get
            {
                return ReservedBedList.PageSize;
            }
            set
            {
                if (value == ReservedBedList.PageSize) return;
                ReservedBedList.PageSize = value;
                OnPropertyChanged("PageSizeReservedBed");
            }
        }

        #endregion

        public frmBedUnreservation()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            //_flip = new PalashDynamics.Animations.SwivelAnimation(BedReservationLayoutRoot, PatientReservationLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        }

   
       
        private void cmdUnReserve_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmdShow_Click(object sender, RoutedEventArgs e)
        {
            BindGridList();
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";
        }
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSizeData = 15;
            dgDataPager.PageSize = PageSizeData;
            dgDataPager.Source = DataList;
            dgUnReserved.ItemsSource = DataList;
            //dtpFromDate.SelectedDate = DateTime.Now;
            //dtpToDate.SelectedDate = DateTime.Now;
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            BindGridList();
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId != null)
                {
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                    UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    PatientName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                    IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                    FindPatient(PatientID, UnitID, null);
                }
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            
        }

        #region  Search Buttons Clicks

        private void cmbView_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            if (txtMRNo.Text.Length != 0)
            {
                IsViewClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void cmbPatientSearch_Click(object sender, RoutedEventArgs e)
        {           
            IsSearchOnList = true;
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientSearch";
            ClearControl();
            BindGridList();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                if (IsSearchOnList == true)
                {
                    FindPatient(PatientID, UnitId, null);
                }
                else
                {
                    GetActiveAdmissionOfPatient(PatientID, UnitId, null);
                }
            }
        }
        bool IsSearchClick;
        private void btnPatientSearch(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            IsSearchClick = true;
            if (!string.IsNullOrEmpty(txtMRNo.Text))
            {
                IsViewClick = false;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW3.Show();
                if (IsFromAdmission == false)//When Comes From Admission Form
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
            }
        }

        #endregion

        #region Private Methods
        List<clsIPDBedReservationVO> List = new List<clsIPDBedReservationVO>();
        private void FilterList()
        {
            if (DataList != null)
            {
                clsGetIPDBedReservationListBizActionVO bizActionVO = new clsGetIPDBedReservationListBizActionVO();
                //if (cmbUnitAppointmentSummary.SelectedItem != null)
                //{
                //    bizActionVO.UnitID = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                //}
                if ((dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null))
                {
                    if (txtMRNo.Text == "" && txtMRNo.Text.Length == 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date) && (obj.UnitID == bizActionVO.UnitID))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }

                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                    }
                    else if (txtMRNo.Text.Length > 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date) && (string.Equals(obj.MRNo, txtMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)) && (obj.UnitID == bizActionVO.UnitID))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date) && (string.Equals(obj.MRNo, txtMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                    }
                }
                else if ((dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate == null))
                {
                    if (txtMRNo.Text == "" && txtMRNo.Text.Length == 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if (obj.UnitID == bizActionVO.UnitID)
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {

                                if (obj.UnitID >= bizActionVO.UnitID)
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                    }
                    else if (txtMRNo.Text.Length > 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {

                                if ((obj.UnitID == bizActionVO.UnitID) && (string.Equals(obj.MRNo, txtMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {

                                if (string.Equals(obj.MRNo, txtMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = List;
                        }
                    }
                }
                else
                {
                    BindGridList();
                }
                dgUnReserved.SelectedItem = null;
                dgDataPager.Source = null;
                dgDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                dgDataPager.Source = ReservedBedList;
            }
        }

        private void GetActiveAdmissionOfPatient(long PatientID, long PatientUnitID, string MRNO)
        {
            clsGetActiveAdmissionBizActionVO BizObject = new clsGetActiveAdmissionBizActionVO();
            BizObject.PatientID = PatientID;
            BizObject.PatientUnitID = PatientUnitID;
            BizObject.MRNo = MRNO;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetActiveAdmissionBizActionVO)arg.Result).Details != null && ((clsGetActiveAdmissionBizActionVO)arg.Result).Details.AdmID > 0)
                        {
                            msgText = "Patient is already admitted.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Text = string.Empty;
                            txtMRNo.Focus();

                            msgW1.Show();
                        }
                        else
                        {
                            FindPatient(PatientID, PatientUnitID, MRNO);
                        }
                    }
                }

            };

            Client.ProcessAsync(BizObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            #region OLD Code
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            // BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (!((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                        {
                            BindSelectedPatientDetails((clsGetPatientBizActionVO)arg.Result, Indicatior);
                            if (IsSearchOnList == true)
                            {
                                FilterList();
                            }
                        }
                        else
                        {
                            Indicatior.Close();

                            MessageBoxControl.MessageBoxChildWindow msgW7 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            if (IsSearchOnList == false)
                            {
                                txtMRNo.Focus();
                            }
                            else
                            {
                                txtMRNo.Focus();
                            }
                            msgW7.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

            #endregion

            #region New Code
            //clsGetIPDPatientBizActionVO BizAction = new clsGetIPDPatientBizActionVO();
            //BizAction.PatientDetails = new clsGetIPDPatientVO();
            //if (IsSearchClick == true)
            //{
            //    BizAction.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(null);
            //    BizAction.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(null);
            //}
            //else
            //{
            //    BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
            //    BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
            //}
            //BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            //BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null)
            //    {
            //        if (arg.Result != null)
            //        {
            //            patientDetails = new clsPatientGeneralVO();
            //            patientDetails = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
            //            if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
            //            {
            //                if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
            //                {
            //                    txtMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
            //                }
            //                if (patientDetails.PatientID != null)
            //                    patientDetails.PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
            //                if (patientDetails.PatientUnitID != null)
            //                    patientDetails.PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
            //                if (patientDetails.GenderID != null)
            //                    patientDetails.GenderID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.GenderID;
            //                if (patientDetails.ClassName != null)
            //                {
            //                    patientDetails.ClassName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
            //                }
            //                //Comman.SetPatientDetailHeader(PatientVO.PatientDetails);
            //                //Indicatior.Close();
            //                if (IsViewClick == true)
            //                {
            //                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = patientDetails.PatientID;
            //                    ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = patientDetails.PatientUnitID;

            //                    ModuleName = "OPDModule";
            //                    Action = "OPDModule.PatientAndVisitDetails";
            //                    UserControl rootPage = Application.Current.RootVisual as UserControl;
            //                    WebClient c = new WebClient();
            //                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            //                }
            //            }
            //            else
            //            {
            //                msgText = "Please check MR number.";
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                txtMRNo.Focus();
            //                msgW1.Show();
            //                Comman.SetDefaultHeader(_SelfMenuDetails);
            //            }
            //        }
            //    }
            //};
            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();


            #endregion
        }

        public void BindGridList()
        {
            try
            {
                clsGetIPDBedReservationListBizActionVO bizActionVO = new clsGetIPDBedReservationListBizActionVO();

                if (IsSearchOnList == true)
                {
                    if (txtMRNo.Text != "")
                    {
                        bizActionVO.MRNo = txtMRNo.Text.Trim();
                    }
                }

                if (dtpFromDate.SelectedDate != null)
                {
                    bizActionVO.FromDate = dtpFromDate.SelectedDate.Value.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    bizActionVO.ToDate = dtpToDate.SelectedDate.Value.Date;
                }

                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = DataList.PageSize;
                bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BedDetails = new clsIPDBedReservationVO();
                bizActionVO.BedList = new List<clsIPDBedReservationVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = (((clsGetIPDBedReservationListBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            DataList.TotalItemCount = (int)(((clsGetIPDBedReservationListBizActionVO)args.Result).TotalRows);
                            DataList.Clear();
                            objBedReservationList = new List<clsIPDBedReservationVO>();
                            foreach (clsIPDBedReservationVO item in bizActionVO.BedList)
                            {
                                DataList.Add(item);
                            }
                            dgUnReserved.ItemsSource = null;
                            dgUnReserved.ItemsSource = DataList;
                            dgUnReserved.SelectedItem = null;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            dgDataPager.Source = ReservedBedList;
                        }
                        else
                        {
                            dgUnReserved.ItemsSource = null;
                            dgDataPager.Source = null;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void BindSelectedPatientDetails(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            if (IsSearchOnList == true)
            {
                txtMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;                       
                lblName.Visibility = Visibility;
                lblPatientName.Visibility = Visibility;
                lblPatientName.Text = PatientVO.PatientDetails.GeneralDetails.PatientName;
                lblgender.Visibility = Visibility;
                lblPatientgender.Visibility = Visibility;
                if (PatientVO.PatientDetails.GenderID == 1)
                    lblPatientgender.Text = "Male";
                else if (PatientVO.PatientDetails.GenderID == 2)
                    lblPatientgender.Text = "Female";
                lblregdate.Visibility = Visibility;
                lblPatientregdate.Visibility = Visibility;
                //lblPatientregdate.Text = Convert.ToString(PatientVO.PatientDetails.GeneralDetails.RegistrationDate);
                String Str = Convert.ToString(PatientVO.PatientDetails.GeneralDetails.RegistrationDate);
                lblPatientregdate.Text = Str.Remove(Str.Length - 11);

                BindGridList();
            }
            else
            {
                if (PatientVO.PatientDetails.GeneralDetails.MRNo != null)
                    txtMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            }
            patientDetails = new clsPatientGeneralVO();
            patientDetails.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
            patientDetails.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;
            patientDetails.GenderID = PatientVO.PatientDetails.GeneralDetails.GenderID;
            patientDetails.MRNo = PatientVO.PatientDetails.GeneralDetails.MRNo;
            Comman.SetPatientDetailHeader(PatientVO.PatientDetails);
            Indicatior.Close();
            if (IsViewClick == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = patientDetails.PatientID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = patientDetails.PatientUnitID;
                ModuleName = "OPDModule";
                Action = "OPDModule.PatientAndVisitDetails";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                IsViewClick = true;
            }
        }

        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;

                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.All.ToString());
                }

                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                if (IsViewClick == false)
                {
                    cw.Closed += new EventHandler(cw_Closed);
                }
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool Checkvalidation()
        {
            bool result = true;

            if (!DataList.Where(S => S.UnResStatus.Equals(true)).Count().Equals(0))
            {
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void ClearControl()
        {
            patientDetails = new clsPatientGeneralVO();
            txtMRNo.Text = string.Empty;
            lblName.Visibility = Visibility.Collapsed;
            lblgender.Visibility = Visibility.Collapsed;
            lblPatientgender.Visibility = Visibility.Collapsed;
            lblPatientName.Visibility = Visibility.Collapsed;
            lblPatientregdate.Visibility = Visibility.Collapsed;
            lblregdate.Visibility = Visibility.Collapsed;
        }

        public void Save()
        {
            try
            {
                clsAddIPDBedUnReservationBizActionVO BizActionVO = new clsAddIPDBedUnReservationBizActionVO();
                BizActionVO.BedUnResDetails = new clsIPDBedUnReservationVO();
                BizActionVO.BedUnResDetails.BedList = new List<clsIPDBedReservationVO>();
                DataList = ((PagedSortableCollectionView<clsIPDBedReservationVO>)dgUnReserved.ItemsSource);
                //BizActionVO.BedUnResDetails.BedList = DataList.Where(S => S.UnResStatus.Equals(true)).ToList();

                BizActionVO.BedUnResDetails.BedList = SelectedBedListForUnReserve;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddIPDBedUnReservationBizActionVO)args.Result).SuccessStatus == 0)
                        {
                            msgText = "Bed unreserved successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    BindGridList();
                                    ClearControl();
                                }
                            };
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                    else
                    {
                        msgText = "Error Occurred While undeserving Bed.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void cw_Closed(object sender, EventArgs e)
        {
            //if ((bool)((PalashDynamics.IPD.IPDPatientSearch)sender).DialogResult)
            //{
            //    GetSelectedPatientDetails();
            //}
            //else
            //{
            //    txtMRNo.Text = string.Empty;
            //    if (IsFromAdmission == false)
            //    {
            //        Comman.SetDefaultHeader(_SelfMenuDetails);
            //    }
            //}
            if ((bool)((OPDModule.Forms.PatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
            }
            else
            {
                txtMRNo.Text = string.Empty;
                if (IsFromAdmission == false)
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
            }
        }
        #endregion


        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            
        }


        private void cmdUnReserve_Checked(object sender, RoutedEventArgs e)
        {
            if (Checkvalidation())
            {
                string msgTitle = "";
                msgText = "Do You Want To Un-reserve Selected Bed/Beds?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
               {
                   if (res == MessageBoxResult.Yes)
                   {
                       Save();
                   }
               };
               msgW.Show();
            }
            else
            {
                string msgTitle = "";
                msgText = "Please select bed from the list.";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            clsIPDBedReservationVO obj = (clsIPDBedReservationVO)dgUnReserved.SelectedItem;
            if (chk.IsChecked == true)
            {
                foreach (var item in DataList.ToList())
                {
                    if (item.ID == obj.ID)
                    {
                        item.IsEnabled = true;
                        SelectedBedListForUnReserve.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in DataList.ToList())
                {
                    if (item.ID == obj.ID)
                    {
                        item.IsEnabled = false;
                        item.UnResRemark = "";
                        SelectedBedListForUnReserve.Remove(item);
                    }
                }
            }
        }

        private void txtMRNo_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
