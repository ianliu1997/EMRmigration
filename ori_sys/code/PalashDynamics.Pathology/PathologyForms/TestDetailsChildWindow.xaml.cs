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
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class TestDetailsChildWindow : ChildWindow    //by rohini dated 12.2.16
    {
        public long OrderID=0;
        public long TestID=0;
        public long UnitId = 0;
        public String TestName = "";
        public string SampleNo=null;
        public long OrderDetailID = 0;
        private List<clsPathOrderBookingDetailVO> OrderTestList { get; set; }
        public TestDetailsChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FatchTestData();
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement;
            //mElement = (TextBlock)rootPage.FindName("SampleHeader");
            //mElement.Text = "" + TestName;
            TestChild.Title = "Test Transation Details" + TestName;

        
        }

        private void FatchTestData()
        {
          clsGetPathOrderTestDetailListBizActionVO BizAction = new clsGetPathOrderTestDetailListBizActionVO();
            BizAction.OrderDetail = new clsPathOrderBookingDetailVO();
            BizAction.OrderID = OrderID;
            BizAction.TestID = TestID;
            BizAction.UnitID = UnitId;
            BizAction.SampleNo = SampleNo;
            BizAction.OrderDetailID = OrderDetailID;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                      //  OrderTestList = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;
                          //foreach (clsPathOrderBookingDetailVO item in OrderTestList)
                          //{
                        dgCollectionList.ItemsSource = null;
                        dgCollectionList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;
                        //(((clsPathOrderBookingDetailVO)dgCollectionList.SelectedItem).SampleAcceptanceDateTime) = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList);
                        //dgReciveList.ItemsSource = null;
                        //dgReciveList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;

                        dgDispatchList.ItemsSource = null;
                        dgDispatchList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;
                        dgAcceptRejectList.ItemsSource = null;
                        dgAcceptRejectList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;

                        dgAuthorizationList.ItemsSource = null;
                        dgAuthorizationList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).AuthorizedOrderBookingDetailList;

                        dgResultEntryList.ItemsSource = null;
                        dgResultEntryList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).ResultOrderBookingDetailList;
                        
                        dgOutsourceList.ItemsSource = null;
                        dgOutsourceList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList;

                        dgHistoryList.ItemsSource = null;
                        dgHistoryList.ItemsSource = ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).ReslutEntryEditList;

                        if (((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList != null)
                        {
                            foreach (clsPathOrderBookingDetailVO item in ((clsGetPathOrderTestDetailListBizActionVO)arg.Result).CollectionOrderBookingDetailList)
                            {
                                TestName = item.TestName +"/"+item.SampleNumber;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            
        }
    }
}

