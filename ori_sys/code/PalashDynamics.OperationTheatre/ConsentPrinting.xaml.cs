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
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.OperationTheatre
{
    public partial class ConsentPrinting : UserControl
    {
        long OTBookingID = 0;
        List<long> ProcdureIDList = new List<long>();
        List<MasterListItem> consentList = new List<MasterListItem>();
        public ConsentPrinting()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OTBookingID = ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).ID;
            FetchProceduresForOTBookingID(OTBookingID);
           

        }
        /// <summary>
        /// Fetch procedures for OT booking ID
        /// </summary>
        /// <param name="OTBookingID">OTBookingID</param>
        private void FetchProceduresForOTBookingID(long OTBookingID)
        {
            try
            {
                clsGetProceduresForOTBookingIDBizActionVO bizActionVo = new clsGetProceduresForOTBookingIDBizActionVO();
                bizActionVo.OTBokingID = OTBookingID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {


                        foreach (var item in ((clsGetProceduresForOTBookingIDBizActionVO)e.Result).procedureList)
                        {

                            ProcdureIDList.Add(item.ID);
                        }

                        for (int i = 0; i < ProcdureIDList.Count ; i++)
                        {
                            FetchConsentForProcedureID(ProcdureIDList[i]);
                        }
                        //CmbOT1.ItemsSource = null;
                        //CmbOT1.ItemsSource = otList;
                        //CmbOT1.SelectedItem = otList[0];

                    

                    }

                };

                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch procedures for OT booking ID
        /// </summary>
        /// <param name="OTBookingID">OTBookingID</param>
        private void FetchConsentForProcedureID(long ProcedureID)
        {
            try
            {

                clsGetConsentForProcedureIDBizActionVO bizActionVo = new clsGetConsentForProcedureIDBizActionVO();
                bizActionVo.ProcedureID = ProcedureID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        foreach (var item in ((clsGetConsentForProcedureIDBizActionVO)e.Result).ConsentList)
                        {
                            if(!consentList.Contains(item))
                            consentList.Add(item);
                        }

                        dgConsent.ItemsSource = null;
                        dgConsent.ItemsSource = consentList;


                    }

                };

                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();


            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Print button click
        /// </summary>

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long ConsentID = 0;
            try
            {
                foreach (var item in dgConsent.ItemsSource)
                {
                    if (((MasterListItem)item).Status == true)
                    {
                        ConsentID = ((MasterListItem)item).ID;

                        string URL = "../Reports/OperationTheatre/ConsentPrintingMIS.aspx?ConsentID=" + ConsentID + "&Date=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).Date + "&PatientID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).PatientID + "&UnitID=" + ((clsPatientProcedureScheduleVO)((IApplicationConfiguration)App.Current).SelectedOTBooking).UnitID ;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Cancel button click
        /// </summary>

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.OperationTheatre.OTScheduling") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                //((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.OperationTheatre.OTScheduling");
             
                ((IApplicationConfiguration)App.Current).FillMenu("Operation Theatre");
              

         
                
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkStores_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in dgConsent.ItemsSource)
                {
                    ((MasterListItem)item).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in dgConsent.ItemsSource)
                {
                    ((MasterListItem)item).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Grid Check box checked
        /// </summary>
        private void chkConsent_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgConsent.SelectedItem != null)
                {
                    ((MasterListItem)dgConsent.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Grid Check box unchecked
        /// </summary>
        private void chkConsent_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgConsent.SelectedItem != null)
                {
                    ((MasterListItem)dgConsent.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Views the consent,modifies it and saves patientwise consent
        /// </summary>
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                clsGetConsetDetailsForConsentIDBizActionVO bizActionVo = new clsGetConsetDetailsForConsentIDBizActionVO();
                if(dgConsent.SelectedItem!=null)
                    bizActionVo.ConsentID = ((MasterListItem)dgConsent.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e1) =>
                {
                    if (e1.Error == null && e1.Result != null)
                    {

                       

                        PalashDynamics.Administration.ConsentMaster consentwin = new Administration.ConsentMaster();
                        consentwin.consentPrintingID = (((clsGetConsetDetailsForConsentIDBizActionVO)e1.Result).consentmaster).ID;
                        consentwin.consentPrintingObj = (((clsGetConsetDetailsForConsentIDBizActionVO)e1.Result).consentmaster);
                        consentwin.consentPrintingHtml = (((clsGetConsetDetailsForConsentIDBizActionVO)e1.Result).consentmaster).TemplateName;
                        consentwin.ConsentPrinting();

                        ChildWindow ch = new ChildWindow();
                        ch.Content = consentwin;
                        ch.Height = Convert.ToDouble("500");
                        ch.Width = Convert.ToDouble("800");
                        ch.Title = "Consent Printing";
                        
                        ch.Show();


                    }

                };

                Client.ProcessAsync(bizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();


            }
            catch (Exception ex)
            {
                throw;
            }
           
            

        }
    }
}
