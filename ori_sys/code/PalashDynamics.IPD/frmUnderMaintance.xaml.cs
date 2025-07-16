using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.IO;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
//using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
//using OPDModule.Forms;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.ValueObjects.IPD;
using System.ComponentModel;
using System.Windows.Media;

namespace PalashDynamics.IPD
{
    public partial class frmUnderMaintance : UserControl
    {
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        bool IsPageLoded = false;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedTransferVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedTransferVO> DataListForTransfer { get; private set; }
        public PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO> DataList1 { get; private set; }
        private List<clsIPDBedMasterVO> NonCensusBedList = new List<clsIPDBedMasterVO>();
        private List<clsIPDBedMasterVO> CensusBedList = new List<clsIPDBedMasterVO>();

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

        public int PageSize1
        {
            get
            {
                return DataList1.PageSize;
            }
            set
            {
                if (value == DataList1.PageSize) return;
                DataList1.PageSize = value;
                OnPropertyChanged("PageSize1");
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
        DateTime CurrentDate;
        public frmUnderMaintance()
        {
            InitializeComponent();
            //DataList1 = new PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO>();
            //DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //PageSize1 = 15;
            //dataGridDischargedBedListPager.PageSize = PageSize1;
            //dataGridDischargedBedListPager.Source = DataList1;
            //dgDischargedBedList.ItemsSource = DataList1;
            //GetBedUMList();
        }

        void frmBedUnderMaintenance_Loaded(object sender, RoutedEventArgs e)
        {
            //
            if (!IsPageLoded)
            {
                DataList1 = new PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO>();
                DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                PageSize1 = 15;
                dataGridDischargedBedListPager.PageSize = PageSize1;
                dataGridDischargedBedListPager.Source = DataList1;
                dgDischargedBedList.ItemsSource = DataList1;
                GetBedUMList();

                FillClass();
                DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                PageSize1 = 15;
                dataGridDischargedBedListPager.PageSize = PageSize1;
                dataGridDischargedBedListPager.Source = DataList1;
                dgDischargedBedList.ItemsSource = DataList1;
                cmdOk.IsEnabled = false;
                MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                PageSizeNonCensus = 10;
                MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
                MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                PageSize = 10;

            }
            IsPageLoded = true;
             FillTabControl();
             //TabBedUnder.SelectionChanged += new SelectionChangedEventHandler(TabBedUnder_SelectionChanged);
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetBedUMList();
        }


        clsIPDBedUnderMaintenanceVO bedUnderRDetail = null;
        public void GetBedUMList()
        {
            try
            {
                clsGetReleaseBedUnderMaintenanceListBizActionVO bizActionVO = new clsGetReleaseBedUnderMaintenanceListBizActionVO();
                bizActionVO.BedUnderMDetails = new clsIPDBedUnderMaintenanceVO();
                bizActionVO.BedUnderMDetails.PagingEnabled = true;
                bizActionVO.BedUnderMDetails.MaximumRows = DataList1.PageSize;
                bizActionVO.BedUnderMDetails.StartRowIndex = DataList1.PageIndex * DataList1.PageSize;
                bedUnderRDetail = new clsIPDBedUnderMaintenanceVO();
                bizActionVO.BedUnderMList = new List<clsIPDBedUnderMaintenanceVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedUnderMList = (((clsGetReleaseBedUnderMaintenanceListBizActionVO)args.Result).BedUnderMList);
                        if (bizActionVO.BedUnderMList.Count > 0)
                        {
                            DataList1.TotalItemCount = (int)(((clsGetReleaseBedUnderMaintenanceListBizActionVO)args.Result).BedUnderMDetails.TotalRows);
                            DataList1.Clear();
                            foreach (clsIPDBedUnderMaintenanceVO item in bizActionVO.BedUnderMList)
                            {
                                DataList1.Add(item);
                            }
                            dgDischargedBedList.ItemsSource = null;
                            dgDischargedBedList.ItemsSource = DataList1;

                            dataGridDischargedBedListPager.Source = null;
                            dataGridDischargedBedListPager.PageSize = Convert.ToInt32(bizActionVO.BedUnderMDetails.MaximumRows);
                            dataGridDischargedBedListPager.Source = DataList;
                        }
                        else
                        {
                            dgDischargedBedList.ItemsSource = null;
                            dataGridDischargedBedListPager.Source = null;
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


        #region Fill Combo box

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
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        #endregion

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
                            MasterList.TotalItemCount = Convert.ToInt32(bizActionVO.TotalRows);
                            MasterList.Clear();
                            CensusBedList = bizActionVO.objBedMasterDetails;
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }

                            dgVacantBed.ItemsSource = null;
                            dgVacantBed.ItemsSource = MasterList;

                            dataGridCensusPager.Source = null;
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
                            MasterNonCensusList.TotalItemCount = Convert.ToInt32(bizActionVO.TotalRows);
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


        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            //PageSize = 15;
            //dataGridCensusPager.PageSize = PageSize;
            //dataGridCensusPager.Source = MasterList;
            //dgVacantBed.ItemsSource = MasterList;

            //MasterNonCensusList.Clear();
            //dataGridNonCensusPager.Source = MasterNonCensusList;
            //dgNonCensusBed.ItemsSource = MasterNonCensusList;
            cmbWard.IsEnabled = true;
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            FillWard();
            FillCensus();
            FillNonCensus();
            cmdOk.IsEnabled = false;
        }

        private void cmbWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillCensus();
            FillNonCensus();
            cmdOk.IsEnabled = false;

        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCensus();
        }

        void MasterNonCensusList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillNonCensus();
        }

        string msgText;
        clsIPDBedUnderMaintenanceVO BedUnderMDetails = null;
        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (IsVacantBed == true)
            {
                if (checkedBed())
                {
                    if (CheckValidation())
                    {
                        string msgTitle = "";
                        msgText = "Are you sure you want to save ?";
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
                }
                else
                {
                    string msgTitle = "";
                    msgText = "Please select at least one bed  to release.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                }
            }
            else if (IsVacantBed == false)
            {
                if (checkReleaseBedValidation())
                {
                    string msgTitle = "";
                    msgText = "Are you sure you want to release bed ?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveBedRelease();
                        }
                    };
                    msgW.Show();
                }
            }
        }

        private bool checkReleaseBedValidation()
        {
            bool result = true;

            if (dgDischargedBedList.ItemsSource != null)
            {
                var BedList = ((PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO>)dgDischargedBedList.ItemsSource).Where(S => S.IsReleased.Equals(true)).ToList();
                if (BedList.Count > 0)
                {

                }
                else
                {
                    result = false;
                    string msgTitle = "";
                    msgText = "Please select at least one bed  to release.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                }
            }
            return result;
        }

        private bool CheckValidation()
        {
            bool result = true;

            if (dtpDate.SelectedDate == null)
            {
                result = false;
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                result = false;
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Gray);
            }

            if (dtpExpectedReleaseDate.SelectedDate == null)
            {
                result = false;
                dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (dtpExpectedReleaseDate.SelectedDate.Value.Date < dtpDate.SelectedDate.Value.Date)
            {
                result = false;
                dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Gray);
            }

            if (string.IsNullOrEmpty(txtRemark.Text))
            {
                result = false;
                txtRemark.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                txtRemark.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
            if (dtpDate.SelectedDate > dtpExpectedReleaseDate.SelectedDate)
            {
                dtpDate.SetValidation("Date should not be Greater Than Excepted Release Date");
                dtpDate.RaiseValidationError();
                dtpDate.Focus();
                result = false;
            }
            return result;
        }

        private bool checkedBed()
        {
            bool result = true;
            List<clsIPDBedMasterVO> ToBedIDCensus = null;
            List<clsIPDBedMasterVO> ToBedIDNonCensus = null;
            if (MasterList != null)
                ToBedIDCensus = MasterList.Where(S => S.Status.Equals(true)).ToList();
            if (MasterNonCensusList != null)
                ToBedIDNonCensus = MasterNonCensusList.Where(S => S.Status.Equals(true)).ToList();

            if (MasterList == null && MasterNonCensusList == null)
            {
                result = false;
            }
            else if (ToBedIDCensus.Count() == 0 && ToBedIDNonCensus.Count() == 0)
            {
                result = false;
            }
            return result;
        }


        public void SaveBedRelease()
        {
            var BedList = ((PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO>)dgDischargedBedList.ItemsSource).Where(S => S.IsReleased.Equals(true)).ToList();
            if (BedList.Count > 0)
            {
                clsAddUpdateReleaseBedUnderMaintenanceBizActionVO bizActionVO = new clsAddUpdateReleaseBedUnderMaintenanceBizActionVO();
                bizActionVO.BedUnderMDetails = new clsIPDBedUnderMaintenanceVO();
                bizActionVO.BedUnderMDetails.BedUnderMList = new List<clsIPDBedUnderMaintenanceVO>();
                //DataList1 = ((PagedSortableCollectionView<clsIPDBedUnderMaintenanceVO>)dgDischargedBedList.ItemsSource);
                bizActionVO.BedUnderMDetails.BedUnderMList = BedList;

                bizActionVO.BedUnderMDetails.ReleasedDate = DateTime.Now;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        msgText = "Bed released successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                GetBedUMList();
                            }
                        };
                        msgW.Show();
                    }
                    else
                    {
                        string msgTitle = "";

                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            else
            {
                string msgTitle = "";
                msgText = "Please select at least one bed  to release.";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        public void Save()
        {
            try
            {
                clsAddBedUnderMaintenanceBizActionVO bizActionVO = new clsAddBedUnderMaintenanceBizActionVO();
                bizActionVO.BedUnderMDetails = new clsIPDBedUnderMaintenanceVO();

                if (dtpDate.SelectedDate != null)
                {
                    bizActionVO.BedUnderMDetails.FromDate = dtpDate.SelectedDate;
                }

                if (!string.IsNullOrEmpty(txtRemark.Text))
                {
                    bizActionVO.BedUnderMDetails.Remark = txtRemark.Text.Trim();
                }

                if (dtpExpectedReleaseDate.SelectedDate != null)
                {
                    bizActionVO.BedUnderMDetails.ExpectedReleasedDate = dtpExpectedReleaseDate.SelectedDate;
                }
                bizActionVO.BedUnderMDetails.BedUnderMList = new List<clsIPDBedUnderMaintenanceVO>();
                if (MasterList != null)
                {
                    foreach (var obj in MasterList)
                    {
                        if (obj.Status == true)
                        {
                            clsIPDBedUnderMaintenanceVO objItem = new clsIPDBedUnderMaintenanceVO();
                            objItem.BedID = obj.ID;
                            objItem.BedUnitID = obj.UnitID;
                            objItem.IsUnderMaintanence = true;
                            bizActionVO.BedUnderMDetails.BedUnderMList.Add(objItem);
                        }
                    }
                }
                if (MasterNonCensusList != null)
                {
                    foreach (var obj in MasterNonCensusList)
                    {
                        if (obj.Status == true)
                        {
                            clsIPDBedUnderMaintenanceVO objItem = new clsIPDBedUnderMaintenanceVO();
                            objItem.BedID = obj.ID;
                            objItem.BedUnitID = obj.UnitID;
                            objItem.IsUnderMaintanence = true;
                            bizActionVO.BedUnderMDetails.BedUnderMList.Add(objItem);
                        }
                    }
                }

                bizActionVO.BedUnderMDetails.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.BedUnderMDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.BedUnderMDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                bizActionVO.BedUnderMDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                bizActionVO.BedUnderMDetails.AddedDateTime = System.DateTime.Now;
                bizActionVO.BedUnderMDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string msgTitle = "";
                        msgText = "Record saved successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        ClearControl();
                        GetBedUMList();

                        //Added by Cds
                        FillCensus();
                        FillNonCensus();
                    }
                    else
                    {
                        string msgTitle = "";
                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
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

        private void ClearControl()
        {
            dgVacantBed.ItemsSource = null;
            dataGridCensusPager.Source = null;
            dgNonCensusBed.ItemsSource = null;
            dataGridNonCensusPager.Source = null;
            dataGridCensusPager.Source = null;
            if (cmbClassName.ItemsSource != null)
                cmbClassName.SelectedItem = ((List<MasterListItem>)cmbClassName.ItemsSource)[0];
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            txtRemark.Text = String.Empty;
            dtpExpectedReleaseDate.SelectedDate = null;
        }

        bool IsVacantBed;
        private void TabBedUnder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPageLoded)
            {
                TabControl objTab = sender as TabControl;

                if (objTab.SelectedIndex == 0)
                {
                    IsVacantBed = true;

                    //Added by cds               
                    FillCensus();
                    FillNonCensus();

                }
                else
                {
                    IsVacantBed = false;
                }
                if (tabReleaseBed != null)
                {
                    if (tabReleaseBed.IsSelected)
                    {
                        GetBedUMList();
                        cmdOk.IsEnabled = false;
                    }
                }
            }
        }

        public void FillTabControl()
        {
            if (IsPageLoded)
            {
                //TabControl objTab = sender as TabControl;

                if (TabBedUnder.SelectedIndex == 0)
                {
                    IsVacantBed = true;

                    //Added by cds               
                    FillCensus();
                    FillNonCensus();

                }
                else
                {
                    IsVacantBed = false;
                }
                if (tabReleaseBed != null)
                {
                    if (tabReleaseBed.IsSelected)
                    {
                        GetBedUMList();
                        cmdOk.IsEnabled = false;
                    }
                }
            }
        }
        private void dtpDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Red);
                string msgTitle = "";
                msgText = "Date should be greater than current date.";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                msgW.Show();
                dtpDate.Focus();
            }
            else
            {
                dtpDate.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
        }

        private void dtpExpectedReleaseDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpExpectedReleaseDate.SelectedDate != null)
            {
                if (dtpExpectedReleaseDate.SelectedDate.Value.Date < dtpDate.SelectedDate.Value.Date)
                {
                    dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    string msgTitle = "";
                    msgText = "Expected Date should be greater than or equal to date.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    dtpExpectedReleaseDate.Focus();
                }
            }
            else if (dtpExpectedReleaseDate.SelectedDate != null)
            {
                if (dtpExpectedReleaseDate.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    string msgTitle = "";
                    msgText = "Expected Date should be greater than or equal to date.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    dtpExpectedReleaseDate.Focus();
                }
            }
            else
            {
                dtpExpectedReleaseDate.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
        }



        private void chkStatusCensus_Click(object sender, RoutedEventArgs e)
        {
            if (tabVacaentBedList != null)
            {
                if (tabVacaentBedList.IsSelected)
                {
                    if (tabCensusBedList != null)
                    {
                        if (tabCensusBedList.IsSelected)
                        {
                            var UnionLBedList = MasterList.Union(MasterNonCensusList);
                            var item = from r in UnionLBedList.ToList()
                                       where r.Status == true
                                       select r;
                            item.Where(x => x.Status = true);
                            if (item != null && item.ToList().Count > 0)
                            {
                                cmdOk.IsEnabled = true;
                            }
                            else
                            {
                                cmdOk.IsEnabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void chkStatusNonCensus_Click(object sender, RoutedEventArgs e)
        {
            if (tabVacaentBedList != null)
            {
                if (tabVacaentBedList.IsSelected)
                {
                    if (tabNonCensusBedList != null)
                    {
                        if (tabNonCensusBedList.IsSelected)
                        {
                            var UnionLBedList = MasterList.Union(MasterNonCensusList);
                            var item = from r in UnionLBedList.ToList()
                                       where r.Status == true
                                       select r;
                            item.Where(x => x.Status = true);
                            if (item != null && item.ToList().Count > 0)
                            {
                                cmdOk.IsEnabled = true;
                            }
                            else
                            {
                                cmdOk.IsEnabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void chkBedReleased_Click(object sender, RoutedEventArgs e)
        {
            if (tabReleaseBed != null)
            {
                if (tabReleaseBed.IsSelected)
                {
                    var item = from r in DataList1
                               where r.IsReleased == true
                               select r;
                    item.Where(x => x.Status = true);
                    if (item != null && item.ToList().Count > 0)
                    {
                        cmdOk.IsEnabled = true;
                    }
                    else
                    {
                        cmdOk.IsEnabled = false;
                    }
                }
            }
        }
    }
}
