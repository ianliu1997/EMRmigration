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
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Pathology;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewReportDelivery : UserControl
    {
        #region Variable AND List Declaration
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        List<MasterListItem> lstSampleType { get; set; }

        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        bool IsPatientExist = false;
        clsPathOrderBookingDetailVO SelectedTest;
        double BalanceAmount = 0;
        long billNoforPrint = 0;
        bool PrintPage;
        clsPathOrderBookingDetailVO SelectedDetails { get; set; }
        List<clsPathOrderBookingDetailVO> testList = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemptestList = new List<clsPathOrderBookingDetailVO>();
        clsPathOrderBookingDetailVO selectedtestReceiveDateime;
        WaitIndicator indicator;
        List<clsPathOrderBookingDetailVO> TestAgencyLinkList;
        List<clsPathOrderBookingDetailVO> DeliveredTestList;
        private List<clsPathOrderBookingDetailVO> TestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestPrintList = new List<clsPathOrderBookingDetailVO>();
        public bool IsSelectAll = false;
        public bool IsSelectAllPrint = false;
        private ObservableCollection<string> listOfReports = new ObservableCollection<string>();
        public List<clsPathOrderBookingDetailVO> SelectedTestList;
        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }
   
        #endregion

        #region Constructor & IInitiate Member
        public NewReportDelivery()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NewReportDelivery_Loaded);
            selectedtestReceiveDateime = new clsPathOrderBookingDetailVO();
            indicator = new WaitIndicator();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            DeliveredTestList = new List<clsPathOrderBookingDetailVO>();
            SelectedTestList = new List<clsPathOrderBookingDetailVO>();
          
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
        void NewReportDelivery_Loaded(object sender, RoutedEventArgs e)
        {
            lstSampleType = new List<MasterListItem>();
            lstUploadStatus = new List<MasterListItem>();
            lstDeliveryStatus = new List<MasterListItem>();
            lstSampleType.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 1, Description = "Collected", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 2, Description = "Not Collected", Status = true });

            lstUploadStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 1, Description = "Uploaded", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 2, Description = "Not Uploaded", Status = true });

            lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });
            DeliveredTestList.Clear();

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            mElement1.Text = "Report Delivery";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            FillOrderBookingList();
            //added by rohini dated 10.2.16
            fillSampleStatus();
          
            //
        }
        #endregion
        #region added by rohini dated 10/2/16
        private void fillSampleStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Billed"));
            MList.Add(new MasterListItem(2, "Collected"));
            MList.Add(new MasterListItem(3, "Dispatched"));
            //MList.Add(new MasterListItem(4, "Received"));
            MList.Add(new MasterListItem(5, "Accepted"));
            MList.Add(new MasterListItem(6, "Rejected"));
            MList.Add(new MasterListItem(7, "Sub Optimal"));
            MList.Add(new MasterListItem(8, "Outsourced"));
            MList.Add(new MasterListItem(9, "Result Entry Done"));
            MList.Add(new MasterListItem(10, "Report Delivered"));
            //MList.Add(new MasterListItem(11, "Closed/Reported"));
            cmbSampleStatus.ItemsSource = null;
            cmbSampleStatus.ItemsSource = MList;
            cmbSampleStatus.SelectedItem = MList[0];

            fillesultEntry();
        }
        private void fillesultEntry()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Done"));
            MList.Add(new MasterListItem(2, "Not Done"));

            cmbResultEntry.ItemsSource = null;
            cmbResultEntry.ItemsSource = MList;
            cmbResultEntry.SelectedItem = MList[0];
            fillProcessedBy();
        }
        private void FillUsers()
        {
            try
            {
                clsGetPathoUsersBizActionVO BizAction = new clsGetPathoUsersBizActionVO();
                BizAction.MenuID = (long)10571;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList1 = new List<MasterListItem>();
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetPathoUsersBizActionVO)arg.Result).MasterList);

                        cmbDeliveredBy.ItemsSource = null;
                        cmbDeliveredBy.ItemsSource = objList;
                        cmbDeliveredBy.SelectedItem = objList[0];

                    }
                    fillBillType();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }
        private void fillProcessedBy()
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
                    cmbProcessedBy.ItemsSource = null;
                    cmbProcessedBy.ItemsSource = objList;
                    //cmbProcessedBy.SelectedItem = objList[0];
                    foreach (var item in objList)
                    {
                        if (item.ID == 0)
                        {
                            cmbProcessedBy.SelectedItem = item;
                        }
                    }


                }
                fillDeliveryType();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void fillDeliveryType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Email"));
            MList.Add(new MasterListItem(2, "By Hand"));
            // cmbDeliveryType.itemSource = MList;
            cmbDeliveryType.ItemsSource = null;
            cmbDeliveryType.ItemsSource = MList;
            cmbDeliveryType.SelectedItem = MList[0];
            fillDelivery();
        }
        private void fillDelivery()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(2, "Partial Delivered"));
            MList.Add(new MasterListItem(3, "Delivered"));
            MList.Add(new MasterListItem(1, "Not Delivered"));
            // cmbDelivery.itemSource = MList;
            cmbDelivery.ItemsSource = null;
            cmbDelivery.ItemsSource = MList;
            cmbDelivery.SelectedItem = MList[0];
            //fillMachine();
            FillUsers();
        }
        private void fillBillType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "OPD"));
            MList.Add(new MasterListItem(2, "IPD"));
            // cmbDeliveryType.itemSource = MList;
            cmbBillType.ItemsSource = null;
            cmbBillType.ItemsSource = MList;
            cmbBillType.SelectedItem = MList[0];


        }
        private void fillMachine()
        {
            //machine name from machine master
            // cmbMachine.itemSource = MList;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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
                    //cmbMachine.ItemsSource = null;
                    //cmbMachine.ItemsSource = objList;
                    //cmbMachine.SelectedItem = objList[0];
                }

                fillSample();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void fillSample()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Collected By"));
            MList.Add(new MasterListItem(2, "Dispatched By"));//Dispatched by
            MList.Add(new MasterListItem(3, "Accepted By"));
            MList.Add(new MasterListItem(4, "Rejected By"));
            MList.Add(new MasterListItem(5, "Report Done By"));
            // cmbSample.itemSource = MList;
            cmbSample.ItemsSource = null;
            cmbSample.ItemsSource = MList;
            cmbSample.SelectedItem = MList[0];

        }
        private void cmbSample_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbSample.SelectedItem).ID == 0)
            {
                txtSample.IsEnabled = false;
                txtSample.Text = "";
            }
            else if (((MasterListItem)cmbSample.SelectedItem).ID != 0)
            {
                txtSample.IsEnabled = true;
            }
        }
        #endregion
        #region Private Methods
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.IsFrom = "Delivery";
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
                    POBBizAction.FirstName = txtFrontFirstName.Text.Trim();
                if (txtFrontLastName.Text != "")
                    POBBizAction.LastName = txtFrontLastName.Text.Trim();
                if (txtFrontMRNO.Text != "")
                    POBBizAction.MRNO = txtFrontMRNO.Text;
                //added by rohini dated 11.2.16
                if (txtBillNO.Text != "")
                    POBBizAction.BillNo = txtBillNO.Text;
                //
                //by rohini for CR Points
                if (cmbDeliveredBy.SelectedItem != null && ((MasterListItem)cmbDeliveredBy.SelectedItem).ID != 0)
                {
                    POBBizAction.UserID = ((MasterListItem)cmbDeliveredBy.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.UserID = 0;
                }
                if (cmbBillType.SelectedItem != null)
                {
                    POBBizAction.PatientType = ((MasterListItem)cmbBillType.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.PatientType = 0;
                }
                if (cmbResultEntry.SelectedItem != null && ((MasterListItem)cmbDelivery.SelectedItem).ID != 0)
                {
                    POBBizAction.StatusID = ((MasterListItem)cmbDelivery.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.StatusID = 0;
                }
                if (cmbResultEntry.SelectedItem != null && ((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)
                {
                    POBBizAction.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.AgencyID = 0;
                }
                if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
                {
                    POBBizAction.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
                }
                if (cmbDeliveryType.SelectedItem != null)
                {
                    POBBizAction.TypeID = ((MasterListItem)cmbDeliveryType.SelectedItem).ID;
                }
                else
                {
                    POBBizAction.TypeID = 0;
                }
                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;
         
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
                                //added by rohini
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
        private void FillOrderBookingDetailsList()
        {
            clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            BizAction.IsFrom = "Delivery";
            //added by rohini
            BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            //
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.CheckExtraCriteria = true;
            BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            BizAction.SampleType = POBBizAction.SampleType;
            BizAction.IsUploaded = POBBizAction.IsUploaded;
            BizAction.IsDelivered = POBBizAction.IsDelivered;
            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
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

            #region added by rohini new
            //added by rohini dated 11.2.16
            if ((MasterListItem)cmbSampleStatus.SelectedItem != null)
            {

                //
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 1)
                {
                    BizAction.OrderDetail.IsBilled = true;
                }
                else
                {
                    BizAction.OrderDetail.IsBilled = false;
                }
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 2)
                {
                    BizAction.OrderDetail.IsSampleCollected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleCollected = false;
                }
                //
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 3)
                {
                    BizAction.OrderDetail.IsSampleDispatch = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleDispatch = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
                {
                    BizAction.OrderDetail.IsSampleReceive = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSampleReceive = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 5)
                {
                    BizAction.OrderDetail.IsAccepted = true;
                }
                else
                {
                    BizAction.OrderDetail.IsAccepted = false;
                }
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 6)
                {
                    BizAction.OrderDetail.IsRejected = true;
                }
                else
                {
                    BizAction.OrderDetail.IsRejected = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 7)
                {
                    BizAction.OrderDetail.IsSubOptimal = true;
                }
                else
                {
                    BizAction.OrderDetail.IsSubOptimal = false;
                }

                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 8)
                {
                    BizAction.OrderDetail.IsOutSourced = true;
                }
                else
                {
                    BizAction.OrderDetail.IsOutSourced = false;
                }
                //added by rohini new
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 9)   //result entry done
                {
                    BizAction.OrderDetail.IsResultEntry = true;
                }
                else
                {
                    BizAction.OrderDetail.IsResultEntry = false;
                }
                if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 10)   //result report done
                {
                    BizAction.OrderDetail.IsDelivered = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDelivered = false;
                }
            }
            #endregion
   
            if ((MasterListItem)cmbDeliveryType.SelectedItem != null)
            {
                if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 1)   //report by email
                {
                    BizAction.OrderDetail.IsDeliverdthroughEmail = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDeliverdthroughEmail = false;
                }
                if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 2)   // report by hand
                {
                    BizAction.OrderDetail.IsDirectDelivered = true;
                }
                else
                {
                    BizAction.OrderDetail.IsDirectDelivered = false;
                }
            }
            //added by rohini 15.2.16
            if (((MasterListItem)cmbSample.SelectedItem != null))
            {
                if ((((MasterListItem)cmbSample.SelectedItem).ID == 1 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.SampleCollectedBy = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 2 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.DispatchBy = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 3 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 4 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
                }
                else if ((((MasterListItem)cmbSample.SelectedItem).ID == 5 && ((txtSample.Text != null) || (txtSample.Text != ""))))
                {
                    BizAction.OrderDetail.SampleReceiveBy = txtSample.Text;
                }
            }

            if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
            {
                BizAction.OrderDetail.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
            }
            //by rohini for cr points
            if (cmbDeliveredBy.SelectedItem != null && ((MasterListItem)cmbDeliveredBy.SelectedItem).ID != 0)
            {
                BizAction.OrderDetail.UserID = ((MasterListItem)cmbDeliveredBy.SelectedItem).ID;
            }
            else
            {
                BizAction.OrderDetail.UserID = 0;
            }
            if ((MasterListItem)cmbProcessedBy.SelectedItem != null)
            {
                if (((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)   // report by hand
                {
                    BizAction.OrderDetail.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
                }
                else
                {
                    BizAction.OrderDetail.AgencyID = 0;
                }
            }
            //if (cmbResultEntry.SelectedItem != null)
            //{
            //    BizAction.OrderDetail.AgencyID = ((MasterListItem)cmbResultEntry.SelectedItem).ID;
            //}
            //else
            //{
            //    BizAction.OrderDetail.AgencyID = 0;
            //}
            
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

                            if (item.IsDelivered == true)
                            {
                                item.IsDeliveredChecked = false;
                                item.IsDelivered = true;

                            }
                            else
                            {
                                item.IsDeliveredChecked = true;
                            }
                            if (item.ReportTemplate == true)
                                item.IsTemplateBased = false;
                            else
                                item.IsTemplateBased = true;
                            if (item.SendEmail == true)
                                item.SendEmailEnabled = false;
                            else
                                item.SendEmailEnabled = true;
                            if (item.IsOutSourced == true)
                                item.SampleOutSourceImage = "../Icons/tick.png";
                            else
                                item.SampleOutSourceImage = "../Icons/error.png";
                            if (item.IsCompleted == true)
                                item.ReportUploadImage = "../Icons/tick.png";
                            else
                                item.ReportUploadImage = "../Icons/error.png";
                            if (item.IsResultEntry == true)
                                item.ResultEntryImage = "../Icons/tick.png";
                            else
                                item.ResultEntryImage = "../Icons/error.png";

                            //
                            if (item.FirstLevel == true && item.SecondLevel == true)  //as per disscussion with anumani
                                item.FinalizedImage = "../Icons/tick.png";
                            else
                                item.FinalizedImage = "../Icons/error.png";
                            if (item.IsCompleted == true)
                                item.UploadImage = "../Icons/tick.png";
                            else
                                item.UploadImage = "../Icons/error.png";


                            if (item.IsOutSourced == true)
                            {
                                item.SampleOutSourceImage = "../Icons/tick.png";
                                item.IsOutSourcedD = false;
                            }                                //by rohinee for only dispatched unit should have rights to display reports
                            else if (item.DispatchToID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.UnitId != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            {
                                item.IsOutSourcedD = false;
                            }
                            else if (item.ReportTemplate == true)
                            {
                                item.IsOutSourcedD = false;
                            }
                            else
                            {
                                item.SampleOutSourceImage = "../Icons/error.png";
                                item.IsOutSourcedD = true;
                            }                            
                           
                            #region commented by rohinee

                            //    if (((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail.Count > 0)
                            //    {
                            //        var List = from r in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail
                            //                   where r.TestID == item.TestID && r.OrderBookingID == item.OrderBookingID
                            //                   select r;
                            //        var List1 = from r in List.ToList()
                            //                    where r.IsDefault == true
                            //                    select r;
                            //        if (List.ToList().Count > 0)
                            //        {
                            //            item.IsOutSourced = true;
                            //            if (List1.ToList().Count > 0)
                            //            {
                            //                foreach (clsPathOrderBookingDetailVO item2 in List1)
                            //                {
                            //                    if (item.IsOutSourced == true)
                            //                    {
                            //                        item.AgencyName = item2.AgencyName;
                            //                        item.AgencyID = item2.AgencyID;
                            //                        break;
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                foreach (clsPathOrderBookingDetailVO item2 in List)
                            //                {
                            //                    item.AgencyName = item2.AgencyName;
                            //                    item.AgencyID = item2.AgencyID;
                            //                    break;
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            item.IsOutSourced = false;
                            //        }
                            //    }
                            //}
                            #endregion
                            TestList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                            dgTest.ItemsSource = null;
                            TestList.ForEach(x => { if (!String.IsNullOrEmpty(x.AgencyName)) { x.SampleOutSourceImage = "../Icons/tick.png"; } });
                            dgTest.ItemsSource = TestList;
                            dgTest.UpdateLayout();
                            TestAgencyLinkList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail;
                            CheckBox chkAllSelect = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            if (TestList.ToList().All(x => x.IsDelivered == true))
                            {
                                chkAllSelect.IsChecked = true;
                                chkAllSelect.IsEnabled = false;
                            }
                            else
                            {
                                chkAllSelect.IsChecked = false;
                                chkAllSelect.IsEnabled = true;
                            }

                            DeliveredTestList.Clear();

                        }

                        CheckBox chkAllSelect1 = GetChildControl(dgTest, "chkSelectAll") as CheckBox;

                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId == ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId)
                        {
                            chkAllSelect1.IsEnabled = true;
                        }
                        else
                        {
                            chkAllSelect1.IsEnabled = false;
                        }


                      
                        indicator.Close();//((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ViewUploadedReport()
        {
            if (SelectedDetails.IsCompleted == true)
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        HtmlPage.Window.Invoke("OpenReport", new string[] { SelectedDetails.SourceURL });
                    }
                };
                client.UploadReportFileAsync(SelectedDetails.SourceURL, SelectedDetails.Report);
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This Test's Report Is Not Uploaded. Please Upload The Report Then Click On Preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }

        }

        //private void PrintReport(long ResultID)
        //{
        //    try
        //    {
        //        if (ResultID > 0)
        //        {
        //            if (dgOrdertList.SelectedItem == null)
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Record", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                return;
        //            }
        //            if (dgTest.SelectedItem == null)
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW =
        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Test", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                return;
        //            }
        //            long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
        //            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
        //            {
        //                if (ResultID == 0)
        //                    ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
        //                long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
        //                string msgTitle = "Palash";
        //                string msgText = "Do you want to print report on letter head?";
        //                MessageBoxControl.MessageBoxChildWindow msgW =
        //                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                {
        //                    if (res == MessageBoxResult.Yes)
        //                    {
        //                        PrintPage = true;
        //                        string URL = "../Reports/Pathology/PathoTemplateResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=true";
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }
        //                    else if (res == MessageBoxResult.No)
        //                    {
        //                        string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }

        //                };
        //                msgW.Show();
        //            }
        //            else
        //            {
        //                long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
        //                if (ResultID == 0)
        //                    ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
        //                long SampleNo = 0;
        //                try
        //                {
        //                    SampleNo = Convert.ToInt64(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo);
        //                }
        //                catch (Exception) { }
        //                string msgTitle = "Palash";
        //                string msgText = "Do you want to print report on letter head?";
        //                MessageBoxControl.MessageBoxChildWindow msgW =
        //                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                {
        //                    if (res == MessageBoxResult.Yes)
        //                    {
        //                        string URL = "../Reports/Pathology/PathoTemplateReportDelivery.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=true";
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }
        //                    else if (res == MessageBoxResult.No)
        //                    {
        //                        string URL = "../Reports/Pathology/PathoTemplateReportDelivery.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=false";
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }
        //                };
        //                msgW.Show();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private object GetChildControl(DependencyObject parent, string controlName)
        {
            Object tempObj = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < count; counter++)
            {
                //Get The Child Control based on Index
                tempObj = VisualTreeHelper.GetChild(parent, counter);

                //If Control's name Property matches with the argument control
                //name supplied then Return Control
                if ((tempObj as DependencyObject).GetValue(NameProperty).ToString() == controlName)
                    return tempObj;
                else //Else Search Recursively
                {
                    tempObj = GetChildControl(tempObj as DependencyObject, controlName);
                    if (tempObj != null)
                        return tempObj;
                }
            }
            return null;
        }
        #endregion

        #region private Event Handlers

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                    FillOrderBookingDetailsList();
                SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
            }
        }


        #endregion

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            FillOrderBookingList();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        private void PrintPathologyReport(string SampleNo)
        {
            bool IsPrinted = false;          
            if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
            {

                double BalanceAmount = Convert.ToDouble(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance);
                string ResultID = "";
                if (((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 ) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))) //by rohini dont change  (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) ||((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false))  && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)
                {
                    if (dgOrdertList.SelectedItem != null)
                    {
                        if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                        {                           
                            IsPrinted = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsPrinted;

                            BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;


                            ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID.ToString();

                        }
                        else
                        {
                           
                            IsPrinted = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsPrinted;
                          
                            ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillID.ToString();

                            BillNumber = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;
                            
                        }


                        List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                        list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                        var ReferralDoctorSignature = from r in list
                                                      where r.IsChecked = true
                                                      select r;
                        if (ReferralDoctorSignature != null)
                        {
                            ISRferalDoctorSignature = 1;
                        }
                        else
                            ISRferalDoctorSignature = 0;


                        if (IsPrinted == false)
                        {
                            IsDuplicate = false;
                        }
                        else
                        {
                            IsDuplicate = true;
                        }

                        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Collect The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();

                    mgbx.Show();
                }
            }
            else
            {
                //    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Please Select Test For Print.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //    mgbx.Show();
            }
        }
        private void CmdDelivered_Click(object sender, RoutedEventArgs e)
        {
            bool ResultStatus = false;
            bool ResultStatusO = false;
            try
            {
                indicator.Show();

                var item4 = (from r in TestList
                             where r.IsDelivered == true
                             select r).ToList();

                if (DeliveredTestList.Count == 0)
                {
                    //BY ROHINI
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test Is Not Selected,Please First Select The Test ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                //else if (item4.ToList().Count == TestList.ToList().Count)
                //{
                //    //BY ROHINI
                //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "All Test Are Already Delivered ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //    msgbox.Show();
                //}               
                else if (item4.ToList().Count > 0)
                {
                    var item1 = (from r in DeliveredTestList
                                 where r.IsOutSourced == false
                                 select r).ToList();
                    if (item1 != null && item1.ToList().Count > 0)
                    {
                        var item = (from r in item1
                                    where r.FirstLevel == true && r.SecondLevel == true && r.IsOutSourced == false
                                    select r).ToList();


                        //by rohini dated 5/12/16 for result entry from dispatched unit only
                        var itemDispatch = from r in DeliveredTestList
                                           where r.UnitId != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId
                                           select r;

                        if (itemDispatch != null && itemDispatch.ToList().Count != 0)
                        {
                            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Test is Collected From Another Clinic.Report Can Not Delivered ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgbox.Show();
                        }
                        else if (item.ToList().Count == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("PALASH", "Result Entry Or Authorization Is Not Done For Any Of Selected Test. \n So Report Can Not Be Deliver", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            mgbx.Show();
                        }
                        else
                        {
                            if (DeliveredTestList != null && DeliveredTestList.Count > 0)
                            {
                                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                                {

                                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Report Cannot Be Delivered From This Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    msgbox.Show();

                                }

                                else if (dgOrdertList.SelectedItem != null)
                                {
                                    if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 ) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                                    {
                                        clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();
                                        BizAction.PathOrderBookingDetail = new clsPathOrderBookingDetailVO();

                                        // Edited on 04/03/2016
                                        BizAction.PathOrderBookList = new List<clsPathOrderBookingDetailVO>();
                                        //  BizAction.PathOrderBookList = DeliveredTestList;


                                        //clsPathOrderBookingDetailVO newobj = (clsPathOrderBookingDetailVO)item;


                                        BizAction.PathOrderBookList = (List<clsPathOrderBookingDetailVO>)item;
                                        //

                                        //BizAction.TestDelivedList = new List<clsPathOrderBookingDetailVO>();
                                        //BizAction.TestDelivedList = DeliveredTestList;

                                        if ((clsPathOrderBookingDetailVO)dgTest.SelectedItem != null)
                                        {
                                            long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                        }
                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
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
                                                        try
                                                        {
                                                            DeliveredTestList.Clear();
                                                            FillOrderBookingList();
                                                        }
                                                        catch (Exception) { throw; }
                                                    }
                                                };
                                                mgbx.Show();

                                            }
                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The Bill Has Some Balance Amount.The Report Delivery Would Be Done Only After The Bill Is Completely Settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                        mgbx.Show();
                                    }
                                }
                                else if (dgOrdertList.SelectedItem == null)
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Is Not Selected. Please Select A Patient To Deliver The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                                else
                                {
                                    try
                                    {
                                        clsPathoCheckBillingStatusVO BizAction1 = new clsPathoCheckBillingStatusVO();
                                        BizAction1.OrderId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                                        BizAction1.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                                        //    BizAction1.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;

                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client1.ProcessCompleted += (s1, arg1) =>
                                        {

                                            if (arg1.Error == null)
                                            {
                                                ResultStatus = ((clsPathoCheckBillingStatusVO)arg1.Result).ResultStatus;



                                                if (ResultStatus == true)
                                                {
                                                    clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();
                                                    BizAction.PathOrderBookingDetail = new clsPathOrderBookingDetailVO();

                                                    // Edited on 04/03/2016
                                                    BizAction.PathOrderBookList = new List<clsPathOrderBookingDetailVO>();
                                                    //  BizAction.PathOrderBookList = DeliveredTestList;


                                                    //clsPathOrderBookingDetailVO newobj = (clsPathOrderBookingDetailVO)item;


                                                    BizAction.PathOrderBookList = (List<clsPathOrderBookingDetailVO>)item;
                                                    //

                                                    //BizAction.TestDelivedList = new List<clsPathOrderBookingDetailVO>();
                                                    //BizAction.TestDelivedList = DeliveredTestList;
                                                    if ((clsPathOrderBookingDetailVO)dgTest.SelectedItem != null)
                                                    {
                                                        long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                                    }
                                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
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
                                                                    try
                                                                    {
                                                                        DeliveredTestList.Clear();
                                                                        FillOrderBookingList();
                                                                    }
                                                                    catch (Exception) { throw; }
                                                                }
                                                            };
                                                            mgbx.Show();
                                                        }
                                                    };
                                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                                }
                                                else
                                                {
                                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The Bill Has Some Balance Amount.The Report Delivery Would Be Done Only After The Bill Is Completely Settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error); mgbx.Show();
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                                msgW1.Show();
                                            }
                                        };

                                        client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                                        client1.CloseAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }

                                }
                            }
                            else if (dgTest.SelectedItem == null)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test Is Not Selected. Please Select A Test To Deliver The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                        }
                    }

                }               
                var item2 = (from r in DeliveredTestList
                             where r.IsOutSourced == true
                             select r).ToList();

                if (item2 != null && item2.ToList().Count > 0)
                {
                    var item = (from r in item2
                                where r.IsCompleted == true
                                select r).ToList();


                    //   if (item == null && item.ToList().Count == 0)
                    if (item.ToList().Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("PALASH", "Result Is Not Uploaded For Any OF Selected Test. \n So Report Can Not Be Deliver", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                    else
                    {
                        var item3 = (from r in DeliveredTestList
                                     where r.IsDelivered == false
                                     select r).ToList();
                        if (DeliveredTestList != null && DeliveredTestList.Count > 0)
                        {
                            if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                            {

                                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Report Cannot Be Delivered From This Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                msgbox.Show();

                            }


                            else if (dgOrdertList.SelectedItem != null && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance <= 0)
                            {
                                clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();
                                BizAction.PathOrderBookingDetail = new clsPathOrderBookingDetailVO();

                                // Edited on 04/03/2016
                                BizAction.PathOrderBookList = new List<clsPathOrderBookingDetailVO>();
                                //  BizAction.PathOrderBookList = DeliveredTestList;
                                if ((List<clsPathOrderBookingDetailVO>)item != null)
                                    BizAction.PathOrderBookList = (List<clsPathOrderBookingDetailVO>)item;
                                //

                                //BizAction.TestDelivedList = new List<clsPathOrderBookingDetailVO>();
                                //BizAction.TestDelivedList = DeliveredTestList;
                                if ((clsPathOrderBookingDetailVO)dgTest.SelectedItem != null)
                                {
                                    long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                }
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
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
                                                try {
                                                    DeliveredTestList.Clear(); 
                                                    FillOrderBookingList();
                                                }
                                                catch (Exception) { throw; }
                                            }
                                        };
                                        mgbx.Show();
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            }
                            else
                            {
                                try
                                {
                                    clsPathoCheckBillingStatusVO BizAction1 = new clsPathoCheckBillingStatusVO();
                                    BizAction1.OrderId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                                    BizAction1.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                                    //    BizAction1.SampleNo = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo;

                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                    client1.ProcessCompleted += (s1, arg1) =>
                                    {

                                        if (arg1.Error == null)
                                        {
                                            ResultStatusO = ((clsPathoCheckBillingStatusVO)arg1.Result).ResultStatus;



                                            //if (ResultStatusO == true)
                                            //{
                                            if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                                            {
                                                clsUpdatePathOrderBookingDetailBizActionVO BizAction = new clsUpdatePathOrderBookingDetailBizActionVO();
                                                BizAction.PathOrderBookingDetail = new clsPathOrderBookingDetailVO();

                                                // Edited on 04/03/2016
                                                BizAction.PathOrderBookList = new List<clsPathOrderBookingDetailVO>();
                                                //  BizAction.PathOrderBookList = DeliveredTestList;


                                                //clsPathOrderBookingDetailVO newobj = (clsPathOrderBookingDetailVO)item;


                                                BizAction.PathOrderBookList = (List<clsPathOrderBookingDetailVO>)item;
                                                //

                                                //BizAction.TestDelivedList = new List<clsPathOrderBookingDetailVO>();
                                                //BizAction.TestDelivedList = DeliveredTestList;
                                                if ((clsPathOrderBookingDetailVO)dgTest.SelectedItem != null)
                                                {
                                                    long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                                                }
                                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
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
                                                                try {
                                                                    DeliveredTestList.Clear(); 
                                                                    FillOrderBookingList();
                                                                }
                                                                catch (Exception) { throw; }
                                                            }
                                                        };
                                                        mgbx.Show();
                                                    }
                                                };
                                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The Bill Has Some Balance Amount.The Report Delivery Would Be Done Only After The Bill Is Completely Settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error); mgbx.Show();
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW1.Show();
                                        }
                                    };

                                    client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    client1.CloseAsync();
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }

                            }
                        }
                        else if (dgTest.SelectedItem == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Test Is Not Selected. Please Select A Test To Deliver The Report.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                            mgbx.Show();
                        }
                    }
                }

                indicator.Close();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                bool check = chk.IsChecked.Value;
                IsSelectAll = true;
                if (check && AllTestList != null)
                {
                    foreach (clsPathOrderBookingDetailVO p in TestList)
                    {
                        if (p.IsDeliveredChecked == true)
                        {
                            AllTestList.Add(p);
                        }
                        TestList.ToList().ForEach(z => z.IsDelivered = true);
                    }
                }
                else
                {
                    AllTestList.Clear();
                    IsSelectAll = false;
                    foreach (clsPathOrderBookingDetailVO item in TestList)
                    {
                        if (item.IsDeliveredChecked == false)
                        {
                            item.IsDelivered = true;
                        }
                        else
                        {
                            item.IsDelivered = false;
                        }
                    }
                }
                DeliveredTestList = AllTestList;
                dgTest.ItemsSource = null;
                dgTest.ItemsSource = TestList;
                dgTest.UpdateLayout();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void chkSelectPrintAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                bool check = chk.IsChecked.Value;
                IsSelectAllPrint = true;
                if (check && AllTestPrintList != null)
                {
                    foreach (clsPathOrderBookingDetailVO p in TestList)
                    {
                        if (p.IsOutSourcedD == true)
                        {
                            AllTestPrintList.Add(p);
                        }
                        TestList.ToList().ForEach(z => z.IsPrint = true);
                    }
                }
                else
                {
                    AllTestPrintList.Clear();
                    IsSelectAllPrint = false;
                    foreach (clsPathOrderBookingDetailVO item in TestList)
                    {
                        if (item.IsOutSourcedD == false)
                        {
                            item.IsPrint = true;
                        }
                        else
                        {
                            item.IsPrint = false;
                        }
                    }
                }
                SelectedTestDetails = AllTestPrintList;
                dgTest.ItemsSource = null;
                dgTest.ItemsSource = TestList;
                dgTest.UpdateLayout();
            }
            catch (Exception)
            {
                throw;
            }
        }
        List<clsPathOrderBookingDetailVO> SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
        bool IsPrint = false;
        private void chk_Click1(object sender, RoutedEventArgs e)
        {
            BalanceAmount = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance;
            CheckBox chk = (CheckBox)sender;
          //  SelectedTestDetails.Clear();
            if (chk.IsChecked == true)
            {
                IsPrint = true;
                SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                //if (TestList.Count == SelectedTestDetails.Count)
                //{
                //    CheckBox chkSelectAllPrint = GetChildControl(dgTest, "chkSelectAllPrint") as CheckBox;
                //    chkSelectAllPrint.IsChecked = true;
                //}
            }
            else
            {
                IsPrint = false;
                SelectedTestDetails.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
              
                //CheckBox chkSelectAllPrint = GetChildControl(dgTest, "chkSelectAllPrint") as CheckBox;
                //chkSelectAllPrint.IsChecked = false;
            }
        }
        private void chk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        if ((bool)((CheckBox)sender).IsChecked)
                        {
                            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsServiceRefunded == true)
                            {
                                string msgText = "The Service of Selected Test Is Refunded.";

                                MessageBoxControl.MessageBoxChildWindow msgW =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW.Show();
                                ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                            }
                            else
                            {
                                SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                                DeliveredTestList.Add(SelectedTest);
                                //var item = from r in TestList
                                //           where r.IsSampleDispatchChecked == true
                                //           select r;
                                if (TestList.Count == DeliveredTestList.Count)
                                {
                                    CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                    chkSelectAll.IsChecked = true;
                                }
                            }
                        }
                        else
                        {
                            ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo = null;
                            ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced = false;
                            ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyName = null;
                            ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Quantity = null;
                            DeliveredTestList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = false;
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected = false;
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palsh", "Clinical Transactions Are Not Allowed At Head Office.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                    }
                }
                else
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsServiceRefunded == true)
                    {
                        string msgText = "The Service of Selected Test Is Refunded.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                        ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                    }
                    else
                    {
                        if ((bool)((CheckBox)sender).IsChecked)
                        {
                            SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                            DeliveredTestList.Add(SelectedTest);
                            var item = from r in TestList
                                       where r.IsDeliveredChecked == true
                                       select r;
                            if (TestList.ToList().Count == DeliveredTestList.Count)
                            {
                                CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                chkSelectAll.IsChecked = true;
                            }
                        }
                        else
                        {
                            //((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo = null;
                            //((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced = false;
                            //((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyName = null;
                            //((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Quantity = null;
                            DeliveredTestList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        String BillNumber = "0";
        int ISRferalDoctorSignature = 0;
        bool IsDuplicate = false;
        clsPathPatientReportVO ObjDetails;
        long TemplateID { get; set; }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if ((SelectedTestDetails != null && SelectedTestDetails.ToList().Count > 0) || SelectedDetails.ReportTemplate == true)
            {
                var ChkAlreadyApproved = from r in SelectedTestDetails
                                         where r.SecondLevel == false
                                         select r;

                if (ChkAlreadyApproved.ToList().Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Please First Approve The Test", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    bool IsPrinted = false;
                    string SampleNo = "";
                    string TestID = "";
                    bool ResultStatus = false;
                    long ResultId;
                    bool IsFinalized = false;

                    try
                    {
                        clsPathoCheckBillingStatusVO BizAction1 = new clsPathoCheckBillingStatusVO();
                        BizAction1.OrderId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        BizAction1.UnitId = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                  
                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client1.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null)
                            {
                                ResultStatus = ((clsPathoCheckBillingStatusVO)arg1.Result).ResultStatus;

                                if ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)) || (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External == 1))  //by rohini dont change (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == true) || ((((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance == 0 && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient == false)) 
                                {
                                   
                                    if (SelectedDetails.ReportTemplate != false)
                                    {
                                        if (SelectedDetails.SecondLevel == true)
                                        {
                                            if (SelectedDetails.UnitId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId || SelectedDetails.DispatchToID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                                            {
                                     
                                                        try
                                                        {
                                                        clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                                                        BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                                                        BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                                                        BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                                        client.ProcessCompleted += (s, arg) =>
                                                        {
                                                            if (arg.Error == null)
                                                            {
                                                                if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                                                                {
                                                                    ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;

                                                                    if (ObjDetails.TemplateDetails != null)
                                                                    {

                                                                        TemplateID = ObjDetails.TemplateDetails.TemplateID;

                                                                        PrintPathoReportDelivery win_PrintReport = new PrintPathoReportDelivery();

                                                                        win_PrintReport.ResultId = ObjDetails.TemplateDetails.ID;
                                                                        win_PrintReport.ObjDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                                                                        win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                                                                        win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                                                        win_PrintReport.IsOpdIpd = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Opd_Ipd_External;
                                                                        win_PrintReport.OrderUnitID = win_PrintReport.OrderUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                                                                        win_PrintReport.Show();
                                                         
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
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
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Test Is Not Collected Or Not Dispatched To This Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW1.Show();
                                        }
                                    }
                                    else if (SelectedDetails.IsOutSourced == true)
                                    {
                                         MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Selected Test Is Outsourced Can Not Print Report.Print Report From Upload Report .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW1.Show();
                                    }                    
                                    
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Authorization Not Done For Selected Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW1.Show();
                                    }                                    
                                   
                                   
                                    }                                 
                                    else                                    
                                     {
                                        
                                        if (SelectedTestDetails != null && SelectedTestDetails.Count() > 0)
                                        {
                                            string ResultID = "";
                                           
                                            if (dgOrdertList.SelectedItem != null)
                                            {
                                                if (SelectedTestDetails.Count() == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count())
                                                {
                                                    BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                                                 
                                                    IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                                                    ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);

                                                }
                                                else
                                                {
                                                    BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                                                   
                                                    IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                                                    ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);

                                                }


                                                if (dgTest.SelectedItem != null)
                                                {
                                                    if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                                                    {
                                                        foreach (var item in SelectedTestDetails)
                                                        {
                                                            if (item.FirstLevel == true && item.SecondLevel == true)  //by rohinee 
                                                            {
                                                                SampleNo = SampleNo + item.SampleNo;
                                                                SampleNo = SampleNo + ",";

                                                                TestID = TestID + item.TestID;
                                                                TestID = TestID + ",";

                                                            }
                                                        }
                                                        if (SampleNo.EndsWith(","))
                                                            SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);

                                                        if (TestID.EndsWith(","))
                                                            TestID = TestID.Remove(TestID.Length - 1, 1);
                                                    }
                                                }

                                                List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
                                                list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                                                var ReferralDoctorSignature = from r in list
                                                                              where r.IsChecked = true
                                                                              select r;
                                                if (ReferralDoctorSignature != null)
                                                {
                                                    ISRferalDoctorSignature = 1;
                                                }
                                                else
                                                    ISRferalDoctorSignature = 0;


                                                if (IsPrinted == false)
                                                {
                                                    IsDuplicate = false;
                                                }
                                                else
                                                {
                                                    IsDuplicate = true;
                                                }

                                                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                                long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
                                                string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo + "&TestID=" + TestID;
                                                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                                            }
                                        }
                                    }
                               
                        }
                        else
                        {
                            //comented by rohinee
                            //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Still You Want To Continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
                            //mgbx.Show();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please First Pay The Bill Then Print The Report.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };

                    client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Test To Print Report", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }
        private void PrintReport(long ResultId1)
        {
            long empid = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
            bool IsPrinted = false;
            string SampleNo = "";
            string ResultID;

            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
            {
                BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
                IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);               
            }
            else
            {
                BillNumber = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);             
                IsPrinted = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsPrinted;
                ResultID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).BillID);
            }
            if (dgTest.SelectedItem != null)
            {
                if (SelectedTestDetails != null && SelectedTestDetails.Count > 0)
                {
                    foreach (var item in SelectedTestDetails)
                    {
                       
                        SampleNo = SampleNo + item.SampleNo;
                        SampleNo = SampleNo + ",";
                      
                    }
                    if (SampleNo.EndsWith(","))
                        SampleNo = SampleNo.Remove(SampleNo.Length - 1, 1);
                }
            }

            List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
            list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
            var ReferralDoctorSignature = from r in list
                                          where r.IsChecked = true
                                          select r;
            if (ReferralDoctorSignature != null)
            {
                ISRferalDoctorSignature = 1;
            }
            else
                ISRferalDoctorSignature = 0;


            if (IsPrinted == false)
            {
                IsDuplicate = false;
            }
            else
            {
                IsDuplicate = true;
            }

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            long EmpID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.EmployeeID;
            string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature + "&EmpID=" + EmpID + "&SampleNo=" + SampleNo;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        PasswordWindow Win = new PasswordWindow();
        void msgW_OnMessageBoxClosed1(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Win = new PasswordWindow();
                Win.IsForReport = true;
                Win.OnOkButton_Click += new RoutedEventHandler(OnOkButton_Click1);
                Win.OnCancelButton_Click += new RoutedEventHandler(OnCancelButton_Click1);
                Win.Show();
            }
            else
            {               
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Having Balance Amount.Please first pay the bill then Take the Report.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
        void OnOkButton_Click1(object sender, RoutedEventArgs e)
        {
            Win.Close();
            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true)
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == true)
                {
                    try
                    {
                        clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
                        BizAction.ResultEntryDetails = new clsPathPatientReportVO();
                        BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                        BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                                {

                                    ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;

                                    if (ObjDetails.TemplateDetails != null)
                                    {
                                        TemplateID = ObjDetails.TemplateDetails.TemplateID;
                                        PrintPathoReportDelivery win_PrintReport = new PrintPathoReportDelivery();
                                        win_PrintReport.ResultId = ObjDetails.TemplateDetails.ID;
                                        win_PrintReport.ObjDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                                        win_PrintReport.ISFinalize = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized;
                                        win_PrintReport.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        win_PrintReport.Show();

                                    }
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
                else
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true)
                    {
                        PrintReport(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID);
                    }
                }
            }
            else
            {
                // To Print Upload Report
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsCompleted == true)
                {
                    ViewUploadedReport();
                }
            }

        }

        void OnCancelButton_Click1(object sender, RoutedEventArgs e)
        {
            //objAnimation.Invoke(RotationType.Backward);
        }

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dgOrdertList.SelectedItem != null)
        //        {
        //            if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).Balance > 0)
        //            {
        //                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "The bill has balance amount.The report can be delivered only after the bill is completely settled.", MessageBoxButtons.Ok, MessageBoxIcon.Error);
        //                mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(mgbxW_OnMessageBoxClosed);
        //                mgbx.Show();
        //            }
        //            else
        //            {
        //                if (dgTest.SelectedItem != null)
        //                {
        //                    long UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
        //                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == false)
        //                    {
        //                        //long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
        //                        //string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&BillID=" + billNoforPrint;
        //                        //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                        long ResultID = 0;
        //                        ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
        //                        BillNumber = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);

        //                        List<clsPathOrderBookingDetailVO> list = new List<clsPathOrderBookingDetailVO>();
        //                        list = (List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
        //                        var ReferralDoctorSignature = from r in list
        //                                                      where r.IsChecked = true
        //                                                      select r;
        //                        if (ReferralDoctorSignature != null)
        //                        {
        //                            ISRferalDoctorSignature = 1;
        //                        }
        //                        else
        //                            ISRferalDoctorSignature = 0;
        //                        string URL = "../Reports/Pathology/BSRPathologyResultEntryReport.aspx?ID=" + ResultID + "&BillNumber=" + BillNumber + "&UnitID=" + UnitID + "&IsDuplicate=" + IsDuplicate + "&ISRferalDoctorSignature=" + ISRferalDoctorSignature;
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }
        //                    else
        //                    {
        //                        long billNo = Convert.ToInt64(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo);
        //                        long SampleNo = 0;
        //                        try
        //                        {
        //                            SampleNo = Convert.ToInt64(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo);
        //                        }
        //                        catch (Exception) { }
        //                        long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
        //                        string URL = "../Reports/Pathology/PathoTemplateReportDelivery.aspx?ID=" + ResultID + "&UnitID=" + UnitID + "&PrintLetterHead=false";
        //                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Select the test to print the report", MessageBoxButtons.Ok, MessageBoxIcon.Error);
        //                    mgbx.Show();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void dgTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                SelectedDetails = new clsPathOrderBookingDetailVO();
                SelectedDetails = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
            }
        }

        void mgbxW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    long ResultID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).PathPatientReportID;
                    long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    MessageBoxControl.MessageBoxChildWindow mgbx =
                        new MessageBoxChildWindow("Palash", "Do You Want To Print Report On Letter Head? ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
                                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                            }
                        }
                        else if (res == MessageBoxResult.No)
                        {
                            string URL = "../Reports/Pathology/PathoResultEntry.aspx?BillID=" + billNoforPrint + "&UnitID=" + UnitID;
                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                        }
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #region Email Related Code
        List<long> SelectedTestIDList = new List<long>();
        List<long> SelectedTemplateTestIDList = new List<long>();
        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }
        private void CmdSendMail_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTestIDList.Count > 0 || SelectedTemplateTestIDList.Count > 0)
            {
                if (BalanceAmount <= 0)
                {
                    //frmSendEmail w = new frmSendEmail();
                    //w.SelectedDetails = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                    //w.SelectedTestDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                    //w.SelectedTestID = SelectedTestIDList;
                    //w.SelectedTestList = SelectedTestList;
                    //w.SelectedTemplateTestID = SelectedTemplateTestIDList;
                    //w.UnitID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
                    //w.ResultID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                    //w.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                    //w.Show();
                    SendMailChildWindow w = new SendMailChildWindow();
                    w.SelectedDetails = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                    w.SelectedTestDetails = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;

                    w.OnSaveButtonClick += new RoutedEventHandler(w_OnSaveButtonClick);
                    w.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Bill Is Not Paid.Please First Paid The Bill Then Send Mail", MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    mgbx.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Select Test.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW2.Show();
            }
        }

        void w_OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            //chkSelectAll.IsChecked = false;
            //AddedListlst.Clear();
            //NotComplteted.Clear();
        }

        private void chkEmailSend_Click(object sender, RoutedEventArgs e)
        {
            //report deleviry mail from only collection unit
            if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
            {

                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", " Email Cannot Be Sent From This Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
            else
                if (dgTest.SelectedItem != null)
                {
                    clsPathOrderBookingDetailVO objVO = (clsPathOrderBookingDetailVO)dgTest.SelectedItem;
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsFinalized == true)
                    {
                        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ReportTemplate == true)
                        {
                            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SendEmail == true)
                            {
                                SelectedTemplateTestIDList.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID);
                            }
                            else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SendEmail == false)
                            {
                                foreach (long item in SelectedTestIDList)
                                {
                                    if (item == ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID)
                                    {
                                        SelectedTemplateTestIDList.Remove(item);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SendEmail == true)
                            {
                                SelectedTestIDList.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID);
                            }
                            else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SendEmail == false)
                            {
                                foreach (long item in SelectedTestIDList)
                                {
                                    if (item == ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID)
                                    {
                                        SelectedTestIDList.Remove(item);
                                        break;
                                    }
                                }
                            }
                        }
                        CheckBox Check = sender as CheckBox;
                        if (Check.IsChecked == true)
                        {
                            #region single Test Selection
                            ////SelectedTestList = new List<clsPathOrderBookingDetailVO>();
                            ////SelectedTestList.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                            ////clsPathOrderBookingDetailVO obj = Check.DataContext as clsPathOrderBookingDetailVO;
                            ////TestList.Where(z => z.SendEmailEnabled == true).ToList().ForEach(z1 => z1.SendEmail = false);
                            ////TestList.Where(z => z.ID == obj.ID).FirstOrDefault().SendEmail = true;
                            #endregion
                            if (SelectedTestList != null)
                                SelectedTestList.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        }
                        else
                        {
                            if (objVO != null)
                            {
                                clsPathOrderBookingDetailVO obj;
                                if (SelectedTestList != null && SelectedTestList.Count > 0)
                                {
                                    obj = SelectedTestList.Where(z => z.TestID == objVO.TestID).FirstOrDefault();
                                    SelectedTestList.Remove(obj);
                                }
                            }
                        }
                        dgTest.UpdateLayout();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Result Not Finalized. Cannot Send Report To Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW2.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                FillOrderBookingList();
                            }
                        };
                        msgW2.Show();
                    }
                }
        }
        #endregion

      
    }
}
