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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Operation_Theatre
{
    public partial class OTBookingList : UserControl
    {
        public OTBookingList()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Fetch OT For front panel
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTForFrontPanel()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_OTTheatreMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbOT.ItemsSource = null;
                        cmbOT.ItemsSource = objList;
                        cmbOT.SelectedItem = objList[0];


                        //if (this.DataContext != null)
                        //{
                        //    CmbOTTable.SelectedValue = ((clsProcedureMasterVO)this.DataContext).OTTableID;

                        //}


                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Fetch OTTable according to OT
        /// </summary>
        /// <param name="OtTableID"></param>
        private void FetchOTTableForFrontPanel(long OtTableID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_OTTableMaster;
                BizAction.Parent = new KeyValue { Value = "OTTheatreID", Key = OtTableID.ToString() };
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbOTable.ItemsSource = null;
                        cmbOTable.ItemsSource = objList;
                        cmbOTable.SelectedItem = objList[0];


                        //if (this.DataContext != null)
                        //{
                        //    CmbOTTable.SelectedValue = ((clsProcedureMasterVO)this.DataContext).OTTableID;

                        //}


                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long OTID = 0;
                long OTTableID = 0;
                long DocID = 0;
                long StaffID = 0;

                if (cmbOT.SelectedItem != null && ((MasterListItem)cmbOT.SelectedItem).ID != 0)
                    OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
                if (cmbOTable.SelectedItem != null && ((MasterListItem)cmbOTable.SelectedItem).ID != 0)
                    OTTableID = ((MasterListItem)cmbOTable.SelectedItem).ID;
                if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID != 0)
                    DocID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

                if (cmbStaff.SelectedItem != null && ((MasterListItem)cmbStaff.SelectedItem).ID != 0)
                    StaffID = ((MasterListItem)cmbStaff.SelectedItem).ID;

                //DateTime? dtpF = null;
                //DateTime? dtpT = null;
                //DateTime? dtpP = null;
                Nullable<DateTime> dtpF = null;
                Nullable<DateTime> dtpT = null;
                Nullable<DateTime> dtpP = null;

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

                    }
                    else
                    {
                        dtpP = dtpT;
                        //dtpT = dtpT.Value.Date.AddDays(1);
                        dtpT = dtpT.Value.AddDays(1);
                        dtpToDate.Focus();
                    }
                }


                if (dtpT != null)
                {
                    if (dtpF != null)
                    {
                        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                        //if (dtpF.Equals(dtpT))
                        //dtpT = dtpT.Value.Date.AddDays(1);
                    }
                }
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/OperationTheatre/OTBookingListMIS.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&OTID=" + OTID + "&OTTableID=" + OTTableID + "&DocID=" + DocID + "&StaffID=" + StaffID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/OperationTheatre/OTBookingListMIS.aspx?OTID=" + OTID + "&OTTableID=" + OTTableID + "&DocID=" + DocID + "&StaffID=" + StaffID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Operation_Theatre.OTReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbOT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOT.SelectedItem != null)
                {
                    FetchOTTableForFrontPanel(((MasterListItem)cmbOT.SelectedItem).ID);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbOTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FetchOTForFrontPanel();
            FetchDocForFrontPanel();
            FetchStaffForFrontPanel();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }
        private void FetchDocForFrontPanel()
        {
            try
            {
                clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();


                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                BizAction.DepartmentId = 0;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                        cmbDoctor.ItemsSource = null;
                        cmbDoctor.ItemsSource = objList;
                        cmbDoctor.SelectedItem = objList[0];

                        //if (this.DataContext != null)
                        //{
                        //    cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorID;

                        //}


                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void FetchStaffForFrontPanel()
        {
            try
            {
                clsGetStaffForOTSchedulingBizActionVO BizAction = new clsGetStaffForOTSchedulingBizActionVO();
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetStaffForOTSchedulingBizActionVO)arg.Result).staffList);

                        cmbStaff.ItemsSource = null;
                        cmbStaff.ItemsSource = objList;
                        cmbStaff.SelectedItem = objList[0];

                        //if (this.DataContext != null)
                        //{
                        //    cmbDoctor.SelectedValue = ((clsDoctorScheduleVO)this.DataContext).DoctorID;

                        //}


                    }
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
