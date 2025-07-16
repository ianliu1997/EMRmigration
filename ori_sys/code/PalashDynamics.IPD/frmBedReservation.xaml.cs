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

    public partial class BedReservation : UserControl
    {
       
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        #region Variables

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
        string PatientName, Bedcode;    
        DateTime frmDt;
        DateTime todt;
        bool IsViewClick = false;
        bool IsSearchClick = false;
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

        #region Refresh Event

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCensus();
        }

        void MasterNonCensusList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillNonCensus();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            BindGridList();
        }

        #endregion
        #region Fill Combo And List
        public void FillCensus()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.IsNonCensus = false;
                //bizActionVO.BedDetails.IsForReservation = true;  -- commented by Ashish- Reason-Showing the allocated Bed.
                
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

                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }

                            dgVacantBed.ItemsSource = null;
                            dgVacantBed.ItemsSource = MasterList;

                            dataGridCensusPager.Source = null;
                            dataGridCensusPager.PageSize = PageSize;
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
                //bizActionVO.BedDetails.IsForReservation = true;  -- commented by Ashish- Reason-Showing the allocated Bed.
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
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterNonCensusList.Add(item);
                            }

                            dgNonCensusBed.ItemsSource = null;
                            dgNonCensusBed.ItemsSource = MasterNonCensusList;
                            dgNonCensusBed.SelectedIndex = -1;
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
                ///Indicatior.Close();
                // throw;
            }
        }

        private void FillWard()
        {
            try
            {

                WaitIndicator Wait = new WaitIndicator();
                Wait.Show();
                clsGetIPDWardByClassIDBizActionVO BizAction = new clsGetIPDWardByClassIDBizActionVO();
                BizAction.BedDetails = new clsIPDBedTransferVO();
                if (cmbClassName.SelectedItem != null && ((MasterListItem)cmbClassName.SelectedItem).ID != 0)
                {
                    BizAction.BedDetails.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
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
                    Wait.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                // throw;
            }
        }

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
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                // throw;
            }
        }
        public void BindGridList()
        {
            try
            {
                clsGetIPDBedReservationListBizActionVO bizActionVO = new clsGetIPDBedReservationListBizActionVO();

                if (IsSearchOnList == true)
                {
                    if (txtBTListMRNo.Text != "")
                    {
                        bizActionVO.MRNo = txtBTListMRNo.Text.Trim();
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
                txtBTListMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
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
                ////lblPatientregdate.Text = Convert.ToString(PatientVO.PatientDetails.GeneralDetails.RegistrationDate);
                //YourString = YourString.Remove(YourString.Length - 1);
                String Str= Convert.ToString(PatientVO.PatientDetails.GeneralDetails.RegistrationDate);
                lblPatientregdate.Text = Str.Remove(Str.Length - 11);
            }
            else
            {
                if (PatientVO.PatientDetails.GeneralDetails.MRNo != null)
                {
                    txtBTListMRNo.Text = PatientVO.PatientDetails.GeneralDetails.MRNo;
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
                    lblPatientregdate.Text = Convert.ToString(PatientVO.PatientDetails.GeneralDetails.RegistrationDate);

                }
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
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Inventory Configuration";
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
                lblgender.Visibility = Visibility.Collapsed;
                lblName.Visibility = Visibility.Collapsed;
                lblPatientgender.Visibility = Visibility.Collapsed;
                lblPatientName.Visibility = Visibility.Collapsed;
                lblPatientregdate.Visibility = Visibility.Collapsed;
                lblregdate.Visibility = Visibility.Collapsed;
                if (IsFromAdmission == false)
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
            }
        }
        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                string MRNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                //if (IsSearchOnList == true)
                //{
                //    FindPatient(PatientID, UnitId, null);
                //}
                //else
                {
                    GetActiveAdmissionOfPatient(PatientID, UnitId, MRNo);
                }
            }
        }
        #endregion
        #region Constructor,Load,Clear and BindGrid

        void frmBedReservation_Loaded(object sender, RoutedEventArgs e)
        {
            cmdNew.Visibility = Visibility.Visible;
            cmdOK.Visibility = Visibility.Collapsed;
            FillClass();
            FillUnit();
            DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSizeData = 15;
            dgDataPager.PageSize = PageSizeData;
            dgDataPager.Source = DataList;
            dgReserved.ItemsSource = DataList;
            FromDate.SelectedDate = DateTime.Now;
            ToDate.SelectedDate = DateTime.Now;
            MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            dataGridCensusPager.PageSize = PageSize;
            dataGridCensusPager.Source = MasterList;
            dgVacantBed.ItemsSource = MasterList;
            BindGridList();
           // dtpFromDate.SelectedDate = DateTime.Now;
           // dtpToDate.SelectedDate = DateTime.Now;
            
            MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
            MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
            PageSizeNonCensus = 15;
            dataGridNonCensusPager.PageSize = PageSizeNonCensus;
            dataGridNonCensusPager.Source = MasterNonCensusList;
            dgNonCensusBed.ItemsSource = MasterNonCensusList;
            txtMRNo.Text = string.Empty;



            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.IsBedReservation == true)
                {
                    cmdNew.Visibility = Visibility.Collapsed;
                    cmdOK.Visibility = Visibility.Visible;
                    IsFromAdmission = ((IApplicationConfiguration)App.Current).SelectedPatient.IsBedReservation;
                    _flip.Invoke(RotationType.Backward);
                }
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            }
            if (dgReserved != null)
            {
                if (dgReserved.Columns.Count > 0)
                {
                    dgReserved.Columns[0].Header = "MR No";
                    dgReserved.Columns[1].Header = " Patient Name";
                    dgReserved.Columns[2].Header = "From Date";
                    dgReserved.Columns[3].Header = "To Date";
                    dgReserved.Columns[4].Header = "Bed No";
                    dgReserved.Columns[5].Header = "Ward Name";
                    dgReserved.Columns[6].Header = "Class Name";
                    dgReserved.Columns[7].Header = "Remark";
                }
            }

            if (dgVacantBed != null)
            {
                if (dgVacantBed.Columns.Count > 0)
                {
                    dgVacantBed.Columns[1].Header = "Code";
                    dgVacantBed.Columns[2].Header = " Bed";
                    dgVacantBed.Columns[3].Header = "Class";
                    dgVacantBed.Columns[4].Header = "Ward";
                    dgVacantBed.Columns[5].Header = "Facilities";
                }
            }

            if (dgNonCensusBed != null)
            {
                if (dgNonCensusBed.Columns.Count > 0)
                {
                    dgNonCensusBed.Columns[1].Header = "Code";
                    dgNonCensusBed.Columns[2].Header = " Bed";
                    dgNonCensusBed.Columns[3].Header = "Class";
                    dgNonCensusBed.Columns[4].Header = "Ward";
                    dgNonCensusBed.Columns[5].Header = "Facilities";
                }
            }
        }
        public BedReservation()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmBedReservation_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(BedReservationLayoutRoot, PatientReservationLayoutRoot, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            DataList = new PagedSortableCollectionView<clsIPDBedReservationVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        }
        private void ClearControl()
        {
            txtMRNo.Text = string.Empty;
            txtRemark.Text = string.Empty;
            if (cmbClassName.ItemsSource != null)
                cmbClassName.SelectedItem = ((List<MasterListItem>)cmbClassName.ItemsSource)[0];
            txtRemark.Text = string.Empty;
            txtBTListMRNo.Text = string.Empty;            
            ToDate.SelectedDate = DateTime.Now;
            FromDate.SelectedDate = DateTime.Now;
            lblName.Visibility = Visibility.Collapsed;
            lblgender.Visibility = Visibility.Collapsed;
            lblPatientgender.Visibility = Visibility.Collapsed;
            lblPatientName.Visibility = Visibility.Collapsed;
            lblPatientregdate.Visibility = Visibility.Collapsed;
            lblregdate.Visibility = Visibility.Collapsed;
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            Comman.SetDefaultHeader(_SelfMenuDetails);
        }
        private bool IsReserveDateValidate(long BedID, DateTime FromDate, DateTime ToDate)
        {
            if (DataList != null)
            {
                var objBed = (from S in DataList
                              where S.BedID == BedID
                              &&
                                (S.FromDate.Value.Date == FromDate.Date && S.ToDate.Value.Date == ToDate.Date ||
                                S.FromDate.Value.Date <= FromDate.Date && S.ToDate.Value.Date >= FromDate.Date ||
                                S.FromDate.Value.Date <=ToDate.Date && S.ToDate.Value.Date >=ToDate.Date
                                //S.FromDate.Value.Date >= FromDate.Date && S.FromDate.Value.Date >= ToDate.Date 
                                //S.FromDate.Value.Date >= FromDate.Date && S.ToDate.Value.Date >= ToDate.Date ||
                                //S.FromDate.Value.Date >= FromDate.Date && S.ToDate.Value.Date <= ToDate.Date
                                )
                              select S.PatientName).ToList();
               
                
                if (objBed.Count().Equals(0))
                {
                    return true;                    
                }
                else
                {
                    PatientName = objBed[0];
                    return false;
                }

            }
            return true;
        }
        private bool IsPatientReserved(string MRNo)
        {
            var objpatient = (from S in DataList
                              where S.MRNo == MRNo
                              select new { S.BedCode, S.FromDate , S.ToDate,S.BedNo }).ToArray();

            if (objpatient.Count().Equals(0))
            {
                return true;
            }
            else
            {
                Bedcode = objpatient.FirstOrDefault().BedNo;               
                frmDt = (DateTime)objpatient.FirstOrDefault().FromDate;                
                todt = (DateTime)objpatient.FirstOrDefault().ToDate;
                return false;
            }
        }
        #endregion

        #region SelectionChanged

        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbWard.IsEnabled = true;
            if (cmbWard.ItemsSource != null)
                cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource)[0];
            FillWard();
            FillCensus();
            FillNonCensus();
        }

        private void cmbWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsForSelectEvent == false)
            {
                FillCensus();
                FillNonCensus();
            }
            else
            {
                IsForSelectEvent = false;
            }
        }

        #endregion

        #region reserve Button
        string msgText;
        private void cmdReserve_Click(object sender, RoutedEventArgs e)
        {            
            if (patientDetails != null)
            {
                if (patientDetails.MRNo == txtMRNo.Text.Trim())
                {
                    if (Checkvalidation())
                    {
                        clsIPDBedMasterVO ToBedIDCensus = MasterList.SingleOrDefault(S => S.Status.Equals(true));
                        clsIPDBedMasterVO ToBedIDNonCensus = MasterNonCensusList.SingleOrDefault(S => S.Status.Equals(true));
                        clsIPDBedReservationVO objBed = new clsIPDBedReservationVO();

                        if (ToBedIDCensus != null)
                        {
                            objBed.BedID = ToBedIDCensus.ID;
                        }
                        else if (ToBedIDNonCensus != null)
                        {
                            objBed.BedID = ToBedIDNonCensus.ID;
                        }
                        if (!objBed.BedID.Equals(0))
                        {
                            if (IsPatientReserved(patientDetails.MRNo))
                            {
                                if (IsReserveDateValidate(objBed.BedID, (DateTime)FromDate.SelectedDate, (DateTime)ToDate.SelectedDate))
                                {
                                    msgText = "Are you sure you want to save ?";
                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                                    msgW.Show();
                                }
                                else
                                {
                                    msgText = "Bed is already reserved for " + PatientName + " in this date range.";
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    txtMRNo.Focus();
                                    msgW1.Show();
                                }
                            }
                            else
                            {
                                msgText = "This patient is already reserved '" + Bedcode + "' from " + frmDt.ToShortDateString() + " to " + todt.ToShortDateString();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                txtMRNo.Focus();
                                msgW1.Show();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    string msgTitle = "";
                    msgText = "Please enter valid MRNo";
                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                }
            }
            else
            {
                string msgTitle = "";
                msgText = "Please select patient.";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWin.Show();
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }
        public bool Checkvalidation()
        {
            bool result = true;
            if (string.IsNullOrEmpty(txtMRNo.Text))
            {
                string msgTitle = "Palash";
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
                result = false;
            }
            else
            {
                if (MasterList.SingleOrDefault(S => S.Status.Equals(true)) != null)
                {
                }
                else if (MasterNonCensusList.SingleOrDefault(S => S.Status.Equals(true)) != null)
                {
                }
                else
                {
                    string msgTitle = "Palash";
                    msgText = "Please select bed from bed list.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    result = false;
                    return result;
                }
                if (FromDate.SelectedDate == null || ToDate.SelectedDate == null)
                {
                    if (FromDate.SelectedDate == null)
                    {
                        FromDate.SelectedDate = null;
                        msgText = "Please select from date";
                        FromDate.SetValidation(msgText);
                        FromDate.RaiseValidationError();
                        result = false;
                    }
                    if (ToDate.SelectedDate == null)
                    {
                        ToDate.SelectedDate = null;
                        msgText = "Please select to date";
                        ToDate.SetValidation(msgText);
                        ToDate.RaiseValidationError();
                        result = false;
                        msgText = "Please select from date and to date.";
                    }

                    if (!string.IsNullOrEmpty(msgText))
                    {
                        string msgTitle = "Palash";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        result = false;
                        return result;
                    }
                }
                else
                {
                    string msgText = string.Empty;
                    if (FromDate.SelectedDate.Value.Date > ToDate.SelectedDate.Value.Date)
                    {
                        ToDate.SelectedDate = null;
                        ToDate.SetValidation("");
                        ToDate.RaiseValidationError();
                        result = false;
                        msgText = " To date can not be less than from date";
                    }
                    else if (FromDate.SelectedDate.Value.Date < DateTime.Now.Date || ToDate.SelectedDate.Value.Date < DateTime.Now.Date)
                    {
                        if (FromDate.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            FromDate.SelectedDate = null;
                            FromDate.SetValidation("");
                            FromDate.RaiseValidationError();
                            result = false;
                        }
                        if (ToDate.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            ToDate.SelectedDate = null;
                            ToDate.SetValidation("");
                            ToDate.RaiseValidationError();
                            result = false;
                        }
                        msgText = "From date and to date can not be less than current date.";
                    }

                    if (!string.IsNullOrEmpty(msgText))
                    {
                        string msgTitle = "Palash";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        result = false;
                        return result;
                    }
                    else
                    {
                        FromDate.ClearValidationError();
                        ToDate.ClearValidationError();
                    }
                }
            }
            return result;
        }

        public void Save()
        {
            try
            {
                this.DailogResult = true;
                clsAddIPDBedReservationBizActionVO bizActionVO = new clsAddIPDBedReservationBizActionVO();
                bizActionVO.BedDetails = new clsIPDBedReservationVO();
                bizActionVO.BedDetails.PatientID = patientDetails.PatientID;
                bizActionVO.BedDetails.PatientUnitID = patientDetails.PatientUnitID;
                if (ToDate.SelectedDate != null)
                {
                    bizActionVO.BedDetails.ToDate = ToDate.SelectedDate;
                }
                if (FromDate.SelectedDate != null)
                {
                    bizActionVO.BedDetails.FromDate = FromDate.SelectedDate;
                }
                if (txtRemark.Text != null)
                {
                    bizActionVO.BedDetails.Remark = txtRemark.Text.Trim();
                }
                clsIPDBedMasterVO ToBedIDCensus = MasterList.SingleOrDefault(S => S.Status.Equals(true));
                clsIPDBedMasterVO ToBedIDNonCensus = MasterNonCensusList.SingleOrDefault(S => S.Status.Equals(true));
                if (ToBedIDCensus != null)
                {
                    bizActionVO.BedDetails.BedID = ToBedIDCensus.ID;
                    bizActionVO.BedDetails.BedUnitID = ToBedIDCensus.UnitID;
                }
                else if (ToBedIDNonCensus != null)
                {
                    bizActionVO.BedDetails.BedID = ToBedIDNonCensus.ID;
                    bizActionVO.BedDetails.BedUnitID = ToBedIDNonCensus.UnitID;
                }
                bizActionVO.BedDetails.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.BedDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizActionVO.BedDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                bizActionVO.BedDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                bizActionVO.BedDetails.AddedDateTime = System.DateTime.Now;
                bizActionVO.BedDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        msgText = "Bed reserved successfully";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        patientDetails = new clsPatientGeneralVO();
                        _flip.Invoke(RotationType.Backward);
                        dtpFromDate.SelectedDate = null;
                        ClearControl();
                        BindGridList();
                    }
                    else
                    {
                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Check and Uncheck Event

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
                                    if (((clsIPDBedMasterVO)dgVacantBed.SelectedItem).GenderID == patientDetails.GenderID)
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
                    if (cmbWard.ItemsSource != null)
                    {
                        if (((MasterListItem)cmbWard.SelectedItem).ID == 0 || item.WardID != ((MasterListItem)cmbWard.SelectedItem).ID)
                        {
                            IsForSelectEvent = true;
                            //cmbWard.SelectedValue = item.WardID;
                        }
                        item.Status = true;
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
                        cmbWard.SelectedValue = 0;
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
            if (cmbWard.ItemsSource != null)
            {
                cmbWard.SelectedValue = 0;
            }
        }

        #endregion

        #region Button Click

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            _flip.Invoke(RotationType.Forward);
            //ClearControl();
            FillCensus();
            FillNonCensus();
            ClearControl();
        }
        public bool DailogResult = false;
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromAdmission == true)
            {
                this.DailogResult = false;
                try
                {
                    OnSaveButton_Click(this, new RoutedEventArgs());
                    ((ChildWindow)(this.Parent)).Close();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                //frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                //((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Admission List";
            }
        }
        private void btnPatientSearch(object sender, RoutedEventArgs e)
        {
            //IsSearchOnList = false;
            if (!string.IsNullOrEmpty(txtMRNo.Text))
            {
                IsViewClick = false;
                IsSearchClick = true;
                //GetPatientData();
                GetActiveAdmissionOfPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW6 = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW6.Show();
                lblName.Visibility = Visibility.Collapsed;
                lblgender.Visibility = Visibility.Collapsed;
                lblPatientgender.Visibility = Visibility.Collapsed;
                lblPatientName.Visibility = Visibility.Collapsed;
                lblPatientregdate.Visibility = Visibility.Collapsed;
                lblregdate.Visibility = Visibility.Collapsed;
                if (IsFromAdmission == false)
                {
                    Comman.SetDefaultHeader(_SelfMenuDetails);
                }
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
                            lblName.Visibility = Visibility.Collapsed;
                            lblgender.Visibility = Visibility.Collapsed;
                            lblPatientgender.Visibility = Visibility.Collapsed;
                            lblPatientName.Visibility = Visibility.Collapsed;
                            lblPatientregdate.Visibility = Visibility.Collapsed;
                            lblregdate.Visibility = Visibility.Collapsed;
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
            #region OLDCode

            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.IsFromSearchWindow = true;
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
                                txtBTListMRNo.Focus();
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

            #region NotworkingCode

            /*
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
                        if (!((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                        {
                            if (((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo != null)
                            {
                                txtMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                txtBTListMRNo.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                                lblName.Visibility = Visibility;
                                lblPatientName.Text = ((clsGetIPDPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.IPDPatientName;

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
                        else
                        {
                            //Indicatior.Close();
                            msgText = "Please check MR number.";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            txtMRNo.Focus();
                            msgW1.Show();
                            Comman.SetDefaultHeader(_SelfMenuDetails);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            */
            #endregion
        }
        private void FilterList()
        {
            List<clsIPDBedReservationVO> List = new List<clsIPDBedReservationVO>();
            if (DataList != null)
            {
                clsGetIPDBedReservationListBizActionVO bizActionVO = new clsGetIPDBedReservationListBizActionVO();
                if (cmbUnitAppointmentSummary.SelectedItem != null)
                {
                    bizActionVO.UnitID = ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID;
                }
                if ((dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null))
                {
                    if (txtBTListMRNo.Text == "" && txtBTListMRNo.Text.Length == 0)
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
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
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
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                    }
                    else if (txtBTListMRNo.Text.Length > 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date) && (string.Equals(obj.MRNo, txtBTListMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)) && (obj.UnitID == bizActionVO.UnitID))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {
                                if ((dtpFromDate.SelectedDate.Value.Date <= obj.FromDate.Value.Date && dtpToDate.SelectedDate.Value.Date >= obj.ToDate.Value.Date) && (string.Equals(obj.MRNo, txtBTListMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                    }
                }
                else if ((dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate == null))
                {
                    if (txtBTListMRNo.Text == "" && txtBTListMRNo.Text.Length == 0)
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
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
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
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                    }
                    else if (txtBTListMRNo.Text.Length > 0)
                    {
                        if (bizActionVO.UnitID > 0)
                        {
                            foreach (var obj in DataList)
                            {

                                if ((obj.UnitID == bizActionVO.UnitID) && (string.Equals(obj.MRNo, txtBTListMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                        else if (bizActionVO.UnitID == 0)
                        {
                            foreach (var obj in DataList)
                            {

                                if (string.Equals(obj.MRNo, txtBTListMRNo.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    List.Add(obj);
                                }
                            }
                            dgReserved.ItemsSource = null;
                            dgReserved.ItemsSource = List;
                        }
                    }
                }
                else
                {
                    BindGridList();
                }
                dgReserved.SelectedItem = null;
                dgDataPager.Source = null;
                dgDataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                dgDataPager.Source = MasterNonCensusList;
            }
        }
        
        private List<clsIPDBedMasterVO> objBedMaster = new List<clsIPDBedMasterVO>();
        #endregion
        private void cmdShow_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            if (!string.IsNullOrEmpty(txtBTListMRNo.Text))
                IsViewClick = false;                           
            BindGridList();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            _flip.Invoke(RotationType.Backward);
            txtBTListMRNo.Text = string.Empty;
            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
            if (cmbUnitAppointmentSummary.ItemsSource != null)
            {
                cmbUnitAppointmentSummary.SelectedItem = ((List<MasterListItem>)cmbUnitAppointmentSummary.ItemsSource)[0];
            }
            //FilterList();
            BindGridList();
            Comman.SetDefaultHeader(_SelfMenuDetails);
        }

        private void cmbView_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = false;
            if (txtMRNo.Text.Length != 0)
            {
                IsViewClick = true;
                GetActiveAdmissionOfPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
                //FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW5 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW5.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void cmbPatientSearch_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientSearch";
            ClearControl();
            FillCensus();
            FillNonCensus();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            cmbClassName.SelectedItem = ((List<MasterListItem>)cmbClassName.ItemsSource).Where(z => z.ID == 0).FirstOrDefault();
            cmbWard.SelectedItem = ((List<MasterListItem>)cmbWard.ItemsSource).Where(z => z.ID == 0).FirstOrDefault(); ;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dgReserved.SelectedItem != null && ((clsIPDBedReservationVO)dgReserved.SelectedItem).ID != 0)
            {
                this.DailogResult = true;
                clsIPDBedReservationVO obj = (clsIPDBedReservationVO)dgReserved.SelectedItem;
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo = ((clsIPDBedReservationVO)dgReserved.SelectedItem).MRNo;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).PatientUnitID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.BedID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).BedID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).ClassID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.WardID = ((clsIPDBedReservationVO)dgReserved.SelectedItem).WardID;
                ((IApplicationConfiguration)App.Current).SelectedPatient.IsBedReservation = true;
                OnSaveButton_Click(this, new RoutedEventArgs());
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                try
                {
                    ((ChildWindow)(this.Parent)).Close();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                msgText = "Please select bed details";
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtMRNo.Focus();
                msgW4.Show();
            }
        }

        bool IsSearchOnList = false;

        private void btn1PatientSearch(object sender, RoutedEventArgs e)
        {
            //IsSearchOnList = true;
            //if (!string.IsNullOrEmpty(txtBTListMRNo.Text))
            //{
            //    IsViewClick = false;
            //    FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtBTListMRNo.Text.Trim());
            //}
            //else
            //{
            //    msgText = "Please enter M.R. Number.";
            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    txtBTListMRNo.Focus();
            //    msgW3.Show();
            //    if (IsFromAdmission == false)//When Comes From Admission Form
            //    {
            //        Comman.SetDefaultHeader(_SelfMenuDetails);
            //    }
            //}
        }

        private void cmb1View_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            if (txtBTListMRNo.Text.Length != 0)
            {
                IsViewClick = true;
                FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtBTListMRNo.Text.Trim());
            }
            else
            {
                msgText = "Please enter M.R. Number.";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                txtBTListMRNo.Focus();
                msgW1.Show();
                Comman.SetDefaultHeader(_SelfMenuDetails);
            }
        }

        private void cmb1PatientSearch_Click(object sender, RoutedEventArgs e)
        {
            //IsSearchOnList = true;
            //ModuleName = "PalashDynamics.IPD";
            //Action = "PalashDynamics.IPD.IPDPatientSearch";
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            IsSearchOnList = true;
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientSearch";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        private void txtMRNo_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtMRNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && txtMRNo.Text.Length != 0)
            {
                string mrno = txtMRNo.Text;
                GetActiveAdmissionOfPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNo.Text.Trim());
            }
            else
            {

            }
        }

    }
}
