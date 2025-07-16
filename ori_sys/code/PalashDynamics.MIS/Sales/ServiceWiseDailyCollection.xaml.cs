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
using System.Reflection;
using CIMS;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Sales
{
    public partial class ServiceWiseDailyCollection : UserControl
    {
        public ServiceWiseDailyCollection()
        {
            InitializeComponent();
            
        }

        #region Fill Combobox

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
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

                    cmbSpecialisation.ItemsSource = null;
                    //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    
                    cmbSpecialisation.ItemsSource = objList;
                    cmbSpecialisation.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                  //  cmbSpecialisation.SelectedValue = ((clsServiceMasterVO)this.DataContext).ID;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillSubSpecialization(long iSupId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            if (iSupId > 0)
                BizAction.Parent = new KeyValue { Key = iSupId, Value = "fkSpecializationID" };
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

                    cmbSubSpecialisation.ItemsSource = null;
                    cmbSubSpecialisation.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSubSpecialisation.SelectedItem = objList[0];

                    if (iSupId > 0)
                        cmbSubSpecialisation.IsEnabled = true;
                    else
                        cmbSubSpecialisation.IsEnabled = false;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void FillService(long SubSepID) 
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
            if (SubSepID > 0)
                BizAction.Parent = new KeyValue { Key = SubSepID, Value = "SubSpecializationId" };
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

                    cmbService.ItemsSource = null;
                    cmbService.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbService.SelectedItem = objList[0];

                    if (SubSepID > 0)
                        cmbService.IsEnabled = true;
                    else
                        cmbService.IsEnabled = false;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        #endregion

        private void cmbSpecialisation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialisation.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialisation.SelectedItem).ID);
            }
        }

        private void cmbSubSpecialisation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSubSpecialisation.SelectedItem != null)
            {
                FillService(((MasterListItem)cmbSubSpecialisation.SelectedItem).ID);
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Sales.SalesReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";
            string ServiceName="";
            long Specialization=0;
            long SubSpecialization = 0;
            long ServiceID = 0;
           // ServiceName = txtServiceName.Text.Trim();
            if (cmbSpecialisation.SelectedItem !=null)
            Specialization = ((MasterListItem)cmbSpecialisation.SelectedItem).ID;
            if (cmbSubSpecialisation.SelectedItem !=null)
            SubSpecialization = ((MasterListItem)cmbSubSpecialisation.SelectedItem).ID;
            long clinicID = 0, counterID = 0; ;
            if (cmbService.SelectedItem !=null)
                ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
            if (cmbClinic.SelectedItem != null)
            {
                clinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            }
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                counterID = 0;
            else
                counterID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
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
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                }
            }



            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/Sales/ServiceWiseDailyCollectionReport.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&ServiceName=" + ServiceName + "&Specialization=" + Specialization + "&SubSpecialization=" + SubSpecialization + "&ServiceID=" + ServiceID + "&clinicID=" + clinicID + "&counterID=" + counterID;
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
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillClinic();
            FillSpecialization();
        }
        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
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
                    //cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
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
        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}
