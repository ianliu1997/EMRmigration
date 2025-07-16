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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.CRM
{
    public partial class EditFamilyTariff : ChildWindow
    {
        public long TariffID { get; set; }
        public long RelationID { get; set; }
        public long LoyaltyID { get; set; }
        public List<clsPatientServiceDetails> ServiceList { get; set; }
        public List<clsPatientServiceDetails> lstService { get; set; }
        public event RoutedEventHandler OnSaveButton_Click;

       
        public EditFamilyTariff()
        {
            InitializeComponent();
        }
        
        private void EditFamilyTariff_Loaded(object sender, RoutedEventArgs e)
        {
            
           
        }
       
        private void GetServiceByTariffID()
        {
            try
            {

                clsGetServiceForIssueBizActionVO BizAction = new clsGetServiceForIssueBizActionVO();
                BizAction.ServiceList = new List<clsPatientServiceDetails>();
                BizAction.TariffID = TariffID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgTariffServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetServiceForIssueBizActionVO)arg.Result).ServiceList != null)
                        {
                            BizAction.ServiceList = ((clsGetServiceForIssueBizActionVO)arg.Result).ServiceList;


                            foreach (var item in BizAction.ServiceList)
                            {
                                lstService.Add(new clsPatientServiceDetails
                                {
                                    ServiceID = item.ServiceID,
                                    ServiceCode = item.ServiceCode,
                                    ServiceName = item.ServiceName,
                                    Rate = item.Rate,
                                    SelectService = item.SelectService,
                                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                                    RelationID = RelationID,
                                    LoyaltyID = LoyaltyID,
                                    TariffID = item.TariffID,


                                });
                            }

                            dgTariffServiceList.ItemsSource = lstService;
                        }
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            string msgText = "Are you sure you want to save the Patient service details?";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            msgW.Show();
            
        }
        
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (OnSaveButton_Click != null)
                {
                    this.DialogResult = true;
                    lstService = (List<clsPatientServiceDetails>)dgTariffServiceList.ItemsSource;
                    OnSaveButton_Click(this, new RoutedEventArgs());

                }
            }
               
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        
       
    }
}

