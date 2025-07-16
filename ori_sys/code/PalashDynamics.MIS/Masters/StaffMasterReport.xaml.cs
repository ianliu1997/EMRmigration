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
using PalashDynamics.UserControls;


namespace PalashDynamics.MIS.Masters
{
    public partial class StaffMasterReport : UserControl
    {
        public StaffMasterReport()
        {
            InitializeComponent();
        }
        WaitIndicator wait = new WaitIndicator();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            FillUnit();
            FillDesignation();
            FillDepartment();
            
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long a=0, b=0, c=0;
            if (((MasterListItem)cmbClinicName.SelectedItem).ID > 0)
            {
                a = ((MasterListItem)cmbClinicName.SelectedItem).ID;
            }
            if(((MasterListItem)cmbDesignation.SelectedItem).ID > 0)
            {
                b = ((MasterListItem)cmbDesignation.SelectedItem).ID;
            }
            if (((MasterListItem)cmbDepartment.SelectedItem).ID > 0)
            {
                c = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            }
            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/rpt_MisStaffMaster.aspx?ClinicName=" + a + "&Designation=" + b + "&Department=" + c + "&Uid=" + lUnitID + "&Excel=" + chkExcel.IsChecked;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Masters.frmRptMaster") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void FillUnit()
        {
            wait.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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
                    cmbClinicName.ItemsSource = null;
                    cmbClinicName.ItemsSource = objList;
                    cmbClinicName.ItemsSource = null;
                    cmbClinicName.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbClinic.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbClinic.SelectedItem = MasterItem;
                    cmbClinicName.SelectedItem = objList[0];

                }
                wait.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDesignation()
        {
            WaitIndicator wait1 = new WaitIndicator();
            wait1.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
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
                    cmbDesignation.ItemsSource = null;
                    cmbDesignation.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbDesignation.SelectedItem = MasterItem;
                    cmbDesignation.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                //}

                wait1.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDepartment()
        {
            WaitIndicator wait2 = new WaitIndicator();
            wait2.Show();
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DepartmentMaster;
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
                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbDepartment.SelectedItem = MasterItem;
                    cmbDepartment.SelectedItem = objList[0];
                }

                //if (this.DataContext != null)
                //{
                //    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                //}

                wait2.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
    }
}
