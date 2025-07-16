/* Added By Sudhir Patil on 05/02/2014 */
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
using System.ComponentModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Windows.Resources;
using PalashDynamics.ValueObjects;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmMultipleBedAllocation : UserControl
    {
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

        #region Variable & List Declaration
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public long PatientID = 0;
        public long UnitID = 0;
        public long IPDAdmissionID = 0;
        public bool IsValidate = true;
        public string IPDAdmissionNo = String.Empty;
        private long PatientUnitID = 0, BedID = 0;
        private string MRNO = null;
        private bool FromBedStatus = false;
        public string PatientName = null;
        clsPatientGeneralVO patientDetails = null;
        clsGetIPDPatientBizActionVO IPDPatientDetails = null;
        public string msgText = null;
        bool IsSearchClick = false;
        bool IsViewClick;
        clsMenuVO _SelfMenuDetails = null;

        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList { get; private set; }
        List<clsIPDBedMasterVO> SelectedBedList = new List<clsIPDBedMasterVO>();
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

        #endregion

        #region Constructor

        public frmMultipleBedAllocation()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded Event

        private void frmMultipleBedAllocation_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now;
            tpTime.Value = DateTime.Now.ToLocalTime();
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
            PageSizeNonCensus = 10;
            if (tabCensusBedList != null && tabCensusBedList.IsSelected)
            {
                FillCensus();
            }
            FillClass();

            //Commentedd By CDS becouse IT gives Wrong Message  with SelectedIPDPatient 

            //if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            //{
            //    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId != null)
            //    {
            //        PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
            //        UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
            //        PatientName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
            //        IPDAdmissionID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
            //        IPDAdmissionNo = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
            //        FindPatient(PatientID, UnitID, null);
            //    }
            //}
        }

        #endregion

        #region OnRefresh Events
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCensus();
        }

        void MasterNonCensusList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillNonCensus();
        }

        #endregion

        #region Button Click Events
        private void cmdCloseTransferTo_Click(object sender, RoutedEventArgs e)
        {
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";
        }

        private void cmdShow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            ////frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ////((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            ////UserControl rootPage = Application.Current.RootVisual as UserControl;
            ////TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            ////mElement.Text = "Admission List";
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                msgText = "Do you want to Allocate Multiple Beds For " + "'" + patientDetails.IPDPatientName + "'";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        SaveMultipleBed();
                    }
                };
                msgW1.Show();
            }
        }

        private void cmdPastDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdBTLSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdBedStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdBTLPatientSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region combo box Selection Changed

        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbWard.IsEnabled = true;
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            FillWard();
            if (tabCensusBedList != null && tabCensusBedList.IsSelected)
            {
                FillCensus();
            }
            else
            {
                FillNonCensus();
            }
        }

        private void cmbWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabCensusBedList != null && tabCensusBedList.IsSelected)
            {
                FillCensus();
            }
            else
            {
                FillNonCensus();
            }
        }

        #endregion

        #region Private Methods

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
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            foreach (var item in MasterList)
                            {
                                foreach (var item1 in SelectedBedList)
                                {
                                    if (item.BedID == item1.BedID)
                                    {
                                        item.Status = item1.Status;
                                    }
                                }
                            }
                            dgCensusBed.ItemsSource = null;
                            dgCensusBed.ItemsSource = MasterList;
                            dataGridCensusPager.Source = null;
                            dataGridCensusPager.PageSize = PageSize;
                            dataGridCensusPager.Source = MasterList;
                        }
                        else
                        {
                            dgCensusBed.ItemsSource = null;
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
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterNonCensusList.Add(item);
                            }
                            foreach (var item in MasterNonCensusList)
                            {
                                foreach (var item1 in SelectedBedList)
                                {
                                    if (item.BedID == item1.BedID)
                                        item.Status = item1.Status;
                                }
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

                                }
                            };
                            msgW1.Show();
                        }
                        else
                        {
                            clsPatientGeneralVO PatientInfo = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                            if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                            {
                                if (PatientInfo.MRNo != null)
                                {
                                    txtMRNo.Text = PatientInfo.MRNo;
                                    cmdSave.IsEnabled = true;
                                }
                                if (patientDetails.PatientID != null)
                                    patientDetails.PatientID = PatientInfo.PatientID;
                                if (patientDetails.PatientUnitID != null)
                                    patientDetails.PatientUnitID = PatientInfo.PatientUnitID;
                                if (patientDetails.GenderID != null)
                                    patientDetails.GenderID = PatientInfo.GenderID;
                                if (patientDetails.ClassName != null)
                                {
                                    patientDetails.ClassName = PatientInfo.ClassName;
                                }
                                if (patientDetails.WardName != null)
                                {
                                    patientDetails.WardName = PatientInfo.WardName;
                                }
                                if (patientDetails.BedID != null)
                                {
                                    int BedId = Convert.ToInt32(PatientInfo.BedID);
                                }
                                if (PatientName != null)
                                {
                                    //lblPatientName.Text = PatientName;
                                }
                                //FillTransferGrid();
                                //if (IsViewClick == true)
                                //{
                                //    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                //    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = patientDetails.PatientID;
                                //    ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = patientDetails.PatientUnitID;

                                //    ModuleName = "OPDModule";
                                //    Action = "OPDModule.PatientAndVisitDetails";
                                //    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                //    WebClient c = new WebClient();
                                //    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                                //    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                                //}
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
            }
        }

        private void SaveMultipleBed()
        {
            try
            {
                clsAddIPDBedTransferBizActionVO BizAction = new clsAddIPDBedTransferBizActionVO();
                BizAction.BedDetails = new clsIPDBedTransferVO();
                BizAction.BedList = new List<clsIPDBedTransferVO>();
                BizAction.BedDetails = new clsIPDBedTransferVO();
                BizAction.BedDetails.IsMultipleBed = true;
                foreach (var item in SelectedBedList)
                {
                    clsIPDBedTransferVO objBedTransfer = new clsIPDBedTransferVO();
                    objBedTransfer.UnitID = patientDetails.UnitId;
                    objBedTransfer.PatientID = patientDetails.PatientID;
                    objBedTransfer.PatientUnitID = patientDetails.PatientUnitID;
                    objBedTransfer.IPDAdmissionID = patientDetails.IPDAdmissionID;
                    objBedTransfer.IPDAdmissionNo = patientDetails.IPDAdmissionNo;
                    objBedTransfer.TransferDate = System.DateTime.Now;
                    objBedTransfer.FromDate = System.DateTime.Now;
                    objBedTransfer.FromTime = System.DateTime.Now;
                    objBedTransfer.BedCategoryID = item.BedCategoryID;
                    objBedTransfer.WardID = item.WardID;
                    objBedTransfer.BedID = item.BedID;
                    objBedTransfer.Status = item.Status;

                    if (cmbBillingClass.SelectedItem != null)
                        objBedTransfer.BillingToBedCategoryID = ((MasterListItem)cmbBillingClass.SelectedItem).ID;
                    else
                        objBedTransfer.BillingToBedCategoryID = BizAction.BedDetails.BedCategoryID;

                    BizAction.BedList.Add(objBedTransfer);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (tabCensusBedList != null && tabCensusBedList.IsSelected)
                        {
                            FillCensus();
                        }
                        else if(tabNonCensusBedList != null && tabNonCensusBedList.IsSelected)
                        {
                            FillNonCensus();
                        }

                        msgText = "Record Saved Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClearControl();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearControl()
        {
            txtMRNo.Text = String.Empty;
            SelectedBedList.Clear();
            cmbBillingClass.SelectedItem = ((List<MasterListItem>)cmbBillingClass.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
            txtRemarks.Text = String.Empty;
        }
        #endregion

        #region FillComboxes

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

        #endregion

        #region TabSelection Changed

        private void BedSelectionTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabCensusBedList != null && tabCensusBedList.IsSelected)
            {
                FillCensus();
            }
            else if (tabNonCensusBedList != null && tabNonCensusBedList.IsSelected)
            {
                FillNonCensus();
            }
        }
        #endregion

        #region DateTime Related Handlers

        private void dtpDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Key Up Events

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {

        }

        #endregion

        #region Find Patient

        private void ClearUI()
        {            
            SelectedBedList.Clear();
            cmbBillingClass.SelectedItem = ((List<MasterListItem>)cmbBillingClass.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
            txtRemarks.Text = String.Empty;
        }
        private void CmddischargePatientListSearch_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
            if (txtMRNo.Text.Length != 0)
            {                
                IsViewClick = false;
                IsSearchClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
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

        #endregion

        #region Check box Click Event

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            SelectedBedList = MasterList.Union(MasterNonCensusList).ToList().Where(S => S.Status.Equals(true)).ToList();
        }
        #endregion

        #region Validation

        private bool Validate()
        {
            IsValidate = true;

            if (String.IsNullOrEmpty(txtMRNo.Text))
            {
                txtMRNo.SetValidation("Please, select Patient Or Enter Valid MR Number");
                txtMRNo.RaiseValidationError();
                txtMRNo.Focus();
                IsValidate = false;
            }
            else
                txtMRNo.ClearValidationError();

            if (SelectedBedList.Count == 0)
            {
                msgText = "Please Select At Least one Bed.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                IsValidate = false;
            }

            if (cmbBillingClass.SelectedItem == null || ((MasterListItem)cmbBillingClass.SelectedItem).ID == 0)
            {
                cmbBillingClass.TextBox.SetValidation("Please select Billing Class");
                cmbBillingClass.TextBox.RaiseValidationError();
                cmbBillingClass.Focus();
                IsValidate = false;
            }
            else
                cmbBillingClass.TextBox.ClearValidationError();

            return IsValidate;
        }

        #endregion
    }
}
