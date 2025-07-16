/* Added By SUDHIR PATIL on 03/March/2014 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using System.Windows.Data;
using PalashDynamics.IPD.Forms;
using PalashDynamics.ValueObjects.IPD;


namespace PalashDynamics.IPD
{
    public partial class frmBedTransfer : UserControl, INotifyPropertyChanged
    {
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedTransferVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedTransferVO> DataListForTransfer { get; private set; }
        bool IsForSelectEvent = false;
        private List<clsIPDBedMasterVO> CensusBedList = new List<clsIPDBedMasterVO>();
        private List<clsIPDBedMasterVO> NonCensusBedList = new List<clsIPDBedMasterVO>();
        public List<clsIPDBedTransferVO> objBedTransferList = null;
        clsMenuVO _SelfMenuDetails = null;

        public long PatientID { get; set; }
        public long UnitID = 0;
        public long IPDAdmissionID = 0;
        public string IPDAdmissionNo = String.Empty;
        private long PatientUnitID = 0, BedID = 0;
        private string MRNO = null;
        private bool FromBedStatus = false;
        public string PatientName {get; set;}
        public void PreInitiate(ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
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

        #region Properties

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

        public int PageSizeTransfer
        {
            get
            {
                return DataListForTransfer.PageSize;
            }
            set
            {
                if (value == DataListForTransfer.PageSize) return;
                DataListForTransfer.PageSize = value;
                OnPropertyChanged("PageSizeTransfer");
            }
        }

        

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

        public int PageSizeNonCensus
        {
            get
            {
                return MasterNonCensusList.PageSize;
            }
            set
            {
                if (value == MasterNonCensusList.PageSize) return;
                MasterNonCensusList.PageSize = value;
                OnPropertyChanged("PageSizeNonCensus");
            }
        }

        public frmBedTransfer()
        {
            InitializeComponent();
            FillBed();
            this.Loaded += new RoutedEventHandler(frmBedTransfer_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(BedTransferLayoutRoot, PatientTransferLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            FromBedStatus = true;
        }
        #endregion

        void frmBedTransfer_Loaded(object sender, RoutedEventArgs e)
        {
            FillClass();
            FillUnit();
            dtpDate.SelectedDate = DateTime.Now;
            tpTime.Value = DateTime.Now.ToLocalTime();
            cmdSave.IsEnabled = false;

            DataList = new PagedSortableCollectionView<clsIPDBedTransferVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSizeData = 15;

            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;

            MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
            PageSizeNonCensus = 10;

            dgDataPager.PageSize = PageSizeData;
            dgDataPager.Source = DataList;
            dgTransfer.ItemsSource = DataList;
            DataListForTransfer = new PagedSortableCollectionView<clsIPDBedTransferVO>();
            DataListForTransfer.OnRefresh += new EventHandler<RefreshEventArgs>(DataListForTransfer_OnRefresh);
            PageSizeTransfer = 15;
            dgrdTransfer.ItemsSource = DataListForTransfer;
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

        #region Refresh Event

        /// <summary>
        /// Get called when  front panel grid refreshed
        /// </summary>
        /// <param name="sender"> grid</param>
        /// <param name="e">grid refresh</param>

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindGridList();
        }

        void DataListForTransfer_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillTransferGrid();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCensus();
        }

        void MasterNonCensusList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillNonCensus();
        }

        #endregion

        private void ClearControl()
        {
            chkKeepOriginalBed.IsChecked = false;
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            dgVacantBed.ItemsSource = null;
            dataGridCensusPager.Source = null;
            dgNonCensusBed.ItemsSource = null;
            dataGridNonCensusPager.Source = null;
            dgrdTransfer.ItemsSource = null;
            dataGridCensusPager.Source = null;
            BedDetails = new clsIPDBedTransferVO();
            txtClass.Text = string.Empty;
            txtWard.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            if (cmbClassName.ItemsSource != null)
                cmbClassName.SelectedItem = ((List<MasterListItem>)cmbClassName.ItemsSource)[0];
            if (cmbBed.ItemsSource != null)
                cmbBed.SelectedItem = ((List<MasterListItem>)cmbBed.ItemsSource)[0];

            if (cmbBillingClass.ItemsSource != null)
                cmbBillingClass.SelectedItem = ((List<MasterListItem>)cmbBillingClass.ItemsSource)[0];

            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];

            dtpDate.SelectedDate = DateTime.Now;
            tpTime.Value = DateTime.Now.ToLocalTime();
        }

        private void ClearMRNo()
        {
            txtMRNo.Text = string.Empty;
            txtBTListMRNo.Text = string.Empty;
        }

        private void cmdCloseTransferTo_Click(object sender, RoutedEventArgs e)
        {
            if (FromBedStatus)
            {
                frmBedStatus objBedStatus = new frmBedStatus();
                UIElement myData = objBedStatus;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                ////frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                ////((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                ////UserControl rootPage = Application.Current.RootVisual as UserControl;
                ////TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                ////mElement.Text = "Admission List";
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            _flip.Invoke(RotationType.Forward);
            ClearControl();
        }

        #region Patient Search Buttons Click

        bool IsSearchClick = false;
        private void CmdBTLPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            IsViewClick = false;
            IsSearchClick = true;
            ModuleName = "PalashDynamics.IPD";
            Action = "PalashDynamics.IPD.IPDPatientSearch";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        private void btnPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearControl();
            IsViewClick = false;
            IsSearchClick = false;
            #region Commented By CDS For Only Get Addmitted Patient List
            //ModuleName = "PalashDynamics.IPD";
            //Action = "PalashDynamics.IPD.IPDPatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            #endregion
            IPDPatientSearch frm = new IPDPatientSearch();
            frm.IsFromDischarge = true;
            if (IsViewClick == false)
            {
                frm.Closed += new EventHandler(cw_Closed);
            }
            frm.Show();
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
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.IPD.ToString());
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

        void cw_Closed(object sender, EventArgs e)
        {
            if ((bool)((PalashDynamics.IPD.IPDPatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
            }
            else
            {
                txtMRNo.Text = "";
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionID;
                IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedPatient.IPDAdmissionNo;
                FindPatient(PatientID, UnitID, null);

                // For Paging these Lines are added After Patient Selected from the Patient List On the Same Form 
                MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;

                MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
                PageSizeNonCensus = 10;
            }
        }
        public bool IsTrasnfer = false;
        clsGetIPDPatientBizActionVO PatientVO;
        string msgText;
        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            clsGetIPDPatientBizActionVO BizAction = new clsGetIPDPatientBizActionVO();
            BizAction.PatientDetails = new clsGetIPDPatientVO();
            if (IsSearchClick == true)
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = Convert.ToInt64(null);
                BizAction.PatientDetails.GeneralDetails.UnitId = Convert.ToInt64(null);
            }
            else
            {
                BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
                BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
            }
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            BizAction.PatientDetails.GeneralDetails.IsIPD = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        patientDetails = new clsPatientGeneralVO();
                        patientDetails = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                        if (patientDetails.IsDischarged == true)
                        {
                            msgText = "' " + patientDetails.IPDPatientName + " '" + "Already Discharged.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                   // FillTransferGrid();
                                }
                            };
                            msgW1.Show();
                        }
                        else
                        {
                            if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                            {                                                                
                                //Added By Bhushan
                                PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                                UnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                                PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientName;
                                IPDAdmissionID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionID;
                                IPDAdmissionNo = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDAdmissionNo;
                                PatientName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName.ToString();
                                if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
                                {
                                    txtMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                    txtBTListMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                    cmdSave.IsEnabled = true;
                                }
                                if (patientDetails.PatientID != null)
                                    patientDetails.PatientID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                                if (patientDetails.PatientUnitID != null)
                                    patientDetails.PatientUnitID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientUnitID;
                                if (patientDetails.GenderID != null)
                                    patientDetails.GenderID = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.GenderID;
                                if (patientDetails.ClassName != null)
                                {
                                    patientDetails.ClassName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                    txtClass.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.ClassName;
                                }
                                if (patientDetails.WardName != null)
                                {
                                    txtWard.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                    patientDetails.WardName = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.WardName;
                                }
                                if (patientDetails.BedID != null)
                                {
                                    int BedId = Convert.ToInt32(((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.BedID);
                                    cmbBed.SelectedItem = ((List<MasterListItem>)cmbBed.ItemsSource).Where(z => z.ID == BedId).FirstOrDefault();
                                }
                                if (PatientName != null)
                                {
                                    lblPatientName.Text = PatientName;
                                }
                                FillTransferGrid();
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
                                }
                            }
                            else
                            {
                                msgText = "Please check MR number.";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                txtMRNo.Focus();
                                msgW1.Show();
                                Comman.SetDefaultHeader(_SelfMenuDetails);
                            }
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void BindSelectedPatientDetails(clsGetIPDPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            txtMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            txtBTListMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
            patientDetails = new clsPatientGeneralVO();
            patientDetails.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
            patientDetails.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;
            patientDetails.GenderID = PatientVO.PatientDetails.GeneralDetails.GenderID;
            if (IsSearchClick.Equals(false))
            {
                FillTransferGrid();
            }
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
            }
        }

        private void CmddischargePatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            dataGridCensusPager.Source = null;
            if (txtMRNo.Text.Length != 0)
            {
                ClearControl();
                IsViewClick = false;
                IsSearchClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
                MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;
            }
            else
            {
                ClearControl();
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        clsPatientGeneralVO patientDetails = null;
        bool IsViewClick;
        private void cmdPastDetails_Click(object sender, RoutedEventArgs e)
        {
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

        private void CmdBTLSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtBTListMRNo.Text.Length != 0)
            {
                IsViewClick = false;
                IsSearchClick = true;
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

        #endregion

        #region Fill ComboBox

        private void FillUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- All -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbUnitAppointmentSummary.ItemsSource = null;
                    cmbUnitAppointmentSummary.ItemsSource = objList;

                    cmbUnitAppointmentSummary.SelectedValue = objList[0].ID;

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void FillClass()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbClassName.ItemsSource = null;
                    cmbClassName.ItemsSource = objList;

                    cmbClassName.SelectedValue = objList[0].ID;

                    cmbBillingClass.ItemsSource = null;
                    cmbBillingClass.ItemsSource = objList;

                    cmbBillingClass.SelectedValue = objList[0].ID;
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void FillWard()
        {
            try
            {
                clsGetIPDWardByClassIDBizActionVO BizAction = new clsGetIPDWardByClassIDBizActionVO();
                BizAction.BedDetails = new clsIPDBedTransferVO();
                BizAction.BedDetails.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetIPDWardByClassIDBizActionVO objClass = ((clsGetIPDWardByClassIDBizActionVO)e.Result);
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        if (objClass != null)
                        {
                            if (objClass.BedList != null)
                            {
                                foreach (var item in objClass.BedList)
                                {
                                    objList.Add(new MasterListItem(item.WardID, item.Ward));
                                }
                                cmbWard.ItemsSource = null;
                                cmbWard.ItemsSource = objList;
                                cmbWard.SelectedValue = objList[0].ID;
                            }
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }
        List<MasterListItem> objList = new List<MasterListItem>();
        private void FillBed()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BedMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbBed.ItemsSource = null;
                    cmbBed.ItemsSource = objList;

                    cmbBed.SelectedValue = objList[0].ID;

                    cmbBed.ItemsSource = null;
                    cmbBed.ItemsSource = objList;

                    cmbBed.SelectedValue = objList[0].ID;
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Fill Census and NonCensus Bed List

        /// <summary>
        /// Fills Census grid
        /// </summary>
        public void FillCensus()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.IsNonCensus = false;
                if (cmbClassName.SelectedItem != null && ((MasterListItem)cmbClassName.SelectedItem).ID != 0)
                {
                    bizActionVO.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
                if (cmbWard.SelectedItem != null && ((MasterListItem)cmbWard.SelectedItem).ID != 0)
                {
                    bizActionVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                }
                bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                bizActionVO.PagingEnabled = true;                
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionVO.objBedMasterDetails = new List<clsIPDBedMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objBedMasterDetails = (((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).objBedMasterDetails);
                        if (bizActionVO.objBedMasterDetails.Count > 0)
                        {
                            MasterList.TotalItemCount = Convert.ToInt32(((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).TotalRows);
                            MasterList.Clear();
                            CensusBedList = bizActionVO.objBedMasterDetails;
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            dgVacantBed.ItemsSource = null;
                            dgVacantBed.ItemsSource = MasterList;
                            dataGridCensusPager.Source = null;
                            //dataGridCensusPager.PageSize = PageSize;
                            dataGridCensusPager.Source = MasterList;
                        }
                        else
                        {
                            dgVacantBed.ItemsSource = null;
                            dataGridCensusPager.Source = null;
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

        /// <summary>
        /// Fills NonCensus grid
        /// </summary>
        public void FillNonCensus()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.IsNonCensus = true;
                if (cmbClassName.SelectedItem != null && ((MasterListItem)cmbClassName.SelectedItem).ID != 0)
                {
                    bizActionVO.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
                if (cmbWard.SelectedItem != null && ((MasterListItem)cmbWard.SelectedItem).ID != 0)
                {
                    bizActionVO.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;
                }
                bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                bizActionVO.PagingEnabled = true;                               
                bizActionVO.MaximumRows = MasterNonCensusList.PageSize;
                bizActionVO.StartRowIndex = MasterNonCensusList.PageIndex * MasterNonCensusList.PageSize;
                bizActionVO.objBedMasterDetails = new List<clsIPDBedMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objBedMasterDetails = (((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).objBedMasterDetails);
                        if (bizActionVO.objBedMasterDetails.Count > 0)
                        {
                            MasterNonCensusList.TotalItemCount = Convert.ToInt32(((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).TotalRows);
                            MasterNonCensusList.Clear();
                            NonCensusBedList = bizActionVO.objBedMasterDetails;
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterNonCensusList.Add(item);
                            }
                            dgNonCensusBed.ItemsSource = null;
                            dgNonCensusBed.ItemsSource = MasterNonCensusList;

                            dataGridNonCensusPager.Source = null;
                            dataGridNonCensusPager.PageSize = PageSizeNonCensus;
                            dataGridNonCensusPager.Source = MasterNonCensusList;
                        }
                        else
                        {
                            dgNonCensusBed.ItemsSource = null;
                            dataGridNonCensusPager.Source = null;
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

        private void chkStatusCensus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox objCheckBox = (CheckBox)sender;

            if (objCheckBox.Tag != null)
            {
                if ((bool)objCheckBox.IsChecked)
                {
                    if (dgVacantBed.SelectedItem != null)
                    {
                        if (patientDetails != null)
                        {
                            if (patientDetails.GenderID > 0)
                            {
                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.FemaleTitle == patientDetails.GenderID)
                                {
                                    if (((clsIPDBedMasterVO)dgVacantBed.SelectedItem).GenderID == 0)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                                    }
                                    else if (((clsIPDBedMasterVO)dgVacantBed.SelectedItem).GenderID == patientDetails.GenderID)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                                    }
                                    else
                                    {
                                        long tag = (long)objCheckBox.Tag;
                                        MasterList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                        msgText = "Please select ward with respect to gender";
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                    }
                                }
                                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaleTitle == patientDetails.GenderID)
                                {
                                    if (((clsIPDBedMasterVO)dgVacantBed.SelectedItem).GenderID == 0)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                                    }
                                    else if (((clsIPDBedMasterVO)dgVacantBed.SelectedItem).GenderID == patientDetails.GenderID)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                                    }
                                    else
                                    {
                                        long tag = (long)objCheckBox.Tag;
                                        MasterList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                        msgText = "Please select ward with respect to gender";
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                    }
                                }
                                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.FemaleTitle == 0 || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaleTitle == 0)
                                {
                                    CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                                }
                                else
                                {
                                    long tag = (long)objCheckBox.Tag;
                                    MasterList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                    msgText = "Please select ward with respect to gender";
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            }
                            else
                            {
                                CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                            }
                        }
                        else
                        {
                            CheckCensusAndNonCensus(objCheckBox, MasterList, MasterNonCensusList);
                        }
                    }
                }
                else
                {
                    long tag = (long)objCheckBox.Tag;
                    MasterList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                    if (cmbWard.ItemsSource != null)
                    {
                        cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
                    }
                }
            }
        }

        private void CheckCensusAndNonCensus(CheckBox objCheckBox, PagedSortableCollectionView<clsIPDBedMasterVO> MasterList, PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList)
        {
            long tag = (long)objCheckBox.Tag;
            foreach (var item in MasterList)
            {
                if (item.ID == tag)
                {
                    item.Status = true;
                    if (cmbWard.ItemsSource != null)
                    {
                        if (((MasterListItem)cmbWard.SelectedItem).ID == 0 || item.WardID != ((MasterListItem)cmbWard.SelectedItem).ID)
                        {
                            IsForSelectEvent = true;
                            //cmbClassName.SelectedValue = item.BedCategoryID;
                            //cmbWard.SelectedValue = item.WardID;
                        }
                    }
                }
                else
                {
                    item.Status = false;
                }
            }
            foreach (var item in MasterNonCensusList)
            {
                item.Status = false;
            }
        }

        private void CheckCensus(CheckBox objCheckBox)
        {
            long tag = (long)objCheckBox.Tag;
            foreach (var item in MasterList)
            {
                if (item.ID == tag)
                {
                    item.Status = true;
                }
                else
                {
                    item.Status = false;
                }
            }
            foreach (var item in MasterNonCensusList)
            {
                item.Status = false;
            }
        }

        private void chkStatusCensus_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox objCheckBox = (CheckBox)sender;

            if (objCheckBox.Tag != null)
            {
                long tag = (long)objCheckBox.Tag;
                MasterList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
            }
        }

        private void chkStatusNonCensus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox objCheckBox = (CheckBox)sender;

            if (objCheckBox.Tag != null)
            {
                if ((bool)objCheckBox.IsChecked)
                {
                    if (dgNonCensusBed.SelectedItem != null)
                    {
                        if (patientDetails != null)
                        {
                            if (patientDetails.GenderID > 0)
                            {
                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.FemaleTitle == patientDetails.GenderID)
                                {
                                    if (((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).GenderID == 0)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                                    }
                                    else if (((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).GenderID == patientDetails.GenderID)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                                    }
                                    else
                                    {
                                        long tag = (long)objCheckBox.Tag;
                                        MasterNonCensusList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                        msgText = "Please select ward with respect to gender";
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                    }
                                }
                                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaleTitle == patientDetails.GenderID)
                                {
                                    if (((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).GenderID == 0)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                                    }
                                    else if (((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).GenderID == patientDetails.GenderID)
                                    {
                                        CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                                    }
                                    else
                                    {
                                        long tag = (long)objCheckBox.Tag;
                                        MasterNonCensusList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                        msgText = "Please select ward with respect to gender";
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                    }
                                }
                                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.FemaleTitle == 0 || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaleTitle == 0)
                                {
                                    CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                                }
                                else
                                {
                                    long tag = (long)objCheckBox.Tag;
                                    MasterNonCensusList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                                    msgText = "Please select ward with respect to gender";
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            }
                            else
                            {
                                CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                            }
                        }
                        else
                        {
                            CheckCensusAndNonCensus(objCheckBox, MasterNonCensusList, MasterList);
                        }
                    }
                }
                else
                {
                    long tag = (long)objCheckBox.Tag;
                    MasterNonCensusList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
                    if (cmbWard.ItemsSource != null)
                    {
                        cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
                    }
                }
            }
        }

        private void chkStatusNonCensus_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox objCheckBox = (CheckBox)sender;

            if (objCheckBox.Tag != null)
            {
                long tag = (long)objCheckBox.Tag;
                MasterNonCensusList.SingleOrDefault(S => S.ID.Equals(tag)).Status = false;
            }
        }

        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbWard.IsEnabled = true;
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            //dataGridCensusPager.PageIndex = 0;
            FillWard();
            FillCensus();
            FillNonCensus();

           
        }

        private void cmbWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FillCensus();
            //FillNonCensus();
            if (IsForSelectEvent == false)
            {
                AutoCompleteBox cmbWard1 = sender as AutoCompleteBox;
                //if(cmbWard1.Text != "- Select -")
                //{
                FillCensus();
                FillNonCensus();
            }
            else
            {
                IsForSelectEvent = false;
            }
        }

        #endregion

        clsIPDBedTransferVO BedDetails = null;
        List<MasterListItem> objBedList = null;
        public void GetTransferDetailsByPatientID()
        {
            try
            {
                clsGetIPDBedTransferListBizActionVO bizActionVO = new clsGetIPDBedTransferListBizActionVO();
                bizActionVO.PatientID = PatientID > 0 ? PatientID : patientDetails.PatientID;
                bizActionVO.PatientUnitID = PatientUnitID > 0 ? PatientUnitID : patientDetails.PatientUnitID;
                bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = DataList.PageSize;
                bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BedDetails = new clsIPDBedTransferVO();
                bizActionVO.BedList = new List<clsIPDBedTransferVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = (((clsGetIPDBedTransferListBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            objBedList = new List<MasterListItem>();
                            objBedTransferList = new List<clsIPDBedTransferVO>();
                            foreach (clsIPDBedTransferVO item in bizActionVO.BedList)
                            {
                                MasterListItem objBed = new MasterListItem();
                                if (item.IsOccupied.Equals(true))
                                {
                                    if (item.IsSecondaryBed.Equals(false))
                                    {
                                        objBed.ID = item.FromBedID;
                                        objBed.Description = item.FromBed;
                                        objBed.Status = true;
                                        if (item.ToDate == null && item.ToTime == null)
                                        {
                                            if (objBedList.SingleOrDefault(S => S.ID.Equals(objBed.ID)) == null)
                                            {
                                                objBedList.Add(objBed);
                                            }
                                        }
                                        if (item.ToBedID == 0 && item.ToDate == null && item.ToTime == null)
                                            BedDetails = item;

                                        item.ClassName = item.FromClass;
                                        item.Ward = item.FromWard;
                                    }
                                    else
                                    {
                                        objBed.ID = item.FromBedID;
                                        objBed.Description = item.FromBed;
                                        objBed.Status = false;

                                        if (item.ToDate == null && item.ToTime == null)
                                        {
                                            if (objBedList.SingleOrDefault(S => S.ID.Equals(objBed.ID)) == null)
                                                objBedList.Add(objBed);
                                        }

                                        item.ClassName = item.FromClass;
                                        item.Ward = item.FromWard;
                                    }
                                }

                                if (item.ToBedID > 0)
                                {
                                    item.TransferDate = item.ToDate;
                                }
                                else if (item.FromBedID > 0)
                                {
                                    item.TransferDate = item.FromDate;
                                }
                                objBedTransferList.Add(item);
                            }

                            txtClass.Text = BedDetails.ClassName;
                            txtWard.Text = BedDetails.Ward;

                            cmbBed.ItemsSource = null;
                            cmbBed.ItemsSource = objBedList;

                            if (FromBedStatus)
                                cmbBed.SelectedValue = BedID;
                            else
                                cmbBed.SelectedValue = BedDetails.FromBedID;
                        }
                        else
                        {
                            txtClass.Text = string.Empty;
                            txtWard.Text = string.Empty;
                            //  cmbBed.ItemsSource = null;
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

        public void FillTransferGrid()
        {
            try
            {
                clsGetIPDBedTransferListBizActionVO bizActionVO = new clsGetIPDBedTransferListBizActionVO();
                bizActionVO.PatientID = PatientID > 0 ? PatientID : patientDetails.PatientID;
                bizActionVO.PatientUnitID = PatientUnitID > 0 ? PatientUnitID : patientDetails.UnitId;
                //bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                PatientID = bizActionVO.PatientID;
                bizActionVO.IsSelectedPatient = true;
                BedDetails = new clsIPDBedTransferVO();
                bizActionVO.BedList = new List<clsIPDBedTransferVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = (((clsGetIPDBedTransferListBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            DataListForTransfer.TotalItemCount = (int)(((clsGetIPDBedTransferListBizActionVO)args.Result).TotalRows);
                            DataListForTransfer.Clear();
                            foreach (clsIPDBedTransferVO item in bizActionVO.BedList)
                            {
                                DataListForTransfer.Add(item);
                            }
                            dgrdTransfer.ItemsSource = null;
                            dgrdTransfer.ItemsSource = DataListForTransfer;
                            foreach (var item in DataListForTransfer)
                            {
                                if (item.ToDate == Convert.ToDateTime("01/01/0001 12:00:00 AM") && item.ToTime == Convert.ToDateTime("01/01/0001 12:00:00 AM"))
                                {
                                    item.ToDate = null;
                                    item.ToTime = null;
                                }
                            }
                            FillCensus();
                            FillNonCensus();
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

        public void CheckFinalBillbyPatientID(clsGetIPDPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            try
            {
                clsGetIPDBedTransferListBizActionVO bizActionVO = new clsGetIPDBedTransferListBizActionVO();
                bizActionVO.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
                bizActionVO.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;
                BedDetails = new clsIPDBedTransferVO();
                bizActionVO.IsCheckFinalBill = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetIPDBedTransferListBizActionVO)args.Result).BedDetails != null)
                        {
                            clsIPDBedTransferVO objBed = ((clsGetIPDBedTransferListBizActionVO)args.Result).BedDetails;
                            if (objBed.InterORFinal.Equals(true) || objBed.ISDischarged.Equals(true) || objBed.IsClosed.Equals(true) || objBed.IsCancel.Equals(true))
                            {
                                string strMsg = string.Empty;
                                if (objBed.InterORFinal.Equals(true))
                                {
                                    strMsg = " Final bill already prepared";
                                }
                                else if (objBed.ISDischarged.Equals(true))
                                {
                                    strMsg = " Patient already discharged";
                                }
                                else if (objBed.IsClosed.Equals(true))
                                {
                                    strMsg = "Admission closed";
                                }
                                else if (objBed.IsCancel.Equals(true))
                                {
                                    strMsg = "Admission canceled successfully.";
                                }

                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                BindSelectedPatientDetails(PatientVO, Indicatior);
                            }
                        }
                        else
                        {
                            BindSelectedPatientDetails(PatientVO, Indicatior);
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

        private void TransferPatientToBed()
        {
            clsIPDBedMasterVO obj = null;
            if (tabCensusBedList.IsSelected)
            {
                obj = ((clsIPDBedMasterVO)dgVacantBed.SelectedItem);
            }
            else if (tabNonCensusBedList.IsSelected)
            {
                obj = ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem);
            }
            clsAddIPDBedTransferBizActionVO BizAction = new clsAddIPDBedTransferBizActionVO();
            BizAction.BedDetails = new clsIPDBedTransferVO();
            BizAction.IsTransfer = true;
            BizAction.BedDetails.UnitID = UnitID;
            BizAction.BedDetails.PatientID = PatientID;
            BizAction.BedDetails.IPDAdmissionID = IPDAdmissionID;
            BizAction.BedDetails.IPDAdmissionNo = IPDAdmissionNo;
            BizAction.BedDetails.TransferDate = System.DateTime.Now;
            BizAction.BedDetails.FromDate = System.DateTime.Now;
            BizAction.BedDetails.FromTime = System.DateTime.Now;

            BizAction.BedDetails.BedCategoryID = patientDetails.ClassID;
            BizAction.BedDetails.BedID = patientDetails.BedID;
            BizAction.BedDetails.WardID = patientDetails.WardID;

            BizAction.BedDetails.ToClassID = obj.BedCategoryID;
            BizAction.BedDetails.ToWardID = obj.WardID;
            BizAction.BedDetails.ToBedID = obj.ID;
            BizAction.BedDetails.Status = obj.Status;

            if (cmbBillingClass.SelectedItem != null)
                BizAction.BedDetails.BillingToBedCategoryID = ((MasterListItem)cmbBillingClass.SelectedItem).ID;
            else
                BizAction.BedDetails.BillingToBedCategoryID = BizAction.BedDetails.BillingBedCategoryID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddIPDBedTransferBizActionVO)arg.Result).SuccessStatus == 0)
                    {
                        string msgTitle = "Palash";
                        msgText = "Bed Transferred Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                FillTransferGrid();
                                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
                            }
                        };
                        msgW.Show();
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        #region Save Data

        /// <summary>
        /// Purpose:To save bed transfer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            cmdSave.IsEnabled = false;
            if (CheckBedvalidation())
            {
                if (!string.IsNullOrEmpty(txtMRNo.Text))
                {
                    if (BedDetails != null)
                    {
                        if (tabCensusBedList != null)
                        {
                            if (tabCensusBedList.IsSelected)
                            {
                                msgText = " Do you Want to Transfer Bed " + "'" + ((MasterListItem)cmbBed.SelectedItem).Description + "'" + "\n to " + " '" + ((clsIPDBedMasterVO)dgVacantBed.SelectedItem).Description + "'" + " for " + "' " + PatientName + " '";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        TransferPatientToBed();
                                        FillTransferGrid();
                                    }
                                    else if (res == MessageBoxResult.No)
                                    {
                                        cmdSave.IsEnabled = true;
                                    }
                                };
                                msgW1.Show();
                            }
                        }
                        if (tabNonCensusBedList != null)
                        {
                            if (tabNonCensusBedList != null)
                            {
                                if (tabNonCensusBedList.IsSelected)
                                {
                                    msgText = " Do you Want to Transfer Bed " + "'" + ((MasterListItem)cmbBed.SelectedItem).Description + "'" + "\n to " + " '" + ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).Description + "'" + " for " + "' " + PatientName + "'";
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.Yes)
                                        {
                                            TransferPatientToBed();
                                            FillTransferGrid();
                                        }
                                        else if (res == MessageBoxResult.No)
                                        {
                                            cmdSave.IsEnabled = true;
                                        }
                                    };
                                    msgW1.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        msgText = "Please select patient.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
                else
                {
                    msgText = "Please enter M.R. Number.";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            //else
            //{
            //    string msgTitle = "Palash";
            //    msgText = "Please select Billing Class."; // "Please select bed from bed list.";
            //    //string msgText = "Please Select Bed from Bed List.";
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW.Show();
            //}
        }

        private void CheckBedClass()
        {
            if (BedDetails != null)
            {
                if (BedDetails.BillingBedCategoryID > 0 && cmbBillingClass.SelectedItem != null && !((MasterListItem)cmbBillingClass.SelectedItem).ID.Equals(BedDetails.BillingBedCategoryID))
                {
                    string msgTitle = "Palash";
                    msgText = "Do you want update the charges as per given Class ?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.Show();
                }
                else
                {
                    CallSave();
                }
            }
            else
            {
                CallSave();
            }
        }

        private void CallSave()
        {
            string msgTitle = "Palash";
            msgText = "Are you sure you want to save ?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.Show();
        }

        int RowCount = 0;
        ObservableCollection<clsVisitConsultationChargesVO> VisitConsultationChargesList = null;
        WaitIndicator Indicatior = null;
        ObservableCollection<clsChargeVO> PackageServiceList = null;
        ObservableCollection<clsChargeVO> HealthPlanServiceList = null;
        private void FillPendingChargesList()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();
                BizAction.PatientID = patientDetails.PatientID;
                BizAction.PatientUnitID = patientDetails.PatientUnitID;
                BizAction.IsGetPendingCharges = true;
                BizAction.Opd_Ipd = (int)PatientTypes.IPD;
                BizAction.IsForIPDBill = true;
                BizAction.IsCancel = false;
                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = 0;
                BizAction.MaximumRows = 15;
                BizAction.IsIPDCharegs = true;
                BizAction.SelfCompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                BizAction.IsForBedTransfer = true;
                BizAction.BillingClass = ((MasterListItem)cmbBillingClass.SelectedItem).ID;

                PackageServiceList = new ObservableCollection<clsChargeVO>();
                HealthPlanServiceList = new ObservableCollection<clsChargeVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {

                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> _List = (from obj in ((clsGetChargeListBizActionVO)arg.Result).List
                                                       orderby obj.ID
                                                       select obj).ToList<clsChargeVO>();


                            List<clsChargeVO> objList = new List<clsChargeVO>();
                            objList.AddRange(_List);// ((clsGetChargeListBizActionVO)arg.Result).List;
                            VisitConsultationChargesList = new ObservableCollection<clsVisitConsultationChargesVO>();
                            //_TempVisitConsultationChargesList = new ObservableCollection<clsVisitConsultationChargesVO>();
                            //TempVisitConsultationChargesList = new List<clsVisitConsultationChargesVO>();
                            foreach (var item in objList)
                            {
                                clsVisitConsultationChargesVO _Object = new clsVisitConsultationChargesVO();

                                _Object.ChargesDetails = item;
                                _Object.ChargesDetails.ActualRate = item.Rate;
                                _Object.IsSelected = true;
                                //if ((bool)rdbAgainstBill.IsChecked)
                                //{
                                //    _Object.ChargesDetails.IsBillOptionEnable = "Visible";
                                //    _Object.ChargesDetails.IsServiceOptionEnable = "Collapsed";
                                //}
                                //else if ((bool)rdbAgainstServices.IsChecked)
                                //{
                                //    _Object.ChargesDetails.IsBillOptionEnable = "Collapsed";
                                //    _Object.ChargesDetails.IsServiceOptionEnable = "Visible";
                                //}
                                if (_Object.ChargesDetails.CompanyID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID))
                                {
                                    _Object.ChargesDetails.IsSelfCompany = true;
                                }

                                if (item.TaxMasterList != null)
                                {
                                    _Object.ChargesDetails.TaxMasterList = item.TaxMasterList;
                                }

                                if (item.RateEditable)
                                {
                                    _Object.ChargesDetails.IsRateEditableEnable = "Visible";
                                    _Object.ChargesDetails.IsRateEditableDisable = "Collapsed";
                                }
                                else if (!item.RateEditable)
                                {
                                    _Object.ChargesDetails.IsRateEditableEnable = "Collapsed";
                                    _Object.ChargesDetails.IsRateEditableDisable = "Visible";
                                }

                                double ConPer = (item.Concession * 100) / item.TotalAmount;
                                _Object.ChargesDetails.ConcessionPercent = ConPer;

                                CalculateTax(_Object);
                                bool IsPackageService = false;
                                bool IsHealthPlanService = false;
                                if (item.ChargeIDPackage > 0)
                                {
                                    if (((clsGetChargeListBizActionVO)arg.Result).List.SingleOrDefault(S => S.ID.Equals(item.ChargeIDPackage)) != null)
                                    {
                                        IsPackageService = ((clsGetChargeListBizActionVO)arg.Result).List.SingleOrDefault(S => S.ID.Equals(item.ChargeIDPackage)).IsServiceAsPackage;
                                        IsHealthPlanService = ((clsGetChargeListBizActionVO)arg.Result).List.SingleOrDefault(S => S.ID.Equals(item.ChargeIDPackage)).IsServiceAsHealthPlan;
                                        item.PackageName = ((clsGetChargeListBizActionVO)arg.Result).List.SingleOrDefault(S => S.ID.Equals(item.ChargeIDPackage)).PackageName;
                                    }
                                }

                                if (IsPackageService)
                                {
                                    BindPackageService(item);
                                }
                                else if (IsHealthPlanService)
                                {
                                    BindHealthPlanService(item);
                                }
                                else if (item.ChargeIDPackage.Equals(0))
                                {
                                    _Object.RowID = VisitConsultationChargesList.Count + 1;
                                    VisitConsultationChargesList.Add(_Object);
                                }
                            }


                            RowCount = 0;

                            SaveCharges();
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void BindPackageService(clsChargeVO item)
        {
            PackageServiceList.Add(item);

            PagedCollectionView PackageServiceListPageColl = new PagedCollectionView(PackageServiceList);
            PackageServiceListPageColl.GroupDescriptions.Add(new PropertyGroupDescription("PackageName"));
        }

        private void BindHealthPlanService(clsChargeVO ServiceUnderPackage)
        {
            HealthPlanServiceList.Add(ServiceUnderPackage);
            PagedCollectionView HealthPalnListPageColl = new PagedCollectionView(HealthPlanServiceList);
            HealthPalnListPageColl.GroupDescriptions.Add(new PropertyGroupDescription("PackageName"));
        }

        private bool IsImplantServiceAddedInBill { get; set; }
        private long ImplantChargeID { get; set; }

        private bool IsMedicineServiceAddedInBill { get; set; }
        private long MedicineChargeID { get; set; }

        private void SaveCharges()
        {
            clsAddChargeBizActionVO BizAction = new clsAddChargeBizActionVO();
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            BizAction.Details = new clsChargeVO();
            BizAction.ChargesList = GetCharge();
            BizAction.Details.Opd_Ipd = 1;

            if (BizAction.ChargesList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)).Count() > 0)
            {
                BizAction.IsPathWorkOrder = true;
            }
            BizAction.IsOPDIPD = true;
            if (BizAction.ChargesList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)).Count() > 0)
            {
                BizAction.IsRadiologyWorkOrder = true;
            }

            if (PackageServiceList != null)
            {
                if (PackageServiceList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)).Count() > 0)
                {
                    BizAction.IsPathWorkOrder = true;
                }

                if (PackageServiceList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)).Count() > 0)
                {
                    BizAction.IsRadiologyWorkOrder = true;
                }
            }

            if (BedDetails != null)
            {
                BizAction.Details.Opd_Ipd_Id = BedDetails.AdmID;
                BizAction.Details.Opd_Ipd_UnitId = BedDetails.AdmUnitID;
                BizAction.Details.Opd_Ipd = 1;
            }
            if (BizAction.ChargesList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)).Count() > 0)
            {
                BizAction.IsPathologyWorkOrder = true;
            }

            if (BizAction.ChargesList.Where(S => S.ServiceSpecilizationID.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID)).Count() > 0)
            {
                BizAction.IsRadWorkOrder = true;
            }

            BizAction.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
            BizAction.RadiologyWorkOrder.RadSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    Indicatior.Close();
                    VisitConsultationChargesList = new ObservableCollection<clsVisitConsultationChargesVO>();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Charges Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while Saving Charges.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private List<clsChargeVO> GetCharge()
        {
            List<clsChargeVO> TempList = new List<clsChargeVO>();
            foreach (clsVisitConsultationChargesVO _Obj in VisitConsultationChargesList)
            {
                if (_Obj.ChargesDetails.SubChargesList == null)
                    _Obj.ChargesDetails.SubChargesList = new List<clsChargeVO>();
                if (_Obj.ChargesDetails.IsServiceAsPackage)
                {

                    var PackageServiceCharges = PackageServiceList.Where(S => (S.ChargeIDPackage.Equals(_Obj.ChargesDetails.ID) || S.PackageID.Equals(_Obj.ChargesDetails.PackageID)));
                    foreach (var item in PackageServiceCharges)
                    {
                        _Obj.ChargesDetails.SubChargesList.Add(item);
                    }

                }
                else if (_Obj.ChargesDetails.IsServiceAsHealthPlan)
                {
                    if (_Obj.ChargesDetails.ID > 0)
                    {
                        var HealthPlanServiceCharges = HealthPlanServiceList.Where(S => S.ChargeIDPackage.Equals(_Obj.ChargesDetails.ID));
                        foreach (var item in HealthPlanServiceCharges)
                        {
                            _Obj.ChargesDetails.SubChargesList.Add(item);
                        }
                    }
                    else
                    {
                        var HealthPlanServiceCharges = HealthPlanServiceList.Where(S => S.PackageID.Equals(_Obj.ChargesDetails.PackageID) && S.RowID.Equals(Convert.ToInt16(_Obj.RowID)));
                        foreach (var item in HealthPlanServiceCharges)
                        {
                            _Obj.ChargesDetails.SubChargesList.Add(item);
                        }
                    }

                }

                _Obj.ChargesDetails.SelfBalance = _Obj.ChargesDetails.SelfAmount;
                _Obj.ChargesDetails.NonSelfBalance = _Obj.ChargesDetails.NonSelfAmount;
                if (patientDetails != null)
                {
                    _Obj.ChargesDetails.PatientID = patientDetails.PatientID;
                    _Obj.ChargesDetails.PatientUnitID = patientDetails.PatientUnitID;
                }
                if (_Obj.ChargesDetails.ServiceId.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.implantServiceID))
                {
                    IsImplantServiceAddedInBill = true;
                    ImplantChargeID = _Obj.ChargesDetails.ID;
                }
                if (_Obj.ChargesDetails.ServiceId.Equals(((IApplicationConfiguration)App.Current).ApplicationConfigurations.MedicineServiceID))
                {
                    IsMedicineServiceAddedInBill = true;
                    MedicineChargeID = _Obj.ChargesDetails.ID;
                }
                TempList.Add(_Obj.ChargesDetails);
            }
            return TempList;
        }

        public void CalculateTax(clsVisitConsultationChargesVO _VisistConsulatationCharge)
        {
            List<clsTaxBuilderVO> TaxMasterList = _VisistConsulatationCharge.ChargesDetails.TaxMasterList;
            if (TaxMasterList != null)
            {
                if (_VisistConsulatationCharge.ChargesDetails.TotalAmount > 0)
                {
                    foreach (clsTaxBuilderVO item in TaxMasterList)
                    {
                        if (item.IsSelected)
                        {
                            if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.GrossAmt))
                            {
                                if (item.TaxPercentage > 0)
                                {
                                    item.TaxAmount = _VisistConsulatationCharge.ChargesDetails.TotalAmount * (item.TaxPercentage / 100);
                                }
                                else
                                {
                                    item.TaxAmount = 0;
                                }
                            }
                            else if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.NetAmt))
                            {
                                if (item.TaxPercentage > 0)
                                {
                                    double Amt = _VisistConsulatationCharge.ChargesDetails.TotalAmount - _VisistConsulatationCharge.ChargesDetails.ConcessionAmount;
                                    item.TaxAmount = Amt * (item.TaxPercentage / 100);
                                }
                                else
                                {
                                    item.TaxAmount = 0;
                                }
                            }
                            else if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.TaxOnTax))
                            {
                                clsTaxBuilderVO DependantTaxitem = TaxMasterList.SingleOrDefault(S => S.ID.Equals(item.TaxOnTaxID));
                                if (DependantTaxitem != null)
                                {
                                    if (DependantTaxitem.TaxAmount > 0)
                                    {
                                        if (item.TaxPercentage > 0)
                                        {
                                            item.TaxAmount = DependantTaxitem.TaxAmount * (item.TaxPercentage / 100);
                                        }
                                        else
                                        {
                                            item.TaxAmount = 0;
                                        }
                                    }
                                    else
                                    {
                                        List<clsTaxBuilderVO> Template = CalculateDependableTax(_VisistConsulatationCharge, item, TaxMasterList);
                                        clsTaxBuilderVO DependTaxitem = Template.SingleOrDefault(S => S.ID.Equals(item.TaxOnTaxID));
                                        if (DependTaxitem != null)
                                        {
                                            if (DependTaxitem.TaxAmount > 0)
                                            {
                                                if (item.TaxPercentage > 0)
                                                {
                                                    item.TaxAmount = DependTaxitem.TaxAmount * (item.TaxPercentage / 100);
                                                }
                                                else
                                                {
                                                    item.TaxAmount = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _VisistConsulatationCharge.ChargesDetails.TaxAmount = TaxMasterList.Where(S => S.IsSelected.Equals(true)).Sum(S => S.TaxAmount);
                    if (_VisistConsulatationCharge.ChargesDetails.IsSelfCompany)
                    {
                        _VisistConsulatationCharge.ChargesDetails.SelfAmount = _VisistConsulatationCharge.ChargesDetails.NetAmount;
                    }
                    else
                    {
                        _VisistConsulatationCharge.ChargesDetails.NonSelfAmount = _VisistConsulatationCharge.ChargesDetails.NetAmount;
                    }
                }
            }
        }

        private List<clsTaxBuilderVO> CalculateDependableTax(clsVisitConsultationChargesVO _VisistConsulatationCharge, clsTaxBuilderVO item, List<clsTaxBuilderVO> TaxMasterList)
        {
            List<clsTaxBuilderVO> Temp = TaxMasterList;
            if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.TaxOnTax))
            {
                clsTaxBuilderVO DependantTaxitem = Temp.SingleOrDefault(S => S.ID.Equals(item.TaxOnTaxID));
                if (DependantTaxitem != null)
                {
                    if (DependantTaxitem.TaxAmount.Equals(0))
                    {
                        CalculateDependableTax(_VisistConsulatationCharge, DependantTaxitem, Temp);
                    }
                    else
                    {
                        if (item.TaxPercentage > 0)
                        {
                            item.TaxAmount = DependantTaxitem.TaxAmount * (item.TaxPercentage / 100);
                        }
                        else
                        {
                            item.TaxAmount = 0;
                        }
                    }
                }
            }

            if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.GrossAmt))
            {
                if (item.TaxPercentage > 0)
                {
                    item.TaxAmount = _VisistConsulatationCharge.ChargesDetails.TotalAmount * (item.TaxPercentage / 100);
                }
            }
            else if (item.TaxApplicableOn.Equals((long)TaxApplicableOn.NetAmt))
            {
                if (item.TaxPercentage > 0)
                {
                    double Amt = _VisistConsulatationCharge.ChargesDetails.TotalAmount - _VisistConsulatationCharge.ChargesDetails.ConcessionAmount;
                    item.TaxAmount = Amt * (item.TaxPercentage / 100);
                }
            }

            return Temp;
        }

        private bool checkDateValidation()
        {
            bool result = true;
            if (dtpDate.SelectedDate < BedDetails.AdmissionDate)
            {
                dtpDate.SelectedDate = null;
                msgText = "Date can not be less than admission date";
                dtpDate.SetValidation(msgText);
                dtpDate.RaiseValidationError();
                result = false;
            }
            else if (dtpDate.SelectedDate == null)
            {
                dtpDate.SelectedDate = null;
                msgText = "Please select the date";
                dtpDate.SetValidation(msgText);
                dtpDate.RaiseValidationError();
                result = false;
            }
            else
            {
                dtpDate.ClearValidationError();
            }

            if (tpTime.Value == null)
            {
                tpTime.BorderBrush = new SolidColorBrush(Colors.Red);
                result = false;
            }
            else
            {
                tpTime.BorderBrush = new SolidColorBrush(Colors.Gray);
            }

            return result;
        }

        public bool CheckBedvalidation()
        {
            //if (tabCensusBedList != null)
            //{
            //    if (tabCensusBedList.IsSelected == true)
            //    {
            //        clsIPDBedMasterVO obj = ((clsIPDBedMasterVO)dgVacantBed.SelectedItem);
            //        if (obj.BedCategoryName.ToUpper().Trim() == txtClass.Text.ToUpper().Trim() && obj.WardName.ToUpper().Trim() == txtWard.Text.ToUpper().Trim() && obj.Description.ToUpper().Trim() == ((MasterListItem)cmbBed.SelectedItem).Description.ToUpper().Trim())
            //        {
            //            msgText = "Bed is already Allocated.";
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                 new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    }
            //}
            //else if(tabNonCensusBedList != null)
            //{
            //    if (tabNonCensusBedList.IsSelected == true)
            //    {
            //        clsIPDBedMasterVO obj1 = ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem);
            //        if (obj1.Description.ToUpper().Trim() == txtClass.Text.ToUpper().Trim() && obj1.WardName.ToUpper().Trim() == txtWard.Text.ToUpper().Trim() && obj1.Description.ToUpper().Trim() == ((MasterListItem)cmbBed.SelectedItem).Description.ToUpper().Trim())
            //        {
            //            msgText = "Bed is already Allocated.";
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                 new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    }

            //}

            bool result = true;

            //if (cmbBillingClass.SelectedItem == null || ((MasterListItem)cmbBillingClass.SelectedItem).ID == 0)
            //{
            //    cmbBillingClass.TextBox.SetValidation("Please select Billing Class");
            //    cmbBillingClass.TextBox.RaiseValidationError();
            //    cmbBillingClass.Focus();
            //    result = false;
            //}
            //else
            //    cmbBillingClass.TextBox.ClearValidationError();

            if (result == true)
            {
                if (MasterList != null)
                {
                    if (MasterList.SingleOrDefault(S => S.Status.Equals(true)) != null)
                    {
                        result = true;
                        return result;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }

                if (MasterNonCensusList != null)
                {
                    if (MasterNonCensusList.SingleOrDefault(S => S.Status.Equals(true)) != null)
                    {
                        result = true;
                        return result;
                    }
                    else
                    {
                        msgText = "Please select bed from bed list.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        result = false;
                        cmdSave.IsEnabled = true;
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

        public void BindGridList()
        {
            try
            {
                clsGetIPDBedTransferListBizActionVO bizActionVO = new clsGetIPDBedTransferListBizActionVO();
                if (cmbUnitAppointmentSummary.SelectedItem != null)
                {
                    bizActionVO.UnitID = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                }
                if (txtBTListMRNo.Text.Length > 0)
                {
                    bizActionVO.MRNo = txtBTListMRNo.Text.Trim();
                }
                if (dtpFromDate.SelectedDate != null)
                {
                    bizActionVO.FromDate = dtpFromDate.SelectedDate;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    bizActionVO.ToDate = dtpToDate.SelectedDate;
                }
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = DataList.PageSize;
                bizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BedDetails = new clsIPDBedTransferVO();
                bizActionVO.BedList = new List<clsIPDBedTransferVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = (((clsGetIPDBedTransferListBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            DataList.TotalItemCount = (int)(((clsGetIPDBedTransferListBizActionVO)args.Result).TotalRows);
                            DataList.Clear();
                            objBedTransferList = new List<clsIPDBedTransferVO>();
                            foreach (clsIPDBedTransferVO item in bizActionVO.BedList)
                            {
                                if (item.ToBedID > 0)
                                {
                                    item.TransferDate = item.ToDate;
                                    item.TransferTime = item.ToTime;
                                }
                                else if (item.FromBedID > 0)
                                {
                                    item.TransferDate = item.FromDate;
                                    item.TransferTime = item.FromTime;
                                }
                                DataList.Add(item);
                            }
                            dgTransfer.ItemsSource = null;
                            dgTransfer.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            dgDataPager.Source = MasterNonCensusList;
                        }
                        else
                        {
                            dgTransfer.ItemsSource = null;
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

        private void cmdShow_Click(object sender, RoutedEventArgs e)
        {
            BindGridList();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            ////frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ////UserControl rootPage = Application.Current.RootVisual as UserControl;
            ////TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            ////mElement.Text = "Admission List";
            ////((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
        }

        private void dtpDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDate.SelectedDate.Value.Date < BedDetails.AdmissionDate.Value.Date)
            {
                dtpDate.SelectedDate = null;
                msgText = "Date can not be less than admission date";
                dtpDate.SetValidation(msgText);
                dtpDate.RaiseValidationError();
            }
            else if (dtpDate.SelectedDate == null)
            {
                dtpDate.SelectedDate = null;
                msgText = "Please select the date";
                dtpDate.SetValidation(msgText);
                dtpDate.RaiseValidationError();
            }
            else
            {
                dtpDate.ClearValidationError();
            }
        }

        private void cmdPrintTransferPaper_Click(object sender, RoutedEventArgs e)
        {
            if (dgTransfer.SelectedItem != null)
            {
                long AdmID = ((clsIPDBedTransferVO)dgTransfer.SelectedItem).AdmID;
                long AdmUnitID = ((clsIPDBedTransferVO)dgTransfer.SelectedItem).AdmUnitID;

                string URL = "../Reports/OPD/PatientAdvance.aspx?AdmID=" + AdmID + "&AdmUnitID=" + AdmUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void CmdBedStatus_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow BedStatus = new ChildWindow();
            frmBedStatus win = new frmBedStatus(false, false);
            BedStatus.Content = win;
            BedStatus.Height = 500;
            BedStatus.Width = 900;
            BedStatus.Title = "Bed Status";
            BedStatus.Show();
        }

        public void FillDataFromBedStatusPage()
        {
            if (DataList == null)
                DataList = new PagedSortableCollectionView<clsIPDBedTransferVO>();

            FillTransferGrid();
            GetTransferDetailsByPatientID();
            FindPatient(PatientID, PatientUnitID, MRNO);
        }

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtMRNo.Text.Length != 0)
            {
                string mrno = txtMRNo.Text;
                ClearControl();
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (txtMRNo.Text.Length <= 0)
                {
                    txtClass.Text = "";
                    txtWard.Text = "";
                    cmbBed.SelectedItem = ((List<MasterListItem>)cmbBed.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
                    lblPatientName.Text = String.Empty;
                    cmdSave.IsEnabled = false;
                }
                else
                {
                    cmdSave.IsEnabled = true;
                }
            }
            if (txtMRNo.Text.Length > 0)
                cmdSave.IsEnabled = true;
        }

        private void hlkListAllBedTrasfer_Click(object sender, RoutedEventArgs e)
        {
            frmListAllBedTransfer winListAllBedTransfer = new frmListAllBedTransfer();
            winListAllBedTransfer.Height = this.ActualHeight;
            winListAllBedTransfer.Width = this.ActualWidth * 0.6;
            winListAllBedTransfer.Show();
        }
    }
}
