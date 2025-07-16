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
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using System.Windows.Data;
using System.IO;
using MessageBoxControl;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.Pathology
{
    public partial class ReportDelivery : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Pathology work order is not selected.\nPlease Select a work order then click on report delivery.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Pathology Work Order.");
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder != null && ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).SampleType == false)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is not Collected for Selected Work Order.\nPlease select the Pathology Work Order whose Sample is Collected.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        //System.Windows.Browser.HtmlPage.Window.Alert("Sample is not Collected for Selected Work Order. Please select the Pathology Work Order whose Sample is Collected.");
                        IsPatientExist = false;
                        break;
                    }
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).PatientName;
                    //mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + "" + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;

                    break;
            }
        }

        #endregion

        bool IsPatientExist = false;
        double BalanceAmount = 0;
        string billNoforPrint = "0";
        bool PrintPage;
        clsPathOrderBookingDetailVO SelectedDetails { get; set; }
        List<clsPathOrderBookingDetailVO> lst = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> AddedListlst = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> DeletedListlst = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> NotComplteted = new List<clsPathOrderBookingDetailVO>();
        //bool IsFinalized { get; set; }
        //bool IsUploded { get; set; }

        public ReportDelivery()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    CmdDelivered.IsEnabled = false;
            }
            this.Loaded += new RoutedEventHandler(ReportDelivery_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }

        void ReportDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
                ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());
                //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            }
            else
            {
                FillOrderBookingList();
            }
            AddedListlst.Clear();
            NotComplteted.Clear();
        }

        #region 'Paging'

        public PagedSortableCollectionView<clsPathOrderBookingVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>

        #endregion

        private void FillOrderBookingList()
        {
            try
            {
                DataList.Clear();
                DataList.Add(((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder);
                dgOrdertList.ItemsSource = null;
                dgOrdertList.ItemsSource = DataList;

                DataPager.Source = null;
                DataPager.PageSize = 1;
                DataPager.Source = DataList;

                dgTest.ItemsSource = null;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillOrderBookingList(bool status)
        {
            try
            {
                indicator.Show();
                clsGetPathOrderBookingListBizActionVO BizAction = new clsGetPathOrderBookingListBizActionVO();
                BizAction.ID = ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.UnitId;

                if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.IsIPDPatient == true)
                {
                    BizAction.PatientType = 2;
                }
                else
                {
                    BizAction.PatientType = 1;
                }

                //Set Paging Variables
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //List<clsPathOrderBookingVO> list = ((clsGetPathOrderBookingListBizActionVO)arg.Result).OrderBookingList;
                            //PagedCollectionView view = new PagedCollectionView(list);
                            //view.GroupDescriptions.Add(new PropertyGroupDescription("SampleType"));

                            //dgOrdertList.ItemsSource = ((clsGetPathOrderBookingListBizActionVO)arg.Result).OrderBookingList;
                            //dgOrdertList.ItemsSource = view;

                            clsGetPathOrderBookingListBizActionVO result = arg.Result as clsGetPathOrderBookingListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.OrderBookingList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.OrderBookingList)
                                {
                                    ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = item;
                                    DataList.Add(item);
                                }

                                dgOrdertList.ItemsSource = null;
                                dgOrdertList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;

                                dgTest.ItemsSource = null;
                            }
                        }
                    }
                    indicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
            }

        }

        private void FillOrderBookingDetails()
        {
            if (dgOrdertList.SelectedItem != null)
            {
                clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

                dgTest.ItemsSource = null;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //dgTest.ItemsSource = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;

                            List<clsPathOrderBookingDetailVO> ObjList = new List<clsPathOrderBookingDetailVO>();

                            ObjList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;

                            var results = from r in ObjList
                                          where r.IsSampleCollected == true && r.IsCompleted == true  //where r.IsSampleCollected == true || r.IsResultEntry == true && r.IsCompleted == true
                                          select r;
                            if (results.ToList().Count > 0)
                            {
                                ObjList = results.ToList();

                                dgTest.ItemsSource = null;
                                dgTest.ItemsSource = ObjList;
                            }
                            else
                            {
                                dgTest.ItemsSource = null;
                            }
                        }
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }



        private void CmdDelivered_Click(object sender, RoutedEventArgs e)
        {
            //IsFinalized = false;
            //IsUploded = false;
            if (dgTest.SelectedItem != null && ((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)  //if (dgTest.SelectedItem != null && ((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true || (bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true) && (bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered != true)
                && (bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered != true)
            {
                if (BalanceAmount <= 0)
                {
                    clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();

                    BizAction.PathOrderBookingDetail = new clsPathOrderBookingDetailVO();
                    BizAction.PathOrderBookingDetail.ID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                    BizAction.PathOrderBookingDetail.UnitId = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
                    long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                    //IsFinalized = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                    //IsUploded = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {

                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Delivered Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                    {

                                        if (SelectedDetails.IsFinalized == true)
                                        {
                                            if (ResultID > 0)
                                                PrintReport(ResultID);
                                            FillOrderBookingList(true);
                                        }
                                        if (SelectedDetails.IsCompleted == true)
                                        {
                                            ViewUploadedReport();
                                            FillOrderBookingList(true);
                                        }
                                    }
                                };
                                mgbx.Show();


                                //FillOrderBookingList(true);
                            }
                        }
                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill is not paid.Please first paid the bill then deliver it", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
            }
            else if (dgTest.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Deliver the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
            else if (((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted != true))  //|| (bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized != true)
            {
                //Only result entry or uploded report can be delivered
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Upload report is not done.So report cannot delivered.", MessageBoxButtons.Ok, MessageBoxIcon.Error);   //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report can not be Delivered.Please Select the Correct Item to Deliver the Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
            else if ((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered == true)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Delivered. Please Select the Correct Item to Deliver the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }

        }

        void w_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            chkSelectAll.IsChecked = false;
            AddedListlst.Clear();
            NotComplteted.Clear();
        }
        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            chkSelectAll.IsChecked = false;
            AddedListlst.Clear();
            NotComplteted.Clear();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());
            //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrdertList.SelectedItem != null)
            {
                chkSelectAll.IsChecked = false;
                AddedListlst.Clear();
                NotComplteted.Clear();
                billNoforPrint = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                BalanceAmount = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance;
                FillOrderBookingDetails();
            }

        }

        private void PrintReport(long ResultID)
        {
            //if (ResultID > 0)
            //{
            //    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //    string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
            //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            //}

            if (dgOrdertList.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Record", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            if (dgTest.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Test", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                return;
            }
            long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
            {

                long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);

                PrintPage = true;
                string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintPage=" + PrintPage;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                long SampleNo = 0;
                try
                {
                    SampleNo = Convert.ToInt64(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo);
                }
                catch (Exception) { }

                string URL = "../Reports/Pathology/PathoTemplateReportDelivery.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=true";
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            clsPathOrderBookingDetailVO objPathOrderBookingDetail = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
            if (objPathOrderBookingDetail.IsCompleted == true)
            {
                ViewUploadedReport();
            }
            if (objPathOrderBookingDetail.IsFinalized == true)
            {
                if (objPathOrderBookingDetail.PathPatientReportID > 0)
                    PrintReport(objPathOrderBookingDetail.PathPatientReportID);
            }
        }

        private void ViewUploadedReport()
        {
            if (SelectedDetails.IsCompleted == true)
            {
                clsGetPatientLabUploadReportDataBizActionVO BizAction = new clsGetPatientLabUploadReportDataBizActionVO();
                BizAction.IsPathology = true;
                BizAction.ID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                BizAction.UnitID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;

                WaitIndicator indicator = new WaitIndicator();
                indicator.Show();
                try
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            Uri Fileaddress = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                            DataTemplateHttpsServiceClient Fileclient = new DataTemplateHttpsServiceClient("CustomBinding_DataTemplateService", Fileaddress.AbsoluteUri);
                            Fileclient.UploadReportFileCompleted += (fs, fargs) =>
                            {
                                if (fargs.Error == null)
                                {
                                    indicator.Close();
                                    HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsGetPatientLabUploadReportDataBizActionVO)arg.Result).SourceURL });   //HtmlPage.Window.Invoke("OpenReport", new string[] { SelectedDetails.SourceURL });
                                    //listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                                }
                            };
                            Fileclient.UploadReportFileAsync(((clsGetPatientLabUploadReportDataBizActionVO)arg.Result).SourceURL, ((clsGetPatientLabUploadReportDataBizActionVO)arg.Result).Report);  //Fileclient.UploadReportFileAsync(SelectedDetails.SourceURL, SelectedDetails.Report);
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception Ex)
                {
                    indicator.Close();
                    throw;
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }

        }

        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

            }
        }


        #region Multiple Test Print
        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrdertList.SelectedItem != null)
            {
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The bill has a balance amount of  Rs... The report delivery would be done only after the bill is completely settled.Still you want to print the report", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(mgbxW_OnMessageBoxClosed);
                    mgbx.Show();
                }
                else
                {
                    if (dgTest.SelectedItem != null)
                    {
                        long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
                        {
                            //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                            PrintPage = true;
                            string URL = "../Reports/Pathology/PathoResultEntry.aspx?BillID=" + billNoforPrint + "&UnitID=" + UnitID + "&PrintPage=" + PrintPage;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                        }
                        else
                        {
                            long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                            long SampleNo = 0;
                            try
                            {
                                SampleNo = Convert.ToInt64(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo);
                            }
                            catch (Exception) { }
                            long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                            string URL = "../Reports/Pathology/PathoTemplateReportDelivery.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=true";
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Select the test to print the report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        mgbx.Show();
                    }

                }
            }
        }
        #endregion
        // To Print the All test Result
        void mgbxW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Do you want to print report on letter head? ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                mgbx.Show();

                mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                {

                    if (res == MessageBoxResult.Yes)
                    {
                        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
                        {

                            string URL = "../Reports/Pathology/PathologyTemplateReportDeliveryHtml2.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                        }
                        else
                        {
                            PrintPage = true;
                            string URL = "../Reports/Pathology/PathoResultEntry.aspx?BillID=" + billNoforPrint + "&UnitID=" + UnitID + "&PrintPage=" + PrintPage;
                            //string URL = "../Reports/Pathology/PathoResultEntry.aspx?BillID=" + billNoforPrint + "&UnitID=" + UnitID + "&PrintPage=" + PrintPage;

                            //string URL = "../Reports/Pathology/PathoTemplateReportDeliveryHTMl.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                        }
                    }
                    else if (res == MessageBoxResult.No)
                    {
                        string URL = "../Reports/Pathology/PathoResultEntry.aspx?BillID=" + billNoforPrint + "&UnitID=" + UnitID;
                        //string URL = "../Reports/Pathology/PathologyTemplateReportDeliveryHtml2.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }

                };
            }
        }


        public string ResultID { get; set; }
        public string UnitID { get; set; }

        private void CmdSendMail_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                if (SelectedDetails.IsCompleted == true)
                {
                    if (BalanceAmount <= 0)
                    {
                        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx1 = new MessageBoxChildWindow("Palash", "Report is Already Uploaded. Do you want to send mail.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            mgbx1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                            {
                                if (res1 == MessageBoxResult.Yes)
                                {
                                    SendMailChildWindow w = new SendMailChildWindow();
                                    w.SelectedDetails = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                                    w.SelectedTestDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;

                                    w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                    w.Show();
                                }
                            };
                            mgbx1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill is not paid.Please first paid the bill then send mail", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The bill has a balance amount of  Rs... The report delivery would be done only after the bill is completely settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        mgbx.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Upload report is not done.So report cannot be mailed.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please select Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW2.Show();
            }
        }

        private void CmdDivertDelivery_Click(object sender, RoutedEventArgs e)
        {
            if (AddedListlst.Count > 0)
            {
                if (BalanceAmount <= 0)
                {
                    clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();
                    BizAction.PathOrderBookList = AddedListlst;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Delivered Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    FillOrderBookingList(true);
                                }
                            };
                            mgbx.Show();
                        }
                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill is not paid.Please first paid the bill then deliver it", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
            }
            else if (dgTest.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Deliver the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
            else if (((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted != true))
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Upload report is not done.So report cannot delivered.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
            else if ((bool?)((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered == true)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Delivered. Please Select the Correct Item to Deliver the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
        }


        private void ChangeStatusOfDeliverythroughEmail(List<clsPathOrderBookingDetailVO> UpdationList)
        {
            clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO BizAction = new clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO();
            BizAction.PathOrderBookList = UpdationList;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    FillOrderBookingDetails();
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void CmdthroughEmail_Click(object sender, RoutedEventArgs e)
        {
            if (AddedListlst.Count > 0)
            {

                if (BalanceAmount <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx1 = new MessageBoxChildWindow("Palash", "Report is Already Uploaded. Do you want to send mail.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    mgbx1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                    {
                        if (res1 == MessageBoxResult.Yes)
                        {
                            ChangeStatusOfDeliverythroughEmail(AddedListlst);
                            frmChdSendMail w = new frmChdSendMail();
                            // frmDeliveredthroughMail w = new frmDeliveredthroughMail();
                            w.SelectedDetails = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                            //      w.SelectedTestDetails = item;
                            w.SelectedTestlst = AddedListlst;
                            w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                            w.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                            w.Show();
                        }
                        else
                        {
                            chkSelectAll.IsChecked = false;
                            AddedListlst.Clear();
                            NotComplteted.Clear();
                        }
                    };
                    mgbx1.Show();

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill is not paid.Please first paid the bill then send mail", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The bill has a balance amount of  Rs... The report delivery would be done only after the bill is completely settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW2.Show();
            }
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == true)
                {

                    lst = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                    AddedListlst.Clear();
                    NotComplteted.Clear();
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectedReport = true;
                        }
                        dgTest.ItemsSource = null;
                        dgTest.ItemsSource = lst;
                        dgTest.SelectedIndex = -1;
                    }
                }
                foreach (var item in lst)
                {
                    if (item.SelectedReport == true)
                    {
                        if (item.IsCompleted == true)
                        {
                            AddedListlst.Add(item);
                        }
                        else
                        {
                            NotComplteted.Add(item);
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == false)
                {
                    lst = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectedReport = false;
                        }
                        dgTest.ItemsSource = null;
                        dgTest.ItemsSource = lst;
                        dgTest.SelectedIndex = -1;

                        foreach (var item in lst)
                        {
                            if (item.SelectedReport == false)
                            {
                                AddedListlst.Remove(item);
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void chkRecord_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                SelectedDetails.SelectedReport = true;
                AddedListlst.Add(SelectedDetails.DeepCopy());
            }

        }

        private void UnchkRecod_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                SelectedDetails.SelectedReport = false;
                AddedListlst.Remove(SelectedDetails.DeepCopy());
            }
        }
    }
}
