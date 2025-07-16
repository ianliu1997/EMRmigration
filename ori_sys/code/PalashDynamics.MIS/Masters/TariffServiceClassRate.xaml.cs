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
//using PalashDynamics.Service.PalashServiceReferance;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.UserControls;
using System.Reflection;
using System.Windows.Browser;

namespace PalashDynamics.MIS.Masters
{
    public partial class TariffServiceClassRate : UserControl
    {
        public TariffServiceClassRate()
        {
            InitializeComponent();
        }
        WaitIndicator wait = new WaitIndicator();
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long  b = 0, c = 0;
             
            if (((MasterListItem)cmbTariff.SelectedItem).ID > 0)
            {
                b = ((MasterListItem)cmbTariff.SelectedItem).ID;
            }
            if (((MasterListItem)cmbUserRole.SelectedItem).ID > 0)
            {
                c = ((MasterListItem)cmbUserRole.SelectedItem).ID;
            }
            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/TariffServiceClassRateReport.aspx?Tariff=" + b + "&Class=" + c + "&Excel=" + chkExcel.IsChecked + "&UnitID=" + lUnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillUserRole();
            FillUserType();
        }

        private void FillUserRole()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbDepartment.SelectedItem = MasterItem;
                    cmbTariff.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                //}


                wait.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillUserType()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbUserRole.ItemsSource = null;
                    cmbUserRole.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbDepartment.SelectedItem = MasterItem;
                    cmbUserRole.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                //}


                wait.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        
    }
}
