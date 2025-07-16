using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IPD
{
    public partial class FrmBedDetails : ChildWindow
    {
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        private long lBedID;
        private long lBedUnitID;

        public long BedID
        {
            get { return lBedID; }
            set
            {
                lBedID = value;
            }
        }
        public long BedUnitID
        {
            get { return lBedUnitID; }
            set
            {
                lBedUnitID = value;
            }
        }
        public FrmBedDetails()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)       // Added on 22Feb2019 for Package Flow in IPD
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            FillBedDetails();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void FillBedDetails()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.BedID = this.BedID;
                bizActionVO.UnitID = this.BedUnitID;
                bizActionVO.IsBedDetails = true;
                //bizActionVO.PagingEnabled = true;
                //bizActionVO.MaximumRows = 15;
                //bizActionVO.StartRowIndex = 0;

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
                            

                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {

                                MasterList.Add(item);
                                txtPatientName.Text=item.PatientName;
                                txtMRNo.Text=item.MrNo;
                                dtpToDate.SelectedDate = item.AdmissionDate == DateTime.MinValue ? null : item.AdmissionDate;
                                txtIPDNo.Text=item.PatientIPDNo;
                            }
                            dgBedDetails.ItemsSource = null;
                            dgBedDetails.ItemsSource = MasterList;
                            dgBedDetails.SelectedIndex = -1;
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
    }
}
