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
    public partial class UserMasterReport : UserControl
    {
        public UserMasterReport()
        {
            InitializeComponent();
        }
        WaitIndicator wait = new WaitIndicator();
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long a=0, b=0, c=0;
            if (UnLocked.IsChecked == true)
            {
                a = 0;
            }
            else if (Locked.IsChecked == true)
            {
                a = 1;
            }
            else 
            {
                a = 2;
            }
            if (((MasterListItem)cmbUserType.SelectedItem).ID > 0)
            {
                b = ((MasterListItem)cmbUserType.SelectedItem).ID;
            }
            if (((MasterListItem)cmbUserRole.SelectedItem).ID > 0)
            {
                c = ((MasterListItem)cmbUserRole.SelectedItem).ID;
            }
            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/Administrator/rptMisStaffMaster.aspx?Check=" + a + "&Type=" + b + "&Role=" + c + "&Uid=" + lUnitID + "&Excel=" + chkExcel.IsChecked;
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
            BizAction.MasterTable = MasterTableNameList.T_UserRoleMaster;
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
                    cmbUserType.ItemsSource = null;
                    cmbUserType.ItemsSource = objList;
                    //MasterListItem MasterItem = ((List<MasterListItem>)cmbDesignation.ItemsSource).FirstOrDefault(p => p.ID == 0);
                    //cmbDepartment.SelectedItem = MasterItem;
                    cmbUserType.SelectedItem = objList[0];
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
            WaitIndicator wait1 = new WaitIndicator();
            wait1.Show();
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- Select --"));
            objList.Add(new MasterListItem(1, "Doctor"));
            objList.Add(new MasterListItem(2, "Employee"));
            cmbUserRole.ItemsSource = null;
            cmbUserRole.ItemsSource = objList;
            cmbUserRole.SelectedItem = objList[0];

            wait1.Close();
        }

        private void Locked_Click(object sender, RoutedEventArgs e)
        {
            UnLocked.IsChecked = false;
        }

        private void UnLocked_Click(object sender, RoutedEventArgs e)
        {
            Locked.IsChecked = false;
        }
    }
}
