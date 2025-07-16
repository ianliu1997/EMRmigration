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
using System.Windows.Browser;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class FNSAnalysis : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public FNSAnalysis()
        {
            InitializeComponent();
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
        }
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            bool chkToDate = true;
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

                    // if (dtpF.Equals(dtpT))
                    //dtpT = dtpT.Value.Date.AddDays(1);
                }
            }

            long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            long store = ((MasterListItem)cmbStore.SelectedItem).ID;

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/InventoryPharmacy/FNSAnalysisMIS.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&clinic=" + clinic + "&store=" + store;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/InventoryPharmacy/FNSAnalysisMIS.aspx?clinic=" + clinic + "&store=" + store;
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
        }
        private void cmdCancel_Checked(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.InventoryPharmacy.InventoryReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
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
                            //for selecting unitid according to user login unit id
                            //for selecting unitid according to user login unit id
                            var res = from r in objList
                                      where r.ID == User.UserLoginInfo.UnitId
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

                throw;
            }
        }
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0)
                {
                    cmdPrint.IsEnabled = true;
                    long clinicId = ((MasterListItem)cmbClinic.SelectedItem).ID;

                    FillStores(clinicId);
                }
                else
                    cmdPrint.IsEnabled = false;
            }
            catch (Exception Ex)
            {
                throw;
            }
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
