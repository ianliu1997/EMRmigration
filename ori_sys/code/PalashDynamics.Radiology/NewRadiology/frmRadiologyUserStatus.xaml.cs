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
using PalashDynamics.ValueObjects.Administration.StaffMaster;

namespace PalashDynamics.Radiology
{
    public partial class frmRadiologyUserStatus : UserControl
    {
        public frmRadiologyUserStatus()
        {
            InitializeComponent();
        }



        private void cmdPrint_Checked(object sender, RoutedEventArgs e)
        {
            List<MasterListItem> SubSpecializationList = new List<MasterListItem>();
            List<MasterListItem> ItemList = new List<MasterListItem>();
            List<MasterListItem> UserListList = new List<MasterListItem>();
            List<MasterListItem> SelectedUserListList = new List<MasterListItem>();
            SubSpecializationList = (List<MasterListItem>)cmbSubSpecialization.ItemsSource;
            foreach (var item in SubSpecializationList)
            {
                if (item.Status == true)
                {
                    ItemList.Add(item);
                }
            }
            UserListList = (List<MasterListItem>)cmbUserList.ItemsSource;
            foreach (var item in UserListList)
            {
                if (item.Status == true)
                {
                    SelectedUserListList.Add(item);
                }
            }

            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            bool IsExporttoExcel = false;
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
            if (cmbClinic.SelectedItem != null)
                ClinicID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            long SpecializationID = 0;
            if (cmbSpecialization.SelectedItem != null)
                SpecializationID = ((MasterListItem)cmbSpecialization.SelectedItem).ID;

            string SubSpecilizaionID = "";
            if (ItemList != null && ItemList.Count > 0)
            {
                foreach (var item in ItemList)
                {
                    SubSpecilizaionID = SubSpecilizaionID + item.ID;
                    SubSpecilizaionID = SubSpecilizaionID + ",";
                }

                if (SubSpecilizaionID.EndsWith(","))
                    SubSpecilizaionID = SubSpecilizaionID.Remove(SubSpecilizaionID.Length - 1, 1);
            }

            string UserListID = "";
            if (SelectedUserListList != null && SelectedUserListList.Count > 0)
            {
                foreach (var item in SelectedUserListList)
                {
                    UserListID = UserListID + item.ID;
                    UserListID = UserListID + ",";
                }

                if (UserListID.EndsWith(","))
                    UserListID = UserListID.Remove(UserListID.Length - 1, 1);
            }

            if (chkToDate == true)
            {
                string URL = "../Reports/Radiology/RadiologyUserStatus.aspx?FromDate=" + dtpF.Value.ToString("MM/dd/yyyy") + "&ToDate=" + dtpT.Value.ToString("MM/dd/yyyy") + "&ToDatePrint=" + dtpP + "&SpecilizaionID=" + SpecializationID + "&SubSpecilizaionID=" + SubSpecilizaionID + "&ListOfUsers=" + UserListID + "&ClinicID=" + ClinicID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
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
            FillSpecialization();
            FillUnitList();
            FillUserList();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            cmbSpecialization.IsEnabled = false;
        }

        private void cmbSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbSpecialization.SelectedItem != null)
            {
                FillSubSpecialization(((MasterListItem)cmbSpecialization.SelectedItem).ID);
            }
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
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
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
                    cmbSpecialization.ItemsSource = null;
                    cmbSpecialization.ItemsSource = objList;
                    cmbSpecialization.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologySpecializationID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

                    cmbSubSpecialization.ItemsSource = null;
                    cmbSubSpecialization.ItemsSource = objList;

                    if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
                        cmbSubSpecialization.SelectedItem = objList[0];
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void FillUserList()
        {
            clsGetStaffBizActionVO BizAction = new clsGetStaffBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
           
            BizAction.IsForRadiology = true;
            //BizAction.StaffDetails.DesignationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DesignationTypeID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetStaffBizActionVO)e.Result).MasterList);
                    cmbUserList.ItemsSource = null;
                    cmbUserList.ItemsSource = objList;
                    cmbUserList.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public bool Isvalidate = true;
        private bool validate()
        {
            if (dtpFromDate.SelectedDate == null || dtpToDate.SelectedDate == null)
            {
                string msgText = "Plea se Select From  Date and To Date";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        Isvalidate = true;
                    }
                };
                msgWindow.Show();
                Isvalidate = false;

            }
            return Isvalidate;
        }
    }
}
