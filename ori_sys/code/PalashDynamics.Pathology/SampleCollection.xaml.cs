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
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using MessageBoxControl;
using System.Windows.Browser;
using PalashDynamics.Pathology.Barcode;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;


namespace PalashDynamics.Pathology
{
    public partial class SampleCollection : UserControl, IInitiateCIMS
    {
        clsPathOrderBookingDetailVO SelectedTest;
        public ObservableCollection<clsPathoTestItemDetailsVO> ItemList
        {
            get;
            set;
        }

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Pathology work order is not selected.\nPlease Select a work order then click on Sample Collection.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Pathology Work Order.");
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
        bool isCollecionPopupShow = false;

        public SampleCollection()
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
                    CmdSave.IsEnabled = false;
            }
            this.Loaded += new RoutedEventHandler(SampleCollection_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }

        void SampleCollection_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
                ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());
                //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            }
            else
            {
                //((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).OrderDate
                //this.dtpFromDate.SelectedDate = DateTime.Now;
                //this.dtpToDate.SelectedDate = DateTime.Now;
                //this.rdbAll.IsChecked = true;

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
            if (!(((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null))
            {
                try
                {
                    DataList.Clear();
                    DataList.Add(((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder);
                    dgOrdertList.ItemsSource = null;
                    dgOrdertList.ItemsSource = DataList;
                    if (DataList.Count > 0)
                        dgOrdertList.SelectedItem = 0;

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
            catch (Exception ex)
            {
                indicator.Close();
            }

        }
        #endregion




        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dgTest.ItemsSource = null;
                indicator.Show();
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
                                dgTest.ItemsSource = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                            }
                        }
                        indicator.Close();
                    };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                indicator.Close();
                //  throw;
            }
        }

        private void dgOrdertList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //clsPathOrderBookingVO item=e.Row.DataContext as clsPathOrderBookingVO;
            //if (item.SampleType)
            //    e.Row.Background = new SolidColorBrush(Colors.Yellow);
            //else
            //    e.Row.Background = null;
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
                        SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        //SampleCollectionChildWindow w = new SampleCollectionChildWindow();
                        //w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        //w.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                        //w.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                        //w.Show();
                        if (isCollecionPopupShow == false)
                        {

                            SampleCollectionTimeChildWindow ww = new SampleCollectionTimeChildWindow();
                            ww.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                            ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                            ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                            ww.Show();

                        }
                        else
                        {
                            AddSaveButton_Click(SelectedTest, e);
                        }
                    }
                    else
                    {
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo = null;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced = false;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyName = null;
                        ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Quantity = null;
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
                if ((bool)((CheckBox)sender).IsChecked)
                {
                    SelectedTest = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                    //SampleCollectionChildWindow w = new SampleCollectionChildWindow();
                    //w.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                    //w.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                    //w.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                    //w.Show();

                    if (isCollecionPopupShow == false)
                    {
                        SampleCollectionTimeChildWindow ww = new SampleCollectionTimeChildWindow();
                        ww.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
                        ww.OnSaveButtonClick += new RoutedEventHandler(AddSaveButton_Click);
                        ww.OnCancelButtonClick += new RoutedEventHandler(w_OnCancelButtonClick);
                        ww.Show();
                        // isCollecionPopupShow = true;
                    }
                    else
                    {
                        AddSaveButton_Click(SelectedTest, e);
                    }

                }
                else
                {
                    ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo = null;
                    ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsOutSourced = false;
                    ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).AgencyName = null;
                    ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).Quantity = null;
                }
            }
        }

        List<clsPathOrderBookingDetailVO> SelectedtestList = new List<clsPathOrderBookingDetailVO>();

        //private void chk_Click(object sender, RoutedEventArgs e)
        //{
        //    //Modified by Saily P on 17.12.13 Purpose - change the flow for item comsumption fro tests.
        //    if ((bool)((CheckBox)sender).IsChecked == true)
        //    //&& ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected == false)
        //    {
        //        SelectedtestList.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem));
        //    }
        //    else if ((bool)((CheckBox)sender).IsChecked == false)
        //    {
        //        SelectedtestList.RemoveAt(dgTest.SelectedIndex);
        //    }
        //}

        void w_OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedTest.IsSampleCollected = false;
            isCollecionPopupShow = false;
        }
        List<clsPathOrderBookingDetailVO> testList = new List<clsPathOrderBookingDetailVO>();
        List<clsPathOrderBookingDetailVO> TemptestList = new List<clsPathOrderBookingDetailVO>();
        private void AddSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsPathOrderBookingDetailVO Obj = new clsPathOrderBookingDetailVO();

            //if (isCollecionPopupShow == true)
            //{
            SelectedTest = (clsPathOrderBookingDetailVO)sender;
            Obj.ID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
            Obj.UnitId = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
            if (SelectedTest.SampleNo != null && SelectedTest.SampleNo != string.Empty)
                Obj.SampleNo = SelectedTest.SampleNo;
            Obj.IsOutSourced = SelectedTest.IsOutSourced;
            Obj.AgencyID = SelectedTest.AgencyID;
            Obj.Quantity = SelectedTest.Quantity;
            Obj.SampleCollected = SelectedTest.SampleCollected;
            Obj.IsSampleCollected = SelectedTest.IsSampleCollected;
            if ((testList).Count > 0)
            {
                Obj.SampleCollectedDateTime = testList[0].SampleCollectedDateTime;
                Obj.SampleReceivedDateTime = testList[0].SampleReceivedDateTime;
                Obj.SampleCollectionCenter = testList[0].SampleCollectionCenter;
                SelectedTest.SampleCollectedDateTime = testList[0].SampleCollectedDateTime;
                SelectedTest.SampleReceivedDateTime = testList[0].SampleReceivedDateTime;
                SelectedTest.SampleCollectionCenter = testList[0].SampleCollectionCenter;
                //SelectedTest.CollectionCenterID = testList[0].CollectionCenterID;
                //SelectedTest.LabID = testList[0].LabID;

                SelectedTest.IsOutSourced = testList[0].IsOutSourced;
                SelectedTest.AgencyID = testList[0].AgencyID;
                SelectedTest.AgencyName = testList[0].AgencyName;
                SelectedTest.Quantity = testList[0].Quantity;
                SelectedTest.SampleCollected = testList[0].SampleCollected;
                SelectedTest.IsSampleCollected = testList[0].IsSampleCollected;

            }
            else
            {

                Obj.SampleCollectedDateTime = SelectedTest.SampleCollectedDateTime;
                Obj.SampleReceivedDateTime = SelectedTest.SampleReceivedDateTime;
                Obj.SampleCollectionCenter = SelectedTest.SampleCollectionCenter;
                //Obj.CollectionCenterID = SelectedTest.CollectionCenterID;
                //Obj.LabID = SelectedTest.LabID;
            }
            testList.Add(SelectedTest);
            isCollecionPopupShow = true;


        }


        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (dgTest.ItemsSource != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem) != null)
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected != false)
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleCollected == null)
                    {
                        clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
                        BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();

                        ////System.Collections.Generic.List<clsPathOrderBookingDetailVO> list = (S--`ystem.Collections.Generic.List<clsPathOrderBookingDetailVO>)dgTest.ItemsSource;
                        //BizAction.OrderBookingDetaildetails.ID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                        //BizAction.OrderBookingDetaildetails.UnitId = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
                        //if (SelectedTest.SampleNo != 0 && SelectedTest.SampleNo!=null)
                        //    BizAction.OrderBookingDetaildetails.SampleNo = SelectedTest.SampleNo;
                        //BizAction.OrderBookingDetaildetails.IsOutSourced = SelectedTest.IsOutSourced;
                        //BizAction.OrderBookingDetaildetails.AgencyID = SelectedTest.AgencyID;
                        //BizAction.OrderBookingDetaildetails.Quantity = SelectedTest.Quantity;
                        //BizAction.OrderBookingDetaildetails.SampleCollected = SelectedTest.SampleCollected;
                        //BizAction.OrderBookingDetaildetails.IsSampleCollected = SelectedTest.IsSampleCollected;

                        BizAction.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        BizAction.OrderBookingDetailList = testList;

                        BizAction.SampleCollectionDate = (DateTime?)DateTime.Now;
                        BizAction.OrderID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).OrderBookingID;
                        BizAction.UnitID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;
                        // BizAction.OrderBookingDetailList = list;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {

                                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is collected successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    msgbox.Show();
                                    testList = new List<clsPathOrderBookingDetailVO>();
                                    isCollecionPopupShow = false;
                                    FillOrderBookingList(true);
                                }
                            }
                        };

                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample details already added.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                    }
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please enter sample details.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();
                }
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Test.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
        }

        //private void SaveSampleCollection()
        //{
        //    //if (dgTest.ItemsSource != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem) != null)
        //    //{
        //    //if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected != false)
        //    //{
        //    //    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleCollected == null)
        //    //    {
        //    if (SelectedtestList != null && SelectedtestList.Count > 0)
        //    {
        //        clsUpdatePathOrderBookingDetailListBizActionVO BizAction = new clsUpdatePathOrderBookingDetailListBizActionVO();
        //        BizAction.OrderBookingDetaildetails = new clsPathOrderBookingDetailVO();
        //        BizAction.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
        //        // BizAction.OrderBookingDetailList = testList;
        //        BizAction.OrderBookingDetailList = SelectedtestList;
        //        //BizAction.ItemList = SelectedTest.ItemList;
        //        BizAction.SampleCollectionDate = (DateTime?)DateTime.Now;

        //        BizAction.OrderID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).OrderBookingID;

        //        BizAction.UnitID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).UnitId;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (arg.Result != null)
        //                {
        //                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample Is Collected Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //                    msgbox.Show();
        //                    msgbox.OnMessageBoxClosed += (arg1) =>
        //                    {
        //                        testList = new List<clsPathOrderBookingDetailVO>();
        //                        isCollecionPopupShow = false;
        //                        //Modifications by Saily P on 07.01.14. Purpose - changes to flow for resolving bugs
        //                        //FillOrderBookingList(true);                               
        //                        ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());
        //                    };
        //                    msgbox.Show();

        //                }
        //            }
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //}

        //private void CmdSave_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgTest.ItemsSource != null && ((clsPathOrderBookingDetailVO)dgTest.SelectedItem) != null)
        //    {
        //        if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected != false)
        //        {
        //            if (SelectedtestList != null && SelectedtestList.Count > 0)
        //            {
        //                if (isCollecionPopupShow == false)
        //                {
        //                    if (System.DateTime.Now.ToShortDateString() != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplicationDateTime.ToShortDateString())
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Server Date & System Date Are Not Matching. Change The System Date & Try Again.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                        {
        //                            //((IApplicationConfiguration)App.Current).CurrentUser = null;
        //                            //Uri addressLogout = new Uri(Application.Current.Host.Source, "../Logout.aspx");
        //                            //HtmlPage.Window.Navigate(addressLogout);
        //                        };
        //                        msgW1.Show();
        //                    }
        //                    else
        //                    {
        //                        SampleCollectionTimeChildWindow Sample = new SampleCollectionTimeChildWindow();   //frmSampleCollectionMultipleTests Sample = new frmSampleCollectionMultipleTests();
        //                        //Sample.DataContext = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem);
        //                        Sample.SelectedTests = SelectedtestList;
        //                        //Sample.DataContext = SelectedtestList;
        //                        Sample.OnSaveButtonClick += new RoutedEventHandler(AddSaveCollection_Click);
        //                        Sample.OnCancelButtonClick += new RoutedEventHandler(CancelCollection_Click);
        //                        Sample.Show();
        //                    }
        //                }
        //                else
        //                {
        //                    AddSaveButton_Click(SelectedTest, e);
        //                }
        //            }
        //            else
        //            {//select the tests first - message. 
        //                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "No Sample Collection For Saving. ", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //                msgbox.Show();
        //            }
        //        }
        //    }
        //}

        //private void AddSaveCollection_Click(object sender, RoutedEventArgs e)
        //{//Save the sample collection details along with item consumption.
        //    //clsPathOrderBookingDetailVO Obj = new clsPathOrderBookingDetailVO();

        //    SelectedTest = (clsPathOrderBookingDetailVO)sender;

        //    foreach (var item in SelectedtestList)
        //    {
        //        item.SampleCollectedDateTime = SelectedTest.SampleCollectedDateTime;
        //        item.SampleReceivedDateTime = SelectedTest.SampleReceivedDateTime;
        //        item.SampleCollectionCenter = SelectedTest.SampleCollectionCenter;
        //        //item.CollectionCenterID = SelectedTest.CollectionCenterID;
        //        //item.LabID = SelectedTest.LabID;
        //    }
        //    isCollecionPopupShow = true;

        //    SaveSampleCollection();
        //}

        private void CancelCollection_Click(object sender, RoutedEventArgs e)
        {//Cancel the sample collection along with item consumption. 
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            isCollecionPopupShow = false;
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }



        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {

            if (dgTest.SelectedItem != null)
            {
                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsSampleCollected)
                {
                    BarcodeForm win = new BarcodeForm();
                    string MRNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).MRNo;
                    string OrderNo = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).OrderNo;
                    string SampleNo = (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo).ToString();
                    win.PrintData = MRNo + OrderNo + SampleNo;
                    win.Show();
                }
                else
                {
                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please enter sample details.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgbox.Show();

                }

            }


        }



    }
}
