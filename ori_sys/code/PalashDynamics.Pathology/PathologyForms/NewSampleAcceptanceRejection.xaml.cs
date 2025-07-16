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
using System.Text;
using PalashDynamics.UserControls;
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Browser;
using PalashDynamics.Service.EmailServiceReference;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewSampleAcceptanceRejection : UserControl
    {
        #region List and Variable Declaration
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        clsPathOrderBookingDetailVO SelectedTest;
        List<clsPathOrderBookingDetailVO> testList = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemptestList = new List<clsPathOrderBookingDetailVO>();
        bool IsPatientExist = false;
        bool isCollecionPopupShow = false;
        clsPathOrderBookingDetailVO selectedtestDispatchDateime;
        List<clsPathOrderBookingDetailVO> TestAgencyLinkList;
        List<clsPathOrderBookingDetailVO> SampleAcceptRejectList;
        List<clsPathOrderBookingDetailVO> TestReceiptList;
        StringBuilder TestIDBuilder;
        StringBuilder SampleNoBuilder;
        WaitIndicator indicator;
        bool IsSelectAll = false;
        public bool IsReject = false;
        SampleCollectionTimeChildWindow ww;
        private List<clsPathOrderBookingDetailVO> TestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestList = new List<clsPathOrderBookingDetailVO>();
        MessageBoxChildWindow msgbox;
        public bool IsSendRejectEmail;
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

        #region IInitiateCIMS Members

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

        #region Constructor
        public NewSampleAcceptanceRejection()
        {
            InitializeComponent();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " ";
            this.Loaded += new RoutedEventHandler(NewSampleAcceptanceRejection_Loaded);
            selectedtestDispatchDateime = new clsPathOrderBookingDetailVO();
            indicator = new WaitIndicator();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            SampleAcceptRejectList = new List<clsPathOrderBookingDetailVO>();
            TestReceiptList = new List<clsPathOrderBookingDetailVO>();
            TestIDBuilder = new StringBuilder();
            SampleNoBuilder = new StringBuilder();


            //added by rohini dated 5.4.16

            //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAllTest") as CheckBox;
            //chkSelectAll.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Loaded Event
        void NewSampleAcceptanceRejection_Loaded(object sender, RoutedEventArgs e)
        {

           

            lstSampleType = new List<MasterListItem>();
            lstUploadStatus = new List<MasterListItem>();
            lstDeliveryStatus = new List<MasterListItem>();
            lstSampleType.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 1, Description = "Accepted", Status = true });
            lstSampleType.Add(new MasterListItem() { ID = 2, Description = "Rejected", Status = true });

            lstUploadStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 1, Description = "Uploaded", Status = true });
            lstUploadStatus.Add(new MasterListItem() { ID = 2, Description = "Not Uploaded", Status = true });

            lstDeliveryStatus.Add(new MasterListItem() { ID = 0, Description = "--All--", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 1, Description = "Delivered", Status = true });
            lstDeliveryStatus.Add(new MasterListItem() { ID = 2, Description = "Not Delivered", Status = true });

           // cmbSampleType.ItemsSource = lstSampleType;
            //cmbUploadStatus.ItemsSource = lstUploadStatus;
            //cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;

           // cmbSampleType.SelectedItem = lstSampleType[0];
            //cmbUploadStatus.SelectedItem = lstUploadStatus[0];
            //cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement1.Text = "Sample Accept/Reject";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            //fillCatagory();
            FillOrderBookingList();

            //added by rohini dated 10.2.16
            fillSampleStatus();
            dtpFromDate.Focus();

            //
        }
        #endregion

        #region Private Methods and Event Handlers

        //private void fillCatagory()
        //{
        //    try
        //    {
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        //BizAction.IsActive = true;
        //        BizAction.MasterTable = MasterTableNameList.M_PathoCategory;
        //        BizAction.MasterList = new List<MasterListItem>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            if (e.Error == null && e.Result != null)
        //            {

        //                List<MasterListItem> objList = new List<MasterListItem>();

        //                objList.Add(new MasterListItem(0, "- Select -"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //                CmbCategory.ItemsSource = null;
        //                CmbCategory.ItemsSource = objList;
        //                CmbCategory.SelectedItem = objList[0];


        //                if (this.DataContext != null)
        //                {
        //                    CmbCategory.SelectedValue = ((clsPathoTestMasterVO)this.DataContext).CategoryID;

        //                }
        //            }

        //        };

        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.PatientID = 0;
                //added by rohini dated 29.2.16
                POBBizAction.IsDispatchedClinic = true;
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
                //added by rohini dated 11.2.16
                if (txtBillNO.Text != "")
                    POBBizAction.BillNo = txtBillNO.Text;
                //
                //if (CmbCategory.SelectedItem != null)
                //{
                //    POBBizAction.CatagoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;
                //}
                //else
                //{
                //    POBBizAction.CatagoryID = 0;
                //}
                //if (!String.IsNullOrEmpty(txtsample.Text))
                //    POBBizAction.SampleNo = Convert.ToInt64(txtsample.Text);

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

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                //BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                //BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                //if (((MasterListItem)cmbSampleType.SelectedItem).ID > 0)
                //{
                //    BizAction.CheckExtraCriteria = true;
                //    BizAction.CheckSampleType = true;
                //    BizAction.IsFromAcceptRejct = true;
                //    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
                //    {
                //        BizAction.SampleType = true;
                //    }
                //    else if (((MasterListItem)cmbSampleType.SelectedItem).ID == 2)
                //    {
                //        BizAction.SampleType = false;
                //    }
                //}
                //if (CmbCategory.SelectedItem != null)
                //{
                //    BizAction.TestCategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;
                //}
                //else
                //{
                //    BizAction.TestCategoryID = 0;
                //}


                BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
                BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.CheckExtraCriteria = true;
                BizAction.CheckSampleType = POBBizAction.CheckSampleType;
                BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
                BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
                BizAction.SampleType = POBBizAction.SampleType;
                BizAction.IsUploaded = POBBizAction.IsUploaded;
                BizAction.IsDelivered = POBBizAction.IsDelivered;
                BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

                //added by rohini dated 11.2.16
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
                    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 11)   //After Autoprint is done by rohini as per client requirement
                    {
                        BizAction.OrderDetail.IsClosedOrReported = true;
                    }
                    else
                    {
                        BizAction.OrderDetail.IsClosedOrReported = false;
                    }
                }
                #endregion

                #region commented by rohini
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
                //if((MasterListItem)cmbDelivery.SelectedItem!=null)
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
                    BizAction.OrderDetail.SampleNo =txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
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

                //
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            foreach (var item in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList)
                            {
                                if (item.IsSampleDispatch == true)
                                {
                                    item.IsSampleDispatchChecked = false;
                                }
                                else
                                {
                                    item.IsSampleDispatchChecked = true;
                                }
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
                                //if (item.IsSampleReceive == true)
                                //    item.SampleReceiveImage = "../Icons/tick.png";
                                //else
                                //    item.SampleReceiveImage = "../Icons/error.png";

                                if (item.IsSampleDispatch == true)
                                    item.SampleReceiveImage = "../Icons/tick.png";
                                else
                                    item.SampleReceiveImage = "../Icons/error.png";

                                if (item.IsSampleCollected == true)
                                    item.SampleCollectedImage = "../Icons/tick.png";
                                else
                                    item.SampleCollectedImage = "../Icons/error.png";

                                if (item.IsSampleCollected == true)
                                    item.IsSampleChecked = true;
                                else
                                    item.IsSampleChecked = false;
                                //COMMENTED BY ROHINI DATED 19.4.16
                                //if (item.IsOutSourced == true)
                                //{
                                //    item.SampleOutSourceImage = "../Icons/tick.png";
                                //}
                                //else if (item.IsOutSourced == false)
                                //{
                                //    item.SamplAcceptImage = "../Icons/error.png";
                                //    item.IsSampleAcceptEnable = false;
                                //    item.IsSampleAccepted = true;
                                //}

                                //commented by rohini for change in outsourcing login dated 13.4.16
                                //if (((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail.Count > 0)
                                //{
                                //    var List = from r in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail
                                //               where r.TestID == item.TestID && r.OrderBookingID == item.OrderBookingID && r.UnitId == item.UnitId
                                //               select r;
                                //    var List1 = from r in List.ToList()
                                //                where r.IsDefault == true
                                //                select r;
                                //    if (List.ToList().Count > 0)
                                //    {
                                //        item.IsOutSourced = true;
                                //        if (item.IsOutSourced == true)
                                //            item.SampleOutSourceImage = "../Icons/tick.png";
                                //        if (List1.ToList().Count > 0)
                                //        {
                                //            foreach (clsPathOrderBookingDetailVO item2 in List1)
                                //            {
                                //                if (item.IsChangedAgency == false)
                                //                    item.AgencyName = item2.AgencyName;
                                //                item.AgencyID = item2.AgencyID;
                                //                break;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            foreach (clsPathOrderBookingDetailVO item2 in List)
                                //            {
                                //                if (item.IsChangedAgency == false)
                                //                    item.AgencyName = item2.AgencyName;
                                //                item.AgencyID = item2.AgencyID;
                                //                break;
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        item.IsOutSourced = false;
                                //        item.SampleOutSourceImage = "../Icons/error.png";
                                //    }
                                //}
                            }
                            TestList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                            dgTest.ItemsSource = null;
                            dgTest.ItemsSource = TestList;
                            TestList.ForEach(x => { if (!String.IsNullOrEmpty(x.AgencyName)) { x.SampleOutSourceImage = "../Icons/tick.png"; } });
                            dgTest.UpdateLayout();
                            TestAgencyLinkList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail;
                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            if (TestList.ToList().All(x => x.SampleAcceptRejectStatus != 0))
                            {
                                chkSelectAll.IsChecked = true;
                                chkSelectAll.IsEnabled = false;
                            }
                            else
                            {
                                chkSelectAll.IsChecked = false;
                                chkSelectAll.IsEnabled = true;
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    if ((bool)((CheckBox)sender).IsChecked)
                    {
                        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsServiceRefunded ==true)
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
                            SampleAcceptRejectList.Add(SelectedTest);
                            var item = from r in TestList
                                       where r.IsSampleAcceptEnable == true
                                       select r;
                            if (item.ToList().Count == SampleAcceptRejectList.Count)
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
                        SampleAcceptRejectList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;
                    }
                }
                else
                {
                    ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected = false;
                    MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palsh", "Clinical transactions are not allowed at Head Office.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbx.Show();
                }
            }
            else
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsServiceRefunded ==true)
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
                        SampleAcceptRejectList.Add(SelectedTest);
                        var item = from r in TestList
                                   where r.IsSampleAcceptEnable == true
                                   select r;
                        if (item.ToList().Count == SampleAcceptRejectList.Count)
                        {
                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = true;
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Quantity = null;
                        SampleAcceptRejectList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;
                    }
                }
            }
        }

        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
            BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();
            BizAction.OrderBookingDetaildetails.IsFromSampleAcceptReject = true;
            this.SampleAcceptRejectList = ww.itemList;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (ww.IsSampleReject == true)
            {
                SampleAcceptRejectList.ToList().ForEach(x => x.SampleAcceptRejectStatus = 2);
            }
            else
            {
                SampleAcceptRejectList.ToList().ForEach(x => x.SampleAcceptRejectStatus = 1);
            }
            BizAction.OrderBookingDetailList = SampleAcceptRejectList;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (ww.IsSampleReject == true)
                        {
                            msgbox = new MessageBoxChildWindow("Palash", "Sample is Rejected.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            if (ww.SendRejectEmail == true)
                            {
                                SendSampleRejectEmail();
                            }
                        }
                        else
                        {
                            msgbox = new MessageBoxChildWindow("Palash", "Sample is Accepted successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        }
                        msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                SampleAcceptRejectList.Clear();
                                FillOrderBookingList();
                                CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                chkSelectAll.IsChecked = false;
                            }
                        };
                        msgbox.Show();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SampleAcceptRejectList.Clear();
            FillOrderBookingList();
            isCollecionPopupShow = false;
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (SampleAcceptRejectList != null && SampleAcceptRejectList.Count > 0)
            {
                var item = from r in SampleAcceptRejectList
                           where r.IsSampleReceive == false
                           select r;

                  var item1 = from r in SampleAcceptRejectList
                           where r.IsSampleDispatch == false
                           select r;

                var itemDispatch = from r in SampleAcceptRejectList
                                   where r.DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId
                                   select r;

                //commented by rohini for no need of receive screen
                //if (item != null && item.ToList().Count > 0)
                //{
                //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select Test/Tests whose Sample is Received.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //    msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                //    {
                //        if (res == MessageBoxResult.OK)
                //        {
                //            SampleAcceptRejectList.Clear();
                //            FillOrderBookingList();
                //            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                //            chkSelectAll.IsChecked = false;
                //        }
                //    };
                //    msgbox.Show();
                //}
                //

                if (item1 != null && item1.ToList().Count > 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select Test/Tests whose Sample is Dispatched.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            SampleAcceptRejectList.Clear();
                            FillOrderBookingList();
                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = false;
                        }
                    };
                    msgbox.Show();
                }
                //added by rohini for cross clinic handle dispatch item to another clinic
                //else if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                //{
                else if (itemDispatch != null && itemDispatch.ToList().Count != 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is dispatched to another Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                else
                {
                    ww = new SampleCollectionTimeChildWindow();
                    ww.IsFromSampleAccept = true;
                    ww.IsSampleReject = false;
                    ww.itemList = SampleAcceptRejectList;
                    ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                    ww.Show();
                }
            }
            else
            {
                var item = from r in TestList
                           where r.IsSampleAcceptEnable == true
                           select r;
                if (item != null && item.ToList().Count == 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "SAMPLE IS ALREADY accepted  for all test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select To Accept ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            isCollecionPopupShow = false;
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
            chkSelectAll.IsChecked = false;
        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected)
            {
                NewBarcodeForm win = new NewBarcodeForm();
                string MRNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo.ToUpper();
                string OrderNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderNo;
                string SampleNo = (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo).ToString();
                string PName = (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).FirstName + " " + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).LastName);
                DateTime C_Time = Convert.ToDateTime(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleCollected);
                string SC_Date = C_Time.ToShortDateString();
                string SC_Time = C_Time.ToShortTimeString();
                string P_Gender = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID);
                if (P_Gender == Convert.ToString(1))
                { P_Gender = "Male"; }
                else
                { P_Gender = "Female"; }
                DateTime Page = Convert.ToDateTime(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DateOfBirth);
                int years = DateTime.Now.Year - Page.Year;
                win.PrintData = MRNo;
                win.P_Name = PName;
                win.P_Date = SC_Date;
                win.P_Time = SC_Time;
                win.P_Gender = P_Gender;
                win.P_Age = Convert.ToString(years);
                win.Show();
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Enter Sample Details.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontLastName.Text = txtFrontLastName.Text.ToTitleCase();
        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFrontFirstName.Text = txtFrontFirstName.Text.ToTitleCase();
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            IsSelectAll = true;
            if (check && AllTestList != null)
            {
                foreach (clsPathOrderBookingDetailVO p in TestList)
                {
                    if (p.IsSampleAcceptEnable == true)
                    {
                        AllTestList.Add(p);
                    }
                    //TestList.ToList().ForEach(z => z.IsSampleAccepted = true);
                    //added by rohini dated 4.4.16
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsProcessingUnit != false)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleAccepted = true);
                    }
                    else if (p.DispatchToID == (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId && p.DispatchToID != 0)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleAccepted = true);
                    }
                  
                }
            }
            else
            {
                AllTestList.Clear();
                IsSelectAll = false;
                foreach (clsPathOrderBookingDetailVO item in TestList)
                {
                    if (item.IsSampleAcceptEnable == false)
                    {
                        item.IsSampleAccepted = true;
                    }
                    else
                    {
                        item.IsSampleAccepted = false;
                    }
                }
            }
            SampleAcceptRejectList = AllTestList;
            dgTest.ItemsSource = null;
            dgTest.ItemsSource = TestList;
            dgTest.UpdateLayout();
        }

        private void CmdReject_Click(object sender, RoutedEventArgs e)
        {
            if (SampleAcceptRejectList != null && SampleAcceptRejectList.Count > 0)
            {
                IsReject = true;
                var item = from r in SampleAcceptRejectList
                           where r.IsSampleReceive == false
                           select r;
                if (item != null && item.ToList().Count > 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select Test/Tests whose Sample is Received.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            SampleAcceptRejectList.Clear();
                            FillOrderBookingList();
                            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = false;
                        }
                    };
                    msgbox.Show();
                }
                else
                {
                    ww = new SampleCollectionTimeChildWindow();
                    ww.IsFromSampleAccept = true;
                    ww.IsSampleReject = true;
                    ww.itemList = SampleAcceptRejectList;
                    ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                    ww.Show();
                }
            }
            else
            {
                //var item = from r in TestList
                //           where r.IsSampleAcceptEnable == false
                //           select r;
                //if (item != null && item.ToList().Count == 0)
                //{
                //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "SAMPLE IS ALREADY rejected for all test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //    msgbox.Show();
                //}
                //else
                //{
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select To  Reject", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
                //}
            }
        }

        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Collapsed;
            docOrderList.Visibility = Visibility.Collapsed;
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Visible;
            docOrderList.Visibility = Visibility.Visible;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

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

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded ==true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;


            //added by rohini for cross clinic handle dispatch item to another clinic            

            if (dgTest.ItemsSource != null)
            {
                              
                //added by rohini for collection center 
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).IsProcessingUnit == false)
                {
                    e.Row.IsEnabled = false;
                }
                else if (((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId && ((clsPathOrderBookingDetailVO)e.Row.DataContext).DispatchToID != 0)
                {
                    e.Row.IsEnabled = false;
                }
                else
                    e.Row.IsEnabled = true;
            }
        }

        long TestID = 0;
        string sampleNo = "";
        long ParameterCategoryID = 0;
        long AgeInDays = 0;

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
            MList.Add(new MasterListItem(11, "Closed/Reported"));
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
                    cmbProcessedBy.SelectedItem = objList[0];

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            fillDeliveryType();
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
            MList.Add(new MasterListItem(1, "Delivered"));
            MList.Add(new MasterListItem(2, "Not Delivered"));
            // cmbDelivery.itemSource = MList;
            cmbDelivery.ItemsSource = null;
            cmbDelivery.ItemsSource = MList;
            cmbDelivery.SelectedItem = MList[0];
            fillMachine();
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

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            fillSample();
        }
        private void fillSample()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Collected By"));
            MList.Add(new MasterListItem(2, "Sent By"));//Dispatched by
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
        private void FillCategory()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PathoParameterCategoryMaster;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 3 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 4)
                        {
                            ParameterCategoryID = objList[1].ID;
                        }
                        else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 1)
                        {
                            ParameterCategoryID = objList[4].ID;
                        }
                        if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 2 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 5 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 6)
                        {
                            ParameterCategoryID = objList[2].ID;
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TestReceipt_Click(object sender, RoutedEventArgs e)
        {

            FillCategory();
            string TestIDList = String.Empty;
            string SampleNoList = String.Empty;
            TestIDBuilder = new StringBuilder();
            SampleNoBuilder = new StringBuilder();
            long PatientID = 0;
            long PatientUnitID = 0;
             if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).DispatchToID != (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is dispatched to another Clinic.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
            else if (TestReceiptList != null && TestReceiptList.Count > 0)
            {
                foreach (clsPathOrderBookingDetailVO item in TestReceiptList)
                {
                    TestID = item.TestID;
                    sampleNo = item.SampleNo;
                    TestIDBuilder.Append(TestID).Append(",");
                    SampleNoBuilder.Append(sampleNo).Append(",");
                }
                TestIDList = TestIDBuilder.ToString();
                TestIDBuilder = TestIDBuilder.Remove(TestIDBuilder.Length - 1, 1);
                TestIDList = TestIDBuilder.ToString();
                SampleNoList = SampleNoBuilder.ToString();
                //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 3 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 4)
                //{
                //    ParameterCategoryID = 1;
                //}
                //else if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 1)
                //{
                //    ParameterCategoryID = 4;
                //}
                //if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 2 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 5 || ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PrefixID == 6)
                //{
                //    ParameterCategoryID = 2;
                //}
                if (dgOrdertList.SelectedItem != null)
                {
                    AgeInDays = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).AgeInDays;
                    PatientID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientID;
                    PatientUnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                }
                if (TestReceiptList.Count > 0)
                {
                    string URL = "../Reports/Pathology/TestReceiptReport.aspx?TestIDList=" + TestIDList + "&SampleNoList=" + SampleNoList + "&ParameterCategoryID=" + ParameterCategoryID + "&AgeInDays=" + AgeInDays + "&PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        private void chkSelectAllTest_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ChkAll = sender as CheckBox;
            TestReceiptList.Clear();
            TestReceiptList = new List<clsPathOrderBookingDetailVO>();
            if (ChkAll.IsChecked == true)
            {
                TestReceiptList.AddRange((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource);
                ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().ForEach(z => z.IsSelectedForReceipt = true);
                dgTest.UpdateLayout();
            }
            else
            {
                TestReceiptList.Clear();
                ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().ForEach(z => z.IsSelectedForReceipt = false);
                dgTest.UpdateLayout();
            }
        }

        private void SelectTest_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.SelectedItem != null)
            {
                CheckBox Chk = sender as CheckBox;
                if (Chk.IsChecked == true)
                {
                    if (TestReceiptList != null)
                        TestReceiptList.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                    if (TestReceiptList.Count == ((List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource).ToList().Count)
                    {
                        CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAllTest") as CheckBox;
                        chkSelectAll.IsChecked = true;
                    }
                }
                else
                {
                    if (TestReceiptList != null && TestReceiptList.Count > 0)
                        TestReceiptList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                    CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAllTest") as CheckBox;
                    chkSelectAll.IsChecked = false;
                }
            }
        }

        private void txtsample_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FillOrderBookingList();
        }

        #region Email and SMS Code
        private void SendSampleRejectEmail()
        {
            if (dgOrdertList.SelectedItem != null)
            {
                //string fromEmail = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
                //string toEmail = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientEmailId;
                //long unitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                //string subject = "Sample Rejection Email-" + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientName + "_Bill Number : " + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).BillNo;
                //StringBuilder myBuilder = new StringBuilder();
                //myBuilder.Append("<p><span class=" + "Normal" + ">Dear  , </span></p>");
                //myBuilder.Append(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).PatientName);
                //myBuilder.Append("<p><span class=" + "Normal" + ">MR No. is   , </span></p>");
                //myBuilder.Append(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo);
                //myBuilder.Append("<p><strong><span>-- </span> </strong></p> " +
                //            " <p><strong><span></span></strong><span>Thank you so much for giving the sample to performe test examination.<p><span> We regret to inform you that the collected sample has been rejected.</span></p> <p><span><b>Requesting you to visit again the Afri Global Laboratories and give you sample again so that we can perform the test.</b></span></p> " +
                //            "<p><span>Apologies for inconvince caused..</span></p>" +
                //            "<p><strong><span></span></strong><span> &nbsp;&nbsp;&nbsp; </span></p>");
                //myBuilder.Append("<p><span class=" + "Normal" + ">Thanks and Regards, </span></p> <p><span class=" + "Normal" + ">Afrigolobal Medicare Ltd </span></p>");
                //string body = myBuilder.ToString();
                //Uri address8 = new Uri(Application.Current.Host.Source, "../EmailService.svc");
                //EmailServiceClient client8 = new EmailServiceClient("CustomBinding_EmailService", address8.AbsoluteUri);
                //client8.SendEmailCompleted += (p1, arg1) =>
                //{
                //    if (arg1.Error == null)
                //    {
                //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Rejection Email Sent Sucessfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //        msgbox.Show();
                //    }
                //    else
                //    {
                //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Email of patient is not mentioned while registering.\n Please, register email of that patient.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //        msgbox.Show();
                //    }
                //};
                //client8.SendEmailAsync(fromEmail, toEmail, subject, body, unitID, string.Empty);
                //client8.CloseAsync();
            }
        }

        private void SendSMSForJeject()
        {
            if (dgOrdertList.SelectedItem != null)
            {
                clsPathOrderBookingVO objVO = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;

            }
        }
        #endregion
    }
}
