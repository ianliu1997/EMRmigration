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
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using System.Text;

namespace PalashDynamics.MIS.Sales
{
    public partial class DoctorBasedConsultationBillingReport : UserControl
    {
        bool IsPageLoded = false;
        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;
        WaitIndicator Indicatior = null;
        public string Email { get; set; }
        long BillTypeID;

        public DoctorBasedConsultationBillingReport(long TypeID)
        {
            BillTypeID = TypeID;
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbClinic.IsEnabled = false;
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                   // DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                };
            }
            else
            {
                this.DataContext = new clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                   // DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                };
            }

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;



        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            if (BillTypeID > 0)
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.IPDSalesReport") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            FillBillInitial();
            FillClinic();
           
        }


        private void FillBillInitial()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CodeType;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbBillInitial.ItemsSource = null;
                    cmbBillInitial.ItemsSource = objList;
                    cmbBillInitial.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        var res = from r in objList
                                  where r.ID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                                  select r;
                        ((MasterListItem)res.First()).Status = true;
                        cmbClinic.SelectedItem = ((MasterListItem)res.First());
                        cmbClinic.IsEnabled = false;
                    }
                    else
                        cmbClinic.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDepartmentList(long iUnitId)
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";

            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];

                }


            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        //private void FillDoctor(long iUnitID)
        //{
        //    clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if ((MasterListItem)cmbClinic.SelectedItem != null)
        //    {
        //        BizAction.UnitId = iUnitID;
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

        //            var myNewList = objList.OrderBy(i => i.Description);
        //            cmbDoctor.ItemsSource = null;
        //            cmbDoctor.ItemsSource = myNewList.ToList();


        //            if (this.DataContext != null)
        //            {

        //                if (UseApplicationDoctorID == true)
        //                {
        //                    cmbDoctor.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
        //                    UseApplicationDoctorID = false;
        //                }

        //                else
        //                    cmbDoctor.SelectedValue = myNewList.ToList()[0].ID;


        //                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
        //                {
        //                    cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
        //                }
        //            }
        //        }
        //    };

        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        private void FillDoctor(string SendClinicID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            //if ((MasterListItem)cmbClinic.SelectedItem != null)
            //{
            //    BizAction.UnitId = iUnitID;
            //}
            BizAction.StrClinicID = SendClinicID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);

                    var myNewList = objList.OrderBy(i => i.Description);
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = myNewList.ToList();


                    if (this.DataContext != null)
                    {

                        if (UseApplicationDoctorID == true)
                        {
                            cmbDoctor.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }

                        else
                            cmbDoctor.SelectedValue = myNewList.ToList()[0].ID;


                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }
                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null)
                {
                    long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;
                   // FillDepartmentList(clinicId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((MasterListItem)cmbDepartment.SelectedItem != null)
                {
                    GetClinicID();
                    FillDoctor(SendClinicID);
                }
                //FillDoctor(((MasterListItem)cmbClinic.SelectedItem).ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SendClinicID = string.Empty;

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;
                bool chkToDate = true;
                bool IsExporttoExcel = false;

                string msgTitle = "";
                if (dtpFromDate.SelectedDate != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                    if (dtpF.Value > dtpT.Value)
                    {
                        dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                        dtpT = dtpF;
                        chkToDate = false;
                    }
                    else
                    {
                        dtpP = dtpT;
                        dtpT = dtpT.Value.AddDays(1);
                        dtpToDate.Focus();
                    }
                }

                if (dtpT != null)
                {
                    if (dtpF != null)
                    {
                        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                    }
                }

                long ClinicID = 0;
                long DepartmentID = 0;
                long DoctorID = 0;
                long BillInitialID = 0;

                GetClinicID();

                if (cmbClinic.SelectedItem != null)
                {
                    ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                //if (cmbDepartment.SelectedItem != null)
                //{
                //    DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
                //}
                //if (cmbDoctor.SelectedItem != null)
                //{
                //    DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

                //}
                if (cmbBillInitial.SelectedItem != null)
                {
                    BillInitialID = ((MasterListItem)cmbBillInitial.SelectedItem).ID;
                }
                long counterID = 0;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    counterID = 0;
                else
                    counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (chkToDate == true)
                {
                    string URL;

                    URL = "../Reports/Sales/DailyBillReport.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + ClinicID + "&Excel=" + chkExcel.IsChecked + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&BillTypeID=" + BillTypeID + "&BillInitialID=" + BillInitialID + "&counterID=" + counterID + "&CategoryWise=" + chkGrp.IsChecked + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");



                }
                else
                {
                    string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
            catch (Exception)
            {

            }
        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void GetClinicID()
        {
            List<MasterListItem> clinicList = new List<MasterListItem>();
            List<MasterListItem> selectedClinicList = new List<MasterListItem>();

            clinicList = (List<MasterListItem>)cmbClinic.ItemsSource;
            if (clinicList.Count > 0)
            {
                foreach (var item in clinicList)
                {
                    if (item.Status == true)
                    {
                        selectedClinicList.Add(item);
                    }
                }
            }

            long clinicID = 0;
            StringBuilder builder = new StringBuilder();
            foreach (var item in selectedClinicList)
            {
                clinicID = item.ID;

                builder.Append(clinicID).Append(",");

            }

            SendClinicID = builder.ToString();

            if (SendClinicID.Length != 0)
            {
                SendClinicID = SendClinicID.TrimEnd(',');
            }
        }

        private void cmbDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            //GetClinicID();
            //FillDoctor(SendClinicID);
        }

        private void cmbDoctor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //GetClinicID();
            //FillDoctor(SendClinicID);
        }

        private void cmbDoctor_MouseEnter(object sender, MouseEventArgs e)
        {
            GetClinicID();
            FillDoctor(SendClinicID);
        }
    }
}
