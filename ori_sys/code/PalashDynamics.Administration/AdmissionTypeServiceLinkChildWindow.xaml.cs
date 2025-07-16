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
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using OPDModule;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Text;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Animations;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class AdmissionTypeServiceLinkChildWindow : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        public List<clsServiceMasterVO> SelectedServices { get; set; }

        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }

        public long PatientSourceID { get; set; }
        public long AdmissionTypeID { get; set; }
        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsServiceMasterVO> DoctorServiceDataList { get; private set; }

        public PagedSortableCollectionView<clsServiceMasterVO> ServiceList { get; private set; }
        public List<clsIPDAdmissionTypeVO> SelectedDoctorServiceList { get; set; }

        WaitIndicator Indicatior = null;

        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }

        string msgTitle = "";
        string msgText = "";

        #endregion

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
        
        public int DoctorServiceDataListPageSize
        {
            get
            {
                return DoctorServiceDataList.PageSize;
            }
            set
            {
                if (value == DoctorServiceDataList.PageSize) return;
                DoctorServiceDataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        public AdmissionTypeServiceLinkChildWindow()
        {
            InitializeComponent();
            ClassID = 1; // Default
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(AdmissionTypeServiceLinkChildWindow_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================

            //Paging
            DoctorServiceDataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DoctorServiceDataList.OnRefresh += new EventHandler<RefreshEventArgs>(DoctorServiceDataList_OnRefresh); //+= new EventHandler<RefreshEventArgs>(DoctorServiceDataList_OnRefresh);
            DoctorServiceDataListPageSize = 15;
            dgDataPager1.PageSize = DoctorServiceDataListPageSize;
            dgDataPager1.Source = DoctorServiceDataList;
            //======================================================

        }

        void DoctorServiceDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            ClinicDoctorServiceList();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //long ID = 0;
            //if ((MasterListItem)cmbTariff.SelectedItem != null)
            //    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            FetchData();
        }
        
        private void AdmissionTypeServiceLinkChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();

                //      PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                // FillTariff(true);
                FillSpecialization();
                SetComboboxValue();

                txtServiceName.Focus();
                ClinicDoctorServiceList();
                //lblClinic.Visibility = Visibility.Collapsed;
                //cmbClinic.Visibility = Visibility.Collapsed;

                this.SetCommandButtonState("Load");
            }
            else
                txtServiceName.Focus();
            FillUnitList();
            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }

         PagedCollectionView collection;
        private void ClinicDoctorServiceList()
        {
            try
            {                
                clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO BizAction = new clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO();
                
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.AdmissionTypeID = AdmissionTypeID;
                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = DoctorServiceDataList.PageIndex * DoctorServiceDataList.PageSize;
                BizAction.MaximumRows = DoctorServiceDataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO)arg.Result).ServiceList != null)
                    {
                        //if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        //{
                        clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO result = arg.Result as clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO;
                        DoctorServiceDataList.TotalItemCount = result.TotalRows;
                        DoctorServiceDataList.Clear();
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                
                                DoctorServiceDataList.Add(item);
                            }
                            dgDoctorServiceLinkedList.ItemsSource = null;
                            collection = new PagedCollectionView(DoctorServiceDataList);
                            //collection.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                            //collection.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                            dgDoctorServiceLinkedList.ItemsSource = collection;
                          

                            dgDataPager1.Source = null;
                            dgDataPager1.PageSize = BizAction.MaximumRows;
                            dgDataPager1.Source = DoctorServiceDataList;
                            //lblClinic.Visibility = Visibility.Collapsed;
                            //cmbClinic.Visibility = Visibility.Collapsed;
                            this.SetCommandButtonState("Load");
                        }
                    }
                    else
                    {
                      
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                   // Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                // throw;
                //Indicatior.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSpecialization.ItemsSource = null;                    
                    cmbSpecialization.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).ID;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSubSpecialization(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSubSpecialization.ItemsSource = null;
                    cmbSubSpecialization.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSubSpecialization.SelectedItem = objList[0];

                    if (iSupId > 0)
                        cmbSubSpecialization.IsEnabled = true;
                    else
                        cmbSubSpecialization.IsEnabled = false;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void SetComboboxValue()
        {
            cmbSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).Specialization;
            cmbSubSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).SubSpecialization;
        }

        

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            //lblClinic.Visibility = Visibility.Collapsed;
            //cmbClinic.Visibility = Visibility.Collapsed;
            this.SetCommandButtonState("Load");
            objAnimation.Invoke(RotationType.Backward);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            #region OLD Code
            //    SelectedServices = new List<clsServiceMasterVO>();

        //    for (int i = 0; i < check.Count; i++)
        //    {
        //        if (check[i])
        //        {
        //            SelectedServices.Add(ServiceItemSource[i]);
        //            //    new clsServiceMasterVO()
        //            //{
        //            //    //ID = ServiceItemSource[i].ID,
        //            //    //Description = ServiceItemSource[i].ServiceName,
        //            //    //TariffServiceMasterID = ServiceItemSource[i].TariffServiceMasterID,
        //            //    //TariffID = ServiceItemSource[i].TariffID,
        //            //    //Specialization = ServiceItemSource[i].Specialization,
        //            //    //Rate = ServiceItemSource[i].Rate, Code = ServiceItemSource[i].ServiceCode
        //            //});
        //        }
        //    }

        //    if (SelectedServices.Count == 0)
        //    {
        //        string strMsg = "No Service/s Selected for Adding";

        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();
        //    }
        //    else
        //    {
        //        this.DialogResult = true;
        //        if (OnAddButton_Click != null)
        //            OnAddButton_Click(this, new RoutedEventArgs());

            //    }
            #endregion

            bool isValid = true;
            if (_OtherServiceSelected == null)
            {
                isValid = false;
            }
            else if (_OtherServiceSelected.Count <= 0)
            {
                isValid = false;
            }

            if (isValid)
            {
               // this.DialogResult = true;
                //if (OnAddButton_Click != null)
                //    OnAddButton_Click(this, new RoutedEventArgs());
                Add(); 
            }
            else
            {
                string strMsg = "No Service/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {           
            FetchData();
        }

        private void Add()
        {
            Indicatior.Show();

            clsIPDAddAdmissionTypeServiceListBizActionVO BizAction = new clsIPDAddAdmissionTypeServiceListBizActionVO();
            
            BizAction.DoctorServiceDetails = new clsIPDAdmissionTypeServiceLinkVO();
            BizAction.DoctorServiceList = new ObservableCollection<clsServiceMasterVO>();

            if (Modify == true)
            {
                BizAction.Modify = true;
            }
            else
                BizAction.Modify = false;

            //BizAction.DoctorServiceList = (List<clsIPDAdmissionTypeServiceLinkVO>)((DoctorServiceLinkChildWindow)sender).SelectedDoctorServiceList.ToList();

            foreach (var item in _OtherServiceSelected)
            {
                item.AdmissionTypeID = AdmissionTypeID;
                item.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;                
            }


            BizAction.DoctorServiceList = (ObservableCollection<clsServiceMasterVO>)_OtherServiceSelected;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    ClinicDoctorServiceList();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp  Master .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        bool IsCancel = true;
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    cmdEdit.IsEnabled = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
                    cmdAdd.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    IsCancel = false;
                    cmdEdit.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    IsCancel = true;
                    cmdEdit.IsEnabled = false;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    IsCancel = true;
                    cmdEdit.IsEnabled = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdAdd.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    cmdEdit.IsEnabled = true;
                    break;
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdAdd.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdClose.IsEnabled = true;
                    IsCancel = false;
                    cmdEdit.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }
       
        private void FetchData()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {   
                //clsIPDGetAdmissionTypeServiceListBizActionVO BizAction = new clsIPDGetAdmissionTypeServiceListBizActionVO();
                clsIPDGetAdmissionTypeServiceListBizActionVO BizAction = new clsIPDGetAdmissionTypeServiceListBizActionVO();

                BizAction.ServiceList = new List<clsServiceMasterVO>();

                if (ViewDoctorServicelink == true)
                {

                    BizAction.AdmissionTypeID = AdmissionTypeID;
                    BizAction.UnitID = ((clsServiceMasterVO)dgDoctorServiceLinkedList.SelectedItem).UnitID;
                }
                else
                {
                    BizAction.AdmissionTypeID = 0;
                }


                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem != null)
                    BizAction.SpecializationID = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (cmbSubSpecialization.SelectedItem != null)
                    BizAction.SubSpecializationID = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                               

                BizAction.IsPagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsIPDGetAdmissionTypeServiceListBizActionVO)arg.Result).ServiceList != null)
                    {
                        //if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        //{

                        clsIPDGetAdmissionTypeServiceListBizActionVO result = arg.Result as clsIPDGetAdmissionTypeServiceListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.ServiceList != null)
                        {

                            foreach (var item in result.ServiceList)
                            {
                                ServiceTariffID = item.TariffID;
                                DataList.Add(item);
                            }


                            if (ViewDoctorServicelink == true)
                            {
                                _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();
                                foreach (var item in result.SelectedServiceList)
                                {
                                    foreach (var item1 in DataList)
                                    {
                                        if (item.ServiceName == item1.ServiceName)
                                        {
                                            item1.SelectService = true;
                                        }
                                    }
                                    item.SelectService = true;
                                    _OtherServiceSelected.Add(item);
                                }
                                dgSelectedServiceList.ItemsSource = null;
                                dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                                dgSelectedServiceList.UpdateLayout();
                                dgSelectedServiceList.Focus();
                            }
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = DataList.DeepCopy();

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;

                        }
                    }
                    else
                    {
                       
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                // throw;
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
            }
        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            #region OLD CODE
            //if (((CheckBox)sender).IsChecked == true)
            //{

            //    check[dgServiceList.SelectedIndex] = true;
            //}
            //else
            //{
            //    check[dgServiceList.SelectedIndex] = false;
            //}
            #endregion

            bool IsValid = true;
            if (dgServiceList.SelectedItem != null)
            {
                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (_OtherServiceSelected.Count > 0)
                    {
                        var item = from r in _OtherServiceSelected
                                   where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID
                                   select new clsServiceMasterVO
                                   {
                                       Status = r.Status,
                                       ID = r.ID,
                                       ServiceName = r.ServiceName
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "Services already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;

                                dgServiceList.ItemsSource = null;
                                dgServiceList.ItemsSource = DataList;
                                dgServiceList.UpdateLayout();
                                dgServiceList.Focus();
                                
                                IsValid = false;
                            }
                        }
                        else
                        {
                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

                           
                        }


                    }
                    else
                    {
                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                    }
                }
                else
                    _OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);


                if (IsValid == true)
                {

                    foreach (var item in _OtherServiceSelected)
                    {
                        item.SelectService = true;
                        item.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    }

                    dgSelectedServiceList.ItemsSource = null;
                    dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                    dgSelectedServiceList.UpdateLayout();
                    dgSelectedServiceList.Focus();
                }

            }



           // SelectedDoctorServiceList = (List<clsIPDAdmissionTypeServiceLinkVO>)(_OtherServiceSelected.ToList());

            foreach (var item in _OtherServiceSelected)
            {
                item.AdmissionTypeID = AdmissionTypeID;
                item.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            
        }

        private void FillUnitList()
        {

            //Indicator = new WaitIndicator();
            //Indicator.Show();

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                  //  cmbClinic.SelectedItem = objList[0];
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    if (this.DataContext != null)
                    {
                        //cmbClinic.SelectedValue = ((clsIPDAdmissionTypeServiceLinkVO)cmbClinic.SelectedItem).UnitID;


                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////if (cmbPatientSource.SelectedItem != null)
            ////{
            ////    PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            ////    // if (IsTariffFisrtFill)
            ////    FillTariff();
            ////    //else
            ////    //    FillTariff(true);
            ////}
        }

        private void chkSelectService_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedServiceList.SelectedItem != null)
            {

                if (((CheckBox)sender).IsChecked == false)
                {
                    string ServiceName = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ServiceName;
                    this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
                    //foreach (var Service in DataList.Where(x => x.ServiceName == ServiceName))
                    //{
                    //    Service.SelectService = false;
                    //}
                    foreach (var item in DataList)
                    {
                        if (item.ServiceName == ServiceName)
                        {
                            item.SelectService = false;
                        }
                       
                    }

                    dgServiceList.ItemsSource = null;
                    dgServiceList.ItemsSource = DataList;
                    dgServiceList.UpdateLayout();
                    dgServiceList.Focus();
                }

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

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
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
                            {
                                if (BirthDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                                    result = "1";
                                else
                                {
                                    int day = BirthDate.Day;
                                    int curday = DateTime.Now.Day;
                                    int dif = 0;
                                    if (day > curday)
                                        dif = (curday + 30) - day;
                                    else
                                        dif = curday - day;
                                    result = dif.ToString();
                                }
                                //result = (age.Day - 1).ToString();
                                break;
                            }
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

        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            long ID = 0;
            if (e.Key == Key.Enter)
            {
                
                //if ((MasterListItem)cmbTariff.SelectedItem != null)
                //{
                //    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    FetchData();
                //}
            }
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
            where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        FrameworkElement element;
        DataGridRow row;
        TextBox TxtRate;
        private void chkSelectRate_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                element = dgSelectedServiceList.Columns.Last().GetCellContent(dgSelectedServiceList.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtRate = FindVisualChild<TextBox>(row, "txtRate");
                TxtRate.IsEnabled = true;
                //string Rate = ((TextBox)sender).Text;


                decimal RateInfo = Convert.ToDecimal(TxtRate.Text);

                foreach (var item in _OtherServiceSelected)
                {
                    if (item.ID == ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID)
                    {
                        item.Rate = RateInfo;
                    }
                }

                //dgSelectedServiceList.ItemsSource = null;
                //dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                //dgSelectedServiceList.UpdateLayout();
                //dgSelectedServiceList.Focus();
            }
            else
            {
                TxtRate.IsEnabled = false;
                ServiceList = (PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource;
                var OBJ = from r in DataList
                                   where r.ID == ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID
                                   select r;

                decimal PreviousRate=0;
                foreach (var item in OBJ)
                {
                    PreviousRate = item.Rate;
                }
                foreach (var item in _OtherServiceSelected)
                {
                    if (item.ID == ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID)
                    {
                        item.Rate = PreviousRate;
                    }
                }
                TxtRate.Text =Convert.ToString(PreviousRate);
              
            }
        }
        private SwivelAnimation objAnimation;
       
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            //lblClinic.Visibility = Visibility.Visible;
            //cmbClinic.Visibility = Visibility.Visible;
            //cmbClinic.IsEnabled = false;
            ViewDoctorServicelink = false;
            _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();
            dgSelectedServiceList.ItemsSource = null;
            FetchData();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "Doctor Service Details";
            objAnimation.Invoke(RotationType.Forward);
        }
       
        bool Modify = false;
        
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

            this.SetCommandButtonState("Modify");
            //lblClinic.Visibility = Visibility.Visible;
            //cmbClinic.Visibility = Visibility.Visible;
            Modify = true;
            Add();
        }
        
        bool ViewDoctorServicelink = false;

        private void hlbViewDoctorMaster_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("View");
            //ClearControl();

           // FillData(((clsIPDAdmissionTypeServiceLinkVO)dgDoctorServiceLinkedList.SelectedItem).ID);
            ViewDoctorServicelink = true;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + ((clsIPDAdmissionTypeServiceLinkVO)dgDoctorServiceLinkedList.SelectedItem).AdmissionTypeID;
        }

        private void FillData(long iDoctorID)
        {
            //clsGetDoctorDetailListForDoctorMasterByIDBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterByIDBizActionVO();
            clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO BizAction = new clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO();
            BizAction.AdmissionTypeID = iDoctorID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                   
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {

            if (dgDoctorServiceLinkedList.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Select the Admission Type Service Linked List.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                this.SetCommandButtonState("View");
                //lblClinic.Visibility = Visibility.Visible;
                //cmbClinic.Visibility = Visibility.Visible;
                //cmbClinic.IsEnabled = false;
                ViewDoctorServicelink = true;
                cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                FetchData();
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                mElement.Text = " : " + "Doctor Service Details";
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void cmdCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsIPDAddUpdateAdmissionTypeServiceListBizActionVO bizActionVO = new clsIPDAddUpdateAdmissionTypeServiceListBizActionVO();

                bizActionVO.DoctorServiceDetails = new clsIPDAdmissionTypeServiceLinkVO();
                bizActionVO.DoctorServiceDetails.ServiceID = ((clsServiceMasterVO)dgDoctorServiceLinkedList.SelectedItem).ID;
                bizActionVO.DoctorServiceDetails.AdmissionTypeID = ((clsServiceMasterVO)dgDoctorServiceLinkedList.SelectedItem).AdmissionTypeID;
                bizActionVO.DoctorServiceDetails.Status = ((clsServiceMasterVO)dgDoctorServiceLinkedList.SelectedItem).Status;
                //bizActionVO.IsStatus = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsIPDAddUpdateAdmissionTypeServiceListBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            msgText = "Status updated successfully!";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                //FillFloorMasterList();
                                //FetchData();
                                ClinicDoctorServiceList();
                            };
                            msgWindow.Show();
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch
            {

            }
        }

      

       
    }

    public class clsService
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Tax { get; set; }
    }



    
}

