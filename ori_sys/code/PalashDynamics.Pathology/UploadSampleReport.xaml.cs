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
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using System.Windows.Data;
using System.IO;
using MessageBoxControl;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;

namespace PalashDynamics.Pathology
{
    public partial class UploadSampleReport : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Pathology work order is not selected.\nPlease Select a work order then click on upload report.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        break;
                    }

                    //if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder != null && ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).IsCompleted ==false)
                    //{
                    //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Result entry is not done for Selected Work Order.\nPlease select the Pathology Work Order whose result entry is done.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                    //    msgbox.Show();
                    //   IsPatientExist = false;
                    //    break;
                    //}
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).PatientName;


                    break;
            }
        }

        #endregion

        bool IsPatientExist = false;
        private System.Collections.ObjectModel.ObservableCollection<string> listOfReports = new System.Collections.ObjectModel.ObservableCollection<string>();
        public UploadSampleReport()
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
                    CmdUpload.IsEnabled = false;
            }
            this.Loaded += new RoutedEventHandler(UploadSampleReport_Loaded);
            this.Unloaded += new RoutedEventHandler(UploadSampleReport_Unloaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }

        void UploadSampleReport_Unloaded(object sender, RoutedEventArgs e)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.DeleteReportFileCompleted += (s1, args1) =>
            {
                if (args1.Error == null)
                {

                }
            };
            client.DeleteReportFileAsync(listOfReports);
            client.CloseAsync();
        }

        void UploadSampleReport_Loaded(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                indicator.Close();
            }

        }

        private void FillOrderBookingDetailsList()
        {
            if (dgOrdertList.SelectedItem != null)
            {
                clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                            List<clsPathOrderBookingDetailVO> ObjList = new List<clsPathOrderBookingDetailVO>();

                            ObjList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;

                            var Result1 = from r in ObjList
                                          where r.IsSampleCollected == true || (r.IsResultEntry == true && r.IsFinalized == true)
                                          select r;


                            if (Result1.ToList().Count > 0)
                            {

                                ObjList = Result1.ToList();

                                dgTest.ItemsSource = null;
                                dgTest.ItemsSource = ObjList;
                            }



                            //Called when result entry is done
                            //var Result1 = from r in ObjList
                            //              where r.IsResultEntry == true && r.IsSampleCollected == true && r.IsFinalized==true
                            //              select r;


                            //if (Result1.ToList().Count > 0)
                            //{

                            //    ObjList = Result1.ToList();

                            //    dgTest.ItemsSource = null;
                            //    dgTest.ItemsSource = ObjList;
                            //}


                            ////Called when result entry is not done


                            //var results = from r in ObjList
                            //              where r.IsSampleCollected == true && r.IsResultEntry == false && r.IsFinalized == false
                            //              //where r.IsResultEntry == true && r.IsSampleCollected == true && r.IsFinalized==true
                            //              select r;
                            //if (results.ToList().Count > 0)
                            //{

                            //    ObjList = results.ToList();

                            //    dgTest.ItemsSource = null;
                            //    dgTest.ItemsSource = ObjList;
                            //}

                        }
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }
        #endregion

        private void CmdUpload_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem == null)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                mgbx.Show();
            }
            else
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered == false)
                {

                    if (dgTest.SelectedItem != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected == true && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted != true)
                    {

                        UploadReportChildWindow w = new UploadReportChildWindow();
                        w.IsResultEntry = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry;
                        w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        w.ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                        w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                        w.Show();
                    }
                    else if (dgTest.SelectedItem == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        mgbx.Show();
                    }
                    else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Uploaded. Do you want to replace existing uploaded report.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                //SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                UploadReportChildWindow w = new UploadReportChildWindow();
                                w.IsResultEntry = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry;
                                w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                w.ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                //w.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                                w.Show();
                            }
                            else
                            {

                            }
                        };
                        mgbx.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Delivered. Please Select the Correct Item to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }

        }

        void w_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            mgbx.Show();
            FillOrderBookingList(true);
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());

        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillOrderBookingDetailsList();

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //clsPathOrderBookingDetailVO item = e.Row.DataContext as clsPathOrderBookingDetailVO;
            //if ((bool?)item.IsCompleted==true)
            //    e.Row.Background = new SolidColorBrush(Colors.Yellow);
            //else
            //    e.Row.Background = null;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
            {
                clsGetPathOrderBookingDetailReportDetailsBizActionVO BizAction = new clsGetPathOrderBookingDetailReportDetailsBizActionVO();
                BizAction.OrderBookingDetail = new clsPathOrderBookingDetailVO();
                BizAction.OrderBookingDetail.ID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                BizAction.OrderBookingDetail.UnitId = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                            clsPathOrderBookingDetailVO ObjDetails = new clsPathOrderBookingDetailVO();

                            ObjDetails = ((clsGetPathOrderBookingDetailReportDetailsBizActionVO)arg.Result).OrderBookingDetail;

                            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                            {
                                Uri address1 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                client1.UploadReportFileCompleted += (s1, args) =>
                                {
                                    if (args.Error == null)
                                    {
                                        HtmlPage.Window.Invoke("OpenReport", new string[] { ObjDetails.SourceURL });   //HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL });
                                        listOfReports.Add(ObjDetails.SourceURL);   //listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                                    }
                                };
                                client1.UploadReportFileAsync(ObjDetails.SourceURL, ObjDetails.Report);   //client.UploadReportFileAsync(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL, ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Report);
                                client1.CloseAsync();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                            }
                        }
                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
    }
}
