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
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Windows.Data;
using System.Windows.Browser;
namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewSampleAcceptanceRejection_new : UserControl
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
        List<clsPathOrderBookingDetailVO> SampleAcceptRejectList;
        clsPathOrderBookingDetailVO selectedtestDispatchDateime;
        List<clsPathOrderBookingDetailVO> TestAgencyLinkList;
        List<clsPathOrderBookingDetailVO> SampleDispatchList;
        Color Accept;
        Color Pending;
        Color Rejected;

        SampleCollectionTimeChildWindow ww;
        private List<clsPathOrderBookingDetailVO> TestList = new List<clsPathOrderBookingDetailVO>();
        private List<clsPathOrderBookingDetailVO> AllTestList = new List<clsPathOrderBookingDetailVO>();
        public bool IsSelectAll = false;
        WaitIndicator indicator;
        public PagedSortableCollectionView<clsPathOrderBookingDetailVO> DataList { get; private set; }
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
        #endregion
        public NewSampleAcceptanceRejection_new()
        {
            InitializeComponent();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " ";

            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            DataList = new PagedSortableCollectionView<clsPathOrderBookingDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            DataPager.PageSize = DataListPageSize;
            DataPager.Source = DataList;

            this.DataPager.DataContext = DataList;
            dgOutSourcedTestList.DataContext = DataList;
            /*==========================================================*/
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            SampleDispatchList = new List<clsPathOrderBookingDetailVO>();

            TestAgencyLinkList = new List<clsPathOrderBookingDetailVO>();
            SampleAcceptRejectList = new List<clsPathOrderBookingDetailVO>();
            indicator = new WaitIndicator();

            Pending = new Color();
            Pending.A = Convert.ToByte(120);
            Pending.R = Convert.ToByte(255);
            Pending.G = Convert.ToByte(0);
            Pending.B = Convert.ToByte(0);
            txtPending.Background = new SolidColorBrush(Pending);

            Accept = new Color();
            Accept.A = Convert.ToByte(120);
            Accept.R = Convert.ToByte(32);
            Accept.G = Convert.ToByte(178);
            Accept.B = Convert.ToByte(170);
            txtAccepted.Background = new SolidColorBrush(Accept);

            Rejected = new Color();
            Rejected.A = Convert.ToByte(222);
            Rejected.R = Convert.ToByte(184);
            Rejected.G = Convert.ToByte(135);
            Rejected.B = Convert.ToByte(0);
            txtRejected.Background = new SolidColorBrush(Rejected);



        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
            fillDispachStatus();
            fillBillType();
            FillUsers();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
        }
        private void fillDispachStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Pending"));
            MList.Add(new MasterListItem(2, "Accepted"));
            MList.Add(new MasterListItem(3, "Rejected"));
            MList.Add(new MasterListItem(4, "Sub optimal"));
            MList.Add(new MasterListItem(5, "Reject Resend"));
            CmbSampleStatus.ItemsSource = null;
            CmbSampleStatus.ItemsSource = MList;
            CmbSampleStatus.SelectedItem = MList[0];
        }
        private void fillBillType()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "OPD"));
            MList.Add(new MasterListItem(2, "IPD"));
            CmbBillType.ItemsSource = null;
            CmbBillType.ItemsSource = MList;
            CmbBillType.SelectedItem = MList[0];
        }
        private void FillUsers()
        {
            try
            {
                clsGetPathoUsersBizActionVO BizAction = new clsGetPathoUsersBizActionVO();
                BizAction.MenuID = (long)10567;
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

                        CmbAcceptRejectBy.ItemsSource = null;
                        CmbAcceptRejectBy.ItemsSource = objList;
                        CmbAcceptRejectBy.SelectedItem = objList[0];

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
        PagedCollectionView collection;
        private void FillOrderBookingList()
        {
            try
            {
                indicator.Show();
                clsGetDispachReciveDetailListBizActionVO BizAction = new clsGetDispachReciveDetailListBizActionVO();
                BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
                //BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.IsSampleDispatched = true;
                BizAction.IsAcceptReject = true;
                if (dtpFromDate.SelectedDate != null)
                    BizAction.FromDate = ((DateTime)dtpFromDate.SelectedDate).Date;
                if (dtpToDate.SelectedDate != null)
                    BizAction.ToDate = ((DateTime)dtpToDate.SelectedDate).Date.AddDays(1);
                if (txtBatch.Text != null && txtBatch.Text != string.Empty)
                {
                    BizAction.BatchNo = Convert.ToString(txtBatch.Text.Trim());
                }
                if (txtSample.Text != null && txtSample.Text != string.Empty)
                {
                    BizAction.SampleNo = Convert.ToString(txtSample.Text.Trim());
                }
                if (txtFrontMRNO.Text != null && txtFrontMRNO.Text != string.Empty)
                {
                    BizAction.MRNo = Convert.ToString(txtFrontMRNO.Text.Trim());
                }
                if (txtFrontFirstName.Text != null && txtFrontFirstName.Text != string.Empty)
                {
                    BizAction.OrderDetail.FirstName = Convert.ToString(txtFrontFirstName.Text.Trim());
                }
                if (txtFrontLastName.Text != null && txtFrontLastName.Text != string.Empty)
                {
                    BizAction.OrderDetail.LastName = Convert.ToString(txtFrontLastName.Text.Trim());
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                if (((MasterListItem)CmbSampleStatus.SelectedItem) != null && ((MasterListItem)CmbSampleStatus.SelectedItem).ID > 0)
                {
                    BizAction.SampleStatus = Convert.ToInt64(((MasterListItem)CmbSampleStatus.SelectedItem).ID);
                }
                if (((MasterListItem)CmbBillType.SelectedItem) != null && ((MasterListItem)CmbBillType.SelectedItem).ID > 0)
                {
                    BizAction.BillType = Convert.ToInt32(((MasterListItem)CmbBillType.SelectedItem).ID);
                }
                if (((MasterListItem)CmbAcceptRejectBy.SelectedItem) != null && ((MasterListItem)CmbAcceptRejectBy.SelectedItem).ID > 0)
                {
                    BizAction.SampleAcceptRejectBy = Convert.ToInt64(((MasterListItem)CmbAcceptRejectBy.SelectedItem).ID);
                }

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            clsGetDispachReciveDetailListBizActionVO result = arg.Result as clsGetDispachReciveDetailListBizActionVO;
                            DataList.Clear();

                            if (result.OrderBookingList != null && result.OrderBookingList.Count > 0)
                            {
                                DataList.TotalItemCount = (int)((clsGetDispachReciveDetailListBizActionVO)arg.Result).TotalRows;
                                foreach (clsPathOrderBookingDetailVO item in result.OrderBookingList)
                                {
                                    if (item.IsOutSourced == true)
                                        item.AgencyChangedImage = "../Icons/tick.png";
                                    else
                                        item.AgencyChangedImage = "../Icons/error.png";
                                    if (item.IsSampleDispatched == true)
                                        item.SampleCollecedImage = "../Icons/tick.png";
                                    else
                                        item.SampleCollecedImage = "../Icons/error.png";
                                    if (item.IsAccepted == true)
                                        item.SamplAcceptImage = "../Icons/tick.png";
                                    else
                                        item.SamplAcceptImage = "../Icons/error.png";
                                    if (item.IsRejected == true)
                                        item.SamplRejectionImage = "../Icons/tick.png";
                                    else
                                        item.SamplRejectionImage = "../Icons/error.png";
                                    if (item.IsResendForNewSample == true)
                                        item.SamplResendImage = "../Icons/tick.png";
                                    else
                                        item.SamplResendImage = "../Icons/error.png";

                                    item.PatientName = item.FirstName + "" + item.MiddleName + "" + item.LastName;
                                    //item.SampleNumber += " Patient Name: " + item.PatientName;
                                    DataList.Add(item);
                                    item.SampleNumber1 = item.SampleNumber;
                                    if (item.IsSampleReceive == true)
                                    {
                                        item.IsSampleAcceptEnable = false;
                                    }
                                    else
                                    {
                                        item.IsSampleAcceptEnable = true;
                                    }
                                }
                                TestList = result.OrderBookingList;
                                //TestList = result.OrderBookingList.Select(x => x.BatchCode).Distinct());
                                dgOutSourcedTestList.ItemsSource = null;
                                collection = new PagedCollectionView(DataList);
                             //added by roini dated 25.5.16
                                //collection.GroupDescriptions.Add(new PropertyGroupDescription("BatchCode"));


                                dgOutSourcedTestList.ItemsSource = collection;
                                DataPager.Source = null;
                                DataPager.PageSize = DataList.PageSize;
                                DataPager.Source = DataList;
                                CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                                //CheckBox chkSelectGrp = GetChildControl(dgOutSourcedTestList, "chkSelectGrp") as CheckBox;
                                if (TestList.ToList().All(x => x.IsSampleReceive == true))
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
                            txtTotalCountRecords.Text = "";
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

            catch (Exception)
            {
                indicator.Close();
            }
            finally
            {
                indicator.Close();
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();
        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsAccepted == false && item.IsRejected == false && item.IsResendForNewSample == false)
            {
                e.Row.Background = new SolidColorBrush(Pending);
            }
            else if (item.IsAccepted == true && item.IsRejected == false && item.IsResendForNewSample == false)
            {
                e.Row.Background = new SolidColorBrush(Accept);
            }
            else if ((item.IsRejected == true || item.IsResendForNewSample == true) && item.IsAccepted == false)
            {
                e.Row.Background = new SolidColorBrush(Rejected);
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
                        if (((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsServiceRefunded == true)
                        {
                            string msgText = "The Service of Selected Test Is Refunded.";

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.Show();
                            ((System.Windows.Controls.Primitives.ToggleButton)(sender)).IsChecked = false;
                        }
                        else
                        {
                            SelectedTest = ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);
                            SampleAcceptRejectList.Add(SelectedTest);
                            var item = from r in TestList
                                       where r.IsSampleAcceptEnable == true
                                       select r;
                            if (item.ToList().Count == SampleAcceptRejectList.Count)
                            {
                                CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                                chkSelectAll.IsChecked = true;
                            }
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).Quantity = null;
                        SampleAcceptRejectList.Remove((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;
                    }
                }
                else
                {
                    ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsSampleCollected = false;
                    MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palsh", "Clinical transactions are not allowed at Head Office.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbx.Show();
                }
            }
            else
            {
                if (((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsServiceRefunded == true)
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
                        SelectedTest = ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);
                        SampleAcceptRejectList.Add(SelectedTest);
                        var item = from r in TestList
                                   where r.IsSampleAcceptEnable == true
                                   select r;
                        if (item.ToList().Count == SampleAcceptRejectList.Count)
                        {
                            CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = true;
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).Quantity = null;
                        SampleAcceptRejectList.Remove((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;
                    }
                }
            }
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
                        TestList.ToList().ForEach(z => z.IsSampleReceive = true);
                    }
                    else if (p.DispatchToID == (((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId && p.DispatchToID != 0)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleReceive = true);
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
                        item.IsSampleReceive = true;
                    }
                    else
                    {
                        item.IsSampleReceive = false;
                    }
                }
            }
            //SampleAcceptRejectList = AllTestList;
            //dgOutSourcedTestList.ItemsSource = null;
            //dgOutSourcedTestList.ItemsSource = TestList;
            //dgOutSourcedTestList.UpdateLayout();

            SampleAcceptRejectList = AllTestList;
            dgOutSourcedTestList.ItemsSource = null;
            collection = new PagedCollectionView(TestList);
            //collection.GroupDescriptions.Add(new PropertyGroupDescription("BatchCode"));
            dgOutSourcedTestList.ItemsSource = collection;
            dgOutSourcedTestList.UpdateLayout();
        }

        private void txtFrontFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TestReceipt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (SampleAcceptRejectList != null && SampleAcceptRejectList.Count > 0)
            {
                var item = from r in SampleAcceptRejectList
                           where r.IsSampleReceive == false
                           select r;

                var item1 = from r in SampleAcceptRejectList
                            where r.IsSampleDispatched == false
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
                            CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
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
                    ww.strUserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
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
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Already Accepted/Rejected Of All Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select To Accept ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                    SampleAcceptRejectList.Clear();
                    FillOrderBookingList();
                   // Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                }
            }
        }
        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SampleAcceptRejectList.Clear();
            FillOrderBookingList();
            //isCollecionPopupShow = false;

        }
        MessageBoxChildWindow msgbox;
        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                indicator.Show();
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
                                //string b = ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).BatchCode;
                                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Pathology/PrintReceiveBatchCode.aspx?UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&BatchNo=" + b), "_blank");

                            }
                            msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    SampleAcceptRejectList.Clear();
                                    FillOrderBookingList();
                                    CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                                    chkSelectAll.IsChecked = false;


                                }
                            };
                            msgbox.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception)
            {
            }
            finally
            {
                indicator.Close();
            }
        }
        private void SendSampleRejectEmail()
        {
            if (dgOutSourcedTestList.SelectedItem != null)
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

        public bool IsReject = false;
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
                            CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
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
                SampleAcceptRejectList.Clear();
                FillOrderBookingList();
                //}
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSelectAllTest_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            IsSelectAll = true;
            if (check && AllTestList != null)
            {
                foreach (clsPathOrderBookingDetailVO p in TestList)
                {
                    if (p.IsSampleDispatchChecked == true)
                    {
                        AllTestList.Add(p);
                    }

                    //TestList.ToList().ForEach(z => z.IsSampleDispatch = true);

                    //added by rohini dated 4.4.16
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == p.UnitID)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleReceive = true);
                    }

                }
            }
            else
            {
                AllTestList.Clear();
                IsSelectAll = false;
                foreach (clsPathOrderBookingDetailVO item in TestList)
                {
                    if (item.IsSampleReceive == false)
                    {
                        item.IsSampleReceive = true;
                    }
                    else
                    {
                        item.IsSampleReceive = false;
                    }
                }
            }
            SampleDispatchList = AllTestList;
            dgOutSourcedTestList.ItemsSource = null;
            collection = new PagedCollectionView(TestList);
            //collection.GroupDescriptions.Add(new PropertyGroupDescription("SampleNumber"));
            dgOutSourcedTestList.ItemsSource = collection;
            dgOutSourcedTestList.UpdateLayout();
        }

        private void txtFrontLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbSample_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintBatchCode ww = new PrintBatchCode();
            ww.IsFromAccept = true;
            //ww.itemList = SampleDispatchList;
            //ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
            ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
            ww.Show();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBox Ch = (CheckBox)sender;
            //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;


            if (Ch.IsChecked == true)
            {
                var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

                //foreach (clsPathOrderBookingDetailVO item2 in grp)
                //{
                //    var item = from r in DataList
                //               where r.SampleNumber1 == item2.SampleNumber1
                //               select new clsPathOrderBookingDetailVO
                //               {
                //                   IsSampleDispatched = true
                //               };
                //    foreach (var item1 in DataList)
                //    {
                //        if (item1.SampleNumber1 == item2.SampleNumber1 && item1.BatchCode == string.Empty && item1.IsSampleDispatched==false)
                //        {
                //            item1.IsSampleDispatched = true;
                //            //ChkGrp.IsChecked = true;
                //            //Ch.IsChecked = true;
                //            //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;

                //        }
                //    }

                //}

                foreach (clsPathOrderBookingDetailVO item2 in grp)
                {
                    var item = from r in DataList
                               where r.SampleNumber1 == item2.SampleNumber1
                               select new clsPathOrderBookingDetailVO
                               {
                                   IsSampleDispatched = true
                               };


                    foreach (var item1 in DataList)
                    {
                        if (item1.SampleNumber1 == item2.SampleNumber1 && item1.BatchCode != string.Empty )
                        {
                            item1.IsSampleReceive = true;
                            Ch.IsChecked = true;

                        }
                    }

                }
                foreach (var item in DataList)
                {
                    if (item.IsSampleDispatch == true)
                    {
                        item.IsSampleReceive = true;
                    }
                }
                dgOutSourcedTestList.UpdateLayout();



            }
            else
            {
                var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();

                foreach (clsPathOrderBookingDetailVO item2 in grp)
                {
                    var item = from r in DataList
                               where r.SampleNumber1 == item2.SampleNumber1
                               select new clsPathOrderBookingDetailVO
                               {
                                   IsSampleDispatched = false

                               };
                    foreach (var item1 in DataList)
                    {
                        if (item1.SampleNumber1 == item2.SampleNumber1 && item1.BatchCode != string.Empty)
                        {
                            item1.IsSampleReceive = false;

                            Ch.IsChecked = false;
                        }
                    }
                }

                foreach (var item in DataList)
                {
                    if (item.IsSampleDispatch == false)
                    {
                        item.IsSampleReceive = false;
                    }
                }
                dgOutSourcedTestList.UpdateLayout();
                //ChkGrp.IsChecked = false;
                //Ch.IsChecked = false;
            }
        }

        private void txtFrontMRNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillOrderBookingList();
            }
        }
    }
}
