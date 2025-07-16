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

using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using CIMS;

namespace PalashDynamics.OperationTheatre
{
    public partial class ServiceSearch : ChildWindow
    {
            public event RoutedEventHandler OnAddButton_Click;
            public List<bool> check = new List<bool>();

            public List<clsServiceMasterVO> ServiceItemSource { get; set; }

            public List<clsServiceMasterVO> SelectedServices { get; set; }

            public long PatientSourceID { get; set; }

            public long PatientID { get; set; }

            public long PatientUnitID { get; set; }
            public long PatientSourceID1 { get; set; }
            public long TariffID { get; set; }
            
            
            
            public long PatientTariffID { get; set; }
            
            bool IsPageLoded = false;
            bool IsTariffFisrtFill = true;
            WaitIndicator Indicatior = null;

            public ServiceSearch()
            {
                InitializeComponent();
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
                        //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
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
                                cmbSubSpecialization.IsEnabled =false;
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

            private void ServiceDetails_Loaded(object sender, RoutedEventArgs e)
            {
                if (!IsPageLoded)
                {
                    this.DataContext = new clsServiceMasterVO();

                    PatientSourceID = PatientSourceID1;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                    FillPatientSponsorDetails();
                   // FillTariff(true);
                    FillSpecialization();
                    SetComboboxValue();
                   
                    txtServiceName.Focus();
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

            private void cmdAdd_Click(object sender, RoutedEventArgs e)
            {
                SelectedServices = new List<clsServiceMasterVO>();

                for (int i = 0; i < check.Count; i++)
                {
                    if (check[i])
                    {
                        SelectedServices.Add(ServiceItemSource[i]);
                        //    new clsServiceMasterVO()
                        //{
                        //    //ID = ServiceItemSource[i].ID,
                        //    //Description = ServiceItemSource[i].ServiceName,
                        //    //TariffServiceMasterID = ServiceItemSource[i].TariffServiceMasterID,
                        //    //TariffID = ServiceItemSource[i].TariffID,
                        //    //Specialization = ServiceItemSource[i].Specialization,
                        //    //Rate = ServiceItemSource[i].Rate, Code = ServiceItemSource[i].ServiceCode
                        //});
                    }
                }

                if (SelectedServices.Count == 0)
                {
                    string strMsg = "No Service/s Selected for Adding";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    this.DialogResult = true;
                    if (OnAddButton_Click != null)
                        OnAddButton_Click(this, new RoutedEventArgs());

                }

            }

            private void cmdSearch_Click(object sender, RoutedEventArgs e)
            {
                long ID = 0 ;

                if((MasterListItem)cmbTariff.SelectedItem != null)
                    ID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                FetchData(ID);
            }
            
            private void FillTariff()
            {
                clsGetPatientTariffsBizActionVO BizAction = new clsGetPatientTariffsBizActionVO();

                BizAction.PatientID = PatientID;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = PatientUnitID;//((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
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
                        //objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetPatientTariffsBizActionVO)arg.Result).MasterList);
                         cmbTariff.ItemsSource = null;
                            //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                            cmbTariff.ItemsSource = objList;
                        if (objList.Count > 0)
                        {

                            if (IsTariffFisrtFill)
                            {
                                cmbTariff.SelectedValue = TariffID;//((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
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
        
            public void FillPatientSponsorDetails()
            {
                try
                { 
                    clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

                    BizAction.SponsorID = 0;
                    BizAction.PatientID = PatientID;//((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction.PatientUnitID = PatientUnitID;//((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
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
                catch (Exception)
                {
                    //Indicatior.Close();
                    // throw;
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
                        if (pTariffID == TariffID)//((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                            BizAction.PatientSourceType = 0;
                        else
                            BizAction.PatientSourceType = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceType;
                       
                        BizAction.PatientSourceTypeID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientSourceTypeID;
                        BizAction.PatientID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).PatientId;
                        BizAction.PatientUnitID = ((clsPatientSponsorVO)cmbPatientSource.SelectedItem).UnitId;
                    }


                   // if (cmbTariff.SelectedItem != null)
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
                                check.Clear();
                                dgServiceList.ItemsSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                                ServiceItemSource = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList;
                                
                                for (int i = 0; i < ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count; i++)
                                {
                                    bool b = false;
                                    check.Add(b);
                                }
                            }
                        }
                        else
                        {
                            //HtmlPage.Window.Alert("Error occured while processing.");
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
                if ((MasterListItem)cmbSpecialization.SelectedItem != null) ;
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
            }

            private void chkService_Click(object sender, RoutedEventArgs e)
            {
                if (((CheckBox)sender).IsChecked == true)
                {
                    
                    check[dgServiceList.SelectedIndex] = true;
                }
                else
                {
                    check[dgServiceList.SelectedIndex] = false;
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
                   // if (IsTariffFisrtFill)
                        FillTariff();
                    //else
                    //    FillTariff(true);
                }
            }

        }

    public class clsService
    {
        public long ID { get; set; }
        public string Code{ get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public double Tax { get; set; }
    }
    
}

