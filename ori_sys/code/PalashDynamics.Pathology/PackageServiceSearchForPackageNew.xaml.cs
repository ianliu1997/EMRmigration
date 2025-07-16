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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Text;
using CIMS.Forms;
using CIMS;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration.PackageNew;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.Pathology
{
    public partial class PackageServiceSearchForPackageNew : ChildWindow
    {
        #region Variable Declaration

        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();

        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        public List<clsServiceMasterVO> SelectedServices { get; set; }

        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }

        public long PatientSourceID { get; set; }

        public long ClassID { get; set; }
        public long PatientTariffID { get; set; }

        // Added  By CDS
        public long PatSourceID = 0;
        public long PatCompanyID = 0;
        public long PatTariffID = 0;
        public long PatPackageID = 0;
        StringBuilder sbPackageList = new StringBuilder();

        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;

        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

        WaitIndicator Indicatior = null;

        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }

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

        #endregion
        public PackageServiceSearchForPackageNew()
        {
            InitializeComponent();

            ClassID = 1; // Default

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

        private void PackageServiceSearchForPackage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();

                PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                FillPatientSponsorDetails();
                FillSpecialization();
                SetComboboxValue();
                //FillPackage();                
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 7)
                    GetDetailsifPatientIsCouple();
                else
                    FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);

                txtServiceName.Focus();
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }

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

                    //Passing The Arument Only In Case Couple then  Female PatientID And UnitID 
                    FillPackage(BizAction.CoupleDetails.FemalePatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillPackage(long PatientID1, long UnitId1)
        {
            clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
            BizAction.PatientID1 = PatientID1;
            BizAction.PatientUnitID1 = UnitId1;
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

                    //StringBuilder sbPackageList = new StringBuilder();
                    //Package ID List 
                    foreach (var item in objList)
                    {
                        if (item.ID > 0)
                        {
                            sbPackageList.Append("," + Convert.ToString(item.ID));
                        }
                    }
                    sbPackageList.Replace(",", "", 0, 1);
                    //Package ID List 

                    if (objList.Count > 1)
                    {
                        ApplicablePack.Visibility = Visibility.Visible;
                        cmbApplicabelPackage.Visibility = Visibility.Visible;

                        var list1 = from ls in objList
                                    orderby ls.FilterID descending
                                    select ls.ID;

                        cmbApplicabelPackage.SelectedValue = list1.ToList()[0];
                    }
                }
            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
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
                        if (((MasterListItem)cmbTariff.SelectedItem).ID > 0 && ((MasterListItem)cmbTariff.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                            GetFollowUpStatus();
                    }
                }




            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }


        public void GetFollowUpStatus()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetFollowUpStatusByPatientIdBizActionVO BizAction = new clsGetFollowUpStatusByPatientIdBizActionVO();

                BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;  //((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        bool IsFollowUpAdded = true;
                        bool IsPackageDetailsAdded = true;

                        IsFollowUpAdded = ((clsGetFollowUpStatusByPatientIdBizActionVO)arg.Result).IsFollowUpAdded;
                        IsPackageDetailsAdded = ((clsGetFollowUpStatusByPatientIdBizActionVO)arg.Result).IsPackageDetailsAdded;

                        if (IsFollowUpAdded == false || IsPackageDetailsAdded == false)
                        {
                            cmdFollowUp.Visibility = Visibility.Visible;
                        }
                        else
                            cmdFollowUp.Visibility = Visibility.Collapsed;
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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

        public long? PatientCategoryL1Id_Retail = 0;



        PalashServiceClient client = null;
        private void FetchData(long pTariffID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetPackageDetailsBizActionVO BizAction = new clsGetPackageDetailsBizActionVO();
                BizAction.PendingServiceList = new List<clsServiceMasterVO>();

                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbSpecialization.SelectedItem != null)
                    BizAction.Specialization = ((MasterListItem)cmbSpecialization.SelectedItem).ID;
                if (cmbSubSpecialization.SelectedItem != null)
                    BizAction.SubSpecialization = ((MasterListItem)cmbSubSpecialization.SelectedItem).ID;


                BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
                BizAction.PatientUnitID = BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId; //((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;

                string Age = null;
                Age = ConvertDate(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth, "YY");
                if (Age != null && Age != "")
                {
                    BizAction.Age = Convert.ToInt16(Age);
                }
                BizAction.TariffID = pTariffID;
                PackageTariffID = pTariffID;
                //BizAction.PackageID = PatPackageID;
                // For Pending Services Selected Package ID 
                if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                {
                    BizAction.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                }
                else
                    BizAction.PackageID = PatPackageID;

                // For Availed Services Selected All PackageID 
                //StringBuilder sbPackageList = new StringBuilder();
                //foreach (var item in ((MasterListItem)cmbApplicabelPackage.ItemsSource))
                //{
                //    if (item.ID > 0)
                //    {                 
                //        sbPackageList.Append("," + Convert.ToString(item.ID));
                //    }
                //}                
                //sbPackageList.Replace(",", "", 0, 1);
                BizAction.PackageIDList = Convert.ToString(sbPackageList);
                //

                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.SearchExpression = " Servicename asc ";
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPackageDetailsBizActionVO)arg.Result).PendingServiceList != null)
                    {
                        clsGetPackageDetailsBizActionVO result = arg.Result as clsGetPackageDetailsBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        if (result.PendingServiceList != null)
                        {
                            foreach (var item in result.PendingServiceList)
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

                        List<clsServiceMasterVO> AvailedServiceList = result.AvailedServiceList;
                        if (AvailedServiceList != null)
                        {
                            var NewList = from r in AvailedServiceList
                                          orderby r.ExpiryDate descending
                                          select r;

                            PagedCollectionView SortableList = new PagedCollectionView(NewList);
                            SortableList.GroupDescriptions.Add(new PropertyGroupDescription("Package_Name"));
                            SortableList.GroupDescriptions.Add(new PropertyGroupDescription("Patient_Name"));
                            if (SortableList.ItemCount != 0)
                            {
                                dgUsedServicesList.ItemsSource = null;
                                dgUsedServicesList.ItemsSource = SortableList;
                            }

                        }
                        else
                        {
                            dgUsedServicesList.ItemsSource = null;
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
                bool CheckGenderForPackage = false;
                bool IsTariffPackage = false;

                if (((MasterListItem)cmbTariff.SelectedItem).Code == "Package")
                {
                    IsTariffPackage = true;

                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                    {
                        CheckGenderForPackage = true;
                    }
                }

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

                            if (IsTariffPackage == false)
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                    GetPackageConditionalServicesAndRelations();
                                else
                                    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            }
                            else if (IsTariffPackage == true)
                            {
                                if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                                {
                                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                        GetPackageConditionalServicesAndRelations();
                                    else
                                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                }
                                else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                                {
                                    if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                        GetPackageConditionalServicesAndRelations();
                                    else
                                        _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                }
                                else
                                {
                                    IsValid = false;

                                    ((CheckBox)sender).IsChecked = false;

                                    string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

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
                            _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                        }
                        else if (IsTariffPackage == true)
                        {
                            if (CheckGenderForPackage == true && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                    GetPackageConditionalServicesAndRelations();
                                else
                                    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            }
                            else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                            {
                                if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                    GetPackageConditionalServicesAndRelations();
                                else
                                    _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            }
                            else
                            {
                                IsValid = false;

                                ((CheckBox)sender).IsChecked = false;

                                string strMsg = "Services is only for : " + ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

                                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW2.Show();
                            }
                        }
                    }
                }
                else
                {
                    _OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);
                    chkSelAllService.IsChecked = false;
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

                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

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

                                dgServiceList.ItemsSource = DataList; //collection; 


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

        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

                BizAction.SponsorID = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");   // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        //cmbPatientSource.SelectedValue = PatientSourceID;  //((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                        cmbPatientSource.SelectedValue = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails[0].PatientSourceID;
                        //foreach (var item in ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails)
                        //{
                        //    if (PatientSourceID == item.PatientSourceID)
                        //    {
                        //        PatientCategoryL1Id_Retail = item.PatientCategoryID;
                        //    }
                        //}
                        cmbPatientSource.SelectedValue = PatSourceID;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientSource.SelectedItem != null)
            //{
            //    PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;                
            //    FillTariff();                
            //}

            // Newly 
            if (cmbPatientSource.SelectedItem != null)
            {
                PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                PatientCategoryL1Id_Retail = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientCategoryID;
                //PatientSourceID = long(cmbPatientSource.SelectedValue);
                FillCompany();
            }
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
                    cmbCompany.SelectedValue = PatCompanyID;

                    if (objList.Count > 0)
                    {

                        if (IsTariffFisrtFill)
                        {
                            cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                            IsTariffFisrtFill = false;
                        }
                        else
                            cmbCompany.SelectedValue = objList[0].ID;
                        //FetchData((long)cmbTariff.SelectedValue);
                    }
                }
            };
            client.ProcessAsync(BizAction, App.SessionUser);
            client.CloseAsync();
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;

                FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);

            }
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
                    cmbTariff.SelectedValue = PatTariffID;
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

        #region Commented Becouse Selected Package Wise Serch Is Required So
        //private void FillPackage()
        //{
        //    clsGetPatientPackageInfoListBizActionVO BizAction = new clsGetPatientPackageInfoListBizActionVO();
        //    BizAction.PatientID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //    BizAction.PatientUnitID1 = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //    BizAction.PatientID2 = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //    BizAction.PatientUnitID2 = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetPatientPackageInfoListBizActionVO)arg.Result).MasterList);

        //            cmbApplicabelPackage.ItemsSource = null;
        //            cmbApplicabelPackage.ItemsSource = objList;

        //            if (objList.Count > 1)
        //            {
        //                ApplicablePack.Visibility = Visibility.Visible;
        //                cmbApplicabelPackage.Visibility = Visibility.Visible;

        //                var list1 = from ls in objList
        //                            orderby ls.ID descending
        //                            select ls.ID;

        //                //var query = from person in people
        //                //orderby person.Name descending, person.Age descending
        //                //select person.Name;
        //                cmbApplicabelPackage.SelectedValue = list1.ToList()[0];
        //                //cmbApplicabelPackage.SelectedValue = objList[0].ID;
        //            }

        //            cmbApplicabelPackage.SelectedValue = PatPackageID;
        //        }
        //    };
        //    client.ProcessAsync(BizAction, App.SessionUser);
        //    client.CloseAsync();
        //}
        #endregion

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

                    chkSelAllService.IsChecked = false;
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

                if ((itemSer).Code == "Package")
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
                                AddMainService(lstMainService); //lServices
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
                                    AddMainService(lstMainService); //lServices
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
                                AddMainService(lstMainService); //lServices
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


        private void chkSelAllService_Click(object sender, RoutedEventArgs e)
        {
            if (chkSelAllService.IsChecked == true)
            {

                foreach (var Service in DataList)
                {
                    Service.SelectService = true;
                }
                dgServiceList.ItemsSource = null;
                dgServiceList.ItemsSource = DataList;
                dgServiceList.UpdateLayout();
                dgServiceList.Focus();

                if (_OtherServiceSelected != null)
                    _OtherServiceSelected.Clear();
                _OtherServiceSelected = DataList.DeepCopy();
                dgSelectedServiceList.ItemsSource = null;
                dgSelectedServiceList.ItemsSource = _OtherServiceSelected;
                dgSelectedServiceList.UpdateLayout();
                dgSelectedServiceList.Focus();
            }
        }


        private void cmdFollowUp_Click(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                cmdFollowUp.IsEnabled = false;

                clsAddFollowUpStatusByPatientIdBizActionVO BizAction = new clsAddFollowUpStatusByPatientIdBizActionVO();

                BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;  //((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;

                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                BizAction.AddedDateTime = DateTime.Now;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {


                        cmdFollowUp.Visibility = Visibility.Collapsed;

                        if ((MasterListItem)cmbTariff.SelectedItem != null)
                        {
                            if (cmbTariff.SelectedItem != null)
                                FetchData(((MasterListItem)cmbTariff.SelectedItem).ID);
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Follow Ups And Package Details are added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                // throw;
                Indicatior.Close();
            }
            finally
            {
                cmdFollowUp.Visibility = Visibility.Collapsed;
                Indicatior.Close();
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long PatientID = 0, PatientUnitID = 0, TariffID = 0, PackageID = 0;
            string PatientName = "", PackageName = "";
            PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
            PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            TariffID = PatTariffID;
            //PackageID = PatPackageID;
            PackageID = PatPackageID;

            PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
            PackageName = ((MasterListItem)cmbApplicabelPackage.SelectedItem).Description;

            if (PackageService.SelectedIndex == 0)      // Pending Services Report
            {
                string URL = "../Reports/OPD/PendingPackageServices.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&TariffID=" + TariffID + "&PackageID=" + PackageID + "&PatientName=" + PatientName + "&PackageName=" + PackageName;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }

        private void cmdPrint1_Click(object sender, RoutedEventArgs e)
        {
            long PatientID = 0, PatientUnitID = 0, TariffID = 0, PackageID = 0;
            string PatientName = "", PackageName = "";
            PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
            PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            TariffID = PatTariffID;
            PackageID = PatPackageID;
            PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
            PackageName = ((MasterListItem)cmbApplicabelPackage.SelectedItem).Description;

            string URL = "../Reports/OPD/AvailedPackageServices.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&TariffID=" + TariffID + "&PackageID=" + PackageID + "&PatientName=" + PatientName + "&PackageName=" + PackageName;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        private void cmbApplicabelPackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



    }
}

