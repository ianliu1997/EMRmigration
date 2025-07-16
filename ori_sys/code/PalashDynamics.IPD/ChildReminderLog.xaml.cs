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
    public partial class ChildReminderLog : ChildWindow
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
        public long PatientID = 0;
        public long UnitID = 0;
        public clsIPDBedReservationVO details { get; set; }
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

        public ChildReminderLog()
        {
            InitializeComponent();
                    }

        private void BindGrid()
        {
            try
            {
                clsAddIPDBedReservationBizActionVO bizActionVO = new clsAddIPDBedReservationBizActionVO();
                bizActionVO.BedDetails = new clsIPDBedReservationVO();
                
                bizActionVO.BedDetails.IsFromReminderLogForGet = true;
                bizActionVO.BedDetails.PatientID = PatientID;
                bizActionVO.BedDetails.UnitID = UnitID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<clsIPDBedReservationVO> List = new List<clsIPDBedReservationVO>();
                        List = (((clsAddIPDBedReservationBizActionVO)args.Result).LogList);
                        if (List != null)
                        {
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                        else
                        {
                            dgReserved.ItemsSource = null;
                           
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
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }
    }
}

