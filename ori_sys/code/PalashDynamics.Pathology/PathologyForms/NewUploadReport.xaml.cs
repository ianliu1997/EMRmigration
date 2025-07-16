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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewUploadReport : UserControl
    {
        #region Variable AND List Declaration
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        clsPathOrderBookingDetailVO SelectedTest;
        List<clsPathOrderBookingDetailVO> testList = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemptestList = new List<clsPathOrderBookingDetailVO>();
        bool IsPatientExist = false;
        bool isCollecionPopupShow = false;
        clsPathOrderBookingDetailVO selectedtestReceiveDateime;
        WaitIndicator indicator;
        List<clsPathOrderBookingDetailVO> TestAgencyLinkList;
        List<clsPathOrderBookingDetailVO> UploadReportList;
        private List<clsPathOrderBookingDetailVO> TestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestList = new List<clsPathOrderBookingDetailVO>();
        public bool IsSelectAll = false;
        private ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        // Added By Anumani

        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }
        #endregion

        #region Constructor & IInitiate Member
        public NewUploadReport()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NewUploadReport_Loaded);
            selectedtestReceiveDateime = new clsPathOrderBookingDetailVO();
            indicator = new WaitIndicator();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            UploadReportList = new List<clsPathOrderBookingDetailVO>();
        }

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement1.Text = "Pathology Work Order";
                    break;
            }
        }
        #endregion

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
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }
        #endregion

        #region Loaded Event
        void NewUploadReport_Loaded(object sender, RoutedEventArgs e)
        {
            lstUploadStatus = new List<MasterListItem>();
            lstDeliveryStatus = new List<MasterListItem>();

            lstUploadStatus.Add(new MasterListItem() { ID = 0, Description = "--Select--", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 3, Description = "Uploaded", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 2, Description = "Partial Uploaded", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 1, Description = "Not Uploaded", Status = true }); 

            cmbUploadStatus.ItemsSource = lstUploadStatus;
            cmbUploadStatus.SelectedItem = lstUploadStatus[0];
            //cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;
            //cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            mElement1.Text = "Upload Report";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;

            fillOutAgencies();
            FillOrderBookingList();  // BY ROHINEE FOR CR Points
        }
        #endregion
        private void fillOutAgencies()
        {
            //agency name from agency master
            // cmbProcessedBy.itemSource = MList;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmboutAgencies.ItemsSource = null;
                    cmboutAgencies.ItemsSource = objList;
                    //cmbProcessedBy.SelectedItem = objList[0];
                    foreach (var item in objList)
                    {
                        if (item.ID == 0)
                        {
                            cmboutAgencies.SelectedItem = item;
                        }
                    }


                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #region Private Methods
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.IsFrom = "UploadReport";  //by rohini for CR Points
                POBBizAction.PatientID = 0;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    POBBizAction.UnitID = 0;
                }
                else
                {
                    POBBizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    POBBizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                    POBBizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
                }
                else
                {
                    POBBizAction.FromDate = null;
                    POBBizAction.ToDate = null;
                }
                if (txtFrontFirstName.Text != "")
                    POBBizAction.FirstName = txtFrontFirstName.Text;
                if (txtFrontLastName.Text != "")
                    POBBizAction.LastName = txtFrontLastName.Text;
                if (txtFrontMRNO.Text != "")
                    POBBizAction.MRNO = txtFrontMRNO.Text;
                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;
                if (cmbSampleType.SelectedItem != null)
                {
                    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 0)
                        POBBizAction.CheckSampleType = false;
                    else
                    {
                        POBBizAction.CheckSampleType = true;
                        if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
                            POBBizAction.SampleType = true;
                        else
                            POBBizAction.SampleType = false;
                    }
                }
                if (cmbUploadStatus.SelectedItem != null)
                {
                    if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 0)
                        POBBizAction.CheckUploadStatus = false;
                    else
                    {
                        POBBizAction.CheckUploadStatus = true;
                        if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 1)
                            POBBizAction.IsUploaded = true;
                        else
                            POBBizAction.IsUploaded = false;
                    }
                }
                if (cmboutAgencies.SelectedItem != null)
                {
                    POBBizAction.AgencyID = ((MasterListItem)cmboutAgencies.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.AgencyID = 0;
                }
                if (cmbUploadStatus.SelectedItem != null)
                {
                    POBBizAction.StatusID = ((MasterListItem)cmbUploadStatus.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.StatusID = 0;
                }

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
                            //DataList.Count = result.TotalRows1;
                            if (result.OrderBookingList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.OrderBookingList)
                                {
                                    DataList.Add(item);
                                }
                                dgOrdertList.ItemsSource = null;
                                dgOrdertList.ItemsSource = DataList;
                                DataPager.Source = null;
                                DataPager.PageSize = POBBizAction.MaximumRows;
                                DataPager.Source = DataList;
                                dgTest.ItemsSource = null;
                                txtTotalCountRecords.Text = "";
                                txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                            }
                        }
                    }
                    indicator.Close();
                };
                client.ProcessAsync(POBBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
            }
        }

        #endregion

        #region private Event Handlers
        List<clsPathOrderBookingDetailVO> LstUploadReport = new List<clsPathOrderBookingDetailVO>();
        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            # region Commented 
            // as per search grid
            //dgTest.ItemsSource = null;
            //if (dgOrdertList.SelectedItem != null)
            //{
            //    clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            //    BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            //    BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            //    if (cmbUploadStatus.SelectedItem != null)
            //    {
            //        if (((MasterListItem)cmbUploadStatus.SelectedItem).ID > 0)
            //        {
            //            BizAction.CheckExtraCriteria = true;
            //            BizAction.CheckSampleType = true;
            //            BizAction.IsFromUpload = true;
            //            if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 1)
            //            {
            //                BizAction.SampleType = true;
            //            }
            //            else if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 2)
            //            {
            //                BizAction.SampleType = false;
            //            }
            //        }
            //    }
            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null)
            //        {
            //            if (arg.Result != null)
            //            {
            //                foreach (var item in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList)
            //                {
            //                    if (item.IsSampleReceive == true)
            //                    {
            //                        item.IsSampleReceiveChecked = false;
            //                    }
            //                    else
            //                    {
            //                        item.IsSampleReceiveChecked = true;
            //                    }
            //                    if (((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail.Count > 0)
            //                    {
            //                        var List = from r in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail
            //                                   where r.TestID == item.TestID && r.OrderBookingID == item.OrderBookingID
            //                                   select r;
            //                        var List1 = from r in List.ToList()
            //                                    where r.IsDefault == true
            //                                    select r;
            //                        if (List.ToList().Count > 0)
            //                        {
            //                            item.IsOutSourced = true;
            //                            if (List1.ToList().Count > 0)
            //                            {
            //                                foreach (clsPathOrderBookingDetailVO item2 in List1)
            //                                {
            //                                    if (item.IsChangedAgency == false)
            //                                        item.AgencyName = item2.AgencyName;
            //                                    item.AgencyID = item2.AgencyID;
            //                                    break;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                foreach (clsPathOrderBookingDetailVO item2 in List)
            //                                {
            //                                    if (item.IsChangedAgency == false)
            //                                        item.AgencyName = item2.AgencyName;
            //                                    item.AgencyID = item2.AgencyID;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            item.IsOutSourced = false;
            //                        }
            //                        if (item.IsOutSourced == true)
            //                        {
            //                            item.SampleOutSourceImage = "../Icons/tick.png";
            //                        }
            //                        else
            //                            item.SampleOutSourceImage = "../Icons/error.png";
                                 
            //                    }
            //                    if (item.IsResultEntry == true)
            //                        item.ResultEntryImage = "../Icons/tick.png";
            //                    else
            //                        item.ResultEntryImage = "../Icons/error.png";
            //                    if (item.IsCompleted == true)
            //                        item.ReportUploadImage = "../Icons/tick.png";
            //                    else
            //                        item.ReportUploadImage = "../Icons/error.png";
            //                }
            //                LstUploadReport = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;//.ToList().Where(z=>z.IsOutSourced = true).ToList();
            //                TestList = LstUploadReport.ToList().Where(z => z.IsOutSourced = true).ToList();
            //                dgTest.ItemsSource = null;
            //                dgTest.ItemsSource = TestList;
            //                dgTest.UpdateLayout();
            //                TestAgencyLinkList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail;
            //            }
            //        }
            //    };

            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //}
            # endregion

            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                    FillOrderBookingDetailsList();
            }
        }

        #endregion

        #region Toggle Button Click Events
        private void CmdUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgTest.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test is Not Selected. Please Select a Test to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
                else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsDelivered == false)
                {

                    if (dgTest.SelectedItem != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected == true &&  ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted != true)
                        {
                            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsRejected == true)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Sample Is Rejected, Cannot Upload The Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                            else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Sample Is Not Outsourced, Cannot Upload The Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                            else if(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleAcceptRejectStatus == 2)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Sample Is Rejected, Cannot Upload The Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                            else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                            {
                                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is Collected From Another Clinic.Can Not Upload Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                msgbox.Show();
                            }
                            else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleAccepted == true)  //change by rohini iscomplited
                            {

                                UploadReportChildWindow w = new UploadReportChildWindow();
                                w.IsResultEntry = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry;
                                w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                w.ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                w.ServiceID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ServiceID; //by rohini
                                w.AgencyID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyID;  //by rohini
                                w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                w.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Sample Is Not Accepted, Cannot Upload The Report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                        }
                        else if (dgTest.SelectedItem == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test Is Not Selected. Please Select a Test to Upload The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                            mgbx.Show();
                        }
                        else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report Is Already Uploaded. Do You Want To Replace Existing Uploaded Report.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    UploadReportChildWindow w = new UploadReportChildWindow();
                                    w.IsResultEntry = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry;
                                    w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                    w.ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                    w.ServiceID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ServiceID;  //by rohini
                                    w.AgencyID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyID;  //by rohini
                                    w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                                    w.Show();
                                }
                            };
                            mgbx.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Sample Collection Is Not Done For The Test. Please Collect The Sample For The Test.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                            mgbx.Show();
                        }
                    //}
                    //else
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash",   .", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    //    mgbx.Show();
                    //}
                }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is Already Delivered. Please Select the Correct Item to Upload the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                        mgbx.Show();
                    }
              //  }
            }
            catch (Exception)
            {
                throw;
            }
        
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
        }
        void w_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            indicator.Show();
            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Report is uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            mgbx.Show();
            FillOrderBookingList();
            indicator.Close();
        }
        #endregion

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FillOrderBookingList();
            }
            catch(Exception)
            {
                throw;
            }
        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL))
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Report != null)
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                    {
                        //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                        //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                        
                        Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                        DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                        client.UploadReportFileCompleted += (s, args) =>
                        {
                            if (args.Error == null)
                            {
                                HtmlPage.Window.Invoke("OpenReport", new string[] { ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL });

                                listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                            }
                        };
                        client.UploadReportFileAsync(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL, ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Report);
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded == true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;


            //added by rohini for cross clinic handle dispatch item to another clinic            

            if (dgTest.ItemsSource != null)
            {
                //if (((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId && ((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != 0)
                //{
                //    e.Row.IsEnabled = false;
                //}
                //else
                //    e.Row.IsEnabled = true;
                //added by rohini for collection center 
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsProcessingUnit == false)
                {
                    e.Row.IsEnabled = false;
                }
                else
                    e.Row.IsEnabled = true;
            }
        }

        // added By Anumani on 24.03.2016
        private void hyTestDetails_Click(object sender, RoutedEventArgs e)
        {
            TestDetailsChildWindow WinHelpValue = new TestDetailsChildWindow();
            WinHelpValue.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            WinHelpValue.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;
            WinHelpValue.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            WinHelpValue.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;
            WinHelpValue.OrderDetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
            //WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
            WinHelpValue.Show();
        }

        private void FillOrderBookingDetailsList()
        {
            clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            BizAction.IsFrom = "UploadReport"; 
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            BizAction.CheckExtraCriteria = true;
            BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            BizAction.SampleType = POBBizAction.SampleType;
            BizAction.IsUploaded = POBBizAction.IsUploaded;
            BizAction.IsDelivered = POBBizAction.IsDelivered;
            //Added By Bhushanp 19012017 For Date Filter
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                BizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
            }
            else
            {
                BizAction.FromDate = null;
                BizAction.ToDate = null;
            }
            //by rohini as per CR Points
            if (cmboutAgencies.SelectedItem != null)
            {
                BizAction.AgencyID = ((MasterListItem)cmboutAgencies.SelectedItem).ID;
            }
            else
            {
                BizAction.AgencyID = 0;
            }
            if (cmbUploadStatus.SelectedItem != null)
            {
                BizAction.StatusID = ((MasterListItem)cmbUploadStatus.SelectedItem).ID;
            }
            else
            {
                BizAction.StatusID = 0;
            }

            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            #region commented as per search grid
            //added by rohini dated 11.2.16
            //if ((MasterListItem)cmbSampleStatus.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 1)
            //    {
            //        BizAction.OrderDetail.IsSampleCollected = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsSampleCollected = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 2)
            //    {
            //        BizAction.OrderDetail.IsSampleDispatch = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsSampleDispatch = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 3)
            //    {
            //        BizAction.OrderDetail.IsSampleReceive = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsSampleReceive = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
            //    {
            //        BizAction.OrderDetail.IsAccepted = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsAccepted = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 5)
            //    {
            //        BizAction.OrderDetail.IsRejected = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsRejected = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 6)
            //    {
            //        BizAction.OrderDetail.IsSubOptimal = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsSubOptimal = false;
            //    }

            //    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 7)
            //    {
            //        BizAction.OrderDetail.IsOutSourced = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsOutSourced = false;
            //    }
            //}

            //if ((MasterListItem)cmbResultEntry.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 1)   //result entry done
            //    {
            //        BizAction.OrderDetail.IsResultEntry = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsResultEntry = false;
            //    }
            //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 2)  //result entry Not done
            //    {
            //        BizAction.OrderDetail.IsResultEntry1 = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsResultEntry1 = false;
            //    }
            //}
            //if ((MasterListItem)cmbDelivery.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 1)   //result report done
            //    {
            //        BizAction.OrderDetail.IsDelivered = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDelivered = false;
            //    }
            //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 2)  //result report Not done
            //    {
            //        BizAction.OrderDetail.IsDelivered1 = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDelivered1 = false;
            //    }
            //}
            //if ((MasterListItem)cmbDeliveryType.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 1)   //report by email
            //    {
            //        BizAction.OrderDetail.IsDeliverdthroughEmail = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDeliverdthroughEmail = false;
            //    }
            //    if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 2)   // report by hand
            //    {
            //        BizAction.OrderDetail.IsDirectDelivered = true;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.IsDirectDelivered = false;
            //    }
            //}
            ////added by rohini 15.2.16
            //if (((MasterListItem)cmbSample.SelectedItem != null))
            //{
            //    if ((((MasterListItem)cmbSample.SelectedItem).ID == 1 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //    {
            //        BizAction.OrderDetail.SampleCollectedBy = txtSample.Text;
            //    }
            //    else if ((((MasterListItem)cmbSample.SelectedItem).ID == 2 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //    {
            //        BizAction.OrderDetail.DispatchBy = txtSample.Text;
            //    }
            //    else if ((((MasterListItem)cmbSample.SelectedItem).ID == 3 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //    {
            //        BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
            //    }
            //    else if ((((MasterListItem)cmbSample.SelectedItem).ID == 4 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //    {
            //        BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
            //    }
            //    else if ((((MasterListItem)cmbSample.SelectedItem).ID == 5 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //    {
            //        BizAction.OrderDetail.SampleReceiveBy = txtSample.Text;
            //    }
            //}

            //if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
            //{
            //    BizAction.OrderDetail.SampleNo = Convert.ToInt64(txtSampleNO.Text.Trim());
            //}
            //if ((MasterListItem)cmbProcessedBy.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)   // report by hand
            //    {
            //        BizAction.OrderDetail.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
            //    }
            //    else
            //    {
            //        BizAction.OrderDetail.AgencyID = 0;
            //    }
            //}

            # endregion 
            //
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        OrderTestList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                        foreach (clsPathOrderBookingDetailVO item in OrderTestList)
                        {
                            if (item.IsSampleCollected == true)
                                item.SampleCollectedImage = "../Icons/tick.png";
                            else
                                item.SampleCollectedImage = "../Icons/error.png";

                            if (item.IsSampleDispatch == true)
                                item.SampleDispatchImage = "../Icons/tick.png";
                            else
                                item.SampleDispatchImage = "../Icons/error.png";

                            if (item.IsSampleReceive == true)
                                item.SampleReceiveImage = "../Icons/tick.png";
                            else
                                item.SampleReceiveImage = "../Icons/error.png";

                            if (item.IsOutSourced == true)
                                item.SampleOutSourceImage = "../Icons/tick.png";
                            else
                                item.SampleOutSourceImage = "../Icons/error.png";

                            if (item.IsResultEntry == true)
                                item.ResultEntryImage = "../Icons/tick.png";
                            else
                                item.ResultEntryImage = "../Icons/error.png";
                            if (item.FirstLevel == true && item.SecondLevel == true)  //as per disscussion with anumani
                                item.FinalizedImage = "../Icons/tick.png";
                            else
                                item.FinalizedImage = "../Icons/error.png";
                            if (item.IsCompleted == true)
                                item.UploadImage = "../Icons/tick.png";
                            else
                                item.UploadImage = "../Icons/error.png";
                            if (item.IsDelivered == true)
                                item.DeliveredImage = "../Icons/tick.png";
                            else
                                item.DeliveredImage = "../Icons/error.png";


                            if (item.SampleAcceptRejectStatus == 1)
                            {
                                item.SamplAcceptImage = "../Icons/tick.png";
                                item.IsSampleAcceptEnable = false;
                                item.IsSampleAccepted = true;
                            }
                            else if (item.SampleAcceptRejectStatus == 2)
                            {
                                item.SamplAcceptImage = "../Icons/error.png";
                                item.IsSampleAcceptEnable = false;
                                item.IsSampleAccepted = true;
                            }
                        }
                        dgTest.ItemsSource = OrderTestList; //((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

   
    }
}
