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
using System.Windows.Data;

namespace PalashDynamics.Radiology
{
    public partial class frmVisitWisePACS : UserControl
    {
        #region List and Variable declaration
        bool IsPatientExist = false;
        private string SeriesIDList = String.Empty;
        StringBuilder SeriesIDBuilder;
        private long SeriesID = 0;
        private string PATIENTIDList = string.Empty;
        private string PATIENTID = string.Empty;
        StringBuilder PATIENTIDBuilder;
        public bool IsForVisitWiseCompare { get; set; }
        ObservableCollection<clsPACSTestSeriesVO> objlistPacs = new ObservableCollection<clsPACSTestSeriesVO>();
        public List<clsPACSTestSeriesVO> PACSSELECTEDTestDetails = new List<clsPACSTestSeriesVO>();
        public List<clsPACSTestSeriesVO> PACSSelectedSeriesList { get; set; }
        public List<clsRadOrderBookingVO> SelectedOrderList { get; set; }
        StringBuilder OrderIDBuilder = new StringBuilder();
        StringBuilder MRNOBuilder = new StringBuilder();
        string MRNO = string.Empty;
        string MRNOList = string.Empty;
        PagedCollectionView collection;
        public List<clsPACSTestSeriesVO> SeriesList = new List<clsPACSTestSeriesVO>();
        #endregion

        #region Constructor and Refresh Events
        public frmVisitWisePACS()
        {
            InitializeComponent();
            SeriesIDBuilder = new StringBuilder();
            objlistPacs = new ObservableCollection<clsPACSTestSeriesVO>();
        }
        private void RadiologyPACS_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderList != null && SelectedOrderList.Count > 0)
            {
                foreach (clsRadOrderBookingVO item in SelectedOrderList)
                {
                    MRNO = item.MRNo;
                    MRNOBuilder.Append(MRNO).Append(",");
                }
                MRNOList = MRNOBuilder.ToString();
                if (MRNOList != null && MRNOList.Length != 0)
                {
                    MRNOList = MRNOList.TrimEnd(',');
                }
                FetchTestListFromPACS(MRNOList);
            }
        }
        #endregion

        #region Private Methods
        List<clsPACSTestPropertiesVO> StudyList = new List<clsPACSTestPropertiesVO>();
        private void FetchTestListFromPACS(string MRNO)
        {
            clsGetPACSTestListBizActionVO BizAction = new clsGetPACSTestListBizActionVO();
            try
            {
                BizAction.IsForStudyComparision = true;
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

        private void FillStudySeriesDetails()
        {
            clsGetPACSTestSeriesListBizActionVO BizAction = new clsGetPACSTestSeriesListBizActionVO();
            try
            {
                BizAction.IsForStudyComparision = true;
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

                            if (concateSeries == true)
                                SeriesList.AddRange(ObjList);
                            else if (concateSeries == false)
                            {
                                objStudyVO = (clsPACSTestPropertiesVO)dgTest.SelectedItem;
                                var itemsToRemove = SeriesList.Where(x => x.PATIENTID == MRNO1 &&
                                        x.MODALITY == objStudyVO.MODALITY &&
                                        x.PATIENTNAME == objStudyVO.PATIENTNAME &&
                                        x.STUDYDESC1 == objStudyVO.STUDYDESC &&
                                        x.STUDYTIME == objStudyVO.STUDYTIME &&
                                        x.STUDYDATE == objStudyVO.STUDYDATE).ToList();
                                foreach (var itemToRemove in itemsToRemove)
                                    SeriesList.Remove(itemToRemove);
                                if (PACSSELECTEDTestDetails != null && PACSSELECTEDTestDetails.Count > 0)
                                {
                                    foreach (var itemToRemove in itemsToRemove)
                                        PACSSELECTEDTestDetails.Remove(itemToRemove);
                                }
                                SeriesList.ForEach(p =>
                                {
                                    if (PACSSELECTEDTestDetails != null && PACSSELECTEDTestDetails.Count > 0)
                                    {
                                        if (PACSSELECTEDTestDetails.Any(sp => sp.PATIENTID == p.PATIENTID && sp.STUDYDESC == p.STUDYDESC && sp.STUDYTIME == p.STUDYTIME))
                                        {
                                            p.IsSelected = true;
                                        };
                                    }
                                });
                            }
                            collection = new PagedCollectionView(SeriesList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("PATIENTNAME"));
                            dgTestSeries.ItemsSource = null;
                            dgTestSeries.ItemsSource = collection;
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
            if (dgTest.SelectedItem != null)
            {
                clsPACSTestSeriesVO objVO = (clsPACSTestSeriesVO)dgTestSeries.SelectedItem;
                if (((CheckBox)sender).IsChecked == true)
                {
                    PACSSELECTEDTestDetails.Add(((clsPACSTestSeriesVO)dgTestSeries.SelectedItem));
                }
                else
                {
                    clsPACSTestSeriesVO obj = PACSSELECTEDTestDetails.Where(z => z.STUDYDESC == objVO.STUDYDESC && z.STUDYTIME == objVO.STUDYTIME).FirstOrDefault();
                    PACSSELECTEDTestDetails.Remove(obj);
                    //PACSSELECTEDTestDetails.Remove(((clsPACSTestSeriesVO)dgTestSeries.SelectedItem));
                }
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
                PATIENTIDList = string.Empty;
                MRNOList = string.Empty;
                SeriesIDBuilder = new StringBuilder();
                PATIENTIDBuilder = new StringBuilder();
                MRNOBuilder = new StringBuilder();
                /*================================================================================================ */
                if (PACSSELECTEDTestDetails.ToList() != null && PACSSELECTEDTestDetails.ToList().Count > 0)
                {
                    foreach (clsPACSTestSeriesVO item in PACSSELECTEDTestDetails.ToList())
                    {
                        /* Do not Use comma (",") for append purpose because PACS Database consist of description by using commas, I will put in troble while splitting Variables */
                        SeriesID = Convert.ToInt64(item.STUDYDESC);
                        //SeriesIDBuilder.Append(SeriesID).Append(",");
                        PATIENTID = Convert.ToString(item.PATIENTID);
                        string StudyDesc = Convert.ToString(item.STUDYDESC1);
                        string StudyTime = Convert.ToString(item.STUDYTIME);
                        string Modality = Convert.ToString(item.MODALITY);
                        string StudyDate = Convert.ToString(item.STUDYDATE);
                        MRNOBuilder.Append(PATIENTID).Append("@").Append(SeriesID).Append("@").Append(StudyDesc).Append("@").Append(StudyTime).Append('@').Append(Modality).Append('@').Append(StudyDate).Append("|");
                    }
                }
                PATIENTIDList = MRNOBuilder.ToString();
                //SeriesIDList = SeriesIDBuilder.ToString();
                //if (SeriesIDList != null && SeriesIDList.Length != 0)
                //{
                //    SeriesIDList = SeriesIDList.TrimEnd(',');
                //}
                if (PATIENTIDList != null && PATIENTIDList.Length != 0)
                {
                    PATIENTIDList = PATIENTIDList.TrimEnd('|');
                }
                bool IsForShowSeriesPACS = true;
                bool IsForShowPACS = true;
                bool IsVisitWise = true;
                string URL;
                //URL = "../Reports/Sales/WriteFile.aspx?IsForShowPACS=" + IsForShowPACS + "&IsForShowSeriesPACS=" + IsForShowSeriesPACS + "&SERIESNUMBER=" + SeriesIDList + "&MRNO=" + ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID + "&IsVisitWise=" + IsVisitWise;
                URL = "../Reports/Sales/WriteFile.aspx?IsForShowPACS=" + IsForShowPACS + "&IsForShowSeriesPACS=" + IsForShowSeriesPACS + "&SERIESNUMBER=" + SeriesIDList + "&MRNO=" + PATIENTIDList + "&IsVisitWise=" + IsVisitWise; //+"&SeriesList=" + (string[])PACSSELECTEDTestDetails.ToArray();
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
        public bool concateSeries = false;
        string MRNO1 = string.Empty;
        clsPACSTestPropertiesVO objStudyVO;
        private void ChkStudy_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                MRNO1 = ((clsPACSTestPropertiesVO)dgTest.SelectedItem).PATIENTID;
                objStudyVO = (clsPACSTestPropertiesVO)dgTest.SelectedItem;
                CheckBox Chk = sender as CheckBox;
                if (Chk.IsChecked == true)
                {
                    concateSeries = true;
                    StudyList.Add((clsPACSTestPropertiesVO)dgTest.SelectedItem);
                }
                else
                {
                    concateSeries = false;
                    clsPACSTestPropertiesVO obj = StudyList.
                        Where(z => z.PATIENTID == objStudyVO.PATIENTID && 
                            z.MODALITY == objStudyVO.MODALITY &&
                            z.PATIENTNAME == objStudyVO.PATIENTNAME &&
                            z.STUDYDESC == objStudyVO.STUDYDESC &&
                            z.STUDYTIME == objStudyVO.STUDYTIME &&
                            z.STUDYDATE == objStudyVO.STUDYDATE
                            ).FirstOrDefault();
                    StudyList.Remove(obj);
                }
                FillStudySeriesDetails();
            }

        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Radiology.ResultEntry") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
    }
}