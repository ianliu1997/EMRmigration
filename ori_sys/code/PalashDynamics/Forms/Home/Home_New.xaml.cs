using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Xml.Linq;
using OPDModule.Forms;
using PalashDynamics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Forms.Home;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace CIMS.Forms
{
    public partial class Home_New : UserControl
    {
        #region Variable Declaration

        WaitIndicator indicator = new WaitIndicator();

        #endregion
        #region DataMembers
        public PagedSortableCollectionView<clsAppointmentVO> AppointmentList { get; private set; }
        //public PagedSortableCollectionView<clsAppointmentVO> TomAppointmentList { get; private set; }
        //public PagedSortableCollectionView<clsDailyCollectionReportVO> DailyCollection { get; private set; }
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
     //   public PagedSortableCollectionView<clsQueueVO> QueueList { get; private set; }
        bool SelectPatientMode = false;
        bool SelectedQueuePatientMode = false;
        bool FromMIS = false;
        #endregion
        public Home_New()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Home_Loaded);
        }
        void Home_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                //this.DataContext = new clsQueueVO()
                //{
                //    DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,

                //    DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor ? ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID : ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID
                //};

                Indicatior.Close();
                Appointments.Width = outterCanvas.ActualWidth - 10;
                Appointments.Height = outterCanvas.ActualHeight - 10;
                Schedule.Height = outterCanvas.ActualHeight - 10;
                Schedule.Width = outterCanvas.ActualWidth - 10;
                Queue.Width = outterCanvas.ActualWidth - 10;
                Queue.Height = outterCanvas.ActualHeight - 10;
                Finance.Width = outterCanvas.ActualWidth - 10;
                Finance.Height = outterCanvas.ActualHeight - 10;
                CryoBank.Width = outterCanvas.ActualWidth - 10;
                CryoBank.Height = outterCanvas.ActualHeight - 10;

                FollowUps.Width = outterCanvas.ActualWidth - 10;
                FollowUps.Height = outterCanvas.ActualHeight - 10;

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement;
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Home";
                cmdAppointments_Click(sender, e);
                //OPDModule.Forms.ExpressRegistration ExpressRegistration = new OPDModule.Forms.ExpressRegistration();
                //ExpressRegistration.Initiate("NEW");
                //grdNewPatient.Content = ExpressRegistration;
                //cmdPatientQueue_Click(sender, e);
                GetPendingVisit();
            }
            if (FromMIS)
                SetDockPanelVisibility("MIS");
            else
                SetDockPanelVisibility("PatientQueue");
            IsPageLoded = true;

        }

        private void GetPendingVisit()
        {
            clsGetPendingVisitBizActioVO BizAction = new clsGetPendingVisitBizActioVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetPendingVisitBizActioVO)arg.Result).Count > 0)
                    {
                        //lblVisit.Visibility = Visibility.Visible;
                        //hlCloseVisit.Visibility = Visibility.Visible;
                        //lblVisit.Text = "Some previous visits are open. Click here to";
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();


        }
        private void cmdAppointments_Click(object sender, RoutedEventArgs e)
        
        {
           
            //HomeAppointment winDisplay = new HomeAppointment();
            //winDisplay.MinWidth = this.ActualWidth;
            //ResultListContent.Content = winDisplay;

            SetDockPanelVisibility("Appointments");
            HomeAppointment Applist = new HomeAppointment();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.AppointmentsContent.Content = Applist;
            AppointmentsContent.Visibility = Visibility.Visible;
            this.AppointmentsContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.AppointmentsContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            
        }
        private void SetDockPanelVisibility(string sState)
        {
            switch (sState)
            {
                case "Schedule":
                    Appointments.Visibility = Visibility.Collapsed;
                     CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Collapsed;                 
                    Queue.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Visible;
                    ScheduleContent.Visibility = Visibility.Visible;
                    Finance.Visibility = Visibility.Collapsed;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;                   
                    break;

                case "Finance":
                    Appointments.Visibility = Visibility.Collapsed;
                     CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    Queue.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Collapsed;
                    Finance.Visibility = Visibility.Visible;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;                 
                    break;

                case "CryoBank":
                    Appointments.Visibility = Visibility.Collapsed;
                    CryoBank.Visibility = Visibility.Visible;
                    CryoBankContent.Visibility = Visibility.Visible;
                    Queue.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    Finance.Visibility = Visibility.Collapsed;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;               
                    break;


                case "Appointments":
                    Appointments.Visibility = Visibility.Visible;
                    Queue.Visibility = Visibility.Collapsed;
                    CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Visible;
                    Finance.Visibility = Visibility.Collapsed;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;
                    break;

                case "Queue":
                    Appointments.Visibility = Visibility.Collapsed;
                     CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    Queue.Visibility = Visibility.Visible;
                    QueueContent.Visibility = Visibility.Visible;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Collapsed;
                    Finance.Visibility = Visibility.Collapsed;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;                    
                    break;

                case "DischargeApproval":
                    Appointments.Visibility = Visibility.Collapsed;
                    CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    Queue.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Collapsed;
                    Finance.Visibility = Visibility.Collapsed;
                    DischargeApproval.Visibility = Visibility.Visible;
                    DischargeApprovalContent.Visibility = Visibility.Visible;
                    FollowUps.Visibility = Visibility.Collapsed;
                    FollowUpsContent.Visibility = Visibility.Collapsed;
                    this.AppointmentsContent.Content = null;
                    break;

                case "FollowUps":
                    Appointments.Visibility = Visibility.Collapsed;
                    Queue.Visibility = Visibility.Collapsed;
                    CryoBank.Visibility = Visibility.Collapsed;
                    CryoBankContent.Visibility = Visibility.Collapsed;
                    QueueContent.Visibility = Visibility.Collapsed;
                    Schedule.Visibility = Visibility.Collapsed;
                    ScheduleContent.Visibility = Visibility.Collapsed;
                    AppointmentsContent.Visibility = Visibility.Visible;
                    Finance.Visibility = Visibility.Collapsed;
                    FollowUps.Visibility = Visibility.Visible;
                    FollowUpsContent.Visibility = Visibility.Visible;
                    this.AppointmentsContent.Content = null;
                    break;
            }
        }
        private void cmdSchedule_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void cmdSchedule_Click_1(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Schedule");
            HomeOPUET Applist = new HomeOPUET();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.Schedule.Content = Applist;
            ScheduleContent.Visibility = Visibility.Visible;
            this.ScheduleContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.ScheduleContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

        }

        private void cmdFinance_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Finance");
            HomeFinance Applist = new HomeFinance();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.Finance.Content = Applist;
            Finance.Visibility = Visibility.Visible;
            this.Finance.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.Finance.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            
        }

        private void cmdCryo_Click(object sender, RoutedEventArgs e)
        {
            //HomeCryoBank winDisplay = new HomeCryoBank();
            //winDisplay.MinWidth = this.ActualWidth;
            //ResultListContent.Content = winDisplay;

            SetDockPanelVisibility("CryoBank");
                  HomeCryoBank Applist = new HomeCryoBank();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.CryoBank.Content = Applist;
            CryoBankContent.Visibility = Visibility.Visible;
            this.AppointmentsContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.AppointmentsContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void cmdSelf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCollection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdRefund_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdExpense_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdQueue_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("Queue");
            HomeQueue Applist = new HomeQueue();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.Queue.Content = Applist;
            QueueContent.Visibility = Visibility.Visible;
            this.AppointmentsContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.AppointmentsContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void outterCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Appointments.Width = outterCanvas.ActualWidth - 10;
            Appointments.Height = outterCanvas.ActualHeight - 10;
            Schedule.Height = outterCanvas.ActualHeight - 10;
            Schedule.Width = outterCanvas.ActualWidth - 10;
            Queue.Width = outterCanvas.ActualWidth - 10;
            Queue.Height = outterCanvas.ActualHeight - 10;
            Finance.Width = outterCanvas.ActualWidth - 10;

            Finance.Height = outterCanvas.ActualHeight - 10;
            CryoBank.Width = outterCanvas.ActualWidth - 10;
            CryoBank.Height = outterCanvas.ActualHeight - 10;
        }

        private void cmdDischargeApproval_Click(object sender, RoutedEventArgs e)
        {
            SetDockPanelVisibility("DischargeApproval");
            frmDischargeApproval Applist = new frmDischargeApproval();
            Applist.Height = Appointments.Height - 20;
            Applist.Width = Appointments.Width - 10;
            this.DischargeApproval.Content = Applist;
            DischargeApprovalContent.Visibility = Visibility.Visible;
            this.DischargeApprovalContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.DischargeApprovalContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
       
        }

        private void cmdFollowUps_Click(object sender, RoutedEventArgs e)       //  To Get FollowUpList on Dashboard 08032017
        {
            SetDockPanelVisibility("FollowUps");
            HomeFollowUp FollowUplist = new HomeFollowUp();
            FollowUplist.Height = FollowUps.Height - 20;
            FollowUplist.Width = FollowUps.Width - 10;
            this.FollowUpsContent.Content = FollowUplist;
            FollowUpsContent.Visibility = Visibility.Visible;
            this.FollowUpsContent.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.FollowUpsContent.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }
    }
}
