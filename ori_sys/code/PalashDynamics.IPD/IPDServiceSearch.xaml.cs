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


namespace PalashDynamics.IPD
{
    public partial class IPDServiceSearch : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();
        public bool IsForIPDBill;
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public clsPatientGeneralVO SelectedPatient { get; set; }
        public List<clsServiceMasterVO> SelectedServices { get; set; }
        public bool IsfromBillList = false;
        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }
        int ClickedFlag = 0;
        public long PatientSourceID { get; set; }

        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;
        public bool IsFromAdmission = false;
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

        WaitIndicator Indicatior = null;

        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }

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
            }
        }
        #region Constructor
        public IPDServiceSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IPDServiceSearch_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
        }
        #endregion

        #region Loaded Event
        private void IPDServiceSearch_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();
                PatientSourceID = SelectedPatient.PatientSourceID;
                FillClass();
                FillPatientSponsorDetails();
                FillSpecialization();
                SetComboboxValue();
                txtServiceName.Focus();
                cmdAdd.IsEnabled = false;
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }
        #endregion

        #region OnRefresh Event
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #region ComboBox Fill
        private void FillClass()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                    cmbClass.ItemsSource = null;
                    cmbClass.ItemsSource = objList;
                    if (IsfromBillList == true)
                    {
                        cmbClass.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID;  //((IApplicationConfiguration)App.Current).SelectedPatient.ClassID; //ClassID;
                    }
                    else
                    {
                        cmbClass.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID;  //((IApplicationConfiguration)App.Current).SelectedIPDPatient.classID; //ClassID;
                    }
                }
                //if (this.DataContext != null)
                //{
                //    cmbClass.SelectedValue = ((clsServiceMasterVO)this.DataContext).ID;
                //}
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
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

        private void FillTariff()
        {
            clsGetPatientTariffsBizActionVO BizAction = new clsGetPatientTariffsBizActionVO();
            BizAction.PatientID = SelectedPatient.PatientID;     //((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = SelectedPatient.UnitId; //((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            BizAction.PatientSourceID = PatientSourceID;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetPatientTariffsBizActionVO)arg.Result).MasterList);
                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    if (objList.Count > 0)
                    {
                        if (IsTariffFisrtFill)
                        {
                            cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                            IsTariffFisrtFill = false;
                        }
                        cmbTariff.SelectedValue = SelectedPatient.TariffID;
                        FetchData((long)cmbTariff.SelectedValue);
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

                BizAction.SponsorID = 0;
                BizAction.PatientID = SelectedPatient.PatientID;
                BizAction.PatientUnitID = SelectedPatient.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        cmbPatientSource.SelectedValue = PatientSourceID;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Button Click Event
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
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
                    this.DialogResult = true;
                    if (OnAddButton_Click != null)
                        OnAddButton_Click(this, new RoutedEventArgs());
                }
                else
                {
                    string strMsg = "No Service/s Selected for Adding";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    ClickedFlag = 0;
                }
            }
            
            
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            FetchData(ID);
        }
        #endregion

        private void FetchData(long pTariffID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem != null)
                    BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (cmbSubSpecialization.SelectedItem != null)
                    BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;
                if (cmbPatientSource.SelectedItem != null)
                {
                    if (pTariffID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                        BizAction.PatientSourceType = 0;
                    else
                        BizAction.PatientSourceType = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceType;
                    BizAction.PatientSourceTypeID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceTypeID;
                    BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
                    BizAction.PatientUnitID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;
                }
                string Age = null;
                Age = ConvertDate(SelectedPatient.DateOfBirth, "YY");
                BizAction.Age = Convert.ToInt16(Age);
                BizAction.TariffID = pTariffID;
                if (cmbClass.SelectedItem != null)
                    BizAction.ClassID = ((MasterListItem)cmbClass.SelectedItem).ID;//ClassID;
                PackageTariffID = pTariffID;
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                    {
                        clsGetTariffServiceListBizActionVO result = arg.Result as clsGetTariffServiceListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                ServiceTariffID = item.TariffID;
                                DataList.Add(item);
                            }
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
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
                                   where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID && r.ClassID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ClassID
                                   select new clsServiceMasterVO
                                   {
                                       Status = r.Status,
                                       ID = r.ID,
                                       ServiceName = r.ServiceName,
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
                    }
                    dgSelectedServiceList.ItemsSource = null;
                    dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                    dgSelectedServiceList.UpdateLayout();
                    dgSelectedServiceList.Focus();
                }
                if (_OtherServiceSelected.Count > 0)
                    cmdAdd.IsEnabled = true;
                else
                    cmdAdd.IsEnabled = false;
            }
        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientSource.SelectedItem != null)
            //{
            //    PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            //    FillTariff();
            //}
            if (cmbPatientSource.SelectedItem != null)
            {
                PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                //PatientSourceID = long(cmbPatientSource.SelectedValue);
                FillCompany();
            }
        }

        private void FillCompany()
        {
            clsGetPatientSponsorCompanyListBizActionVO BizAction = new clsGetPatientSponsorCompanyListBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;  //((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID;//((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            BizAction.PatientSourceID = PatientSourceID;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.AddRange(((clsGetPatientSponsorCompanyListBizActionVO)arg.Result).MasterList);
                    cmbCompany.ItemsSource = null;

                    cmbCompany.ItemsSource = objList;
                    if (objList.Count > 0)
                    {
                        if (IsTariffFisrtFill)
                        {
                            cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                            IsTariffFisrtFill = false;
                        }
                        else
                            cmbCompany.SelectedValue = objList[0].ID;
                        cmbCompany.SelectedValue = objList[0].ID;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void chkSelectService_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedServiceList.SelectedItem != null)
            {
                if (((CheckBox)sender).IsChecked == false)
                {
                    long ServiceID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID;
                    this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
                    foreach (var Service in DataList.Where(x => x.ID == ServiceID))
                    {
                        Service.SelectService = false;
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
                if ((MasterListItem)cmbTariff.SelectedItem != null)
                {
                    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    FetchData(ID);
                }
            }
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;

                FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);

            }
        }

        private void FillNewTariff(long pCompanyID)
        {
            clsGetPatientSponsorTariffListBizActionVO BizAction = new clsGetPatientSponsorTariffListBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId; //((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID; //((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now;// DateTime.Now.Date.Date;
            BizAction.PatientCompanyID = pCompanyID;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.AddRange(((clsGetPatientSponsorTariffListBizActionVO)arg.Result).MasterList);
                    cmbTariff.ItemsSource = null;

                    cmbTariff.ItemsSource = objList;
                    if (objList.Count > 0)
                    {
                        if (IsTariffFisrtFill)
                        {
                            cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.TariffID; //((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                            IsTariffFisrtFill = false;
                        }
                        else
                            cmbTariff.SelectedValue = objList[0].ID;

                        FetchData((long)cmbTariff.SelectedValue);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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

