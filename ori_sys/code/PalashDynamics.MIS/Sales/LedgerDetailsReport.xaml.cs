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
using System.Windows.Browser;
using System.Reflection;

namespace PalashDynamics.MIS.Sales
{
    public partial class LedgerDetailsReport : UserControl
    {
        public LedgerDetailsReport()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now.Date;
            //dtpToDate.SelectedDate = DateTime.Now.Date;
            FillClinic();
        }

        private void FillClinic()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            string msgText = "";

            if (dtpDate.SelectedDate == null)
            {
                msgText = "Please Select Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                msgWindow.Show();
                return;
            }

            long ClinicID = 0;
            bool IsExporttoExcel = (bool)chkExcel.IsChecked;

            if (cmbClinic.SelectedItem != null)
            {
                ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //DateTime? dtpF = null;
            Nullable<DateTime> dtpF = null;

            if (dtpDate.SelectedDate != null)
            {
                dtpF = dtpDate.SelectedDate.Value.Date;
            }

            string URL;
            if (dtpF != null)
            {
                URL = "../Reports/Sales/LedgerDetailsReports.aspx?FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDatePrint=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ClinicID=" + ClinicID + "&Excel=" + chkExcel.IsChecked + "&LoginUnitID=" + lUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }
    }
}
