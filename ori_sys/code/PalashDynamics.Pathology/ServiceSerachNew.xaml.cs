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

using System.Windows.Browser;
using OPDModule;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using OPDModule.Forms;


namespace PalashDynamics.Pathology
{
    public partial class ServiceSerachNew : ChildWindow
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
        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

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
        public ServiceSerachNew()
        {
            InitializeComponent();
            ClassID = 1; // Default

            this.Loaded += new RoutedEventHandler(ServiceSearchForPackage_Loaded);  //ServiceSearch_Loaded

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
        }
        private void ServiceSearchForPackage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();

                PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                PatientTariffID= ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                //commented by rohini
               // FillPatientSponsorDetails();
                //added by rohini
                FillPatientSource();
                FillSpecialization();
                SetComboboxValue();
                txtServiceName.Focus();
                rdbServices.IsChecked = true;
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }



        # region  added by rohini
        private void FillPatientSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
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

                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList;
                }
                if (PatientSourceID != 0)
                {
                    cmbPatientSource.SelectedValue = PatientSourceID;
                    ((MasterListItem)cmbPatientSource.SelectedItem).ID = PatientSourceID;
                    FillCompanyMaster(((MasterListItem)cmbPatientSource.SelectedItem).ID);

                }
                else
                {
                    cmbPatientSource.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }
        private void FillCompanyMaster(long PatientSourceID)
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

                if (CompanyID != 0)
                {
                    cmbCompany.SelectedValue = CompanyID;
                    ((MasterListItem)cmbCompany.SelectedItem).ID = CompanyID;
                    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
                    //if (((MasterListItem)cmbCompany.SelectedItem).ID == 0)
                    //{
                    //    cmbCompany.TextBox.SetValidation("Please select the Company");
                    //    cmbCompany.TextBox.RaiseValidationError();
                    //}

                }
                else
                {
                    cmbCompany.SelectedValue = (long)0;
                }



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
               
                if (PatientTariffID != 0)
                {
                    cmbTariff.SelectedValue = PatientTariffID;
                   // ((MasterListItem)cmbTariff.SelectedItem).ID = PatientTariffID;                  
                    //if (((MasterListItem)cmbTariff.SelectedItem).ID == 0)
                    //{
                    //    cmbTariff.TextBox.SetValidation("Please select the Triff");
                    //    cmbTariff.TextBox.RaiseValidationError();
                    //}
                }
                else
                {
                    cmbTariff.SelectedValue = (long)0;
                }
              //  cmbTariff.SelectedItem = objList[0];
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }
        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //commented by rohini 
            //if (cmbPatientSource.SelectedItem != null)
            //{
            //    PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            //    PatientCategoryL1Id_Retail = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientCategoryID;
            //    //PatientSourceID = long(cmbPatientSource.SelectedValue);
            //    FillCompany();

            //}

            //  added by rohini
            if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            {
                FillCompanyMaster(((MasterListItem)cmbPatientSource.SelectedItem).ID);
            }


        }
        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //commented by rohini
            //if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            //{
            //    //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            //    FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);

            //}

            //added by rohini 
            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
            }
        }
        private void FetchDataNew(long pTariffID)
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
        #endregion
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
                    cmbSpecialization.SelectedValue = (long)14;
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
            this.DialogResult = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            #region commented by rohini
            //bool isValid = true;
            //bool Ismultisponser = false;
            //if (_OtherServiceSelected == null)
            //{
            //    isValid = false;
            //}
            //else if (_OtherServiceSelected.Count <= 0)
            //{
            //    isValid = false;
            //}
            //else if (_OtherServiceSelected.Count > 0)
            //{

            //    var item5 = from r in _OtherServiceSelected
            //                where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
            //                r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
            //                r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID
            //                select r;
            //    if (item5.ToList().Count == 0)
            //    {
            //        Ismultisponser = true;
            //    }


            //}
            //if (Ismultisponser == false)
            //{
            //    if (isValid)
            //    {
            //        this.DialogResult = true;
            //        if (OnAddButton_Click != null)
            //            OnAddButton_Click(this, new RoutedEventArgs());
            //    }
            //    else
            //    {
            //        string strMsg = "No Service/s Selected for Adding";

            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW1.Show();

            //    }
            //}
            //else
            //{
            //    string strMsg = "Multiple Sponsor Billing is not Allowed";

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();

            //}
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
                //if (((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
                //{
                //    PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                //}
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
            //commented by rohini
           // FetchData(ID);
            FetchDataNew(ID);
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
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }

        public long? PatientCategoryL1Id_Retail = 0;

        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

                BizAction.SponsorID = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        //cmbPatientSource.SelectedValue = PatientSourceID;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                        if (((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null || ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count > 0)
                        {
                            cmbPatientSource.SelectedValue = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails[((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count - 1].PatientSourceID;
                        }

                        //foreach (var item in ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails)
                        //{
                        //    if (PatientSourceID == item.PatientSourceID)
                        //    {
                        //        PatientCategoryL1Id_Retail = item.PatientCategoryID;
                        //    }
                        //}

                        FillCompany();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                //Indicatior.Close();
                // throw;
            }
        }
        PalashServiceClient client = null;

        PagedCollectionView collection;

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


                if (rdbServices.IsChecked == true)
                {
                    BizAction.IsPackage = false;
                    if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                        BizAction.ForFilterPackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                }
                else if (rdbPackage.IsChecked == true)
                {
                    BizAction.IsPackage = true;
                }


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

            # region commented by rohini
            //bool IsValid = true;
            //if (dgServiceList.SelectedItem != null)
            //{
            //    if (_OtherServiceSelected == null)
            //        _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

            //    CheckBox chk = (CheckBox)sender;
            //    StringBuilder strError = new StringBuilder();
            //    bool CheckGenderForPackage = false;
            //    bool IsTariffPackage = false;

            //    // Commented By CDS 
            //    //if (((MasterListItem)cmbTariff.SelectedItem).Code == "Package" && ((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
            //    //{
            //    //    IsTariffPackage = true;


            //    //    if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
            //    //    {
            //    //        CheckGenderForPackage = true;
            //    //    }
            //    //}
            //    // END

            //    if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
            //    {
            //        if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
            //        {
            //            IsTariffPackage = true;


            //            if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
            //            {
            //                CheckGenderForPackage = true;
            //            }
            //        }
            //    }
            //    # region Code when checkbox for selected service is checked

            //    if (chk.IsChecked == true)
            //    {
            //        if (cmbCompany.SelectedItem != null)
            //        {
            //            ((clsServiceMasterVO)dgServiceList.SelectedItem).CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            //        }
            //        if (cmbPatientSource.SelectedItem != null)
            //        {
            //            ((clsServiceMasterVO)dgServiceList.SelectedItem).PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID;
            //        }
            //        if (cmbTariff.SelectedItem != null)
            //        {
            //            ((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            //        }

            //        if (_OtherServiceSelected.Count > 0)
            //        {


            //            var item1 = from r in _OtherServiceSelected
            //                        where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
            //                        r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
            //                        r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID
            //                        select r;


            //            if (item1.ToList().Count > 0)
            //            {


            //                var item = from r in _OtherServiceSelected
            //                           where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID
            //                           select new clsServiceMasterVO
            //                           {
            //                               Status = r.Status,
            //                               ID = r.ID,
            //                               ServiceName = r.ServiceName
            //                           };

            //                if (item.ToList().Count > 0)
            //                {
            //                    if (strError.ToString().Length > 0)
            //                        strError.Append(",");
            //                    strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

            //                    if (!string.IsNullOrEmpty(strError.ToString()))
            //                    {
            //                        string strMsg = "Services already Selected : " + strError.ToString();

            //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                        msgW1.Show();
            //                        ((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;

            //                        dgServiceList.ItemsSource = null;
            //                        dgServiceList.ItemsSource = DataList;
            //                        dgServiceList.UpdateLayout();
            //                        dgServiceList.Focus();

            //                        IsValid = false;
            //                    }
            //                }
            //                else
            //                {


            //                    if (IsTariffPackage == false)
            //                    {
            //                        if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                            GetPackageConditionalServicesAndRelations();
            //                        else
            //                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                    }
            //                    else if (IsTariffPackage == true)
            //                    {
            //                        if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
            //                        {
            //                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                                GetPackageConditionalServicesAndRelations();
            //                            else
            //                                _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                        }
            //                        else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
            //                        {
            //                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                                GetPackageConditionalServicesAndRelations();
            //                            else
            //                                _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                        }
            //                        else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
            //                        {

            //                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                                GetPackageConditionalServicesAndRelations();
            //                            else
            //                                _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                        }
            //                        else
            //                        {
            //                            IsValid = false;

            //                            ((CheckBox)sender).IsChecked = false;

            //                            string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

            //                            MessageBoxControl.MessageBoxChildWindow msgW2 =
            //                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                            msgW2.Show();
            //                        }
            //                    }

            //                }

            //            }// block for sponser
            //            else
            //            {
            //                IsValid = false;
            //                ((CheckBox)sender).IsChecked = false;

            //                MessageBoxControl.MessageBoxChildWindow msgW2 =
            //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //                msgW2.Show();
            //            }
            //        }
            //        else
            //        {
            //            if (IsTariffPackage == false)
            //            {
            //                if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                    GetPackageConditionalServicesAndRelations();
            //                else
            //                    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //            }
            //            else if (IsTariffPackage == true)
            //            {
            //                if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
            //                {
            //                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                        GetPackageConditionalServicesAndRelations();
            //                    else
            //                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                }
            //                else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
            //                {
            //                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                        GetPackageConditionalServicesAndRelations();
            //                    else
            //                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                }
            //                else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
            //                {
            //                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
            //                        GetPackageConditionalServicesAndRelations();
            //                    else
            //                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
            //                }
            //                else
            //                {
            //                    IsValid = false;

            //                    ((CheckBox)sender).IsChecked = false;

            //                    string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

            //                    MessageBoxControl.MessageBoxChildWindow msgW2 =
            //                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                    msgW2.Show();
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        _OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);
            //    }



            //    # endregion

            //    if (IsValid == true)
            //    {

            //        foreach (var item in _OtherServiceSelected)
            //        {
            //            item.SelectService = true;

            //        }

            //        dgSelectedServiceList.ItemsSource = null;
            //        dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
            //        dgSelectedServiceList.UpdateLayout();
            //        dgSelectedServiceList.Focus();
            //    }

            //}
            #endregion
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

        private void GetPackageConditionalServicesAndRelations()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetPackageConditionalServicesNewBizActionVO BizAction = new clsGetPackageConditionalServicesNewBizActionVO();

                BizAction.ServiceConditionList = new List<clsServiceMasterVO>();
                BizAction.TariffID = (long)cmbTariff.SelectedValue; //TariffID;
                BizAction.ServiceID = ((clsServiceMasterVO)dgServiceList.SelectedItem).ID;  //MainServiceID;
                BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId; //PatientID;
                BizAction.PatientUnitID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;  //PatientUnitID;

                //Added BY CDS
                BizAction.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                //END
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

                // New Code by CDS
                BizAction.PatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                //BizAction.MemberRelationID = ((IApplicationConfiguration)App.Current).SelectedPatient.MemberRelationID;   
                BizAction.MemberRelationID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).MemberRelationID;
                //BizAction.MemberRelationID = 2;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);



                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPackageConditionalServicesNewBizActionVO)arg.Result).ServiceConditionList != null)
                    {

                        clsGetPackageConditionalServicesNewBizActionVO result = arg.Result as clsGetPackageConditionalServicesNewBizActionVO;

                        bool IsAddMainService = true;

                        if (result.ServiceConditionList != null && result.ServiceConditionList.Count > 0)
                        {

                            foreach (var item in result.ServiceConditionList)
                            {


                                if (item.PackageServiceID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID && item.ConditionType == "OR")
                                {
                                    item.TotalORUsedQuantity += item.ConditionalUsedQuantity;
                                    ((clsServiceMasterVO)dgServiceList.SelectedItem).TotalORUsedQuantity = item.TotalORUsedQuantity;
                                }

                                if ((item.MainServiceUsedQuantity + item.ConditionalUsedQuantity) >= item.ConditionalQuantity && item.ConditionType == "OR")   //if (item.ConditionalUsedQuantity > 0 && item.ConditionType == "OR")
                                {

                                }
                                else
                                {

                                }
                            }

                            if (IsAddMainService == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "You can't select this Service , as you have already select service from OR Conditional Services !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                foreach (var Service in DataList.Where(x => x.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID))
                                {
                                    Service.SelectService = false;
                                }
                                dgServiceList.ItemsSource = null;
                                collection = new PagedCollectionView(DataList);
                                collection.GroupDescriptions.Add(new PropertyGroupDescription("PatientCategoryL3"));
                                dgServiceList.ItemsSource = collection;  //DataList;



                                dgServiceList.UpdateLayout();
                                dgServiceList.Focus();
                            }
                            else if (IsAddMainService == true)
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

            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // cmbSpecialization.IsEnabled = false;

        }

   

        private void FillCompany()
        {
            clsGetPatientSponsorCompanyListBizActionVO BizAction = new clsGetPatientSponsorCompanyListBizActionVO();

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

                    objList.AddRange(((clsGetPatientSponsorCompanyListBizActionVO)arg.Result).MasterList);
                    cmbCompany.ItemsSource = null;

                    cmbCompany.ItemsSource = objList;
                    if (objList.Count > 0)
                    {
                        #region OLD Code Commented By CDS
                        //if (IsTariffFisrtFill)
                        //{
                        //    cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                        //    IsTariffFisrtFill = false;
                        //}
                        //else
                        #endregion
                        cmbCompany.SelectedValue = objList[0].ID;

                        //FetchData((long)cmbTariff.SelectedValue);
                    }
                }




            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
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

                        FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        FetchData((long)cmbTariff.SelectedValue);
                    }
                }
            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }

        private void chkSelectService_Click(object sender, RoutedEventArgs e)
        {
            #region commented by rohini
            //if (dgSelectedServiceList.SelectedItem != null)
            //{

            //    if (((CheckBox)sender).IsChecked == false)
            //    {
            //        long ServiceID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID;

            //        long MainServiceID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).PackageServiceID;

            //        foreach (clsServiceMasterVO item in _OtherServiceSelected)
            //        {
            //            if (((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ConditionServiceID > 0 && ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).PackageServiceID == item.ID && ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ConditionType == "AND")
            //            {
            //                _OtherServiceSelected.Remove(item);
            //                break;
            //            }
            //            else if (((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ID == item.PackageServiceID && item.ConditionServiceID > 0 && item.ConditionType == "AND")
            //            {
            //                _OtherServiceSelected.Remove(item);
            //                break;
            //            }
            //        }

            //        this._OtherServiceSelected.Remove((clsServiceMasterVO)dgSelectedServiceList.SelectedItem);
            //        foreach (var Service in DataList.Where(x => x.ID == ServiceID || x.ID == MainServiceID))
            //        {
            //            Service.SelectService = false;
            //        }
            //        dgServiceList.ItemsSource = null;
            //        dgServiceList.ItemsSource = DataList;
            //        dgServiceList.UpdateLayout();
            //        dgServiceList.Focus();
            //    }
            //}
            #endregion
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

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            ConditionalServiceSearchForPackageNew ConditionalserviceSearch = null;
            ConditionalserviceSearch = new ConditionalServiceSearchForPackageNew();

            ConditionalserviceSearch.TariffID = ((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID;
            //Added BY CDS
            ConditionalserviceSearch.PackageID = ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID;
            //cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
            ConditionalserviceSearch.MemberRelationID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).MemberRelationID;
            //END
            ConditionalserviceSearch.MainServiceID = ((clsServiceMasterVO)dgServiceList.SelectedItem).ID;

            ConditionalserviceSearch.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            ConditionalserviceSearch.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            ConditionalserviceSearch.MainServiceName = ((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName;

            ConditionalserviceSearch.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
            ConditionalserviceSearch.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

            ConditionalserviceSearch.OnAddButton_Click += new RoutedEventHandler(ConditionalserviceSearch_OnAddButton_Click);

            ConditionalserviceSearch.Show();
        }

        void ConditionalserviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            lServices = ((ConditionalServiceSearchForPackageNew)sender).SelectedOtherServices.ToList();

            List<clsServiceMasterVO> lstMainService = new List<clsServiceMasterVO>();

            if (dgServiceList.SelectedItem != null)  //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService == true
                lstMainService.Add((clsServiceMasterVO)dgServiceList.SelectedItem);

            if (_OtherServiceSelected == null)
                _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

            bool IsValid = true;
            StringBuilder strError = new StringBuilder();
            bool CheckGenderForPackage = false;
            bool IsTariffPackage = false;

            foreach (clsServiceMasterVO itemSer in lServices)
            {
                // Commented By CDS 
                //if ((itemSer).Code == "Package")
                //{
                //    IsTariffPackage = true;


                //    if ((itemSer).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && (itemSer).ApplicableToString != "Both")
                //    {
                //        CheckGenderForPackage = true;
                //    }
                //}
                // Commented By CDS 

                if ((itemSer).PackageID > 0)
                {
                    IsTariffPackage = true;


                    if ((itemSer).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && (itemSer).ApplicableToString != "Both")
                    {
                        CheckGenderForPackage = true;
                    }
                }

                if (_OtherServiceSelected.Count > 0)
                {
                    var item = from r in _OtherServiceSelected
                               where r.ID == itemSer.ID
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
                        strError.Append(itemSer.ServiceName);

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

                        if (IsTariffPackage == false)
                        {
                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                                AddMainService(lstMainService);  //lServices
                            _OtherServiceSelected.Add(itemSer);
                        }
                        else if (IsTariffPackage == true)
                        {
                            if (CheckGenderForPackage == true && (itemSer.ApplicableToString != "Both"))
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                                    AddMainService(lstMainService);  //lServices
                                _OtherServiceSelected.Add(itemSer);
                            }
                            else if (CheckGenderForPackage == false && (itemSer.ApplicableToString == "Both"))
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                                    AddMainService(lstMainService);  //lServices
                                _OtherServiceSelected.Add(itemSer);
                            }
                            else
                            {
                                IsValid = false;

                                ((CheckBox)sender).IsChecked = false;

                                string strMsg = "Services is only for : " + (itemSer).ApplicableToString;

                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW2.Show();
                            }
                        }

                    }


                }
                else
                {
                    if (IsTariffPackage == false)
                    {
                        if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                            AddMainService(lstMainService);  //lServices
                        _OtherServiceSelected.Add(itemSer);
                    }
                    else if (IsTariffPackage == true)
                    {
                        if (CheckGenderForPackage == true && (itemSer.ApplicableToString != "Both"))
                        {
                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                                AddMainService(lstMainService);  //lServices
                            _OtherServiceSelected.Add(itemSer);
                        }
                        else if (CheckGenderForPackage == false && (itemSer).ApplicableToString == "Both")
                        {
                            if (((clsServiceMasterVO)dgServiceList.SelectedItem).ID == itemSer.PackageServiceID && itemSer.ConditionType == "AND")
                                AddMainService(lstMainService);  //lServices
                            _OtherServiceSelected.Add(itemSer);
                        }
                        else
                        {
                            IsValid = false;
                            string strMsg = "Services is only for : " + (itemSer).ApplicableToString;

                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW2.Show();
                        }
                    }
                }

            }

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

        //private void rdbServices_Click(object sender, RoutedEventArgs e)
        //{

        //    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 7)
        //        GetDetailsifPatientIsCouple();
        //    else
        //        FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId, 0);
        //}

        //private void rdbPackage_Click(object sender, RoutedEventArgs e)
        //{
        //    ApplicablePack.Visibility = Visibility.Collapsed;
        //    cmbApplicabelPackage.Visibility = Visibility.Collapsed;
        //}

        private void GetDetailsifPatientIsCouple()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    //CoupleDetails.MalePatient = new clsPatientGeneralVO();
                    //CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                    //CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    //if (CoupleDetails.CoupleId != 0)
                    //{
                    //    IsCouple = true;
                    //}
                    //MalePatient.PatientID   --- FemalePatient.PatientID

                    //FillPackage(BizAction.CoupleDetails.MalePatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId, BizAction.CoupleDetails.FemalePatient.PatientID);

                    FillPackage(BizAction.CoupleDetails.FemalePatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //private void FillPackage(long PatientID1, long UnitId1, long PatientID2)
        private void FillPackage(long PatientID1, long UnitId1)
        {
            clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
            BizAction.PatientID1 = PatientID1;
            BizAction.PatientUnitID1 = UnitId1;
            BizAction.PatientSourceID = PatientSourceID;
            if (((MasterListItem)cmbCompany.SelectedItem).ID != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                BizAction.PatientCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            }
            if (((MasterListItem)cmbTariff.SelectedItem).ID != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0)
            {
                BizAction.PatientTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            }
            //BizAction.PatientID2 = PatientID2;
            //BizAction.PatientUnitID2 = UnitId1;
            BizAction.CheckDate = DateTime.Now.Date.Date;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

                    cmbApplicabelPackage.ItemsSource = null;
                    cmbApplicabelPackage.ItemsSource = objList;

                    if (objList.Count > 1)
                    {
                        ApplicablePack.Visibility = Visibility.Visible;
                        cmbApplicabelPackage.Visibility = Visibility.Visible;

                        //var list1 = from ls in objList
                        //            orderby ls.ID descending
                        //            select ls.ID;

                        var list1 = from ls in objList
                                    orderby ls.FilterID descending
                                    select ls.ID;

                        cmbApplicabelPackage.SelectedValue = list1.ToList()[0];
                    }
                    else
                    {
                        cmbApplicabelPackage.ItemsSource = null;
                        cmbApplicabelPackage.SelectedItem = objList[0];
                    }
                }
            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }

        private void rdbServices_Checked(object sender, RoutedEventArgs e)
        {
            // Fetch Tariff Wise Services 
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null && (MasterListItem)cmbCompany.SelectedItem != null)
            {
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                FetchData(ID);


                // Fetch Tariff Wise Services 


                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 7)
                    GetDetailsifPatientIsCouple();
                else
                    FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
            }

        }

        private void rdbPackage_Checked(object sender, RoutedEventArgs e)
        {
            ApplicablePack.Visibility = Visibility.Collapsed;
            cmbApplicabelPackage.Visibility = Visibility.Collapsed;

            // Fetch Tariff Wise Services 
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null)
            {
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                FetchData(ID);

            }
            // Fetch Tariff Wise Services 
        }

        private void rdbServices_Unchecked(object sender, RoutedEventArgs e)
        {
            rdbPackage.IsChecked = true;
        }

        private void rdbPackage_Unchecked(object sender, RoutedEventArgs e)
        {
            rdbServices.IsChecked = true;
        }

        private void cmbApplicabelPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
            {
                // This Block Fills All General Services For That Tariff
                long ID = 0;
                if ((MasterListItem)cmbTariff.SelectedItem != null)
                {
                    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                    FetchData(ID);

                }
                //END

                if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                {
                    cmbPendingServices.Visibility = Visibility.Visible;
                    // This Block Fills All Services For That Particulat Package
                    if ((MasterListItem)cmbTariff.SelectedItem != null)
                    {
                        ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                        FetchData(ID);

                    }
                    //END
                }
                else
                {
                    cmbPendingServices.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cmbPendingServices_Click(object sender, RoutedEventArgs e)
        {
            PackageServiceSearchForPackageNew serviceSearch = null;
            serviceSearch = new PackageServiceSearchForPackageNew();
            //serviceSearch.PatSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID; 

            serviceSearch.PatSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            serviceSearch.PatCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            serviceSearch.PatTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            serviceSearch.PatPackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
            //serviceSearch.OnAddButton_Click += new RoutedEventHandler(servicePackageSearch_OnAddButton_Click);
            serviceSearch.Show();
        }




    }

    public class clsServiceForPack1
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Tax { get; set; }
    }

}

