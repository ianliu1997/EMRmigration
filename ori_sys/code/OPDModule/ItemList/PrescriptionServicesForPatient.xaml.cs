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
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Windows.Data;

namespace OPDModule
{
    public partial class PrescriptionServicesForPatient : ChildWindow
    {

        public long PatientSourceID { get; set; }
        public long PatientTariffID { get; set; }
        bool IsTariffFisrtFill = true;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public event RoutedEventHandler OnAddButton_Click;
        public List<bool> check = new List<bool>();
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public List<clsServiceMasterVO> SelectedServices { get; set; }



        public PagedSortableCollectionView<clsServiceMasterVO> DataList { get; private set; }

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

        PagedCollectionView collection2;    // Package New Changes for Process Added on 20042018

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            long ID = 0;
            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            FetchData(ID);
        }

        public PrescriptionServicesForPatient()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        private void PrescriptionServices_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsServiceMasterVO();
                PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                FillPatientSponsorDetails();
                txtServiceName.Focus();
                rdbServices.IsChecked = true;   // Package New Changes for Process Added on 23042018
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;

        }
        
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
        }

        //added by vikrant for ipd grouping
        ObservableCollection<clsServiceMasterVO> getpastServiceList { get; set; }
        private void FetchData(long pTariffID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                //....................
                BizAction.PrescribedService = true;
                //.........................
                if (txtServiceName.Text != null)
                    BizAction.ServiceName = txtServiceName.Text;
                if (cmbPatientSource.SelectedItem != null)
                {
                    BizAction.PatientSourceType = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceType;
                    BizAction.PatientSourceTypeID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceTypeID;
                    BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
                    BizAction.PatientUnitID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;
                }

                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows1 = DataList.PageSize;

                BizAction.TariffID = pTariffID;
                BizAction.GetSuggestedServices = true;
                if (!IsOPDIPD)
                {
                    BizAction.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                }
                else
                {
                    BizAction.VisitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                }
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.IsOPDIPD = IsOPDIPD;

                #region Package New Changes for Process Added on 24042018

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


                if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                {
                    BizAction.ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                }

                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgPrescriptionServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {
                            if (!IsOPDIPD)
                            {
                                //DataList = null;
                                // DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
                                dgPrescriptionServiceList.ItemsSource = null;
                                check.Clear();
                                DataList.TotalItemCount = ((clsGetTariffServiceListBizActionVO)arg.Result).TotalRows;
                                DataList.Clear();
                                for (int i = 0; i < ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count; i++)
                                {
                                    if (cmbCompany.SelectedItem != null)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                                    }
                                    if (cmbPatientSource.SelectedItem != null)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                                    }
                                    if (cmbTariff.SelectedItem != null)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                                    }
                                    ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].IsPrescribeServiceFromEMR = true;

                                    if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].IsBilledEMR)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].VisibleBill = "Visible";
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].collapseBill = "Collapsed";
                                    }
                                    else
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].VisibleBill = "Collapsed";
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].collapseBill = "Visible";
                                    }

                                    ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].Datetime = String.Format(((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].RoundDate.ToString("dd/MM/yyyy"));

                                    #region Package New Changes for Process Added on 24042018

                                    if (cmbApplicabelPackage.SelectedItem != null && ((MasterListItem)cmbApplicabelPackage.SelectedItem).ID > 0)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].OPDConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OPDConsumption;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].OpdExcludeServiceConsumption = ((MasterListItem)cmbApplicabelPackage.SelectedItem).OpdExcludeServiceConsumption;

                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].ChargeID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).ChargeID;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].PackageBillID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillID;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].PackageBillUnitID = ((MasterListItem)cmbApplicabelPackage.SelectedItem).PackageBillUnitID;
                                    }
                                    else
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].ChargeID = 0;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].PackageBillID = 0;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].PackageBillUnitID = 0;
                                    }

                                    #endregion

                                    bool b = false;
                                    check.Add(b);
                                    //***//
                                    if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].Billed || ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].InvestigationBilled)
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].SelectService = true;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].IsBillEnabled = false;
                                    }
                                    else
                                    {
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].SelectService = false;
                                        ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i].IsBillEnabled = true;
                                    }
                                    //

                                    DataList.Add(((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[i]);
                                }
                                ServiceItemSource = DataList.ToList();
                                getpastServiceList = new ObservableCollection<clsServiceMasterVO>(ServiceItemSource);
                                PagedCollectionView pcvPastInvest = new PagedCollectionView(getpastServiceList);
                                pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("ProcessName"));      // Package New Changes for Process Added on 23042018
                                dgPrescriptionServiceList.ItemsSource = null;
                                dgPrescriptionServiceList.ItemsSource = pcvPastInvest;
                                dgPrescriptionServiceList.UpdateLayout();
                                pgrPatientDiagnosis.Source = null;
                                pgrPatientDiagnosis.PageSize = BizAction.MaximumRows1;
                                pgrPatientDiagnosis.Source = DataList;
                            }
                            else
                            {
                                //DataList = null;
                                // added by vikrant for get ipd prescription service
                               // DataList = new PagedSortableCollectionView<clsServiceMasterVO>();
                                DataList.TotalItemCount = ((clsGetTariffServiceListBizActionVO)arg.Result).TotalRows;
                                DataList.Clear();
                                foreach (var item in ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList)
                                {
                                    item.Datetime = String.Format(item.RoundDate.ToString("dd/MM/yyyy") + " - " + item.DoctorName.Trim() + " - " + item.SpecializationString);

                                    if (cmbCompany.SelectedItem != null)
                                    {
                                        item.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                                    }
                                    if (cmbPatientSource.SelectedItem != null)
                                    {
                                        item.PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                                    }
                                    if (cmbTariff.SelectedItem != null)
                                    {
                                        item.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                                    }
                                    if (item.IsBilledEMR)
                                    {
                                        item.VisibleBill = "Visible";
                                        item.collapseBill = "Collapsed";
                                    }
                                    else
                                    {
                                        item.VisibleBill = "Collapsed";
                                        item.collapseBill = "Visible";
                                    }
                                    item.IsPrescribeServiceFromEMR = true;
                                    //***//
                                    if (item.Billed || item.InvestigationBilled)
                                    {
                                        item.SelectService = true;
                                        item.IsBillEnabled = false;
                                    }
                                    else
                                    {
                                        item.SelectService = false;
                                        item.IsBillEnabled = true;
                                    }
                                    //
                                    DataList.Add(item);
                                }
                                ServiceItemSource = DataList.ToList(); //((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                                getpastServiceList = new ObservableCollection<clsServiceMasterVO>(ServiceItemSource);
                                PagedCollectionView pcvPastInvest = new PagedCollectionView(getpastServiceList);
                                pcvPastInvest.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                                dgPrescriptionServiceList.ItemsSource = null;
                                dgPrescriptionServiceList.ItemsSource = pcvPastInvest;
                                dgPrescriptionServiceList.UpdateLayout();
                                pgrPatientDiagnosis.Source = null;
                                pgrPatientDiagnosis.PageSize = BizAction.MaximumRows1;
                                pgrPatientDiagnosis.Source = DataList;
                                for (int i = 0; i < ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count; i++)
                                {
                                    bool b = false;
                                    check.Add(b);
                                }
                            }

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
        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);
        //    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        //}
        
        #region FillCombobox
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
            client.ProcessAsync(BizAction, new clsUserVO());
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
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public long? PatientCategoryL1Id_Retail = 0;

        public long PackageTariffID { get; set; }
        public long ServiceTariffID { get; set; }
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
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        //commented by rohini dated 26.5.16

        //public void FillPatientSponsorDetails()
        //{
        //    try
        //    {
        //        clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

        //        BizAction.SponsorID = 0;
        //        BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //        BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
        //            {
        //                cmbPatientSource.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;
        //                cmbPatientSource.SelectedValue = PatientSourceID;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
        //                FillCompany();
        //            }
        //        };
        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        //Indicatior.Close();
        //        // throw;
        //    }
        //}


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

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPatientSource.SelectedItem != null)
            {
                PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                FillTariff();

            }
        }

        #endregion
      

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                if(dgPrescriptionServiceList.SelectedItem!=null)
                    if (((clsServiceMasterVO)dgPrescriptionServiceList.SelectedItem).ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Service is not available in clinic", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                       // ((clsServiceMasterVO)dgPrescriptionServiceList.SelectedItem).SelectService = false;
                        ServiceItemSource = (List<clsServiceMasterVO>)dgPrescriptionServiceList.ItemsSource;

                        ServiceItemSource[dgPrescriptionServiceList.SelectedIndex].SelectService = false;

                        dgPrescriptionServiceList.ItemsSource = null;
                        dgPrescriptionServiceList.ItemsSource = ServiceItemSource;
                    }

                    else
                    {
                        check[dgPrescriptionServiceList.SelectedIndex] = true;
                    }
            }
            else
            {
                check[dgPrescriptionServiceList.SelectedIndex] = false;
            }

        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            SelectedServices = new List<clsServiceMasterVO>();

            for (int i = 0; i < check.Count; i++)
            {
                if (check[i])
                {
                    SelectedServices.Add(ServiceItemSource[i]);
                }
                   
            }
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());

        }

        private void txtServiceName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtServiceName.Text = txtServiceName.Text.ToTitleCase();
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            {
                //PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                FillNewTariff(((MasterListItem)cmbCompany.SelectedItem).ID);

            }
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

        private void rdbServices_Checked(object sender, RoutedEventArgs e)
        {
            // Fetch Tariff Wise Services 
            long ID = 0;
            //if ((MasterListItem)cmbTariff.ItemsSource != null && (MasterListItem)cmbCompany.ItemsSource != null)      // Package New Changes for Process Commented on 20042018
            //{

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
            //}   // Package New Changes for Process Commented on 20042018
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

        private void rdbServices_Unchecked(object sender, RoutedEventArgs e)
        {
            rdbPackage.IsChecked = true;
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

        private void rdbPackage_Unchecked(object sender, RoutedEventArgs e)
        {
            rdbServices.IsChecked = true;
        }
       

        
    }
}

