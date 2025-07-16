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
using PalashDynamics.ValueObjects.Administration.PackageNew;
using System.Windows.Data;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using OPDModule.Forms;

namespace CIMS.Forms
{
    public partial class ServiceSearchForPackage : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public List<clsServiceMasterVO> SelectedServices { get; set; }
        private ObservableCollection<clsServiceMasterVO> _OtherServiceSelected;
        public ObservableCollection<clsServiceMasterVO> SelectedOtherServices { get { return _OtherServiceSelected; } }
        public long PatientSourceID { get; set; }
        public long ClassID = 0;//{ get; set; }
        public long PatientTariffID { get; set; }
        bool IsPageLoded = false;
        bool IsTariffFisrtFill = true;
        PalashServiceClient client = null;
        PagedCollectionView collection;
        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }
        WaitIndicator Indicatior = null;
        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }
        long ChargeIDSend = 0;
        public long VisitID = 0;

        //Added By CDS 
        double SumNotInAdjHead = 0;

        PagedCollectionView collection2;    // Package New Changes for Process Added on 20042018

        #endregion

        public bool IsFromSaveAndPackageBill = false;
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

        //***//-----
        public long LinkPatientID;
        public long LinkPatientUnitID;
        public long LinkCompanyID;
        public long LinkTariffID;
        public long LinkPatientSourceID;
        //---------

        public ServiceSearchForPackage()
        {
            InitializeComponent();
            if (ClassID == 0)
            {
                ClassID = 1;
            }
            //ClassID = 1; // Default
            this.Loaded += new RoutedEventHandler(ServiceSearchForPackage_Loaded);  //ServiceSearch_Loaded         
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

                if (LinkPatientSourceID > 0)
                {
                    PatientSourceID = LinkPatientSourceID;
                }
                else
                {
                    PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                }
                FillPatientSponsorDetails();
                FillSpecialization();
                SetComboboxValue();
                txtServiceName.Focus();
                if (IsFromSaveAndPackageBill)
                {
                    rdbPackage.IsChecked = true;
                    rdbServices.IsEnabled = false;
                    rdbPackage.IsEnabled = false;
                }
                else
                {
                    rdbServices.IsChecked = true;
                    rdbPackage.IsEnabled = false;   // added on 01022018 to disable package radio button if this window open from Clinical Bill 
                }
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
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
            bool IsLinkPatient = false;
            if (LinkPatientSourceID > 0 )
            {
                if (cmbApplicabelPackage.SelectedItem == null)
                {
                    IsLinkPatient = true;
                }
                else if (cmbApplicabelPackage.SelectedItem != null)  
                {
                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID == 0)
                    {
                        IsLinkPatient = true;
                    }
                }

            }

            bool isValid = true;
            bool Ismultisponser = false;
            bool IscomboPackage = false;    // added on 13082016

            if (_OtherServiceSelected == null)
            {
                isValid = false;
            }
            else if (_OtherServiceSelected.Count <= 0)
            {
                isValid = false;
            }
            else if (_OtherServiceSelected.Count > 0)
            {

                var item5 = from r in _OtherServiceSelected
                            where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                            r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID && //r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID && // added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                            r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID

                            select r;
                if (item5.ToList().Count == 0)
                {
                    Ismultisponser = true;
                }

                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // added on 13082016
                {
                    var item52 = from r in _OtherServiceSelected
                                 where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                                 r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID && //r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID && // added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                                 r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID &&
                                 r.PackageID == ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID
                                 select r;

                    if (item52.ToList().Count == 0)     // added on 13082016
                    {
                        IscomboPackage = true;      // Sets true when non-package & package ; or multi - package services find while add
                    }
                }

                double x = SumNotInAdjHead;

                //foreach (var item in _OtherServiceSelected)
                //{
                //    if (item.PackageID > 0 && item.IsConsiderAdjustable==false && item.IsPackage==false)
                //    {
                //        SumNotInAdjHead = SumNotInAdjHead +Convert.ToDouble(item.Rate);
                //    }
                //}

                //foreach (var item in _OtherServiceSelected)
                //{
                //    //if (item.PackageID > 0 && item.IsPackage==true)
                //    //{
                //        item.SumOfExludedServices =Convert.ToDecimal(SumNotInAdjHead);
                //    //}
                //}

            }

            //if (Ismultisponser == false)                              // commented on 13082018
            if (IsLinkPatient == false)
            {
                if (Ismultisponser == false && IscomboPackage == false)     // added on 13082016
                {
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
                else
                {
                    string strMsg = "Multiple Sponsor Billing is not Allowed";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                IsLinkPatient = true;
                string strMsg = "Consume the services from patient package!";

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
            if (LinkPatientID > 0 && LinkPatientUnitID > 0)
            {
                BizAction.PatientID = LinkPatientID;
                BizAction.PatientUnitID = LinkPatientUnitID;
            }
            else
            {
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            }
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

        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();
                BizAction.SponsorID = 0;
                if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                {
                    BizAction.PatientID = LinkPatientID;
                    BizAction.PatientUnitID = LinkPatientUnitID;
                }
                else
                {
                    BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.ForEach(z => z.ID = z.PatientSourceID);  //// added by ashish Z. on Dated 260716 for checking the Multiple Sponsor Billing on on frmBill
                        cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
                        if (((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null || ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count > 0)
                        {
                            cmbPatientSource.SelectedValue = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails[((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails.Count - 1].PatientSourceID;
                        }
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

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            //PagedSortableCollectionView<clsServiceMasterVO> ObjList = new PagedSortableCollectionView<clsServiceMasterVO>();
            if (dgServiceList.ItemsSource != null)
            {
                PagedCollectionView ObjList;
                //ObjList = new PagedCollectionView(((PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource).ToList()); 
                ObjList = new PagedCollectionView(DataList); //PagedCollectionView(((PagedSortableCollectionView<clsServiceMasterVO>)dgServiceList.ItemsSource).ToList()); 

                if (ObjList != null && ObjList.Count > 0)
                {
                    if (chkSelectAll.IsChecked == true)
                    {
                        foreach (clsServiceMasterVO itemService in ObjList)
                        {
                            itemService.SelectService = true;
                            ////if (_ItemSelected == null)
                            ////    _ItemSelected = new ObservableCollection<clsServiceMasterVO>();
                            ////if (item.IsItemBlock == false) //&& !IsItemSuspend
                            ////    _ItemSelected.Add(item);


                            #region Checkbox select

                            ////itemService.ChargeID = ChargeIDSend;

                            if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                            {
                                itemService.ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                                itemService.PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                itemService.PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                            }
                            else
                            {
                                itemService.ChargeID = 0;
                                itemService.PackageBillID = 0;
                                itemService.PackageBillUnitID = 0;
                            }

                            bool IsValid = true;
                            if (itemService != null)     //if (dgServiceList.SelectedItem != null)
                            {
                                if (_OtherServiceSelected == null)
                                    _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

                                //CheckBox chk = (CheckBox)sender;
                                StringBuilder strError = new StringBuilder();
                                bool CheckGenderForPackage = false;
                                bool IsTariffPackage = false;

                                if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                                {
                                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                    {
                                        IsTariffPackage = true;
                                        //if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                                        if (itemService.ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && itemService.ApplicableToString != "Both")
                                        {
                                            CheckGenderForPackage = true;
                                        }
                                    }
                                }
                                if (cmbApplicabelPackage.SelectedItem != null)
                                {
                                    itemService.OPDConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OPDConsumption;
                                    itemService.OpdExcludeServiceConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OpdExcludeServiceConsumption;
                                }

                                # region Code when checkbox for selected service is checked
                                //if (chkSelectAll.IsChecked == true)     //if (chk.IsChecked == true)
                                //{
                                if (cmbCompany.SelectedItem != null)
                                {
                                    itemService.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;  //((clsServiceMasterVO)dgServiceList.SelectedItem).CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                                }
                                if (cmbPatientSource.SelectedItem != null)
                                {
                                    itemService.PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID;     //((clsServiceMasterVO)dgServiceList.SelectedItem).PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID;
                                }
                                if (cmbTariff.SelectedItem != null)
                                {
                                    itemService.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;    //((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                                }

                                if (_OtherServiceSelected.Count > 0)
                                {


                                    var item1 = from r in _OtherServiceSelected
                                                where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                                                r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
                                                r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID
                                                select r;

                                    bool IscomboPackage = false;

                                    if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // added on 13082016
                                    {
                                        var item12 = from r in _OtherServiceSelected
                                                     where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                                                     r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
                                                     r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID &&
                                                     r.PackageID == ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID
                                                     select r;

                                        if (item12.ToList().Count == 0)     // added on 13082016
                                        {
                                            IscomboPackage = true;      // Sets true when non-package & package ; or multi - package services find while add
                                        }
                                    }

                                    //if (item1.ToList().Count > 0)                             // commented on 13082016
                                    if (item1.ToList().Count > 0 && IscomboPackage == false)    // modified on 13082016
                                    {

                                        ////Commented By Bhushanp 18012017
                                        ////var item = from r in _OtherServiceSelected
                                        ////           where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID
                                        ////           select new clsServiceMasterVO
                                        ////           {
                                        ////               Status = r.Status,
                                        ////               ID = r.ID,
                                        ////               ServiceName = r.ServiceName
                                        ////           };

                                        if (false)
                                        {
                                            if (strError.ToString().Length > 0)
                                                strError.Append(",");
                                            strError.Append(itemService.ServiceName);      //strError.Append(((clsServiceMasterVO)dgServiceList.SelectedItem).ServiceName);

                                            if (!string.IsNullOrEmpty(strError.ToString()))
                                            {
                                                //string strMsg = "Services already Selected : " + strError.ToString();

                                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                //           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                //msgW1.Show();

                                                itemService.SelectService = false;     //((clsServiceMasterVO)dgServiceList.SelectedItem).SelectService = false;

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
                                                if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                    GetPackageConditionalServicesAndRelationsForAll(itemService);
                                                else
                                                    _OtherServiceSelected.Add(itemService);  //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                            }
                                            else if (IsTariffPackage == true)
                                            {
                                                if (CheckGenderForPackage == true && itemService.ApplicableToString != "Both")     //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                                                {
                                                    if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                        GetPackageConditionalServicesAndRelationsForAll(itemService);
                                                    else
                                                        _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                                }
                                                else if (CheckGenderForPackage == false && itemService.ApplicableToString == "Both")       //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                                                {
                                                    if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                        GetPackageConditionalServicesAndRelationsForAll(itemService);
                                                    else
                                                        _OtherServiceSelected.Add(itemService);  //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                                }
                                                else if (CheckGenderForPackage == false && itemService.ApplicableToString == "")      //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
                                                {

                                                    if (itemService.PackageServiceConditionID > 0)     // if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                        GetPackageConditionalServicesAndRelationsForAll(itemService);
                                                    else
                                                        _OtherServiceSelected.Add(itemService);  //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                                }
                                                else
                                                {
                                                    IsValid = false;

                                                    itemService.SelectService = false;       // ((CheckBox)sender).IsChecked = false;

                                                    string strMsg = "Services is only for : " + itemService.ApplicableToString;    //((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

                                                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    msgW2.Show();
                                                }
                                            }

                                        }

                                    }// block for sponser
                                    else
                                    {
                                        IsValid = false;
                                        itemService.SelectService = false;       // ((CheckBox)sender).IsChecked = false;

                                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                        msgW2.Show();
                                    }
                                }
                                else
                                {
                                    if (IsTariffPackage == false)
                                    {
                                        if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                            GetPackageConditionalServicesAndRelationsForAll(itemService);
                                        else
                                            _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                    }
                                    else if (IsTariffPackage == true)
                                    {
                                        if (CheckGenderForPackage == true && itemService.ApplicableToString != "Both")     //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                                        {
                                            if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                GetPackageConditionalServicesAndRelationsForAll(itemService);
                                            else
                                                _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                        }
                                        else if (CheckGenderForPackage == false && itemService.ApplicableToString == "Both")   //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "Both")
                                        {
                                            if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                GetPackageConditionalServicesAndRelationsForAll(itemService);
                                            else
                                                _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                        }
                                        else if (CheckGenderForPackage == false && itemService.ApplicableToString == "")   //&& ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
                                        {
                                            if (itemService.PackageServiceConditionID > 0)     //if (((clsServiceMasterVO)dgServiceList.SelectedItem).PackageServiceConditionID > 0)
                                                GetPackageConditionalServicesAndRelationsForAll(itemService);
                                            else
                                                _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                                        }
                                        else
                                        {
                                            IsValid = false;

                                            itemService.SelectService = false;  //((CheckBox)sender).IsChecked = false;

                                            string strMsg = "Services is only for : " + itemService.ApplicableToString;    //((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString;

                                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW2.Show();
                                        }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    _OtherServiceSelected.Remove(itemService);       //_OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);
                                //}
                                # endregion

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
                            #endregion

                        }
                    }
                    else
                    {
                        foreach (clsServiceMasterVO item in ObjList)
                        {
                            item.SelectService = false;

                            _OtherServiceSelected.Remove(item);       //_OtherServiceSelected.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);

                            //if (_ItemSelected != null)
                            //    _ItemSelected.Remove(item);
                        }
                    }
                    dgServiceList.ItemsSource = null;
                    dgServiceList.ItemsSource = ObjList;
                }
                else
                    chkSelectAll.IsChecked = false;
            }
        }

        private void GetPackageConditionalServicesAndRelationsForAll(clsServiceMasterVO itemService)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsGetPackageConditionalServicesNewBizActionVO BizAction = new clsGetPackageConditionalServicesNewBizActionVO();
                BizAction.ServiceConditionList = new List<clsServiceMasterVO>();
                BizAction.TariffID = (long)cmbTariff.SelectedValue; //TariffID;
                BizAction.ServiceID = itemService.ID;   // ((clsServiceMasterVO)dgServiceList.SelectedItem).ID;  //MainServiceID;
                BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId; //PatientID;
                BizAction.PatientUnitID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;  //PatientUnitID;
                BizAction.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;
                BizAction.PatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                BizAction.MemberRelationID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).MemberRelationID;
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
                                if (item.PackageServiceID == itemService.ID && item.ConditionType == "OR")     // if (item.PackageServiceID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID && item.ConditionType == "OR")
                                {
                                    item.TotalORUsedQuantity += item.ConditionalUsedQuantity;
                                    itemService.TotalORUsedQuantity = item.TotalORUsedQuantity;       //((clsServiceMasterVO)dgServiceList.SelectedItem).TotalORUsedQuantity = item.TotalORUsedQuantity;
                                }
                            }

                            if (IsAddMainService == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "You can't select this Service , as you have already select service from OR Conditional Services !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                foreach (var Service in DataList.Where(x => x.ID == itemService.ID))   //foreach (var Service in DataList.Where(x => x.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID))
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
                                _OtherServiceSelected.Add(itemService);      //_OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                            }
                        }
                        else
                        {
                            _OtherServiceSelected.Add(itemService);      // _OtherServiceSelected.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
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
                if (rdbServices.IsChecked == true)
                {
                    BizAction.IsPackage = false;
                    if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                    {
                        BizAction.ForFilterPackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                        BizAction.ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                    }

                }
                else if (rdbPackage.IsChecked == true)
                {
                    BizAction.IsPackage = true;
                }
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                {
                    BizAction.ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                    BizAction.IsPagingEnabled = false;
                    BizAction.StartIndex = DataList.PageIndex * 100;    // DataList.PageSize;
                    BizAction.MaximumRows = 100;    // DataList.PageSize;
                    BizAction.SearchExpression = " IsFavorite desc,servicename asc ";
                }
                else
                {
                    BizAction.IsPagingEnabled = true;
                    BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                    BizAction.MaximumRows = DataList.PageSize;
                    BizAction.SearchExpression = " IsFavorite desc,servicename asc ";
                }

                BizAction.VisitID = VisitID; // 

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
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("ProcessName"));          // Package New Changes for Process Added on 20042018

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
            bool IsValid = true;
            if (dgServiceList.SelectedItem != null)
            {

                if (LinkPatientID > 0)
                {
                    if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID <= 0)
                        {
                            IsValid = false;

                            ((CheckBox)sender).IsChecked = false;

                            string strMsg = "Services is only for Self ";

                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW2.Show();
                            return;
                        }
                    }
                }

                if (_OtherServiceSelected == null)
                    _OtherServiceSelected = new ObservableCollection<clsServiceMasterVO>();

                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                bool CheckGenderForPackage = false;
                bool IsTariffPackage = false;
                long geID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;
                if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
                {
                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                    {
                        IsTariffPackage = true;
                        if (((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableTo == ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString != "Both")
                        {
                            CheckGenderForPackage = true;
                        }

                        ////ChargeIDSend = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                    }
                }
                # region Code when checkbox for selected service is checked
                if (chk.IsChecked == true)
                {
                    if (cmbCompany.SelectedItem != null)
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                    }
                    if (cmbPatientSource.SelectedItem != null)
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID;
                    }
                    if (cmbTariff.SelectedItem != null)
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                    }
                    if (cmbApplicabelPackage.SelectedItem != null)
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).OPDConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OPDConsumption;
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).OpdExcludeServiceConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OpdExcludeServiceConsumption;
                    }
                    //((clsServiceMasterVO)dgServiceList.SelectedItem).ChargeID = ChargeIDSend;
                    if (((MasterListItem)cmbApplicabelPackage.SelectedItem) != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                    }
                    else
                    {
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).ChargeID = 0;
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageBillID = 0;
                        ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageBillUnitID = 0;
                    }

                    if (_OtherServiceSelected.Count > 0)
                    {


                        var item1 = from r in _OtherServiceSelected
                                    where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                                    r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
                                    r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID
                                    select r;

                        bool IscomboPackage = false;

                        if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)    // added on 13082016
                        {
                            var item12 = from r in _OtherServiceSelected
                                         where r.CompanyID == ((MasterListItem)cmbCompany.SelectedItem).ID &&
                                         r.PatientSourceID == ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID &&
                                         r.TariffID == ((MasterListItem)cmbTariff.SelectedItem).ID &&
                                         r.PackageID == ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID
                                         select r;

                            if (item12.ToList().Count == 0)     // added on 13082016
                            {
                                IscomboPackage = true;      // Sets true when non-package & package ; or multi - package services find while add
                            }
                        }

                        //if (item1.ToList().Count > 0)                             // commented on 13082016
                        if (item1.ToList().Count > 0 && IscomboPackage == false)     // modified on 13082016
                        {

                            //Commented By Bhushanp 18012017
                            //var item = from r in _OtherServiceSelected
                            //           where r.ID == ((clsServiceMasterVO)dgServiceList.SelectedItem).ID
                            //           select new clsServiceMasterVO
                            //           {
                            //               Status = r.Status,
                            //               ID = r.ID,
                            //               ServiceName = r.ServiceName
                            //           };

                            if (false)
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
                                    else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
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

                        }// block for sponser
                        else
                        {
                            IsValid = false;
                            ((CheckBox)sender).IsChecked = false;

                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW2.Show();
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
                            else if (CheckGenderForPackage == false && ((clsServiceMasterVO)dgServiceList.SelectedItem).ApplicableToString == "")
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
                }
                # endregion

                if (IsValid == true)
                {
                    foreach (var item in _OtherServiceSelected)
                    {
                        item.SelectService = true;
                    }

                    collection2 = new PagedCollectionView(_OtherServiceSelected);

                    //collection2.GroupDescriptions.Add(new PropertyGroupDescription("PatientCategoryL3"));
                    collection2.GroupDescriptions.Add(new PropertyGroupDescription("ProcessName"));          // Package New Changes for Process Added on 20042018

                    dgSelectedServiceList.ItemsSource = null;
                    //dgSelectedServiceList.ItemsSource = _OtherServiceSelected;    // Package New Changes for Process Added on 20042018
                    dgSelectedServiceList.ItemsSource = collection2;
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
                BizAction.PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
                BizAction.SponsorID = ((MasterListItem)cmbTariff.SelectedItem).SponsorID;
                BizAction.SponsorUnitID = ((MasterListItem)cmbTariff.SelectedItem).SponsorUnitID;
                BizAction.PatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                BizAction.MemberRelationID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).MemberRelationID;
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

        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            try
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
                            cmbCompany.SelectedValue = objList[0].ID;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;

                if (LinkCompanyID > 0)
                {
                    FillNewTariff(LinkCompanyID);
                }
                else
                {
                    FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);
                }

            }
        }

        private void FillNewTariff(long pCompanyID)
        {
            clsGetPatientSponsorTariffListBizActionVO BizAction = new clsGetPatientSponsorTariffListBizActionVO();
            if (LinkPatientID > 0 && LinkPatientUnitID > 0)
            {
                BizAction.PatientID = LinkPatientID;
                BizAction.PatientUnitID = LinkPatientUnitID;
            }
            else
            {
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            }

            BizAction.CheckDate = DateTime.Now.Date.Date;
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
                        cmbTariff.SelectedValue = objList[0].ID;
                        if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                        {
                            FillPackage(LinkPatientID, LinkPatientUnitID);
                        }
                        else
                        {
                            FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        }
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
                    long ProcessID = ((clsServiceMasterVO)dgSelectedServiceList.SelectedItem).ProcessID;                // Package New Changes for Process Added on 20042018

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

                    //foreach (var Service in DataList.Where(x => x.ID == ServiceID || x.ID == MainServiceID))      // Package New Changes for Process Commented on 20042018
                    foreach (var Service in DataList.Where(x => (x.ID == ServiceID && x.ProcessID == ProcessID) || (x.ID == MainServiceID && x.ProcessID == ProcessID)))    // Package New Changes for Process Added on 20042018
                    {
                        Service.SelectService = false;
                    }

                    collection = new PagedCollectionView(DataList);     // Package New Changes for Process Added on 20042018

                    collection.GroupDescriptions.Add(new PropertyGroupDescription("PatientCategoryL3"));    // Package New Changes for Process Added on 20042018
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("ProcessName"));          // Package New Changes for Process Added on 20042018

                    dgServiceList.ItemsSource = null;
                    //dgServiceList.ItemsSource = DataList;     // Package New Changes for Process Commented on 20042018
                    dgServiceList.ItemsSource = collection;     // Package New Changes for Process Added on 20042018
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

            //foreach (var item in _OtherServiceSelected)
            //{
            //if (item.PackageID > 0 && item.IsConsiderAdjustable == false && item.IsPackage == false && item.IsAdjustableHead==false)
            //{
            //    SumNotInAdjHead = SumNotInAdjHead + Convert.ToDouble(item.Rate);
            //}
            ////}

            ////foreach (var item in _OtherServiceSelected)
            ////{
            //if (item.PackageID > 0) // && item.IsPackage==true)
            //{
            //    item.SumOfExludedServices = Convert.ToDecimal(SumNotInAdjHead);
            //}
            //}
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            ConditionalServiceSearchForPackage ConditionalserviceSearch = null;
            ConditionalserviceSearch = new ConditionalServiceSearchForPackage();
            ConditionalserviceSearch.TariffID = ((clsServiceMasterVO)dgServiceList.SelectedItem).TariffID;
            ConditionalserviceSearch.PackageID = ((clsServiceMasterVO)dgServiceList.SelectedItem).PackageID;
            ConditionalserviceSearch.MemberRelationID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).MemberRelationID;
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
            lServices = ((ConditionalServiceSearchForPackage)sender).SelectedOtherServices.ToList();
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
            BizAction.PatientSourceID = PatientSourceID;
            if (((MasterListItem)cmbCompany.SelectedItem).ID != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                BizAction.PatientCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            }
            if (((MasterListItem)cmbTariff.SelectedItem).ID != null && ((MasterListItem)cmbTariff.SelectedItem).ID > 0)
            {
                BizAction.PatientTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            }
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
                        ////ChargeIDSend = objList[1].ChargeID;

                        if (objList.Count > 1)
                        {
                            ApplicablePack.Visibility = Visibility.Visible;
                            cmbApplicabelPackage.Visibility = Visibility.Visible;
                            //var list1 = from ls in objList
                            //            orderby ls.FilterID descending
                            //            select ls.ID;

                            //var list1 = from ls in objList
                            //            orderby ls.ChargeID descending
                            //            select ls.ID;

                            //cmbApplicabelPackage.SelectedValue = list1.ToList()[0];

                            //cmbApplicabelPackage.ItemsSource = null;
                            cmbApplicabelPackage.SelectedItem = objList[0];
                        }
                        else
                        {
                            cmbApplicabelPackage.ItemsSource = null;
                            cmbApplicabelPackage.SelectedItem = objList[0];
                        }                   
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void rdbServices_Checked(object sender, RoutedEventArgs e)
        {
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null && (MasterListItem)cmbCompany.SelectedItem != null)
            {
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                FetchData(ID);
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 7)
                {
                    GetDetailsifPatientIsCouple();
                }
                else
                {
                    if (LinkPatientID > 0 && LinkPatientUnitID > 0)
                    {
                        FillPackage(LinkPatientID, LinkPatientUnitID);
                    }
                    else
                    {
                        FillPackage(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                    }
                }
            }
        }

        private void rdbPackage_Checked(object sender, RoutedEventArgs e)
        {
            ApplicablePack.Visibility = Visibility.Collapsed;
            cmbApplicabelPackage.Visibility = Visibility.Collapsed;
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null)
            {
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                FetchData(ID);
            }
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
            //var cmb = (AutoCompleteBox)sender;
            //long TemChargeID = ((MasterListItem)cmb.SelectedItem).ChargeID;

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

                //if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                if (((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID > 0)
                {
                    cmbPendingServices.Visibility = Visibility.Visible;
                    cmdPrint.Visibility = Visibility.Visible;
                    // This Block Fills All Services For That Particulat Package
                    if ((MasterListItem)cmbTariff.SelectedItem != null)
                    {
                        ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                        FetchData(ID);
                        //Visible Select All items check box
                        chkSelectAll.Visibility = Visibility.Visible;
                    }
                    //END
                }
                else
                {
                    cmbPendingServices.Visibility = Visibility.Collapsed;
                    cmdPrint.Visibility = Visibility.Collapsed;
                    //Collapsed Select All items check box
                    chkSelectAll.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cmbPendingServices_Click(object sender, RoutedEventArgs e)
        {
            PackageServiceSearchForPackage serviceSearch = null;
            serviceSearch = new PackageServiceSearchForPackage();
            //serviceSearch.PatSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).ID; 

            serviceSearch.PatSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
            serviceSearch.PatCompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
            serviceSearch.PatTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            serviceSearch.PatPackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
            //serviceSearch.OnAddButton_Click += new RoutedEventHandler(servicePackageSearch_OnAddButton_Click);
            serviceSearch.Show();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //frmReceiptList receiptWin = new frmReceiptList();
            //receiptWin.BillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
            //receiptWin.BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
            //receiptWin.BillNo = ((clsBillVO)dgBillList.SelectedItem).BillNo;
            //receiptWin.Show();

            long PatientID = 0, PatientUnitID = 0, TariffID = 0, PackageID = 0;
            string PatientName = "", PackageName = "";
            PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
            PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
            {
                TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            }
            if ((MasterListItem)cmbApplicabelPackage.SelectedItem != null)
            {
                PackageID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID;
            }
            //TariffID = PatTariffID;
            //PackageID = PatPackageID;
            //PackageID = PatPackageID;

            PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
            PackageName = ((MasterListItem)cmbApplicabelPackage.SelectedItem).Description;

            if (true)      // Pending Services Report
            {
                string URL = "../Reports/OPD/AvailedAndPendingPackageServices.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID + "&TariffID=" + TariffID + "&PackageID=" + PackageID + "&PatientName=" + PatientName + "&PackageName=" + PackageName;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
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

