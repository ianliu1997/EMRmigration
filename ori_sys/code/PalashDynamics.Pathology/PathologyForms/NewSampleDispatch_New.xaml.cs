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
using System.Collections.ObjectModel;
using System.Windows.Browser;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewSampleDispatch_New : UserControl
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
        List<clsPathOrderBookingDetailVO>  SampleDispatchList;
      
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

        Color Pending;
        Color Dispatch;
       
        public NewSampleDispatch_New()
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
            indicator = new WaitIndicator();
            //UnAssignedAgnecyTestList = new List<clsPathoTestOutSourceDetailsVO>();

            Pending = new Color();
            Pending.A = Convert.ToByte(120);
            Pending.R = Convert.ToByte(255);
            Pending.G = Convert.ToByte(0);
            Pending.B = Convert.ToByte(0);
            txtPending.Background = new SolidColorBrush(Pending);

            Dispatch = new Color();
            Dispatch.A = Convert.ToByte(120);
            Dispatch.R = Convert.ToByte(32);
            Dispatch.G = Convert.ToByte(178);
            Dispatch.B = Convert.ToByte(170);
            txtCollected.Background = new SolidColorBrush(Dispatch);

        }
        private void fillDispachStatus()
        {
            List<MasterListItem> MList = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            MList.Add(new MasterListItem(0, "- Select -"));
            MList.Add(new MasterListItem(1, "Pending"));
            MList.Add(new MasterListItem(2, "dispatch"));
            cmbDispatchStatus.ItemsSource = null;
            cmbDispatchStatus.ItemsSource = MList;
            cmbDispatchStatus.SelectedItem = MList[0];
        }
        private void FillUsers()
        {
            try
            {
                clsGetPathoUsersBizActionVO BizAction = new clsGetPathoUsersBizActionVO();
                BizAction.MenuID = (long)10565;
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

                        cmbDispatchBy.ItemsSource = null;
                        cmbDispatchBy.ItemsSource = objList;
                        cmbDispatchBy.SelectedItem = objList[0];

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
        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "IsProcessingUnit";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cmbDispatchTo.ItemsSource = null;
                    cmbDispatchTo.ItemsSource = objList;
                    cmbDispatchTo.SelectedValue = (long)0;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
            FillOrderBookingList();
            fillDispachStatus();
            FillUsers();
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
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
                //BizAction.CheckExtraCriteria = true;
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

                if ((MasterListItem)cmbDispatchTo.SelectedItem != null)
                {
                    BizAction.ClinicID = ((MasterListItem)cmbDispatchTo.SelectedItem).ID;
                }
                else
                {
                    BizAction.ClinicID = 0;
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                if ((MasterListItem)cmbDispatchStatus.SelectedItem != null)
                {
                    if (((MasterListItem)cmbDispatchStatus.SelectedItem).ID == 1)
                    {
                        BizAction.IsPending = true;
                    }
                    else
                    {
                        BizAction.IsPending =  false;
                    }
                    if (((MasterListItem)cmbDispatchStatus.SelectedItem).ID == 2)
                    {
                        BizAction.IsCollected = true;
                    }
                    else
                    {
                        BizAction.IsCollected = false;
                    }
                }
                if ((MasterListItem)cmbDispatchBy.SelectedItem != null)
                {
                    BizAction.IsDispatchByID = ((MasterListItem)cmbDispatchBy.SelectedItem).ID;
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
                                    if (item.IsSampleCollected == true)
                                        item.SampleCollecedImage = "../Icons/tick.png";
                                    else
                                        item.SampleCollecedImage = "../Icons/error.png";

                                    item.SampleNumber1 = item.SampleNumber;
                                    item.PatientName = item.FirstName + "" + item.MiddleName + "" + item.LastName;
                                    //item.SampleNumber += " Patient Name: " + item.PatientName;
                                    DataList.Add(item);

                                    if (item.IsSampleDispatched == true)
                                    {
                                        item.IsSampleDispatchChecked = false;
                                    }
                                    else
                                    {
                                        item.IsSampleDispatchChecked = true;
                                    }
                                }
                                TestList = result.OrderBookingList;
                                dgOutSourcedTestList.ItemsSource = null;
                                collection = new PagedCollectionView(DataList);
                               //commented by rohini as poer call with dr gautam on 24.5.16
                                //collection.GroupDescriptions.Add(new PropertyGroupDescription("SampleNumber"));


                                dgOutSourcedTestList.ItemsSource = collection;
                                DataPager.Source = null;
                                DataPager.PageSize = DataList.PageSize;
                                DataPager.Source = DataList;
                                CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                                //CheckBox chkSelectGrp = GetChildControl(dgOutSourcedTestList, "chkSelectGrp") as CheckBox;
                                if (TestList.ToList().All(x => x.IsSampleDispatched == true))
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

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {

            PrintBatchCode ww = new PrintBatchCode();
            ww.IsFromAccept = false;
            //ww.itemList = SampleDispatchList;
            //ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
            ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
            ww.Show();
        }

        private void dgOutSourcedTestList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgOutSourcedTestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (SampleDispatchList != null && SampleDispatchList.Count > 0)
            {
                var item = from r in SampleDispatchList
                           where r.IsSampleCollected == false
                           select r;
                if (IsSelectAll == false)
                {
                    if (item != null && item.ToList().Count > 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select Test/Tests whose Sample is Collected.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                SampleDispatchList.Clear();
                                FillOrderBookingList();
                                //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                //chkSelectAll.IsChecked = false;
                            }
                        };
                        msgbox.Show();
                    }
                    //commented by rohini 5.2.16 for temporary perpose 
                    //else if (dgOrdertList.SelectedItem != null && ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).SampleType == false)
                    //{
                    //    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "You Can Not Dispatch Sample Without Collection. \n Sample Is Not Collected.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //    msgbox.Show();
                    //}
                    //
                    else
                    {
                        ww = new SampleCollectionTimeChildWindow();
                        ww.IsFromSampleDispatch = true;
                        ww.itemList = SampleDispatchList;
                        ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                        ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                        ww.Show();
                    }
                }
                else
                {
                    if (item != null && item.ToList().Count > 0)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "You Can't Select All Test. \n Sample Of Some Test/Tests Is Not Collected", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                SampleDispatchList.Clear();
                                FillOrderBookingList();
                               // CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                                //chkSelectAll.IsChecked = false;
                            }
                        };
                        msgbox.Show();
                    }
                    else
                    {
                        ww = new SampleCollectionTimeChildWindow();
                        ww.IsFromSampleDispatch = true;
                        ww.itemList = SampleDispatchList;
                        ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                        ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                        ww.Show();
                    }
                }
            }
            else
            {
                var item = from r in TestList
                           where r.IsSampleDispatchChecked == true
                           select r;
                if (item != null && item.ToList().Count == 0)
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "SAMPLE IS ALREADY DISPATCHED FOR ALL TEST.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please, Select Test To Dispatch Sample", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                    SampleDispatchList.Clear();
                    FillOrderBookingList();
                }
            }
        }
        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SampleDispatchList.Clear();
            FillOrderBookingList();
            //isCollecionPopupShow = false;
        }

        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                indicator.Show();
                clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
                BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();
                BizAction.IsGenerateBatch = true;
                foreach (var item in ww.itemList)
                {
                    BizAction.DID = item.DispatchToID;
                    BizAction.UID = item.UnitID;
                }
                BizAction.OrderBookingDetaildetails.IsFromSampleDispatch = true;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                this.SampleDispatchList = ww.itemList;
                BizAction.OrderBookingDetailList = SampleDispatchList;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //auto print report of dispatch 
                            //string b = ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).BatchCode;
                            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is dispatched successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);

                            msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    //NEWLY ADDED 
                                    string b = ((clsUpdatePathOrderBookingDetailListBizActionVO)arg.Result).returnBatch;
                                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Pathology/PrintDispatchBatchReport.aspx?UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&BatchNo=" + b), "_blank");


                                    SampleDispatchList.Clear();
                                    FillOrderBookingList();


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
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {

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
                    if (p.IsSampleDispatchChecked == true)
                    {
                        AllTestList.Add(p);
                    }

                    //TestList.ToList().ForEach(z => z.IsSampleDispatch = true);

                    //added by rohini dated 4.4.16
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == p.UnitID)
                    {
                        TestList.ToList().ForEach(z => z.IsSampleDispatched = true);
                        //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;
                        //ChkGrp.IsChecked = true;//FOR GROUP BOX
                    }

                }
            }
            else
            {
                AllTestList.Clear();
                IsSelectAll = false;
                foreach (clsPathOrderBookingDetailVO item in TestList)
                {
                    if (item.IsSampleDispatchChecked == false)
                    {
                        item.IsSampleDispatched = true;
                    }
                    else
                    {
                        item.IsSampleDispatched = false;
                    }
                }
            }
            SampleDispatchList = AllTestList;
            dgOutSourcedTestList.ItemsSource = null;            
            collection = new PagedCollectionView(TestList);
            //commented by rohini dated 25.2.16
            //collection.GroupDescriptions.Add(new PropertyGroupDescription("SampleNumber"));            
            dgOutSourcedTestList.ItemsSource = collection;
            dgOutSourcedTestList.UpdateLayout();

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillOrderBookingList();

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
                        if (((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsServiceRefunded ==true)
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
                            SampleDispatchList.Add(SelectedTest);
                            var item = from r in TestList
                                       where r.IsSampleDispatchChecked == true
                                       select r;
                            if (item.ToList().Count == SampleDispatchList.Count)
                            {
                                CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                                chkSelectAll.IsChecked = true;

                                //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;
                                //ChkGrp.IsChecked = true;
                            }
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).Quantity = null;
                        SampleDispatchList.Remove((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;

                        //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;
                        //ChkGrp.IsChecked = false;
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
                if (((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsServiceRefunded ==true)
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
                        SampleDispatchList.Add(SelectedTest);
                        var item = from r in TestList
                                   where r.IsSampleDispatchChecked == true
                                   select r;
                        if (item.ToList().Count == SampleDispatchList.Count)
                        {
                            CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                            chkSelectAll.IsChecked = true;

                            //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;
                            //ChkGrp.IsChecked = true;
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem).Quantity = null;
                        SampleDispatchList.Remove((clsPathOrderBookingDetailVO)dgOutSourcedTestList.SelectedItem);

                        CheckBox chkSelectAll = GetChildControl(dgOutSourcedTestList, "chkSelectAll") as CheckBox;
                        chkSelectAll.IsChecked = false;

                        //CheckBox ChkGrp = GetChildControl(dgOutSourcedTestList, "ChkGrp") as CheckBox;
                        //ChkGrp.IsChecked = false;
                        //dgOutSourcedTestList.UpdateLayout();
                    }
                }
            }
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void dgTest_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)e.Row.DataContext;
            if (item.IsServiceRefunded == true)
                e.Row.Background = new SolidColorBrush(Colors.Red);
            else
                e.Row.Background = null;

            if (item.IsSampleDispatched)
            {
                //collected
                e.Row.Background = new SolidColorBrush(Dispatch);
            }
            else
            {
                //pending
                e.Row.Background = new SolidColorBrush(Pending);
            }

            if (dgOutSourcedTestList.ItemsSource != null)
            {
                //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;            


                //added by rohini for sample should no collect bill by other clinic
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).UnitID == ((clsPathOrderBookingDetailVO)e.Row.DataContext).UnitId)
                {

                    //e.Row.IsEnabled = true;
                    //chkSelectAll.IsEnabled = true;     
                    // CheckBox chkFlagAll = (CheckBox)e.Row;                    
                    //CheckBox chk = (CheckBox)this.dgTest.Columns[0].GetCellContent(e.Row);
                    //chk.IsEnabled = false;


                    //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                    //chkSelectAll.IsEnabled = true;

                    //chkSelectAll.Visibility = Visibility.Visible;
                }
                else
                {

                    //e.Row.IsEnabled = false;


                    //chkSelectAll.IsEnabled = false;
                    //string str = Convert.ToString(dgTest.Columns[0].Header);
                    //CheckBox chk = (CheckBox)this.dgTest.Columns[0].Header;
                    //chk.IsChecked = true;

                    //CheckBox chkFlagAll = (CheckBox)e.Row.FindName("chkSelectAll");
                    //chkFlagAll.IsEnabled = false;
                    //chk.IsEnabled = false;

                    //CheckBox chkSelectAll = GetChildControl(dgTest, "chkSelectAll") as CheckBox;
                    //chkSelectAll.IsEnabled = false;
                    //chkSelectAll.Visibility = Visibility.Collapsed;
                    //chkSelectAll_Click(null,null);
                }
               

            }
        }

        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public ObservableCollection<clsPathOrderBookingDetailVO> DrugList { get; set; }
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
                        if (item1.SampleNumber1 == item2.SampleNumber1 && item1.BatchCode == string.Empty && item1.IsSampleDispatched==false)
                        {
                            item1.IsSampleDispatch = true;
                            Ch.IsChecked = true;
                           
                        }
                    }

                }
                 foreach (var item in DataList)
                 {
                     if (item.IsSampleDispatch == true)
                     {
                         item.IsSampleDispatched = true;
                     }
                 }
                dgOutSourcedTestList.UpdateLayout();
                //SampleDispatchList = DataList;

                
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
                        if (item1.SampleNumber1 == item2.SampleNumber1 && item1.BatchCode == string.Empty )
                        {
                            item1.IsSampleDispatch = false;

                            Ch.IsChecked = false;
                        }
                    }
                }

                foreach (var item in DataList)
                {
                    if (item.IsSampleDispatch == false)
                    {
                        item.IsSampleDispatched = false;
                    }
                }
                dgOutSourcedTestList.UpdateLayout();
                //ChkGrp.IsChecked = false;
                //Ch.IsChecked = false;
            }

           
            
        }

        private void chkSelectGrp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtFrontFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillOrderBookingList();
            }
        }
    }
}
