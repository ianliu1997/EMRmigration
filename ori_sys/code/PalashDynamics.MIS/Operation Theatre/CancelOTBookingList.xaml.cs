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
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Operation_Theatre
{
    public partial class CancelOTBookingList : UserControl
    {
        public CancelOTBookingList()
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FetchOTForFrontPanel();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long OTID = 0;
                long OTTableID = 0;


                if (cmbOT.SelectedItem != null && ((MasterListItem)cmbOT.SelectedItem).ID != 0)
                    OTID = ((MasterListItem)cmbOT.SelectedItem).ID;
                if (cmbOTable.SelectedItem != null && ((MasterListItem)cmbOTable.SelectedItem).ID != 0)
                    OTTableID = ((MasterListItem)cmbOTable.SelectedItem).ID;


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
                    URL = "../Reports/OperationTheatre/CancelOTBookingList.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&OTID=" + OTID + "&OTTableID=" + OTTableID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/OperationTheatre/CancelOTBookingList.aspx?OTID=" + OTID + "&OTTableID=" + OTTableID;
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
    }
}
