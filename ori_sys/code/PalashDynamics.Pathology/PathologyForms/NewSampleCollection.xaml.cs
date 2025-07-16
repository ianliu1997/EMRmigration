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
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewSampleCollection : UserControl
    {
        #region List and Variable Declaration
        List<MasterListItem> lstSampleType { get; set; }
        List<MasterListItem> lstUploadStatus { get; set; }
        List<MasterListItem> lstDeliveryStatus { get; set; }
        clsGetPathOrderBookingListBizActionVO POBBizAction { get; set; }
        clsPathOrderBookingDetailVO SelectedTest;
        List<clsPathOrderBookingDetailVO> TemptestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> TestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestList = new List<clsPathOrderBookingDetailVO>();
        bool IsPatientExist = false;
        bool isCollecionPopupShow = false;
        List<clsPathOrderBookingDetailVO> TestAgencyLinkList;
        List<clsPathOrderBookingDetailVO> SampleCollectionList;
        SampleCollectionTimeChildWindow ww;
        clsPathOrderBookingDetailVO selectedtestcollectiontime = new clsPathOrderBookingDetailVO();

        Color Pending;
        Color Partial;
        Color Collected;
        #endregion

        #region Constructor
        public NewSampleCollection()
        {
           

            InitializeComponent();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " "; 
            this.Loaded += new RoutedEventHandler(NewSampleCollection_Loaded);
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            //======================================================
            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            SampleCollectionList = new List<clsPathOrderBookingDetailVO>();

            Pending = new Color();
            Pending.A = Convert.ToByte(120);
            Pending.R = Convert.ToByte(255);
            Pending.G = Convert.ToByte(0);
            Pending.B = Convert.ToByte(0);
            txtPending.Background = new SolidColorBrush(Pending);

            Partial = new Color();
            Partial.A = Convert.ToByte(120);
            Partial.R = Convert.ToByte(240);
            Partial.G = Convert.ToByte(230);
            Partial.B = Convert.ToByte(140);
            txtpartial.Background = new SolidColorBrush(Partial);

            Collected = new Color();
            Collected.A = Convert.ToByte(120);
            Collected.R = Convert.ToByte(32);
            Collected.G = Convert.ToByte(178);
            Collected.B = Convert.ToByte(170);
            txtCollected.Background = new SolidColorBrush(Collected);    


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

        #region Loaded Event
        void NewSampleCollection_Loaded(object sender, RoutedEventArgs e)
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

           // cmbSampleType.ItemsSource = lstSampleType;
            //cmbUploadStatus.ItemsSource = lstUploadStatus;
            //cmbDeliveryStatus.ItemsSource = lstDeliveryStatus;

           // cmbSampleType.SelectedItem = lstSampleType[0];
            //cmbUploadStatus.SelectedItem = lstUploadStatus[0];
            //cmbDeliveryStatus.SelectedItem = lstDeliveryStatus[0];

            (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement1.Text = "Sample Collection";
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            //fillCatagory();
            FillOrderBookingList();
            //added by rohini dated 10.2.16
            fillSampleStatus();
            dtpFromDate.Focus();
            fillBillType();
            //
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
        WaitIndicator indicator = new WaitIndicator();
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }
        #endregion

        #region added by rohini dated 10/2/16
        private void fillSampleStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();

            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Pending"));
            MList.Add(new MasterListItem(2, "Partial Collected"));
            MList.Add(new MasterListItem(3, "Collected"));
            MList.Add(new MasterListItem(4, "Sub optimal"));
            //MList.Add(new MasterListItem(6, "Rejected"));
            //MList.Add(new MasterListItem(7, "Sub Optimal"));
            //MList.Add(new MasterListItem(8, "Outsourced"));
            //MList.Add(new MasterListItem(9, "Result Entry Done"));
            //MList.Add(new MasterListItem(10, "Report Delivered"));
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
                    //cmbProcessedBy.SelectedItem = objList[0];
                    foreach (var item in objList)
                    {
                        if (item.ID==0)
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

                fillSample();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            
        }
        private void fillSample()
        {
            //List<MasterListItem> MList = new List<MasterListItem>();
            //MasterListItem objConversion = new MasterListItem();

            //MList.Add(new MasterListItem(0, "- Select -"));
            //MList.Add(new MasterListItem(1, "Collected By"));
            //MList.Add(new MasterListItem(2, "Dispatched By"));//Dispatched by
            //MList.Add(new MasterListItem(3, "Accepted By"));
            //MList.Add(new MasterListItem(4, "Rejected By"));
            //MList.Add(new MasterListItem(5, "Report Done By"));
            //// cmbSample.itemSource = MList;
            //cmbSample.ItemsSource = null;
            //cmbSample.ItemsSource = MList;
            //cmbSample.SelectedItem = MList[0];

            try
            {
                clsGetPathoUsersBizActionVO BizAction = new clsGetPathoUsersBizActionVO();
                BizAction.MenuID = (long)10564;
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

                        cmbSample.ItemsSource = null;
                        cmbSample.ItemsSource = objList;
                        cmbSample.SelectedItem = objList[0];
                    }
                    // FillTestList();
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
        private void fillBillType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "OPD"));
            MList.Add(new MasterListItem(2, "IPD"));
            cmbbilltype.ItemsSource = null;
            cmbbilltype.ItemsSource = MList;
            cmbbilltype.SelectedItem = MList[0];
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
        #region Private Methods and Event Handlers
        private void FillOrderBookingList()
        {
            try
            {
                (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder) = null;
                indicator.Show();
                POBBizAction = new clsGetPathOrderBookingListBizActionVO();
                POBBizAction.PatientID = 0;
                POBBizAction.IsFrom = "SampleCollection";
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
                //added by vikrant for search critria 
                if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
                {
                    POBBizAction.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
                }
                if ((MasterListItem)cmbbilltype.SelectedItem != null && ((MasterListItem)cmbbilltype.SelectedItem).ID > 0)
                {
                    POBBizAction.PatientType = Convert.ToInt32(((MasterListItem)cmbbilltype.SelectedItem).ID);
                }
                if ((MasterListItem)cmbSample.SelectedItem != null && ((MasterListItem)cmbSample.SelectedItem).ID > 0)
                {
                    POBBizAction.UserID = Convert.ToInt64(((MasterListItem)cmbSample.SelectedItem).ID);
                }
                if ((MasterListItem)cmbSampleStatus.SelectedItem != null && ((MasterListItem)cmbSampleStatus.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
                    {
                        POBBizAction.IsSubOptimal = true;
                    }
                    else
                    {
                        POBBizAction.StatusID = Convert.ToInt64(((MasterListItem)cmbSampleStatus.SelectedItem).ID);
                    }
                }

                POBBizAction.IsPagingEnabled = true;
                POBBizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                POBBizAction.MaximumRows = DataList.PageSize;
                //if (cmbSampleType.SelectedItem != null && ((MasterListItem)cmbSampleType.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckSampleType = false;
                //}
                //else
                //{
                //    POBBizAction.CheckSampleType = true;
                //    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
                //        POBBizAction.SampleType = true;
                //    else
                //        POBBizAction.SampleType = false;
                //}
                //if (cmbUploadStatus.SelectedItem != null && ((MasterListItem)cmbUploadStatus.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckUploadStatus = false;
                //}
                //else
                //{
                //    POBBizAction.CheckUploadStatus = true;
                //    if (((MasterListItem)cmbUploadStatus.SelectedItem).ID == 1)
                //        POBBizAction.IsUploaded = true;
                //    else
                //        POBBizAction.IsUploaded = false;
                //}
                //if (cmbDeliveryStatus.SelectedItem != null && ((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 0)
                //{
                //    POBBizAction.CheckDeliveryStatus = false;
                //}
                //else
                //{
                //    POBBizAction.CheckDeliveryStatus = true;
                //    if (((MasterListItem)cmbDeliveryStatus.SelectedItem).ID == 1)
                //        POBBizAction.IsDelivered = true;
                //    else
                //        POBBizAction.IsDelivered = false;
                //}
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
                                    //code to change
                                    //if (item.IsExternalPatient == false || item.IsResendForNewSample == true)
                                    //{
                                    //    DataList.Add(item);
                                    //}
                                    //if (item.IsExternalPatient == true || item.IsResendForNewSample == true)
                                    //{
                                        DataList.Add(item);
                                    //}
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

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = (clsPathOrderBookingVO)dgOrdertList.SelectedItem;
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                    FillOrderBookingDetailsList();
            }
            #region commented by rohini
            //try
            //{
            //    indicator.Show();
            //    dgTest.ItemsSource = null;
            //    SampleCollectionList.Clear();
            //    SelectedTestDetails.Clear();//by roihini for barcode
            //    if (dgOrdertList.SelectedItem != null)
            //    {
            //        CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
            //        chkSelectAll.IsChecked = false;

            //        clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            //        //BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            //        //BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            //        //if (((MasterListItem)cmbSampleType.SelectedItem).ID > 0)
            //        //{
            //        //    BizAction.CheckExtraCriteria = true;
            //        //    BizAction.CheckSampleType = true;
            //        //    BizAction.IsFromCollection = true;
            //        //    if (((MasterListItem)cmbSampleType.SelectedItem).ID == 1)
            //        //    {
            //        //        BizAction.SampleType = true;
            //        //    }
            //        //    else if (((MasterListItem)cmbSampleType.SelectedItem).ID == 2)
            //        //    {
            //        //        BizAction.SampleType = false;
            //        //    }
            //        //}
            //        //if (CmbCategory.SelectedItem != null)
            //        //{
            //        //    BizAction.TestCategoryID = ((MasterListItem)CmbCategory.SelectedItem).ID;
            //        //}
            //        //else
            //        //{
            //        //    BizAction.TestCategoryID = 0;
            //        //}

            //        BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            //        BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            //        BizAction.CheckExtraCriteria = true;
            //        BizAction.CheckSampleType = POBBizAction.CheckSampleType;
            //        BizAction.CheckUploadStatus = POBBizAction.CheckUploadStatus;
            //        BizAction.CheckDeliveryStatus = POBBizAction.CheckDeliveryStatus;
            //        BizAction.SampleType = POBBizAction.SampleType;
            //        BizAction.IsUploaded = POBBizAction.IsUploaded;
            //        BizAction.IsDelivered = POBBizAction.IsDelivered;
            //        BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
            //        BizAction.IsExternalPatient = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient;

            //        //Added By Bhushanp 19012017 For Date Filter
            //        if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            //        {
            //            BizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
            //            BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
            //        }
            //        else
            //        {
            //            BizAction.FromDate = null;
            //            BizAction.ToDate = null;
            //        }
            //        //added by rohini dated 11.2.16
            //        #region added by rohini new
            //        //added by rohini dated 11.2.16
            //        if ((MasterListItem)cmbSampleStatus.SelectedItem != null)
            //        {

            //            //
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 1)
            //            {
            //                BizAction.OrderDetail.IsBilled = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsBilled = false;
            //            }
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 2)
            //            {
            //                BizAction.OrderDetail.IsSampleCollected = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsSampleCollected = false;
            //            }
            //            //
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 3)
            //            {
            //                BizAction.OrderDetail.IsSampleDispatch = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsSampleDispatch = false;
            //            }

            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 4)
            //            {
            //                BizAction.OrderDetail.IsSampleReceive = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsSampleReceive = false;
            //            }

            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 5)
            //            {
            //                BizAction.OrderDetail.IsAccepted = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsAccepted = false;
            //            }
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 6)
            //            {
            //                BizAction.OrderDetail.IsRejected = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsRejected = false;
            //            }

            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 7)
            //            {
            //                BizAction.OrderDetail.IsSubOptimal = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsSubOptimal = false;
            //            }

            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 8)
            //            {
            //                BizAction.OrderDetail.IsOutSourced = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsOutSourced = false;
            //            }
            //            //added by rohini new
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 9)   //result entry done
            //            {
            //                BizAction.OrderDetail.IsResultEntry = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsResultEntry = false;
            //            }
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 10)   //result report done
            //            {
            //                BizAction.OrderDetail.IsDelivered = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsDelivered = false;
            //            }
            //            if (((MasterListItem)cmbSampleStatus.SelectedItem).ID == 11)   //After Autoprint is done by rohini as per client requirement
            //            {
            //                BizAction.OrderDetail.IsClosedOrReported = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsClosedOrReported = false;
            //            }
            //        }
            //        #endregion

            //        #region commented by rohini
            //        //if ((MasterListItem)cmbResultEntry.SelectedItem != null)
            //        //{
            //        //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 1)   //result entry done
            //        //    {
            //        //        BizAction.OrderDetail.IsResultEntry = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        BizAction.OrderDetail.IsResultEntry = false;
            //        //    }
            //        //    if (((MasterListItem)cmbResultEntry.SelectedItem).ID == 2)  //result entry Not done
            //        //    {
            //        //        BizAction.OrderDetail.IsResultEntry1 = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        BizAction.OrderDetail.IsResultEntry1 = false;
            //        //    }
            //        //}
            //        //if((MasterListItem)cmbDelivery.SelectedItem!=null)
            //        //{
            //        //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 1)   //result report done
            //        //    {
            //        //        BizAction.OrderDetail.IsDelivered = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        BizAction.OrderDetail.IsDelivered = false;
            //        //    }
            //        //    if (((MasterListItem)cmbDelivery.SelectedItem).ID == 2)  //result report Not done
            //        //    {
            //        //        BizAction.OrderDetail.IsDelivered1 = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        BizAction.OrderDetail.IsDelivered1 = false;
            //        //    }
            //        //}
            //        #endregion
            //        if ((MasterListItem)cmbDeliveryType.SelectedItem != null)
            //        {
            //            if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 1)   //report by email
            //            {
            //                BizAction.OrderDetail.IsDeliverdthroughEmail = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsDeliverdthroughEmail = false;
            //            }
            //            if (((MasterListItem)cmbDeliveryType.SelectedItem).ID == 2)   // report by hand
            //            {
            //                BizAction.OrderDetail.IsDirectDelivered = true;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.IsDirectDelivered = false;
            //            }
            //        }
            //        //added by rohini 15.2.16
            //        if (((MasterListItem)cmbSample.SelectedItem != null))
            //        {
            //            if ((((MasterListItem)cmbSample.SelectedItem).ID == 1 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //            {
            //                BizAction.OrderDetail.SampleCollectedBy = txtSample.Text;
            //            }
            //            else if ((((MasterListItem)cmbSample.SelectedItem).ID == 2 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //            {
            //                BizAction.OrderDetail.DispatchBy = txtSample.Text;
            //            }
            //            else if ((((MasterListItem)cmbSample.SelectedItem).ID == 3 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //            {
            //                BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
            //            }
            //            else if ((((MasterListItem)cmbSample.SelectedItem).ID == 4 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //            {
            //                BizAction.OrderDetail.AcceptedOrRejectedByName = txtSample.Text;
            //            }
            //            else if ((((MasterListItem)cmbSample.SelectedItem).ID == 5 && ((txtSample.Text != null) || (txtSample.Text != ""))))
            //            {
            //                BizAction.OrderDetail.SampleReceiveBy = txtSample.Text;
            //            }
            //        }

            //        if (txtSampleNO.Text != null && txtSampleNO.Text != string.Empty)
            //        {
            //            BizAction.OrderDetail.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
            //        }
            //        if ((MasterListItem)cmbProcessedBy.SelectedItem != null)
            //        {
            //            if (((MasterListItem)cmbProcessedBy.SelectedItem).ID != 0)   // report by hand
            //            {
            //                BizAction.OrderDetail.AgencyID = ((MasterListItem)cmbProcessedBy.SelectedItem).ID;
            //            }
            //            else
            //            {
            //                BizAction.OrderDetail.AgencyID = 0;
            //            }
            //        }

            //        //
            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        client.ProcessCompleted += (s, arg) =>
            //        {
            //            if (arg.Error == null)
            //            {
            //                if (arg.Result != null)
            //                {
            //                    List<clsPathOrderBookingDetailVO> OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
            //                    clsPathOrderBookingDetailVO OrderBookingDetail = new clsPathOrderBookingDetailVO();

            //                    foreach (var item1 in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList)
            //                    {

            //                        //if ((item1.IsExternalPatient == true && item1.IsResendForNewSample == true) || (item1.IsExternalPatient == false))    //IF IS EXTERNAL PATIENT IS SHOULD NOT BE ON SAMPLE COLLECTION WINDOW
            //                        //{
            //                        //if ((item1.IsExternalPatient == true && item1.IsResendForNewSample == true) || (item1.IsExternalPatient == true))    //IF IS EXTERNAL PATIENT IS SHOULD NOT BE ON SAMPLE COLLECTION WINDOW
            //                        //{
            //                        OrderBookingDetail = item1;
            //                        OrderBookingDetailList.Add(OrderBookingDetail);
            //                        //}
            //                    }

            //                    foreach (var item in OrderBookingDetailList)
            //                    {
            //                        if (item.IsSampleCollected == true)
            //                        {
            //                            item.IsSampleChecked = false;
            //                        }
            //                        else
            //                        {
            //                            item.IsSampleChecked = true;
            //                        }
            //                        //added by rohini dated 9.1.16
            //                        if (item.SampleAcceptRejectStatus == 1 && item.IsResendForNewSample == false)
            //                        {
            //                            item.SamplAcceptImage = "../Icons/tick.png";
            //                            item.IsSampleAcceptEnable = false;
            //                            item.IsSampleAccepted = true;
            //                        }
            //                        else if (item.SampleAcceptRejectStatus == 2 && item.IsResendForNewSample == true)
            //                        {
            //                            item.SamplAcceptImage = "../Icons/error.png";
            //                            item.IsSampleAcceptEnable = false;
            //                            item.IsSampleAccepted = true;
            //                        }
            //                        if (item.IsOutSourced == true)
            //                        {
            //                            item.SampleOutSourceImage = "../Icons/tick.png";
            //                        }
            //                        else if (item.IsOutSourced == false)
            //                        {
            //                            item.SampleOutSourceImage = "../Icons/error.png";
            //                            //item.IsSampleAcceptEnable = false;
            //                            //item.IsSampleAccepted = true;
            //                        }
                                    
            //                        TestList = OrderBookingDetailList;
            //                        dgTest.ItemsSource = null;
            //                        TestList.ForEach(x => { if (!String.IsNullOrEmpty(x.AgencyName)) { x.SampleOutSourceImage = "../Icons/tick.png"; } });
            //                        dgTest.ItemsSource = TestList;
            //                        dgTest.UpdateLayout();
            //                        TestAgencyLinkList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail;
            //                        CheckBox chkAllSelect = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
            //                        if (TestList.ToList().All(x => x.IsSampleCollected == true))
            //                        {
            //                            chkAllSelect.IsChecked = true;
            //                            chkAllSelect.IsEnabled = false;
            //                        }
            //                        else
            //                        {
            //                            chkAllSelect.IsChecked = false;
            //                            chkAllSelect.IsEnabled = true;
            //                        }                                    

            //                    }

            //                    CheckBox chkAllSelect1 = GetChildControl(dgTest, "chkSelectAll") as CheckBox;

            //                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId == ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId)
            //                    {
            //                        chkAllSelect1.IsEnabled = true;
            //                    }
            //                    else
            //                    {
            //                        chkAllSelect1.IsEnabled = false;
            //                    }

            //                    indicator.Close();


                               
            //                }
            //                else
            //                {
            //                    indicator.Close();
            //                }

            //            }
            //            else
            //            {
            //                indicator.Close();
            //            }

            //        };

            //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        client.CloseAsync();
            //    }
            //}
            //catch (Exception )
            //{
            //    //throw;
            //    indicator.Close();
            //}
            #endregion
        }
        void FillOrderBookingDetailsList()
        {
            #region commented by rohini
            try
            {
                indicator.Show();
                dgTest.ItemsSource = null;
                SampleCollectionList.Clear();
                SelectedTestDetails.Clear();//by roihini for barcode
                if (dgOrdertList.SelectedItem != null)
                {
                    CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                    chkSelectAll.IsChecked = false;

                    clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
                    BizAction.IsFrom = "SampleCollection";
                    //BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                    //BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;
                    //if (((MasterListItem)cmbSampleType.SelectedItem).ID > 0)
                    //{
                    //    BizAction.CheckExtraCriteria = true;
                    //    BizAction.CheckSampleType = true;
                    //    BizAction.IsFromCollection = true;
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
                    BizAction.IsExternalPatient = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).IsExternalPatient;

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
                        BizAction.OrderDetail.SampleNo = txtSampleNO.Text.Trim();//Convert.ToInt64(txtSampleNO.Text.Trim());
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
                    if ((MasterListItem)cmbSampleStatus.SelectedItem != null && ((MasterListItem)cmbSampleStatus.SelectedItem).ID > 0)
                    {
                        BizAction.StatusID = Convert.ToInt64(((MasterListItem)cmbSampleStatus.SelectedItem).ID);
                    }
                    if ((MasterListItem)cmbSample.SelectedItem != null && ((MasterListItem)cmbSample.SelectedItem).ID > 0)
                    {
                        BizAction.OrderDetail.UserID = Convert.ToInt64(((MasterListItem)cmbSample.SelectedItem).ID);
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
                                List<clsPathOrderBookingDetailVO> OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                                clsPathOrderBookingDetailVO OrderBookingDetail = new clsPathOrderBookingDetailVO();

                                foreach (var item1 in ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList)
                                {

                                    //if ((item1.IsExternalPatient == true && item1.IsResendForNewSample == true) || (item1.IsExternalPatient == false))    //IF IS EXTERNAL PATIENT IS SHOULD NOT BE ON SAMPLE COLLECTION WINDOW
                                    //{
                                    //if ((item1.IsExternalPatient == true && item1.IsResendForNewSample == true) || (item1.IsExternalPatient == true))    //IF IS EXTERNAL PATIENT IS SHOULD NOT BE ON SAMPLE COLLECTION WINDOW
                                    //{
                                    OrderBookingDetail = item1;
                                    OrderBookingDetailList.Add(OrderBookingDetail);
                                    //}
                                }

                                foreach (var item in OrderBookingDetailList)
                                {
                                    if (item.IsSampleCollected == true)
                                    {
                                        item.IsSampleChecked = false;
                                    }
                                    else
                                    {
                                        item.IsSampleChecked = true;
                                    }
                                    //added by rohini dated 9.1.16
                                    if (item.SampleAcceptRejectStatus == 1 && item.IsResendForNewSample == false)
                                    {
                                        item.SamplAcceptImage = "../Icons/tick.png";
                                        item.IsSampleAcceptEnable = false;
                                        item.IsSampleAccepted = true;
                                    }
                                    else if (item.SampleAcceptRejectStatus == 2 && item.IsResendForNewSample == true)
                                    {
                                        item.SamplAcceptImage = "../Icons/error.png";
                                        item.IsSampleAcceptEnable = false;
                                        item.IsSampleAccepted = true;
                                    }
                                    if (item.IsOutSourced == true)
                                    {
                                        item.SampleOutSourceImage = "../Icons/tick.png";
                                    }
                                    else if (item.IsOutSourced == false)
                                    {
                                        item.SampleOutSourceImage = "../Icons/error.png";
                                        //item.IsSampleAcceptEnable = false;
                                        //item.IsSampleAccepted = true;
                                    }
                                }//added by rohini
                                    TestList = OrderBookingDetailList;
                                    dgTest.ItemsSource = null;
                                    TestList.ForEach(x => { if (!String.IsNullOrEmpty(x.AgencyName)) { x.SampleOutSourceImage = "../Icons/tick.png"; } });
                                    dgTest.ItemsSource = TestList;
                                    dgTest.UpdateLayout();
                                    TestAgencyLinkList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).objOutsourceOrderBookingDetail;
                                    CheckBox chkAllSelect = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                    if (TestList.ToList().All(x => x.IsSampleCollected == true))
                                    {
                                        chkAllSelect.IsChecked = true;
                                        chkAllSelect.IsEnabled = false;
                                    }
                                    else
                                    {
                                        chkAllSelect.IsChecked = false;
                                        chkAllSelect.IsEnabled = true;
                                    }
                                    //SampleCollectionList.Clear();

                            //} //comented by rohini

                                CheckBox chkAllSelect1 = GetChildControl(dgTest, "chkSelectAll") as CheckBox;

                                if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId == ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId)
                                {
                                    chkAllSelect1.IsEnabled = true;
                                }
                                else
                                {
                                    chkAllSelect1.IsEnabled = false;
                                }

                                indicator.Close();

                              

                            }
                            else
                            {
                                indicator.Close();
                            }

                        }
                        else
                        {
                            indicator.Close();
                        }

                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception)
            {
                //throw;
                indicator.Close();
            }
            #endregion
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
                            SampleCollectionList.Add(SelectedTest);
                            if (SampleCollectionList.Count == TestList.Count)
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
               // FillOrderBookingList();
               

                //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).RefundID > 0)
                //{
                //    string msgText = "No Need To Take Sample For The Test.The Test is Already Canceled...";

                //    MessageBoxControl.MessageBoxChildWindow msgW =
                //          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW.Show();
                //}
                //by rohini dated 12/5/16 for refund service sample no is should not generated
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsServiceRefunded == true)
                {
                    string msgText = "The Service of Selected Test Is Refunded.";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW.Show();
                    //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                    //chkSelectAll.IsChecked = false;
                    ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                }
                else
                {
                    if ((bool)((CheckBox)sender).IsChecked)
                    {
                        SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        SampleCollectionList.Add(SelectedTest);

                        var item = from r in TestList
                                   where r.IsSampleChecked == true
                                   select r;
                        if (item.ToList().Count == SampleCollectionList.Count)
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
                        SampleCollectionList.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;
                    }
                }
            }
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
        
        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                indicator.Show();
                clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
                BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();
                BizAction.OrderBookingDetaildetails.IsFromSampleColletion = true;
                this.SampleCollectionList = ww.itemList;
                
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.OrderBookingDetailList = SampleCollectionList;
                BizAction.IsSampleGenerated = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is collected successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    SampleCollectionList.Clear();
                                    SelectedTestDetails.Clear();//by roihini for barcode
                                    FillOrderBookingList();
                                }
                            };
                            msgbox.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
            finally
            {
                indicator.Close();
            }
        }

        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SampleCollectionList.Clear();
            SelectedTestDetails.Clear();//by roihini for barcode
            FillOrderBookingList();
           // isCollecionPopupShow = false;
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            string GetUserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            var itemG = from r in SampleCollectionList
                       where r.IsSampleGenerated == false
                       select r;
            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
            //chkSelectAll.IsChecked = false;
            if (itemG != null && itemG.ToList().Count > 0)
            {
                if (itemG.ToList().Count == 1)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample No Is Not Generated For Selected Tests", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample No Is Not Generated For Some Tests", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
            }
            else if (SampleCollectionList != null && SampleCollectionList.Count > 0)   // && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleGenerated==true
            {
                ww = new SampleCollectionTimeChildWindow();
                ww.IsFromSampleCollection = true;
                ww.UserName = GetUserName; //((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                ww.itemList = SampleCollectionList;
                ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                ww.Show();
            }           
            else
            {
                var item = from r in TestList
                           where r.IsSampleChecked == true
                           select r;
                //
                var item1 = from r in TestList
                            where r.IsSampleGenerated == false && r.IsChecked == true
                            select r;


                //if ()
                //{

                //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Select Test To Collect Sample", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //    msgbox.Show();
                //}

                if (item != null && item.ToList().Count == 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Already Collected For All Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                else if (item1 != null && item1.ToList().Count > 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Is Not Generated Or Test Is Not Selected!.", MessageBoxButtons.Ok, MessageBoxIcon.Information);   //Please Select The Test Whose Sample Number Is Generated
                    msgbox.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Is Not Generated Or Test Is Not Selected!", MessageBoxButtons.Ok, MessageBoxIcon.Information);  // Select Test To Collect Sample
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
            #region Old Code
            //var itemC = from r in SampleCollectionList
            //           where r.IsSampleGenerated == false
            //           select r;

            //   string TestCode1 = "";
            // if (itemC != null && itemC.ToList().Count > 0)
            //{
            //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Not Generated Selected Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //    msgbox.Show();
            //    ClickedFlag = 0;
            //     TestCode1 = "";
            //}
            //else if (SampleCollectionList != null && SampleCollectionList.Count > 0) //&& ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleGenerated != true
            //{
            //    //int index = -1;
            //     TestCode1 = "";

            //    string str = SampleCollectionList[0].SampleNo;
            //    var ListAll= from r in SampleCollectionList
            //                 where r.SampleNo == str
            //                 select r;



            //    if (ListAll.ToList().Count == SampleCollectionList.Count)
            //    {
            //        foreach(var items in ListAll)
            //        {
            //            TestCode1 = TestCode1 + items.TestCode.Trim() + '/';
            //        }
            //        TestCode1 = TestCode1.Remove(TestCode1.Length - 1);
            //        NewBarcodeForm win = new NewBarcodeForm();
            //        string MRNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo.ToUpper();
            //        string OrderNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderNo;
            //        string SampleNo = (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo).ToString();
            //        string TestCode = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestCode;
            //        string PName = (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).FirstName + " " + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).LastName);
            //        DateTime C_Time = Convert.ToDateTime(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleCollected);
            //        string SC_Date = C_Time.ToShortDateString();
            //        string SC_Time = C_Time.ToShortTimeString();
            //        string P_Gender = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID);
            //        if (P_Gender == Convert.ToString(1))
            //        { P_Gender = "Male"; }
            //        else
            //        { P_Gender = "Female"; }
            //        DateTime Page = Convert.ToDateTime(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DateOfBirth);
            //        int years = DateTime.Now.Year - Page.Year;
            //        win.PrintData = SampleNo;
            //        win.P_Name = PName;
            //        win.P_Date = SC_Date;
            //        win.P_Time = SC_Time;
            //        win.P_Gender = P_Gender;
            //        win.P_SampleNo = SampleNo;
            //        win.P_TestCode = TestCode1;
            //        win.P_MRNO = MRNo;
            //        win.P_Age = Convert.ToString(years);
            //        win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
            //        win.Show();
            //        }                   
            //        else
            //        {
            //            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select Same Sample Numbers.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //            msgbox.Show();
            //            ClickedFlag = 0;
            //            TestCode1 = "";
            //        }               
            //}
            //else
            //{
            //    var item = from r in TestList
            //               where r.IsSampleGenerated == false
            //               select r;
            //    var ListAllCollected = from r in TestList
            //                           where r.IsSampleCollected == true
            //                           select r;
            //    if (item != null && item.ToList().Count != 0 && TestList.ToList().Count==item.ToList().Count)
            //    {
            //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Not Generated For All Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //        msgbox.Show();
            //        ClickedFlag = 0;
            //        TestCode1 = "";
            //    }
            //    else if (ListAllCollected.ToList().Count > 0 && TestList.ToList().Count == ListAllCollected.ToList().Count)
            //    {
            //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Already Collected. Can Not Generate Barcode", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //        msgbox.Show();
            //        ClickedFlag = 0;
            //        TestCode1 = "";
            //    }
            //    else
            //    {
            //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select Test To Print Barcode", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //        msgbox.Show();
            //        ClickedFlag = 0;
            //        TestCode1 = "";
            //    }
            //}
            #endregion
            //by rohinee Dated 12 jan 17
            if (SelectedTestDetails != null && SelectedTestDetails.ToList().Count > 0)
            {
                var itemC = from r in SelectedTestDetails
                            where r.IsSampleGenerated == false //&& r.IsSampleCollected==false
                            select r;

                string TestCode1 = "";
                if (itemC != null && itemC.ToList().Count > 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Not Generated For Selected Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                    ClickedFlag = 0;
                    TestCode1 = "";
                }
                else if (SelectedTestDetails != null && SelectedTestDetails.Count > 0) //&& ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleGenerated != true
                {
                    //int index = -1;
                    TestCode1 = "";

                    string str = SelectedTestDetails[0].SampleNo;
                    var ListAll = from r in SelectedTestDetails
                                  where r.SampleNo == str
                                  select r;



                    if (ListAll.ToList().Count == SelectedTestDetails.Count)
                    {
                        foreach (var items in ListAll)
                        {
                            TestCode1 = TestCode1 + items.TestCode.Trim() + '/';
                        }
                        TestCode1 = TestCode1.Remove(TestCode1.Length - 1);
                        NewBarcodeForm win = new NewBarcodeForm();
                        string MRNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo.ToUpper();
                        string OrderNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderNo;
                        string SampleNo = str;//(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo).ToString();
                        string TestCode = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestCode;
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
                        win.PrintData = SampleNo;
                        win.P_Name = PName;
                        win.P_Date = SC_Date;
                        win.P_Time = SC_Time;
                        win.P_Gender = P_Gender;
                        win.P_SampleNo = SampleNo;
                        win.P_TestCode = TestCode1;
                        win.P_MRNO = MRNo;
                        win.P_Age = Convert.ToString(years);
                        win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
                        win.Show();
                    }
                    else
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select Same Sample Numbers.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                        ClickedFlag = 0;
                        TestCode1 = "";
                    }
                }
                else
                {
                    var item = from r in SelectedTestDetails
                               where r.IsSampleGenerated == false
                               select r;
                    //var ListAllCollected = from r in TestList
                    //                       where r.IsSampleCollected == true
                    //                       select r;
                    if (item != null && item.ToList().Count != 0 && TestList.ToList().Count == item.ToList().Count)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Not Generated For Selected Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                        ClickedFlag = 0;
                        TestCode1 = "";
                    }
                    //else if (ListAllCollected.ToList().Count > 0 && TestList.ToList().Count == ListAllCollected.ToList().Count)
                    //{
                    //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Already Collected. Can Not Generate Barcode", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //    msgbox.Show();
                    //    ClickedFlag = 0;
                    //    TestCode1 = "";
                    //}
                    else
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select Test To Print Barcode", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                        ClickedFlag = 0;
                        TestCode1 = "";
                    }
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Test To Print Barcode", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            //comented By Rohinee As Per Disscussed with Abaso
            #region
            // if (dgTest.SelectedItem != null)
            //{
            //    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleGenerated == true)
            //    {                    
            //        NewBarcodeForm win = new NewBarcodeForm();
            //        string MRNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo.ToUpper();
            //        string OrderNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderNo;
            //        string SampleNo = (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo).ToString();
            //        string TestCode = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestCode;
            //        string PName = (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).FirstName + " " + ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).LastName);
            //        DateTime C_Time = Convert.ToDateTime(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleCollected);
            //        string SC_Date = C_Time.ToShortDateString();
            //        string SC_Time = C_Time.ToShortTimeString();
            //        string P_Gender = Convert.ToString(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).GenderID);
            //        if (P_Gender == Convert.ToString(1))
            //        { P_Gender = "Male"; }
            //        else
            //        { P_Gender = "Female"; }
            //        DateTime Page = Convert.ToDateTime(((clsPathOrderBookingVO)dgOrdertList.SelectedItem).DateOfBirth);
            //        int years = DateTime.Now.Year - Page.Year;
            //        win.PrintData = SampleNo;
            //        win.P_Name = PName;
            //        win.P_Date = SC_Date;
            //        win.P_Time = SC_Time;
            //        win.P_Gender = P_Gender;
            //        win.P_SampleNo = SampleNo;
            //        win.P_TestCode = TestCode;
            //        win.P_MRNO = MRNo;
            //        win.P_Age = Convert.ToString(years);
            //        win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
            //        win.Show();
            //    }
            //    else
            //    {
            //        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select The Sample Whose Sample No Is Generated To Print Barcode.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //        msgbox.Show();
            //    }
            //}
            //else
            //{
            //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select The Sample.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
            //    msgbox.Show();
            //}
            #endregion
        }


        void Win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

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
            if (check && AllTestList != null)
            {
                foreach (clsPathOrderBookingDetailVO p in TestList)
                {
                    if (p.IsSampleChecked == true)
                    {
                        AllTestList.Add(p);
                    }
                    //TestList.ToList().ForEach(z => z.IsSampleCollected = true);

                    //added by rohini dated 4.4.16
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == p.UnitId)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleCollected = true);
                    }
                    //chk = dgTest.Columns[0].GetCellContent(p) as CheckBox;
                    //if (chk != null)
                    //    chk.IsChecked = true;   
                }
            }
            else
            {
                //foreach (clsPathOrderBookingDetailVO p in TestList)
                //{
                //    //chk = dgTest.Columns[0].GetCellContent(p) as CheckBox;
                //    //if (chk != null)
                //    //    chk.IsChecked = false;
                //}
                AllTestList.Clear();
                foreach (clsPathOrderBookingDetailVO item in TestList)
                {
                    if (item.IsSampleChecked == false)
                    {
                        item.IsSampleCollected = true;
                    }
                    else
                    {
                        item.IsSampleCollected = false;
                    }
                }
            }
            SampleCollectionList = AllTestList;
            dgTest.ItemsSource = null;
            dgTest.ItemsSource = TestList;
            dgTest.UpdateLayout();
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

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded == true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;
            //ADDED BY ROHINI DATED 23.3.16
            if (item.IsResendForNewSample == true)
                e.Row.Background = new SolidColorBrush(Colors.LightGray);
            else
                e.Row.Background = null;
            if (dgTest.ItemsSource != null)
            {
                CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
            
               
                //added by rohini for sample should no collect bill by other clinic
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == ((clsPathOrderBookingDetailVO)e.Row.DataContext).UnitId)
                {
                    e.Row.IsEnabled = true;
                    chkSelectAll.IsEnabled = true;
                }
                else
                {
                    e.Row.IsEnabled = false;
                    chkSelectAll.IsEnabled = false;
                }
            }
                }
            catch (Exception)
            {
                throw;
            }
        

        }

        private void txtsample_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                FillOrderBookingList();
        }
        int ClickedFlag = 0;
        private void CmdCreateSampleNo_Click(object sender, RoutedEventArgs e)
        {
           
                       var itemG = from r in TestList
                       where r.IsSampleGenerated == true
                        select r;
            CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;

           
            var itemC = from r in SampleCollectionList
                       where r.IsSampleGenerated == true
                       select r;

            var itemR = from r in SampleCollectionList
                        where r.IsServiceRefunded == true
                        select r;
            
           
            if (itemR != null && itemR.ToList().Count > 0)
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "The Service of Selected Test Is Refunded.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
                ClickedFlag = 0;
            }
            else if (itemC != null && itemC.ToList().Count > 0)
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Is Already Generated Selected Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
                ClickedFlag = 0;
            }
            else if (SampleCollectionList != null && SampleCollectionList.Count > 0) //&& ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleGenerated != true
            {
                try
                {
                    
                   ClickedFlag += 1;
                    if (ClickedFlag == 1)
                     {
                        indicator.Show();
                        clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
                        BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();
                        BizAction.OrderBookingDetaildetails.IsFromSampleColletion = true;
                        //this.SampleCollectionList = ww.itemList;

                        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizAction.OrderBookingDetailList = SampleCollectionList;
                        BizAction.IsSampleGenerated = true;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {

                                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Generated successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.OK)
                                        {
                                            SampleCollectionList.Clear();
                                            SelectedTestDetails.Clear();//by roihini for barcode
                                            ClickedFlag = 0;
                                            FillOrderBookingList();
                                        }
                                    };
                                    msgbox.Show();
                                }
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                }
                }
                catch (Exception)
                {
                    ClickedFlag = 0;
                }
                finally
                {
                    indicator.Close();
                    ClickedFlag = 0;
                }
            }          
            else
            {
                var item = from r in TestList
                           where r.IsSampleGenerated == true
                           select r;
                if (item != null && item.ToList().Count != 0 && TestList.ToList().Count==item.ToList().Count)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Number Is Already Generated For All Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                    ClickedFlag = 0;
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please Select Test To Generate Sample No", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                    ClickedFlag = 0;
                }
            }
        }

        //private void VGridSplitter_ButtonClick(object sender, EventArgs e)
        //{
        //    if (LeftColumn.Width.Value == 0)
        //    {
        //        OpenLeft.Begin();
        //    }
        //    else if (sender != this)
        //    {
        //        animCloseCol.From = LeftColumn.Width.Value;
        //        CloseLeft.Begin();
        //    }
        //}
        #endregion
        //by Rohinee for barcode
        List<clsPathOrderBookingDetailVO> SelectedTestDetails = new List<clsPathOrderBookingDetailVO>();
        bool IsPrint = false;
        private void chkBarcode_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            //  SelectedTestDetails.Clear();
            if (chk.IsChecked == true)
            {
                IsPrint = true;
                SelectedTestDetails.Add((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
            }
            else
            {
                IsPrint = false;
                SelectedTestDetails.Remove((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
            }
        }

        private void chkSelectAllBarcode_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            if (check && AllTestList != null)
            {
                foreach (clsPathOrderBookingDetailVO p in TestList)
                {
                    if (p.IsSampleChecked == true)
                    {
                        AllTestList.Add(p);
                    }
                    //added by rohini dated 4.4.16
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == p.UnitId)
                    {
                        TestList.ToList().ForEach(z => z.IsPrint = true);
                    }
                }
            }
            else
            {
                AllTestList.Clear();
                foreach (clsPathOrderBookingDetailVO item in TestList)
                {
                    if (item.IsPrint == false)
                    {
                        item.IsPrint = true;
                    }
                    else
                    {
                        item.IsPrint = false;
                    }
                }
            }
            SampleCollectionList = AllTestList;
            dgTest.ItemsSource = null;
            dgTest.ItemsSource = TestList;
            dgTest.UpdateLayout();
        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)   //by rohini for CR
        {
             clsPathOrderBookingVO item = (clsPathOrderBookingVO)e.Row.DataContext;

           if (item.ResultColor == 1)
           {              
               e.Row.Background = new SolidColorBrush(Pending);
           }           
           else if (item.ResultColor == 2)
           {
                e.Row.Background = new SolidColorBrush(Partial);
            }
           else if (item.ResultColor == 3)
           {
               e.Row.Background = new SolidColorBrush(Collected);
           }
        }

    }
}
