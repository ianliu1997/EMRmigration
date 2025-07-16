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
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using System.Text;

namespace PalashDynamics.MIS.PathologyReport
{
    public partial class OutSourceReport : UserControl
    {
        Nullable<DateTime> dtpF = null;
        Nullable<DateTime> dtpT = null;
        Nullable<DateTime> dtpP = null;
        public string msgTitle;
        public OutSourceReport()
        {
            InitializeComponent();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
        }
        private void FillClinic()
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
        public string SendClinicID = string.Empty;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillClinic();
        }
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            //if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            //{
            //    if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
            //    {
            //        dtpFromDate.SetValidation("From Date should be less than To Date");
            //        dtpFromDate.RaiseValidationError();
            //        dtpFromDate.Focus();
            //        res = false;
            //    }
            //    else
            //    {
            //        dtpFromDate.ClearValidationError();
            //    }
            //}



            if (dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            //if ((MasterListItem)cmbClinic.SelectedItem == null)
            //{
            //    cmbClinic.TextBox.SetValidation("Please Select Clinic");
            //    cmbClinic.TextBox.RaiseValidationError();
            //    cmbClinic.Focus();
            //    res = false;
            //}
            //else if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
            //{
            //    cmbClinic.TextBox.SetValidation("Please Select Clinic");
            //    cmbClinic.TextBox.RaiseValidationError();
            //    cmbClinic.Focus();
            //    res = false;
            //}
            //else
            //{
            //    cmbClinic.TextBox.ClearValidationError();
            //}

            if (res)
            {
                bool IsExporttoExcel = false;
                bool chkToDate = true;

                if (dtpFromDate.SelectedDate != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                    if (dtpF.Value > dtpT.Value)
                    {
                        dtpToDate.SelectedDate = dtpFromDate.SelectedDate.Value.Date.Date;
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

                        //if (dtpF.Equals(dtpT))
                        //    dtpT = dtpF.Value.Date.AddDays(1);
                    }
                }

                long clinic = 0;
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
                if (cmbClinic.SelectedItem != null)
                    clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
                IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;

                long lUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (chkToDate == true)
                {
                    string URL;
                    if (dtpF != null && dtpT != null && dtpP != null)
                    {
                        URL = "../Reports/Pathology/MIS/RptMISPathologyOutSource.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&ClinicID=" + clinic + "&ToDatePrint=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&IsExporttoExcel=" + IsExporttoExcel + "&LoginUnitID=" + lUnitID;
                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                    }
                    else
                    {
                        URL = "../Reports/Pathology/MIS/RptMISPathologyOutSource.aspx?ClinicID=" + clinic + "&IsExporttoExcel=" + IsExporttoExcel + "&LoginUnitID=" + lUnitID;
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
            else
            {
                string msgText = "Please Select Mandatory Fields";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }


        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.PathologyReport.PathologyReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void dtpFromDate_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
