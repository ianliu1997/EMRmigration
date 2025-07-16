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
using PalashDynamics;
using PalashDynamics.OutPatientDepartment.ViewModels;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using Blacklight.Silverlight.Controls;
using PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using MessageBoxControl;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Billing;
using System.Windows.Browser;
//using PalashDynamics.ValueObjects;
//using System.ServiceModel;
//using CIMS.PalashService;
//using PalashDynamics.ValueObjects.Patient;

namespace CIMS.Forms
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Home_Loaded);
            //chartCollection = new System.Windows.Controls.DataVisualization.Charting.PieSeries();

        }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
       // public long User = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
        HomePatientsViewModel RecentPatientsnew;
        PagedCollectionView pcv = null;
        bool SelectPatientMode = false;
        bool SelectedQueuePatientMode = false;

        void Home_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                GetUserDashBoardDetails();

                RecentPatientsnew = new HomePatientsViewModel();
                //  dgPatientList.ItemsSource = RecentPatientsnew.DataList;
                //grdPgrRecentPatients.Source = RecentPatientsnew.DataList;
                this.DataContext = RecentPatientsnew;

                grdAppointments.ItemsSource = RecentPatientsnew.AppointmentList;
                DataPagerAppointment.Source = RecentPatientsnew.AppointmentList;

                //commented by rohini 
                //grdTomAppointments.ItemsSource = RecentPatientsnew.TomAppointmentList;
                //DataPagerTomAppointment.Source = RecentPatientsnew.TomAppointmentList;

                //grdPatientQ.ItemsSource = RecentPatientsnew.QueueList;
                //grdPgrPatientQ.Source = RecentPatientsnew.QueueList;

                //
                //chartCollection.ItemsSource = RecentPatientsnew.DailyCollection;
                          
               // BY BHUSHAN . . . . . .
                grdAlerts.ItemsSource = RecentPatientsnew.TherapyList; //pcv;
               DataPagerAlerts.Source = RecentPatientsnew.TherapyList; //pcv;S
               //commented by rohini 
               //dgBillList.ItemsSource = RecentPatientsnew.PendingBillList;
               //DataPagerPendingBills.Source = RecentPatientsnew.PendingBillList;
                //

                GetPendingVisit();
                Indicatior.Close();
            }

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

        private void hlCloseVisit_Click(object sender, RoutedEventArgs e)
        {
            clsClosePendingVisitBizActioVO BizAction = new clsClosePendingVisitBizActioVO();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    //rohini
                        //lblVisit.Visibility = Visibility.Collapsed;
                        //hlCloseVisit.Visibility = Visibility.Collapsed;
                    //-----
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Previous visit close successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbox.Show();
                    
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
  
        }
       
        void GetUserDashBoardDetails()
        {
            clsGetLoginNamePasswordBizActionVO objBizVO = new clsGetLoginNamePasswordBizActionVO();
            // objBizVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            // objBizVO.ID= ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            //objBizVO.UserType = 0;
            objBizVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
           string temp;
           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsUserVO objRoleDetails = ((clsGetLoginNamePasswordBizActionVO)ea.Result).LoginDetails;
                    List<clsDashBoardVO> lList = new List<clsDashBoardVO>();
                 //   lstItems.ItemsSource = ((clsGetLoginNamePasswordBizActionVO)ea.Result).DashBoardList;
                   // clsUserVO objDashBoardDetails = ((clsGetLoginNamePasswordBizActionVO)ea.Result).DashBoardList;
                    lList = ((clsGetLoginNamePasswordBizActionVO)ea.Result).DashBoardList;
                    
                    //lList. 
                    //RecentPatients.Visibility = Visibility.Collapsed;
                     Appointments.Visibility = Visibility.Collapsed;
                 
                     ScheduleForETOPU.Visibility = Visibility.Collapsed;
                    //rohini
                    // PatientQueue.Visibility = Visibility.Collapsed;
                    //----------
                    foreach (var item in lList)
                    {
                        if (item.Status == true)
                        {

                            if (item.ID == 1 && item.Status == true)
                            {
                                //RecentPatients.Visibility = Visibility.Visible;
                            }              
                         
                            if (item.ID == 2 && item.Status == true)
                            {
                                Appointments.Visibility = Visibility.Visible;
                            }

                            if (item.ID == 5 && item.Status == true)
                            {
                              //  PatientQueue.Visibility = Visibility.Visible;
                               // PatientQueue.Visibility = Visibility.Collapsed;
                            }
                            if (item.ID == 7 && item.Status == true)
                            {
                               // TomorrowAppointments.Visibility = Visibility.Visible;
                              //  TomorrowAppointments.Visibility = Visibility.Collapsed;
                            }
                            if (item.ID == 8 && item.Status == true)
                            {
                                ScheduleForETOPU.Visibility = Visibility.Visible;
                            }
                            if (item.ID == 8 && item.Status == true)
                            {
                                //commented by rohini
                                //PendingBills.Visibility = Visibility.Visible;
                               
                            }

                        }
                        
                    }
                }
            };

            // client.ProcessCompleted += new EventHandler<ProcessCompletedEventArgs>(client_GetUserDashBoardDetails);
            client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        private void DragDockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void DragDockPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // MessageBox.Show("DragDockPanel_SizeChanged");
        }

        private void DragDockPanel_Maximized(object sender, EventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            App.MainPage.RequestXML("Find Patient");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetData();
            //RecentPatientsnew.GetCollectionList();
            //chartCollection.ItemsSource = RecentPatientsnew.DailyCollection;
            //dgPatientList.ItemsSource = null;
            //grdPgrRecentPatients.Source = null;
            //dgPatientList.ItemsSource = RecentPatientsnew.DataList;
            //grdPgrRecentPatients.Source = RecentPatientsnew.DataList;
            SelectPatientMode = true;
            //lblGraphSummary.Text = RecentPatientsnew.TotalCollection.ToString();
            SelectedQueuePatientMode = false;
            Indicatior.Close();
        }

        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            App.MainPage.RequestXML("Home");
           // lblGraphSummary.Text = RecentPatientsnew.TotalCollection.ToString();
            //HomePatientsViewModel RecentPatients = new HomePatientsViewModel();
            RecentPatientsnew.PageSize = 15;
            if (((DragDockPanel)sender).Name == "RecentPatients")
            {
                RecentPatientsnew.GetData();
                //dgPatientList.ItemsSource = null;
                //grdPgrRecentPatients.Source = null;
                //dgPatientList.ItemsSource = RecentPatientsnew.DataList;
                //grdPgrRecentPatients.Source = RecentPatientsnew.DataList;
                SelectPatientMode = false;
            }
            else if (((DragDockPanel)sender).Name == "Appointments")
            {
                RecentPatientsnew.GetAppointmentList();
                grdAppointments.ItemsSource = null;
                DataPagerAppointment.Source = null;
                grdAppointments.ItemsSource = RecentPatientsnew.AppointmentList;
                DataPagerAppointment.Source = RecentPatientsnew.AppointmentList;
            }

                //commented by rohini
            //else if (((DragDockPanel)sender).Name == "PatientQueue")
            //{
            //    RecentPatientsnew.GetQueueList();
            //    grdPatientQ.ItemsSource = null;
            //    grdPgrPatientQ.Source = null;
            //    grdPatientQ.ItemsSource = RecentPatientsnew.QueueList;
            //    grdPgrPatientQ.Source = RecentPatientsnew.QueueList;
            //    SelectedQueuePatientMode = false;
            //}
                //
            else if (((DragDockPanel)sender).Name == "ScheduleForETOPU")
            {
                RecentPatientsnew.GetTherapyList();
                grdAlerts.ItemsSource = null;
                DataPagerAlerts.Source = null;
                grdAlerts.ItemsSource = RecentPatientsnew.TherapyList;
                DataPagerAlerts.Source = RecentPatientsnew.TherapyList;

            }
            Indicatior.Close();
        }

        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectPatientMode == true)
            //   ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dgPatientList.SelectedItem;
            {
            }
            else
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
        }

        private void grdAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdAppointments.SelectedItem= new clsAppointmentVO();
        }

        private void PatientQueue_Maximized(object sender, EventArgs e)
        {            
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            App.MainPage.RequestXML("Queue Management");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetQueueList();
            // commented by rohini
            //grdPatientQ.ItemsSource = null;
            //grdPgrPatientQ.Source = null;
            //grdPatientQ.ItemsSource = RecentPatientsnew.QueueList;
            //grdPgrPatientQ.Source = RecentPatientsnew.QueueList;
            // -----------------------
            SelectedQueuePatientMode = true;
            SelectPatientMode = false;
            Indicatior.Close();
        }

        private void Appointments_Maximized(object sender, EventArgs e)
        {            
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            App.MainPage.RequestXML("Appointment");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetAppointmentList();
            grdAppointments.ItemsSource = null;
            DataPagerAppointment.Source = null;
            grdAppointments.ItemsSource = RecentPatientsnew.AppointmentList;
            DataPagerAppointment.Source = RecentPatientsnew.AppointmentList;
            SelectedQueuePatientMode = false;
            SelectPatientMode = false;    
            Indicatior.Close();
        }

        private void grdPatientQ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //commented by rohini
            //if (SelectedQueuePatientMode == true)
            //{
            //    if (grdPatientQ.SelectedItem != null)
            //    {
            //        clsQueueVO ObjQueueVO = new clsQueueVO();
            //        ObjQueueVO = ((clsQueueVO)grdPatientQ.SelectedItem);

            //        clsGetPatientGeneralDetailsListBizActionVO BizAction = new clsGetPatientGeneralDetailsListBizActionVO();
            //        BizAction.PatientDetailsList = new List<clsPatientGeneralVO>();
            //        BizAction.MRNo = ObjQueueVO.MRNO;
            //        BizAction.OPDNo = ObjQueueVO.OPDNO;

            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //        client.ProcessCompleted += (s, arg) =>
            //        {
            //            if (arg.Error == null && arg.Result != null)
            //            {
            //                if (((clsGetPatientGeneralDetailsListBizActionVO)arg.Result).PatientDetailsList != null)
            //                {
            //                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientGeneralDetailsListBizActionVO)arg.Result).PatientDetailsList[0];

            //                }
            //            }
            //            else
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                msgW1.Show();
            //            }
            //        };
            //        client.ProcessAsync(BizAction, new clsUserVO());
            //        client.CloseAsync();

            //    }
            //}
            //else
            //{
            //    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //}
        }

        private void ScheduleForETOPU_Maximized(object sender, EventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            //App.MainPage.RequestXML("Appointment");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetTherapyList();
            grdAlerts.ItemsSource = null;
            DataPagerAlerts.Source = null;
          
            grdAlerts.ItemsSource = RecentPatientsnew.TherapyList;
            DataPagerAlerts.Source = RecentPatientsnew.TherapyList;       
            SelectedQueuePatientMode = false;
            SelectPatientMode = false;
            Indicatior.Close();
        }

        private void ScheduleForETOPU_Minimized(object sender, EventArgs e)
        {
            // BY BHUSHAN . . .
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            //App.MainPage.RequestXML("Appointment");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetTherapyList();
            grdAlerts.ItemsSource = null;
            DataPagerAlerts.Source = null;

            grdAlerts.ItemsSource = RecentPatientsnew.TherapyList;
            DataPagerAlerts.Source = RecentPatientsnew.TherapyList;
            SelectedQueuePatientMode = false;
            SelectPatientMode = false;
            Indicatior.Close();
        }

        private void grdAlerts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdAlerts.SelectedItem = new clsTherapyDashBoardVO();
        }

        private void PendingBills_Maximized(object sender, EventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            App.MainPage.RequestXML("Billing");
            RecentPatientsnew.PageSize = 15;
            RecentPatientsnew.GetAppointmentList();
            grdAppointments.ItemsSource = null;
            DataPagerAppointment.Source = null;
            grdAppointments.ItemsSource = RecentPatientsnew.AppointmentList;
            DataPagerAppointment.Source = RecentPatientsnew.AppointmentList;
            SelectedQueuePatientMode = false;
            SelectPatientMode = false;
            Indicatior.Close();
        }

        clsBillVO SelectedBill { get; set; }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //commented by Rohini 26 oct 2015
            //if (dgBillList.SelectedItem != null)
            //{
            //    SelectedBill = new clsBillVO();
            //    SelectedBill = (clsBillVO)dgBillList.SelectedItem;
            //    if (SelectedBill.IsFreezed == true)
            //    {
            //        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
            //        {
            //                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
            //        }
            //        else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
            //        {
            //            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
            //        }
            //        //else
            //        //{
            //        //    if (SelectedBill.IsCashTariff == true)
            //        //        PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
            //        //    else
            //        //        PrintCompayBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);
            //        //    PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID);

            //        //}
            //        ////}
            //    }
            //}
            /////-------------------
        }


        private void PrintPharmacyBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID; ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        private void PrintBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {

        }        
    }
}
