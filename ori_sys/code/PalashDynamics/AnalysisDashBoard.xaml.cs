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
using CIMS;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using System.Windows.Controls.Primitives;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.OutPatientDepartment;

namespace PalashDynamics.Forms
{
    public partial class AnalysisDashBoard : ChildWindow
    {
        public AnalysisDashBoard()
        {
            InitializeComponent();
            chartCollection = new System.Windows.Controls.DataVisualization.Charting.PieSeries();
            DailySalesCollection = new System.Windows.Controls.DataVisualization.Charting.PieSeries();
            PatientAppointment = new System.Windows.Controls.DataVisualization.Charting.PieSeries();
            VisitList = new System.Windows.Controls.DataVisualization.Charting.PieSeries();          
        }

        public Double TotalCollection { get; private set; }
        public PagedSortableCollectionView<clsDailyCollectionReportVO> DailyCollection { get; private set; }
        public PagedSortableCollectionView<clsAppointmentVO> Appointment { get; private set; }
        public PagedSortableCollectionView<clsDailySalesReportVO> DailySaleslst { get; private set; }
        public PagedSortableCollectionView<clsVisitVO> VisitList_new { get; private set; }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WaitIndicator wait = new WaitIndicator();
            wait.Show();
            try
            {
                DailyCollection = new PagedSortableCollectionView<clsDailyCollectionReportVO>();
                DailySaleslst = new PagedSortableCollectionView<clsDailySalesReportVO>();
                Appointment = new PagedSortableCollectionView<clsAppointmentVO>();
                VisitList_new = new PagedSortableCollectionView<clsVisitVO>();
           
                GetCollectionList();
                GetSalesList();
                GetAppointmentList();
                GetVisitList();
                wait.Close();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        public void GetSalesList()
        {
            clsGetDailyCollectionListBizActionVO BizAction = new clsGetDailyCollectionListBizActionVO();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.CollectionDate = DateTime.Now;
            BizAction.DailySales = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetDailyCollectionListBizActionVO result = ea.Result as clsGetDailyCollectionListBizActionVO;

                    DailySaleslst.Clear();
                    if (result != null && result.SalesList != null)
                    {
                        foreach (clsDailySalesReportVO Queue in result.SalesList)
                        {
                            DailySaleslst.Add(Queue);
                        }

                        var res = from c in DailySaleslst
                                  select c;
                        DailySalesCollection.ItemsSource = DailySaleslst;
                        lblSalesSummary.Text = res.Sum(cnt => cnt.TotalAmount).ToString();
                        ((System.Windows.Controls.DataVisualization.Charting.PieSeries)DailySales.Series[0]).ItemsSource = DailySaleslst;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void GetVisitList()
        {
            clsGetDailyCollectionListBizActionVO BizAction = new clsGetDailyCollectionListBizActionVO();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.CollectionDate = DateTime.Now;
            BizAction.IsVisit = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetDailyCollectionListBizActionVO result = ea.Result as clsGetDailyCollectionListBizActionVO;
                    if (result != null && result.VisitList != null)
                    {
                        foreach (clsVisitVO Queue in result.VisitList)
                        {
                            VisitList_new.Add(Queue);
                        }
                        var res = from c in VisitList_new
                                  select c;
                        VisitList.ItemsSource = VisitList_new;
                        lblVisitSummary.Text = res.Sum(c => c.VisitID).ToString();
                        ((System.Windows.Controls.DataVisualization.Charting.PieSeries)VisitSummary.Series[0]).ItemsSource = VisitList_new;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void GetCollectionList()
        {

            clsGetDailyCollectionListBizActionVO BizAction = new clsGetDailyCollectionListBizActionVO();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.CollectionDate = DateTime.Now;
            BizAction.DailySales = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetDailyCollectionListBizActionVO result = ea.Result as clsGetDailyCollectionListBizActionVO;

                    DailyCollection.Clear();
                    if (result != null && result.List != null)
                    {
                        foreach (clsDailyCollectionReportVO Queue in result.List)
                        {
                            DailyCollection.Add(Queue);
                        }

                        var res = from c in DailyCollection
                                  select c;
                        TotalCollection = res.Sum(cnt => cnt.Collection);
                        chartCollection.ItemsSource = DailyCollection;
                        lblGraphSummary.Text = TotalCollection.ToString();
                        ((System.Windows.Controls.DataVisualization.Charting.PieSeries)PieChart.Series[0]).ItemsSource = DailyCollection;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        public void GetAppointmentList()
        {
            clsGetDailyCollectionListBizActionVO BizAction = new clsGetDailyCollectionListBizActionVO();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            BizAction.CollectionDate = DateTime.Now;
            BizAction.ISAppointmentList = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetDailyCollectionListBizActionVO result = ea.Result as clsGetDailyCollectionListBizActionVO;
                    if (result != null && result.AppointmentList != null)
                    {
                        foreach (clsAppointmentVO Queue in result.AppointmentList)
                        {
                            Appointment.Add(Queue);
                        }
                        var res = from c in Appointment
                                  select c;
                        //TotalCollection = res.Sum(cnt => cnt.AppointmentID);
                        PatientAppointment.ItemsSource = Appointment;
                        lblReviewSummary.Text = res.Sum(c => c.AppointmentID).ToString();
                        ((System.Windows.Controls.DataVisualization.Charting.PieSeries)PatientReview.Series[0]).ItemsSource = Appointment;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {
        }

        ChildWindow rootPage = Application.Current.RootVisual as ChildWindow;

        private void PatientQueue_Maximized(object sender, EventArgs e)
        {      
        }
    }
}

