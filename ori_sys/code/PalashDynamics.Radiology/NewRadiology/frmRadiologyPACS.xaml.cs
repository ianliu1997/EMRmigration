using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Radiology;
using CIMS;
using PalashDynamics.Pharmacy;
using System.Collections.ObjectModel;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.Radiology.ItemSearch;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Text;
using System.IO.IsolatedStorage;
using System.Security.AccessControl;
using System.Net;
using System.Reflection;
using System.Diagnostics;



namespace PalashDynamics.Radiology
{
    public partial class frmRadiologyPACS : UserControl
    {
        #region List and Variable declaration
        bool IsPatientExist = false;
        private string SeriesIDList = String.Empty;
        StringBuilder SeriesIDBuilder;
        private long SeriesID = 0;
        ObservableCollection<clsPACSTestSeriesVO> objlistPacs = new ObservableCollection<clsPACSTestSeriesVO>();
        List<clsPACSTestSeriesVO> PACSSELECTEDTestDetails = new List<clsPACSTestSeriesVO>();
        #endregion

        #region IInitiateCIMS Members
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Select the patient.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                    }
                    else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        IsPatientExist = true;
                        FetchTestListFromPACS(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                        mElement.Text = " : " + ((clsRadOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder).PatientName;
                    }
                    break;
            }
        }

        #endregion

        #region Constructor and Loaded Event
        public frmRadiologyPACS()
        {
            InitializeComponent();
            SeriesIDBuilder = new StringBuilder();
            objlistPacs = new ObservableCollection<clsPACSTestSeriesVO>();
        }
        private void RadiologyPACS_Loaded(object sender, RoutedEventArgs e)
        {

            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Select the patient.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                msgbox.Show();
                IsPatientExist = false;
            }

            else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                IsPatientExist = true;
                FetchTestListFromPACS(((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder.MRNo);
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                mElement.Text = " : " + ((clsRadOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedRadiologyWorkOrder).PatientName;
            }
        }
        #endregion

        #region Private Methods

        private void FetchTestListFromPACS(string MRNO)
        {
            clsGetPACSTestListBizActionVO BizAction = new clsGetPACSTestListBizActionVO();
            try
            {
                BizAction.MRNO = MRNO;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPACSTestListBizActionVO)arg.Result).PACSTestList != null)
                        {
                            List<clsPACSTestPropertiesVO> ObjList = new List<clsPACSTestPropertiesVO>();
                            ObjList = ((clsGetPACSTestListBizActionVO)arg.Result).PACSTestList;
                            dgTest.ItemsSource = null;
                            dgTest.ItemsSource = ObjList;
                            dgTest.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clsGetPACSTestSeriesListBizActionVO BizAction = new clsGetPACSTestSeriesListBizActionVO();
            try
            {
                BizAction.PACSTestDetails = new clsPACSTestSeriesVO();
                BizAction.PACSTestDetails.MRNO = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID;
                BizAction.PACSTestDetails.PATIENTNAME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTNAME;
                BizAction.PACSTestDetails.MODALITY = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).MODALITY;
                BizAction.PACSTestDetails.STUDYDATE = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDATE;
                BizAction.PACSTestDetails.STUDYTIME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYTIME;
                BizAction.PACSTestDetails.STUDYDESC = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDESC;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesList != null)
                        {
                            List<clsPACSTestSeriesVO> ObjList = new List<clsPACSTestSeriesVO>();
                            ObjList = ((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesList;
                            dgTestSeries.ItemsSource = null;
                            dgTestSeries.ItemsSource = ObjList;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Other Events
        private void btnShowPACS_Click(object sender, RoutedEventArgs e)
        {
            bool IsForShowSeriesPACS = true;
            bool IsForShowPACS = true;
            string URL;
            URL = "../Reports/Sales/WriteFile.aspx?IsForShowPACS=" + IsForShowPACS + "&IsForShowSeriesPACS=" + IsForShowSeriesPACS + "&SERIESNUMBER=" + ((clsPACSTestSeriesVO)dgTestSeries.SelectedItem).STUDYDESC + "&MRNO=" + ((clsPACSTestSeriesVO)dgTestSeries.SelectedItem).PATIENTID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
           
            #region Unused
            ////clsGetPACSTestSeriesListBizActionVO BizAction = new clsGetPACSTestSeriesListBizActionVO();
            ////try
            ////{   
            ////    BizAction.PACSTestDetails = new clsPACSTestSeriesVO();
            ////    BizAction.IsForShowPACS = true;
            ////    BizAction.IsForShowSeriesPACS = true;
            ////    BizAction.PACSTestDetails.MRNO = ((clsPACSTestSeriesVO)dgTestSeries.SelectedItem).PATIENTID;
            ////    //BizAction.PACSTestDetails.PATIENTNAME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTNAME;
            ////    //BizAction.PACSTestDetails.MODALITY = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).MODALITY;
            ////    //BizAction.PACSTestDetails.STUDYDATE = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDATE;
            ////    //BizAction.PACSTestDetails.STUDYTIME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYTIME;
            ////    //BizAction.PACSTestDetails.STUDYDESC = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDESC;
            ////    BizAction.PACSTestDetails.SERIESNUMBER = ((clsPACSTestSeriesVO)dgTestSeries.SelectedItem).STUDYDESC;

            ////    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            ////    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            ////    client.ProcessCompleted += (s, arg) =>
            ////    {
            ////        if (arg.Error == null)
            ////        {
            ////            if (((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList != null)
            ////            {
            ////                List<clsPACSTestSeriesVO> ObjImageList = new List<clsPACSTestSeriesVO>();
            ////                ObjImageList = ((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList;
            ////                ObservableCollection<clsPACSTestSeriesVO> objlist = new ObservableCollection<clsPACSTestSeriesVO>();

            ////                foreach (var item in ObjImageList)
            ////                {
            ////                    objlist.Add(item);
            ////                }

            ////                Uri address1 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            ////                DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

            ////                client1.SavePACSFileCompleted += (s1, args1) =>
            ////                {
            ////                    if (args1.Error == null)
            ////                    {
            ////                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://192.168.1.7:8080/DicomWebViewer1/"), "_blank");
            ////                    }
            ////                };

            ////                client1.SavePACSFileAsync("AA.txt", objlist);
            ////                client1.CloseAsync();

            ////            }
            ////        }
            ////        else
            ////        {
            ////            MessageBoxControl.MessageBoxChildWindow msgW1 =
            ////                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            ////            msgW1.Show();
            ////        }
            ////    };
            ////    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            ////    client.CloseAsync();
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw;
            ////}
            #endregion
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                PACSSELECTEDTestDetails.Add(((clsPACSTestSeriesVO)dgTestSeries.SelectedItem));
            }
            else
            {
                PACSSELECTEDTestDetails.Remove(((clsPACSTestSeriesVO)dgTestSeries.SelectedItem));
            }
        }

        public string path = string.Empty;
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null && PACSSELECTEDTestDetails.Count == 0)
            {
                #region Show All images
                //////clsGetPACSTestSeriesListBizActionVO BizAction = new clsGetPACSTestSeriesListBizActionVO();
                //////try
                //////{
                //////    BizAction.PACSTestDetails = new clsPACSTestSeriesVO();
                //////    BizAction.IsForShowPACS = true;
                //////    BizAction.PACSTestDetails.MRNO = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID;
                //////    BizAction.PACSTestDetails.PATIENTNAME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTNAME;
                //////    BizAction.PACSTestDetails.MODALITY = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).MODALITY;
                //////    BizAction.PACSTestDetails.STUDYDATE = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDATE;
                //////    BizAction.PACSTestDetails.STUDYTIME = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYTIME;
                //////    BizAction.PACSTestDetails.STUDYDESC = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDESC;
                //////    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //////    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //////    client.ProcessCompleted += (s, arg) =>
                //////    {
                //////        if (arg.Error == null)
                //////        {
                //////            if (((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList != null)
                //////            {
                //////                List<clsPACSTestSeriesVO> ObjImageList = new List<clsPACSTestSeriesVO>();
                //////                ObjImageList = ((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList;
                //////                foreach (var item in ObjImageList)
                //////                {
                //////                    objlistPacs.Add(item);
                //////                }

                //////                //string fileName1 = @"C:\pacs\data.txt";
                //////                //FileStream File = new FileStream("C:\\pacs\\data.txt", FileMode.OpenOrCreate, FileAccess.Write);
                //////                //using (System.IO.StreamWriter file1 = new System.IO.StreamWriter(File))
                //////                //{
                //////                //    foreach (clsPACSTestSeriesVO c in objlistPacs)
                //////                //    {
                //////                //        file1.WriteLine(c.IMAGEPATH);
                //////                //        file1.WriteLine("\n");
                //////                //    }
                //////                //    file1.Close();
                //////                //}

                //////                //string URL = "../Reports/Sales/WriteFile.aspx?PacsList="+ objlist;
                //////                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                //////                System.Windows.Browser.HtmlPage.Window.Invoke("DisplayAlertMessage", "From Silverlight");


                //////                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://192.168.1.7:8080/DicomWebViewer1"), "_blank");
                                    
                //////                ////Uri address1 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //////                ////DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                //////                ////client1.SavePACSFileCompleted += (s1, args1) =>
                //////                ////{
                //////                ////    if (args1.Error == null)
                //////                ////    {
                //////                ////        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://192.168.1.7:8080/DicomWebViewer1/"), "_blank");
                //////                ////    }
                //////                ////};

                //////                ////client1.SavePACSFileAsync("AA.txt", objlist);
                //////                ////client1.CloseAsync();
                //////            }
                //////        }
                //////        else
                //////        {
                //////            MessageBoxControl.MessageBoxChildWindow msgW1 =
                //////                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                //////            msgW1.Show();
                //////        }
                //////    };
                //////    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //////    client.CloseAsync();
                //////}
                //////catch (Exception ex)
                //////{
                //////    throw;
                //////}
                #endregion

                string URL;
                URL = "../Reports/Sales/WriteFile.aspx?MRNO=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID + "&PATIENTNAME=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTNAME + "&MODALITY=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).MODALITY + "&STUDYDATE=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDATE + "&STUDYTIME=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYTIME + "&STUDYDESC=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).STUDYDESC;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                #region Show Series wise Images
                SeriesIDList = String.Empty;
                SeriesIDBuilder = new StringBuilder();
                /*================================================================================================ */
                if (PACSSELECTEDTestDetails.ToList() != null && PACSSELECTEDTestDetails.ToList().Count > 0)
                {
                    foreach (clsPACSTestSeriesVO item in PACSSELECTEDTestDetails.ToList())
                    {
                        SeriesID = Convert.ToInt64(item.STUDYDESC);
                        SeriesIDBuilder.Append(SeriesID).Append(",");
                    }
                }
                SeriesIDList = SeriesIDBuilder.ToString();
                if (SeriesIDList != null && SeriesIDList.Length != 0)
                {
                    SeriesIDList = SeriesIDList.TrimEnd(',');
                }


                bool IsForShowSeriesPACS = true;
                bool IsForShowPACS = true;
                string URL;
                URL = "../Reports/Sales/WriteFile.aspx?IsForShowPACS=" + IsForShowPACS + "&IsForShowSeriesPACS=" + IsForShowSeriesPACS + "&SERIESNUMBER=" + SeriesIDList + "&MRNO=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                #region Unused Code
                /* ================================================================================================ */
                ////////clsGetPACSTestSeriesListBizActionVO BizAction = new clsGetPACSTestSeriesListBizActionVO();
                ////////try
                ////////{
                ////////    BizAction.PACSTestDetails = new clsPACSTestSeriesVO();
                ////////    BizAction.IsForShowPACS = true;
                ////////    BizAction.IsForShowSeriesPACS = true;
                ////////    BizAction.PACSTestDetails.MRNO = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID;
                ////////    BizAction.PACSTestDetails.SERIESNUMBER = SeriesIDList;
                ////////    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                ////////    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                ////////    client.ProcessCompleted += (s, arg) =>
                ////////    {
                ////////        if (arg.Error == null && ((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList != null)
                ////////        {
                ////////            List<clsPACSTestSeriesVO> ObjImageList = new List<clsPACSTestSeriesVO>();
                ////////            ObjImageList = ((clsGetPACSTestSeriesListBizActionVO)arg.Result).PACSTestSeriesImageList;
                ////////            ObservableCollection<clsPACSTestSeriesVO> objlist = new ObservableCollection<clsPACSTestSeriesVO>();
                ////////            foreach (var item in ObjImageList)
                ////////            {
                ////////                objlist.Add(item);
                ////////            }
                ////////            Uri address1 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                ////////            DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                ////////            client1.SavePACSFileCompleted += (s1, args1) =>
                ////////            {
                ////////                if (args1.Error == null)
                ////////                {
                ////////                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://192.168.1.132:8080/DicomWebViewer03/"), "_blank");
                ////////                }
                ////////            };
                ////////            client1.SavePACSFileAsync("AA.txt", objlist);
                ////////            client1.CloseAsync();
                ////////        }
                ////////        else
                ////////        {
                ////////            MessageBoxControl.MessageBoxChildWindow msgW1 =
                ////////                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                ////////            msgW1.Show();
                ////////        }
                ////////    };
                ////////    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                ////////    client.CloseAsync();
                ////////}
                ////////catch (Exception ex)
                ////////{
                ////////    throw;
                ////////}
                #endregion

                #endregion
            }
        }
        #endregion
    }
}