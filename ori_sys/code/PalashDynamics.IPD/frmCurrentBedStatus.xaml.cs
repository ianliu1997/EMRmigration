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
namespace PalashDynamics.IPD
{
    public partial class frmCurrentBedStatus : UserControl
    {
        public event RoutedEventHandler OnPhoneButton_Click;
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        #region Variables
        string msgText;
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> FilterDataList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedReservationVO> DataListForTransfer { get; private set; }
        public List<clsIPDBedReservationVO> objBedReservationList = null;
        clsIPDBedReservationVO BedDetails = null;
        clsPatientGeneralVO patientDetails = null;               
        bool IsSearchClick = false;      
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

        

        #endregion
        private void frmCurrentBedStatus_Loaded(object sender, RoutedEventArgs e)
        {           
            DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();           
            PageSizeData = 15;
            dgDataPager.PageSize = PageSizeData;
            dgDataPager.Source = DataList;
            dgReserved.ItemsSource = DataList;
            dtpFromDate.SelectedDate = DateTime.Now.AddDays(1);
            dtpToDate.SelectedDate = null;
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();          
            PageSize = 15;
            dgDataPager.PageSize = PageSize;
            dgDataPager.Source = MasterList;
            dgReserved.ItemsSource = MasterList;
            cmdEmail.IsEnabled = false;
            cmdSms.IsEnabled = false;
            cmdReminderLog.IsEnabled = false;
            BindGridList();  
              
        }
        public frmCurrentBedStatus()
        {
            InitializeComponent();
           
            //this.Loaded += new RoutedEventHandler(frmBedReservation_Loaded);
            //_flip = new PalashDynamics.Animations.SwivelAnimation(BedReservationLayoutRoot, PatientReservationLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        }

        public void BindGridList()
        {
            try
            {
                clsGetIPDBedReservationStatusBizActionVO bizActionVO = new clsGetIPDBedReservationStatusBizActionVO();              
                if (txtBTListMRNo.Text != "")
                {
                    bizActionVO.MRNo = txtBTListMRNo.Text.Trim();
                }                
                if (txtFirstName.Text != "")
                {

                    bizActionVO.FirstName = txtFirstName.Text.Trim();
                }
                if (txtLastName.Text != "")
                {
                    bizActionVO.LastName = txtLastName.Text.Trim();
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
                        bizActionVO.BedList = (((clsGetIPDBedReservationStatusBizActionVO)args.Result).BedList);
                        if (bizActionVO.BedList.Count > 0)
                        {
                            DataList.TotalItemCount = (int)(((clsGetIPDBedReservationStatusBizActionVO)args.Result).TotalRows);
                            DataList.Clear();
                            objBedReservationList = new List<clsIPDBedReservationVO>();

                            foreach (clsIPDBedReservationVO item in bizActionVO.BedList)
                            {
                                DataList.Add(item);
                            }
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = DataList;
                            dgReserved.SelectedItem = null;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            dgDataPager.Source = MasterNonCensusList;
                        }
                        else
                        {
                            dgReserved.ItemsSource = null;
                            dgDataPager.Source = null;
                            cmdEmail.IsEnabled = false;
                            cmdSms.IsEnabled = false;
                            cmdReminderLog.IsEnabled = false;
                           
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

        private void cmdSms_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdEmail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdReminderLog_Click(object sender, RoutedEventArgs e)
        {
            ChildReminderLog chlRemLog = new ChildReminderLog();
            if (dgReserved.SelectedItem != null && ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientID != 0)
            {                
                chlRemLog.PatientID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientID;
                chlRemLog.UnitID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).UnitID;
                chlRemLog.details = ((clsIPDBedReservationVO)dgReserved.SelectedItem);
                chlRemLog.Show();
            }
            else
            {
                msgText = "Please Select The Patient";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
            }
        }

        private void cmdShow_Click(object sender, RoutedEventArgs e)
        {
            BindGridList();
        }

        private void phoneDetails_Click(object sender, RoutedEventArgs e)
        {
            ChildPhonedetails chldph=new ChildPhonedetails();
            if (dgReserved.SelectedItem != null && ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientID != 0)
            {               
                 chldph.details = ((clsIPDBedReservationVO)dgReserved.SelectedItem);                    
                 chldph.Show();
                
            }
        }

        private void dgReserved_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cmdEmail.IsEnabled = true;
            //cmdSms.IsEnabled = true;
            //cmdReminderLog.IsEnabled = true;

            cmdEmail.IsEnabled = false;
            cmdSms.IsEnabled = false;
            cmdReminderLog.IsEnabled = false;
        }
    }
}
