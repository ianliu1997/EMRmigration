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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using System.Reflection;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.MIS.CRM
{
    public partial class LoyaltyProgramReport : UserControl
    {
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public LoyaltyProgramReport()
        {
            InitializeComponent();
        }

        private void FillLoyaltyCard()
        {
            try
            {


                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
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

                        cmbLoyaltyCard.ItemsSource = null;
                        cmbLoyaltyCard.ItemsSource = objList;
                        cmbLoyaltyCard.SelectedItem = objList[0];
                  
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

        private void LoyaltyProgramReport_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            FillClinic();
            FillLoyaltyCard();

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            //DateTime? dtpF = null;
            //DateTime? dtpT = null;
            //DateTime? dtpP = null;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;

            long Clinic=0;
            long LoyaltyID=0;
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";

            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate;
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

            if (((MasterListItem)cmbClinic.SelectedItem).ID != null && ((MasterListItem)cmbClinic.SelectedItem).ID != 0)
            {
                Clinic = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }

            if (((MasterListItem)cmbLoyaltyCard.SelectedItem).ID != null && ((MasterListItem)cmbLoyaltyCard.SelectedItem).ID != 0)
            {
                LoyaltyID = ((MasterListItem)cmbLoyaltyCard.SelectedItem).ID;
            }
            IsExporttoExcel = (bool)chkExporttoExcel.IsChecked;        

            //dtpP = DateTime.Now.Date.Date;

            if (chkToDate == true)
            {
                string URL;
                if (dtpF != null && dtpT != null && dtpP != null)
                {
                    URL = "../Reports/CRM/LoyaltyProgram.aspx?FromDate=" + dtpF.Value.ToString("dd/MMM/yyyy") + "&ToDate=" + dtpT.Value.ToString("dd/MMM/yyyy") + "&LoyaltyID=" + LoyaltyID + "&ClinicID=" + Clinic + "&PrintDate=" + dtpP.Value.ToString("dd/MMM/yyyy") + "&IsExporttoExcel=" + IsExporttoExcel;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    URL = "../Reports/CRM/LoyaltyProgram.aspx?LoyaltyID=" + LoyaltyID + "&ClinicID=" + Clinic + "&IsExporttoExcel=" + IsExporttoExcel;
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.MIS.CRM.CRMReport") as UIElement;
            ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if ((bool)chkExporttoExcel.IsChecked)
            {
                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.DeleteMISReportFileCompleted += (s1, args1) =>
                {
                    if (args1.Error == null)
                    {

                    }
                };
                client.DeleteMISReportFileAsync("/Reports/CRM");
            }
        }   
    }
}
