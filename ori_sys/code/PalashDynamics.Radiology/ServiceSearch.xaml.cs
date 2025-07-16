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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;

namespace PalashDynamics.Radiology
{
    public partial class ServiceSearch : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        
        public List<bool> check = new List<bool>();
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }

        bool IsPageLoded = false;

        private ObservableCollection<clsServiceMasterVO> _SelectedServices;
        public ObservableCollection<clsServiceMasterVO> SelectedServices { get { return _SelectedServices; } }

        public long PatientSourceID { get; set; }
        public long PatientTariffID { get; set; }
        public long Specialization { get; set; }

        WaitIndicator Indicatior = null;

        public ServiceSearch()
        {
            InitializeComponent();
        }

        private void ServiceDetails_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (_SelectedServices == null)
                    _SelectedServices = new ObservableCollection<clsServiceMasterVO>();

                this.DataContext = new clsServiceMasterVO();
                Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID;
               
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {
                    PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }
                else
                {
                    cmbPatientSource.IsEnabled = false;
                    cmbTariff.IsEnabled = false;
                    PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    PatientTariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;
                }
                FillPatientSponsorDetails();
                FillSpecialization();
                txtServiceName.Focus();
            }
            else
                txtServiceName.Focus();

            txtServiceName.UpdateLayout();
            IsPageLoded = true;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (_SelectedServices.Count==0)
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

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

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
                    cmbSpecialization.SelectedValue = Specialization;
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
                        cmbPatientSource.SelectedValue = PatientSourceID;
                                              
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                
            }
        }
        
        private void FillTariff()
        {
            clsGetPatientTariffsBizActionVO BizAction = new clsGetPatientTariffsBizActionVO();

            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.CheckDate = DateTime.Now.Date.Date;
            //BizAction.PatientSourceID = PatientSourceID;

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
                        cmbTariff.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                        cmbTariff.SelectedValue = objList[0].ID;
                        FetchData((long)cmbTariff.SelectedValue);
                        
                    }
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSelfTariff()
        {  
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
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

                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedValue = PatientTariffID;
                    FetchData((long)cmbTariff.SelectedValue);
                }
                
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null) ;
            FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPatientSource.SelectedItem != null)
            {
               if(PatientTariffID >0)
               {
                   FillSelfTariff();

               }
               else
               {
                 PatientSourceID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceID;
                 FillTariff();
               }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
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
               
                BizAction.Specialization = Specialization;
                
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


              
                BizAction.TariffID = pTariffID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                    {
                        if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                        {
                            dgServiceList.ItemsSource = null;
                            dgServiceList.ItemsSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                            ServiceItemSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
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

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceList.SelectedItem != null)
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)

                    _SelectedServices.Add((clsServiceMasterVO)dgServiceList.SelectedItem);
                else
                    _SelectedServices.Remove((clsServiceMasterVO)dgServiceList.SelectedItem);

            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            long ID = 0;

            if ((MasterListItem)cmbTariff.SelectedItem != null)
                ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            FetchData(ID);
        }
       

    }
}

