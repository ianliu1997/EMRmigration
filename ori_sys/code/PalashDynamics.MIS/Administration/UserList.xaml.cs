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
using System.Text;

namespace PalashDynamics.MIS.Administration
{
    public partial class UserList : UserControl
    {

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; 
        public UserList()
        {
            InitializeComponent();
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
                            ((MasterListItem)res.First()).Status = true;
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

        public string SendClinicID = string.Empty;
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;

            //if (dtpFromDate.SelectedDate != null)
            //{
            //    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            //}

            //if (dtpToDate.SelectedDate != null)
            //{
            //    dtpT = dtpToDate.SelectedDate.Value.Date.Date;
            //    dtpP = dtpT;
            //}

            //if (dtpT != null)
            //{
            //    if (dtpF != null)
            //    {
            //        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

            //        if (dtpF.Equals(dtpT))
            //            dtpT = dtpF.Value.Date.AddDays(1);
            //    }
            //}           

            List<MasterListItem> clinicList = new List<MasterListItem>();
            List<MasterListItem> selectedClinicList = new List<MasterListItem>();



            clinicList = (List<MasterListItem>)cmbClinic.ItemsSource;
            if (clinicList.Count > 0)
            {
                foreach (var item in clinicList)
                {
                    if (item.Status == true)
                    {
                        selectedClinicList.Add(item);
                    }
                }
            }

            long clinicID = 0;
            StringBuilder builder = new StringBuilder();
            foreach (var item in selectedClinicList)
            {
                clinicID = item.ID;

                builder.Append(clinicID).Append(",");

            }

            SendClinicID = builder.ToString();

            if (SendClinicID.Length != 0)
            {
                SendClinicID = SendClinicID.TrimEnd(',');
            }


            long clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            long UserStatus=0;
            bool IsExporttoExcel = false;

            IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;

            if (optAll.IsChecked == true)
                UserStatus = 2;
            else if (optActive.IsChecked == true)
                UserStatus = 1;
            else if (optInActive.IsChecked == true)
                UserStatus = 0;
            else if (optLocked.IsChecked == true)
                UserStatus = 3;
            long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //string URL = "../Reports/Administration/UserListReport.aspx?FromDate=" + dtpF + "&ToDate=" + dtpT + "&LoyaltiCardID=" + LoyaltyCardID + "&ClinicID=" + clinic + "&Area=" + Area + "&visitCount=" + visitCount + "&ToDatePrint=" + dtpP;
            string URL = "../Reports/Administrator/UserListReport.aspx?ClinicID=" + clinic + "&UserStatus=" + UserStatus + "&IsExporttoExcel=" + IsExporttoExcel + "&LoginUnitID=" + lUnitID + "&SendClinicID=" + SendClinicID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.Administration.AdministrationReports") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
        }

        private void chkTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkTerms_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        

       
    }
}
