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
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pathology
{
    public partial class ServiceSearch : ChildWindow
    {

        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        public List<clsServiceMasterVO> SelectedServices { get; set; }

        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }

        public long PatientSourceID { get; set; }
        public long CompanyID { get; set; }
        public long PatientCategoryID { get; set; }

        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

        //flag for new patient or existing patient
       public bool IsFromNew = false;
       public bool IsFromEx = false;

       public long PatientSourceID1 { get; set; }
       public long CompanyID1 { get; set; }
       public long PatientCategoryID1 { get; set; }
       public long PatientTariffID1 { get; set; }
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

        public ServiceSearch()
        {
            InitializeComponent();
            ClassID = 1; // Default

            this.Loaded += new RoutedEventHandler(ServiceSearch_Loaded);  //ServiceSearch_Loaded

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;


            //======================================================
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
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
                    int i=(int)((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                    foreach (var item in objList)
                    {
                        if (item.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID)
                        {
                            cmbSpecialization.SelectedItem = item;
                        }
                        
                    }
                    
                        //objList[9];   //by default specialization will be pathology
                }
                //if (this.DataContext != null)
                //{
                //    cmbSpecialization.SelectedValue = ((clsServiceMasterVO)this.DataContext).ID;
                //}
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
                    cmbSubSpecialization.SelectedItem = objList[0];
                    //if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                    //    cmbSubSpecialization.SelectedItem = objList[0];

                    //if (iSupId > 0)
                    //    cmbSubSpecialization.IsEnabled = true;
                    //else
                    //    cmbSubSpecialization.IsEnabled = false;
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
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
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
                    if (((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
                    {
                        PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                    }
                    if (((MasterListItem)cmbCompany.SelectedItem).ID > 0)
                    {
                        CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                    }
                    if (((MasterListItem)cmbTariff.SelectedItem).ID > 0)
                    {
                        ServiceTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    }
                    if (((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
                    {
                        PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                    }
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

                }
          
         

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
        }

        private void FillTariff()
        {
            clsGetPatientTariffsBizActionVO BizAction = new clsGetPatientTariffsBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
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
                        else
                            cmbTariff.SelectedValue = objList[0].ID;


                        FetchData((long)cmbTariff.SelectedValue);
                    }
                }




            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public long? PatientCategoryL1Id_Retail = 0;

     
        PalashServiceClient client = null;

        PagedCollectionView collection;

        private void FetchData(long pTariffID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetTariffServiceListBizActionForPathologyVO BizAction = new clsGetTariffServiceListBizActionForPathologyVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();

                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem != null)
                    BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (cmbSubSpecialization.SelectedItem != null)
                    BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;

                if (cmbPatientSource.SelectedItem != null)
                {
                    BizAction.PatientSourceTypeID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                }

                string Age = null;
                Age = ConvertDate(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth, "YY");
                if (Age != null && Age != "")
                {
                    BizAction.Age = Convert.ToInt16(Age);
                }

                BizAction.TariffID = pTariffID;
                BizAction.ClassID = ClassID;
                PackageTariffID = pTariffID;

                BizAction.UsePackageSubsql = true;  //used to set @subSqlQuerry in CIMS_GetTariffServiceListNew

                //if (rdbServices.IsChecked == true)                 
                //    BizAction.IsPackage = false;
                //else if (rdbPackage.IsChecked == true)
                //    BizAction.IsPackage = true;


              

                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.SearchExpression = " IsFavorite desc,servicename asc ";

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetTariffServiceListBizActionForPathologyVO)arg.Result).ServiceList != null)
                    {

                        clsGetTariffServiceListBizActionForPathologyVO result = arg.Result as clsGetTariffServiceListBizActionForPathologyVO;
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
                            collection = new PagedCollectionView(DataList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("PatientCategoryL3"));
                            dgServiceList.ItemsSource = collection;   //DataList;

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


                foreach (var item in _OtherServiceSelected)
                {
                    item.SelectService = true;
                }
                dgSelectedServiceList.ItemsSource = null;
                dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                dgSelectedServiceList.UpdateLayout();
                dgSelectedServiceList.Focus();
            }
        }

    
        private void FillPatientCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;
                   // cmbPatientCategory.SelectedItem = objList[0];
                    if (PatientCategoryID1 != 0)
                    {
                        foreach (MasterListItem item in objList)
                        {
                            if (item.ID == PatientCategoryID1)
                            {
                                cmbPatientCategory.SelectedItem = item;
                            }
                        }
                        // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                    }
                    else if (PatientCategoryID == 0)
                    {
                        cmbPatientCategory.SelectedItem = objList[0];
                    }
                    
                    else if (PatientCategoryID != 0)
                    {
                        foreach (MasterListItem item in objList)
                        {
                            if (item.ID == PatientCategoryID)
                            {
                                cmbPatientCategory.SelectedItem = item;
                            }
                        }
                       // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                    }

                    //if (this.DataContext != null)
                    //{
                    //    //cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                    //    cmbPatientCategory.SelectedValue = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;

                    //    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);

                    //    if (((MasterListItem)cmbPatientCategory.SelectedItem).ID == 0)
                    //    {
                    //        cmbPatientCategory.TextBox.SetValidation("Please select the Patient Category");
                    //        cmbPatientCategory.TextBox.RaiseValidationError();
                    //    }
                    //}

                    
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillPatientSource1()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList;
                    cmbPatientSource.SelectedItem = objList[0];

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillPatientSource(long PatientCategoryID)
        {
            clsGetPatientCategoryMasterVO BizAction = new clsGetPatientCategoryMasterVO();

            BizAction.PatientSourceID = PatientCategoryID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetPatientCategoryMasterVO)e.Result).List);
                }

                cmbPatientSource.ItemsSource = null;
                cmbPatientSource.ItemsSource = objList;
                //cmbPatientSource.SelectedItem = objList[0];
                if (PatientSourceID1 != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientSourceID1)
                        {
                            cmbPatientSource.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
                else if (PatientSourceID== 0)
                {
                    cmbPatientSource.SelectedItem = objList[0];
                }
               
                else if (PatientSourceID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientSourceID)
                        {
                            cmbPatientSource.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }

                //if (this.DataContext != null)
                //{
                //    cmbPatientSource.SelectedValue = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

                //    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);

                //    if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //    {
                //        cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");
                //        cmbPatientSource.TextBox.RaiseValidationError();
                //    }
                //}
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        //private void FillCompany(long PatientSourceID)
        //{
        //    clsGetCompanyMasterVO BizAction = new clsGetCompanyMasterVO();

        //    BizAction.PatientCategoryID = PatientSourceID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        List<MasterListItem> objList = new List<MasterListItem>();

        //        objList.Add(new MasterListItem(0, "- Select -"));
        //        if (e.Error == null && e.Result != null)
        //        {
        //            objList.AddRange(((clsGetCompanyMasterVO)e.Result).List);
        //        }

        //        cmbCompany.ItemsSource = null;
        //        cmbCompany.ItemsSource = objList;
        //        cmbCompany.SelectedItem = objList[0];
               
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        //}
        private void FillCompany(long PatientSourceID)
        {
            clsGetCompanyMasterVO BizAction = new clsGetCompanyMasterVO();

            BizAction.PatientCategoryID = PatientSourceID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetCompanyMasterVO)e.Result).List);
                }

                cmbCompany.ItemsSource = null;
                cmbCompany.ItemsSource = objList;
                //cmbCompany.SelectedItem = objList[0];
                if (CompanyID1 != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == CompanyID1)
                        {
                            cmbCompany.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
                else if (CompanyID == 0)
                {
                    cmbCompany.SelectedItem = objList[0];
                }
               
                else if (CompanyID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == CompanyID)
                        {
                            cmbCompany.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
                //if (this.DataContext != null)
                //{
                //    //cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                //    cmbCompany.SelectedValue = ((MasterListItem)cmbCompany.SelectedItem).ID;

                //    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);

                //    if (((MasterListItem)cmbCompany.SelectedItem).ID == 0)
                //    {
                //        cmbCompany.TextBox.SetValidation("Please select the Company");
                //        cmbCompany.TextBox.RaiseValidationError();
                //    }
                //}

          
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillTariffMaster(long pPatientsourceID, long pCompanyID)
        {
            clsGetTariffMasterVO BizAction = new clsGetTariffMasterVO();

            BizAction.CompanyID = pCompanyID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetTariffMasterVO)e.Result).List);
                }

                cmbTariff.ItemsSource = null;
                cmbTariff.ItemsSource = objList;
                //cmbTariff.SelectedItem = objList[0];
                if (PatientTariffID1 != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientTariffID1)
                        {
                            cmbTariff.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
                else if (PatientTariffID == 0)
                {
                    cmbTariff.SelectedValue = objList[0];
                }
                
                else if (PatientTariffID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientTariffID)
                        {
                            cmbTariff.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

       


      

        private void FillNewTariff(long pCompanyID)
        {
            clsGetPatientSponsorTariffListBizActionVO BizAction = new clsGetPatientSponsorTariffListBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            //BizAction.PatientSourceID = PatientSourceID;
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
                        #region OLD Code Commented By CDS
                        //if (IsTariffFisrtFill)
                        //{
                        //    cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                        //    IsTariffFisrtFill = false;
                        //}
                        //else
                        #endregion
                        cmbTariff.SelectedValue = objList[0].ID;

                       
                        FetchData((long)cmbTariff.SelectedValue);
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

                    long MainServiceID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).PackageServiceID;

                    foreach (clsServiceMasterVO item in _OtherServiceSelected)
                    {
                        if (((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ConditionServiceID > 0 && ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).PackageServiceID == item.ID && ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ConditionType == "AND")
                        {
                            _OtherServiceSelected.Remove(item);
                            break;
                        }
                        else if (((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID == item.PackageServiceID && item.ConditionServiceID > 0 && item.ConditionType == "AND")
                        {
                            _OtherServiceSelected.Remove(item);
                            break;
                        }
                    }

                    this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
                    foreach (var Service in DataList.Where(x => x.ID == ServiceID || x.ID == MainServiceID))
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

                    DateTime age = DateTime.MinValue + difference;

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

        private void dgServiceList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsServiceMasterVO item = (clsServiceMasterVO)e.Row.DataContext;
            if (item.PackageServiceConditionID > 0)
                e.Row.Background = new SolidColorBrush(Colors.Gray);
            else
                e.Row.Background = null;
        }

  

        public void AddMainService(List<clsServiceMasterVO> lServices2)
        {
            bool IsValid = true;
            StringBuilder strError = new StringBuilder();

            foreach (clsServiceMasterVO item in lServices2)
            {
                var item2 = from r in _OtherServiceSelected
                            where r.ID == item.ID
                            select new clsServiceMasterVO
                            {
                                Status = r.Status,
                                ID = r.ID,
                                ServiceName = r.ServiceName
                            };

                if (item2.ToList().Count > 0)
                {
                    if (strError.ToString().Length > 0)
                        strError.Append(",");
                    strError.Append(item.ServiceName);

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
        }

  

       
       
       

        //private void rdbServices_Checked(object sender, RoutedEventArgs e)
        //{
        //    // Fetch Tariff Wise Services 
        //    long ID = 0;
        //    if ((MasterListItem)cmbTariff.SelectedItem != null && (MasterListItem)cmbCompany.SelectedItem != null)
        //    {
        //        ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

        //        FetchData(ID);
        //    }

        //}

        private void ServiceSearch_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();
                if (IsFromNew == true)
                {
                    PatientSourceID = 0;
                    PatientCategoryID = 0;
                    PatientTariffID = 0;
                    CompanyID = 0;
                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //MasterListItem objM = new MasterListItem(0, "-- Select --");
                    //objList.Add(objM);
                    //cmbPatientSource.ItemsSource = objList;
                    //cmbPatientSource.SelectedItem = objM;
                    //cmbCompany.ItemsSource = objList;
                    //cmbCompany.SelectedItem = objM;
                    //cmbTariff.ItemsSource = objList;
                    //cmbTariff.SelectedItem = objM;

                    //PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                    //PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                    //PatientTariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                    //CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }
                else if(IsFromEx==true)
                {
                    PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                    PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                    PatientTariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                    CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }
                FillPatientCategory();
               // FillPatientSource1();
                FillSpecialization();
                SetComboboxValue();
                cmbPatientCategory.Focus();
               // rdbServices.IsChecked = true;
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;

        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            //{
            //    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);
            //}
            //added by rohini dated 4.3.16
            if (cmbPatientSource.SelectedItem != null || cmbPatientSource.SelectedValue != null)
                if (((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    //cmbPatientSource.ItemsSource = objList;
                    //cmbPatientSource.SelectedItem = objM;
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;

                    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);
                }

        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            //{
            //    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
            //}
            //added by rohini dated 4.3.16
            if (cmbCompany.SelectedItem != null || cmbCompany.SelectedValue != null)
                if (((MasterListItem)cmbCompany.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    //cmbPatientSource.ItemsSource = objList;
                    //cmbPatientSource.SelectedItem = objM;
                   // cmbCompany.ItemsSource = objList;
                    //cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;

                    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
                }

        }

        private void cmbPatientCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
            //{
            //    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);
            //}

            //added by rohini dated 4.3.16
            if (cmbPatientCategory.SelectedItem != null || cmbPatientCategory.SelectedValue != null)
                if (((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbPatientSource.ItemsSource = objList;
                    cmbPatientSource.SelectedItem = objM;
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;

                    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);
                }
        }

    }

    public class clsServiceForPack
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Tax { get; set; }
    }

}

