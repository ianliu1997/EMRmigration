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

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class SotckCheckReport : UserControl
    {
      public  long ClinicID { get; set; }
        public SotckCheckReport()
        {
            InitializeComponent();
        }

        private void FillUnitList()
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

                    if (ClinicID > 0)
                    {
                        cmbClinic.SelectedValue = ClinicID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                            cmbClinic.SelectedItem = objList[1];
                        else
                            cmbClinic.SelectedItem = objList[0];
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillUnitList();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtFT = null;
            //DateTime? dtTT = null;
            //DateTime? dtTP = dtpToDate.SelectedDate.Value.Date;
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            Nullable<DateTime> dtTP = null;
            bool chkToDate = true;
            string msgTitle = "";

            if (dtpFromDate.SelectedDate != null)
            {
                dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
            }
            if (dtpToDate.SelectedDate != null)
            {
                dtTT = dtpToDate.SelectedDate.Value.Date.Date.AddDays(1);
                if (dtFT.Value > dtTT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtTT = dtFT;
                    chkToDate = false;
                }
                else
                {
                    dtTP = dtTT;
                    //dtTT = dtTT.Value.Date.AddDays(1);
                    dtTT = dtTT.Value.AddDays(1);
                    dtpToDate.Focus();
                }
            }         

            long clinic = 0;

            if (cmbClinic.SelectedItem != null)
            {
                clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            long store = 0;

            if (cmbStore.SelectedItem != null)
            {
                store = ((MasterListItem)cmbStore.SelectedItem).ID;
            }

            if (chkToDate == true)
            {
                string URL;
                if (dtFT != null && dtTT != null && dtTP != null)
                {
                    URL = "../Reports/InventoryPharmacy/SotckCheckReport.aspx?FromDate=" + dtFT.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtTT.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + clinic + "&ToDatePrint=" + dtTP.Value.ToString("dd/MMM/yyyy") + "&store=" + store;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/SotckCheckReport.aspx?ClinicID=" + clinic + "&store=" + store;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else
            {
                string msgText = "Incorrect Date Range. From Date cannot be greater than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
        }

        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

            FillStores(clinicId);
        }
        private void FillStores(long clinicId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (clinicId > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    cmbStore.SelectedItem = objList[0];

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
    }
}
